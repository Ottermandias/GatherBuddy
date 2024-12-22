using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;

namespace GatherBuddy.AutoGather.Movement;


public class SoundHelper
{
    private const string SoundResource = "GatherBuddy.CustomInfo.honk-sound.wav";

    public void PlayHonkSound(int repeatCount)
    {
        // Load the embedded WAV file stream
        var assembly = Assembly.GetExecutingAssembly();
        using (Stream audioStream = assembly.GetManifestResourceStream(SoundResource))
        {
            if (audioStream == null)
            {
                throw new FileNotFoundException($"Embedded resource {SoundResource} not found.");
            }

            // Repeat the sound the specified number of times
            for (int i = 0; i < repeatCount; i++)
            {
                PlaySound(audioStream);
                audioStream.Position = 0; // Reset stream for next play
            }
        }
    }

    private void PlaySound(Stream stream)
    {
        // Use SoundPlayer to play the WAV file from the stream
        using (var player = new SoundPlayer(stream))
        {
            player.Load();     // Load the stream into the player
            player.PlaySync(); // Play synchronously
        }
    }
}
