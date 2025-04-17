using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GatherBuddy.Models;
using Newtonsoft.Json;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder
{
#if DEBUG
    public const string RemoteUrl = "http://localhost:5000/api/fishrecords/";
#else
    public const string RemoteUrl = "https://gatherbuddy.nostrathomas.net/api/fishrecords/";
#endif
    private const string                  RemoteFishRecordsFileName = "remote_fish_records.dat";
    private const int                     RecordsPerRequest         = 1000;
    internal      List<FishRecord>        RemoteRecords             = [];
    internal      Queue<FishRecord>       RecordsToUpload           = new();
    public        Task                    RemoteRecordsUploadTask              { get; private set; } = Task.CompletedTask;
    public        Task                    RemoteRecordsDownloadTask            { get; private set; } = Task.CompletedTask;
    private       CancellationTokenSource RemoteRecordsCancellationTokenSource { get; }              = new();
    public        DateTime                NextRemoteRecordsUpdate = DateTime.MinValue;
    public        bool                    UploadTaskReady         => RemoteRecordsUploadTask.IsCompleted;

    public void StartLoadRemoteRecords()
    {
        if (!GatherBuddy.Config.AutoGatherConfig.FishDataCollection)
            return;
        var token = RemoteRecordsCancellationTokenSource.Token;
        RemoteRecordsDownloadTask = Task.Run(() => LoadRemoteRecords(token), token);
    }

    public void StartUploadLocalRecords()
    {
        if (!RemoteRecordsUploadTask.IsCompleted)
        {
            GatherBuddy.Log.Warning("Remote records upload task is still running, skipping.");
            return;
        }
        var token = RemoteRecordsCancellationTokenSource.Token;
        RemoteRecordsUploadTask = Task.Run(() => UploadLocalRecords(token), token);
    }

    public void StopRemoteRecordsRequests()
    {
        RemoteRecordsCancellationTokenSource.Cancel();
        WriteRemoteFile();
    }

    private async Task UploadLocalRecords(CancellationToken cancellationToken = default)
    {
        if (RecordsToUpload.Count == 0)
        {
            GatherBuddy.Log.Verbose("No records to upload.");
        }
        else
        {
            try
            {
                var records = new List<FishRecord>();
                while (RecordsToUpload.TryDequeue(out var record) && records.Count < RecordsPerRequest)
                {
                    records.Add(record);
                }

                GatherBuddy.Log.Debug($"Uploading {records.Count} local fish records to remote server.");
                var simpleRecords = new List<SimpleFishRecord>(records.Count);
                foreach (var record in records)
                {
                    var simpleRecord = record.ToSimpleRecord();
                    simpleRecords.Add(simpleRecord);
                }

                var       json     = JsonConvert.SerializeObject(simpleRecords);
                var       content  = new StringContent(json, Encoding.UTF8, "application/json");
                using var client   = new FishRecorderClient();
                var       response = await client.PostAsync(RemoteUrl + "add", content, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    GatherBuddy.Log.Error($"Could not upload local fish records: {response.StatusCode}");
                GatherBuddy.Log.Information($"Uploaded {records.Count} local fish records to remote server.");
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error($"Could not upload local fish records:\n{e}");
            }
        }
        NextRemoteRecordsUpdate = DateTime.Now.AddMinutes(1);
    }

    private async Task LoadRemoteRecords(CancellationToken cancellationToken = default)
    {
        try
        {
            var total = await GetRemoteRecordsTotal(cancellationToken: cancellationToken);
            if (total == 0)
                throw new Exception("No records found.");

            using var client = new FishRecorderClient();
            for (var i = 0; i < total; i += RecordsPerRequest)
            {
                var responseMessage = await client.GetAsync(RemoteUrl + "get" + $"?page={i}" + $"&pageSize={RecordsPerRequest}",
                    cancellationToken: cancellationToken);
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception($"Could not get remote records: {responseMessage.StatusCode}");

                var remoteRecordsJson = await responseMessage.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                var records           = JsonConvert.DeserializeObject<List<SimpleFishRecord>>(remoteRecordsJson);
                if (records == null)
                    throw new Exception("Could not deserialize remote records.");

                foreach (var record in records)
                {
                    RemoteRecords.Add(FishRecord.FromSimpleRecord(record));
                }

                WriteRemoteFile();
            }

            GatherBuddy.Log.Information($"Loaded {RemoteRecords.Count} remote fish records.");
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not load remote fish records:\n{e}");
        }
    }

    public async Task<int> GetRemoteRecordsTotal(CancellationToken cancellationToken = default)
    {
        try
        {
            using var client   = new FishRecorderClient();
            var       response = await client.GetAsync(RemoteUrl + "total", cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            var total = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            GatherBuddy.Log.Information($"Remote fish records total: {total}");
            return int.Parse(total);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not get remote fish records total:\n{e}");
            return 0;
        }
    }

    private void LoadRemoteFile(FileInfo file)
    {
        if (!file.Exists)
            return;

        try
        {
            RemoteRecords.AddRange(ReadFile(file));
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not read fish record file {file.FullName}:\n{e}");
        }
    }

    private void WriteRemoteFile()
    {
        var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, RemoteFishRecordsFileName));
        WriteFileInternal(file, true);
    }

    private class FishRecorderClient : HttpClient
    {
        internal FishRecorderClient()
            : base(new RateLimitingHandler(new HttpClientHandler()))
        {
            var version = typeof(GatherBuddy).Assembly.GetName().Version?.ToString() ?? "0.0.0.0";
            DefaultRequestHeaders.Add("X-Client-Version", version);
            var hash = GetClientHash();
            DefaultRequestHeaders.Add("X-Client-Hash", hash);
        }

        private string GetClientHash()
        {
            var pluginPath = Dalamud.PluginInterface.AssemblyLocation.FullName;
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            using var stream = File.OpenRead(pluginPath);
            var       hash   = sha256.ComputeHash(stream);
            return Convert.ToHexString(hash);
        }
    }

    private class RateLimitingHandler : DelegatingHandler
    {
        internal RateLimitingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            while (true)
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429
                {
                    if (response.Headers.TryGetValues("X-Rate-Limit-Reset", out var values)
                     && int.TryParse(values.FirstOrDefault(), out var retrySeconds))
                    {
                        await Task.Delay(TimeSpan.FromSeconds(retrySeconds), cancellationToken);
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                    }
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return response;
                }
            }
        }
    }
}
