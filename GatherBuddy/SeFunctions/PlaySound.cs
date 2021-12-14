using Dalamud.Game;
using GatherBuddy.Alarms;

namespace GatherBuddy.SeFunctions;

public delegate ulong PlaySoundDelegate(int id, ulong unk1, ulong unk2);

public sealed class PlaySound : SeFunctionBase<PlaySoundDelegate>
{
    public PlaySound(SigScanner sigScanner)
        : base(sigScanner, "E8 ?? ?? ?? ?? 4D 39 BE")
    { }

    public void Play(Sounds id)
        => Invoke((int)id, 0ul, 0ul);
}