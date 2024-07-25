using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace GatherBuddy.AutoGather.Movement;

public class SoundHelper
{
    private const string SoundResource = "GatherBuddy.CustomInfo.honk-sound.mp3";
    
    public void PlayHonkSound(int repeatCount)
    {
        Task.Run(() => PlayFromResource(SoundResource, repeatCount));
    }

    private void PlayFromResource(string resourceName, int repeatCount)
    {
        var    assembly    = typeof(GatherBuddy).Assembly;
        Stream audioStream = assembly.GetManifestResourceStream(resourceName);

        if (audioStream != null)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                using (Mp3FileReader mp3Reader = new Mp3FileReader(audioStream))
                {
                    using (WaveOutEvent waveOutEvent = new WaveOutEvent())
                    {
                        waveOutEvent.Init(mp3Reader);
                        waveOutEvent.Play();
                        while (waveOutEvent.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Tasks.Task.Delay(500).Wait();
                        }
                    }
                }
                audioStream.Position = 0; // reset stream position for the next play
            }
        }
        else
        {
            throw new FileNotFoundException($"Embedded resource {resourceName} not found");
        }
    }
}
