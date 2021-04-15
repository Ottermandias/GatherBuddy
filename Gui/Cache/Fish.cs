using GatherBuddy.Utility;
using ImGuiScene;

namespace GatherBuddy.Gui.Cache
{
    internal struct Fish
    {
        public readonly TextureWrap IconHookSet;
        public readonly TextureWrap IconPowerfulHookSet;
        public readonly TextureWrap IconPrecisionHookSet;
        public readonly TextureWrap IconSnagging;
        public readonly TextureWrap IconGigs;
        public readonly TextureWrap IconSmallGig;
        public readonly TextureWrap IconNormalGig;
        public readonly TextureWrap IconLargeGig;

        public Fish(Cache.Icons icons)
        {
            IconHookSet          = icons[1103];
            IconPowerfulHookSet  = icons[1115];
            IconPrecisionHookSet = icons[1116];
            IconSnagging         = icons[1109];
            IconGigs             = icons[1121];
            IconSmallGig         = icons[60671];
            IconNormalGig        = icons[60672];
            IconLargeGig         = icons[60673];
        }
    }
}
