using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using ECommons.DalamudServices;
using NAudio.Wave;

namespace GatherBuddy.AutoGather.Helpers;

public class SoundHelper
{
    private const string SoundResource = "GatherBuddy.CustomInfo.honk-sound.wav";

    private WaveOutEvent _waveOut = new();

    public void StartHonkSoundTask(int repeatCount)
        => Task.Run(() => PlayHonkSound(repeatCount));

    private void PlayHonkSound(int repeatCount)
    {
        try
        {
            // Load the embedded WAV file stream
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream audioStream = assembly.GetManifestResourceStream(SoundResource))
            {
                if (audioStream == null)
                {
                    throw new FileNotFoundException($"Embedded resource {SoundResource} not found.");
                }

                using var reader = new WaveFileReader(audioStream);
                _waveOut.Init(reader);
                // Repeat the sound the specified number of times
                for (int i = 0; i < repeatCount; i++)
                {
                    PlaySound(audioStream);
                    audioStream.Position = 0; // Reset stream for next play
                    Task.Delay(500).Wait();
                }
            }
        }
        catch (Exception ex)
        {
            Svc.Log.Error(ex, "Error during honk: ");
        }
    }

    private void PlaySound(Stream stream)
    {
        var prevVolume = _waveOut.Volume;
        _waveOut.Volume = GatherBuddy.Config.AutoGatherConfig.SoundPlaybackVolume / 100f;
        _waveOut.Play();
        _waveOut.PlaybackStopped += (sender, args) => _waveOut.Volume = prevVolume;
    }
}
