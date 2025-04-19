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
    public const  string                  RemoteUrl                 = "https://sevxzz9056.execute-api.us-east-1.amazonaws.com/fishwrap";
    private const string                  RemoteFishRecordsFileName = "remote_fish_records.dat";
    private const int                     RecordsPerRequest         = 100;
    internal      List<FishRecord>        RemoteRecords             = [];
    internal      Queue<FishRecord>       RecordsToUpload           = new();
    public        Task                    RemoteRecordsUploadTask              { get; private set; } = Task.CompletedTask;
    public        Task                    RemoteRecordsDownloadTask            { get; private set; } = Task.CompletedTask;
    private       CancellationTokenSource RemoteRecordsCancellationTokenSource { get; }              = new();
    public        DateTime                NextRemoteRecordsUpdate = DateTime.MinValue;

    public bool UploadTaskReady
        => RemoteRecordsUploadTask.IsCompleted;

    public void StopRemoteRecordsRequests()
    {
        RemoteRecordsCancellationTokenSource.Cancel();
        WriteRemoteFile();
    }

    public void QueueHistoricalRecords()
    {
        var records = Records.Where(r => r.PositionDataValid).ToList();
        foreach (var record in records)
        {
            RecordsToUpload.Enqueue(record);
        }
        GatherBuddy.Log.Information($"Queued {records.Count()} records for upload");
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
                var       response = await client.PostAsync(RemoteUrl, content, cancellationToken);
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

            DefaultRequestHeaders.UserAgent.Clear();
            DefaultRequestHeaders.UserAgent.ParseAdd($"GatherBuddyReborn/{version} (https://github.com/FFXIV-CombatReborn/GatherBuddyReborn)");
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
