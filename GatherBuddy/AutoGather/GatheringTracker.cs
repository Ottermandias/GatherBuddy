using ECommons.DalamudServices;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.Addon.Lifecycle;
using System;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ValueType = FFXIVClientStructs.FFXIV.Component.GUI.ValueType;
using System.Buffers.Binary;
using System.Collections;
using GatheringType = GatherBuddy.Enums.GatheringType;
using GatherBuddy.Classes;
using System.Collections.Specialized;
using System.Collections.Generic;
using static GatherBuddy.AutoGather.GatheringTracker;
using System.Linq;

namespace GatherBuddy.AutoGather
{
    public sealed class GatheringTracker : IDisposable, IReadOnlyList<ItemSlot>
    {
        public bool Ready { get => _ready; private set { _ready = value; } }
        public GatheringType NodeType { get; private set; }
        public bool Revisit { get; private set; }
        public bool QuckGatheringAllowed { get; private set; }
        public bool QuckGatheringChecked { get; private set; }
        public bool QuckGatheringInProcess { get; private set; }
        public int Integrity { get; private set; }
        public int MaxIntegrity { get; private set; }
        public bool Touched { get; private set; }
        public bool HiddenRevealed { get; private set; }
        public int Count => 8;
        public ItemSlot this[int key] => Items[key];
        public IEnumerable<ItemSlot> Aviable => Items.Where(i => i.Aviable);

        private ulong GatherChances;
        private ulong ItemsLevels;
        private BitVector32 Enabled;
        private BitVector32 Bonus;
        private BitVector32 RandomYield;
        private BitVector32 Collectable;
        private BitVector32 Flags;
        private readonly uint[] ItemsIds = new uint[8];
        private readonly sbyte[] ItemsYields = new sbyte[8];
        private readonly sbyte[] BoonChances = new sbyte[8];        
        private readonly ItemSlot[] Items = new ItemSlot[8];
        private volatile bool _ready;
        public sealed class ItemSlot
        {
            private readonly GatheringTracker node;
            private readonly int index;
            private Gatherable? cachedItem;

            internal ItemSlot(GatheringTracker node, int index)
            {
                this.node = node;
                this.index = index;
            }

            public int Index => index;
            public GatheringTracker Node => node;
            public uint Id => node.ItemsIds[index];

            public Gatherable Item
            {
                get
                {
                    if (cachedItem != null && cachedItem.ItemId == Id)
                        return cachedItem;

                    if (GatherBuddy.GameData.Gatherables.TryGetValue(Id, out var gatherable))
                    {
                        cachedItem = gatherable;
                        return cachedItem;
                    }

                    return GatherBuddy.GameData.Gatherables.Values.First();
                }
            }
            public int GatherChance => unchecked((sbyte)(node.GatherChances >> (index * 8)));
            public int Level => unchecked((sbyte)(node.ItemsLevels >> (index * 8)));
            public bool Enabled => node.Enabled[1 << index];
            public bool Empty => Id == 0;
            public bool Aviable => !Empty && Enabled;
            public bool Bonus => node.Bonus[1 << index];
            public int Yield => node.ItemsYields[index];
            public bool RandomYield => node.RandomYield[1 << index];
            public int BoonChance => node.BoonChances[index];
            public bool Hidden => node.Flags[1 << index];
            public bool Rare => node.Flags[1 << index << 16];
            public bool Collectable => node.Collectable[1 << index];
        }
        public GatheringTracker() 
        {
            for (var i = 0; i < 8; i++) Items[i] = new(this, i);
            ResetArgs();

            Svc.AddonLifecycle.RegisterListener(AddonEvent.PostRefresh, "Gathering", Handler);
            Svc.AddonLifecycle.RegisterListener(AddonEvent.PreFinalize, "Gathering", Handler);
            Svc.AddonLifecycle.RegisterListener(AddonEvent.PostSetup, "Gathering", Handler);
        }

        private unsafe void Handler(AddonEvent type, AddonArgs args)
        {
            switch (args)
            {
                case AddonSetupArgs sargs:
                    ProcessArgs(sargs.AtkValueSpan);
                    break;
                case AddonRefreshArgs rargs:
                    ProcessArgs(rargs.AtkValueSpan);
                    break;
                case AddonFinalizeArgs:
                    ResetArgs();
                    break;
            }
        }
        private void ProcessArgs(Span<AtkValue> values)
        {
            if (values.Length != 113)
                return;

            var n = 0; //node type 2=logging 3=quarring 4=harvesting 5=mining; 0 after revisit; 
            if (values[n].Type == ValueType.UInt && values[n].UInt is 0 or > 1 and < 6)
            {
                if (values[n].UInt == 0)
                    Revisit = true; //May be a bug and could be fixed soon, since all revisited nodes are reported as mining
                else
                    NodeType = values[n].UInt switch
                    {
                        2 => GatheringType.Logging,
                        3 => GatheringType.Quarrying,
                        4 => GatheringType.Harvesting,
                        5 => GatheringType.Mining,
                        _ => GatheringType.Unknown
                    };
            } 
            else
                LogUnexpectedValue(values, n);

            n = 1; //gather chance byte array
            if (values[n].Type == ValueType.UInt && values[n + 1].Type == ValueType.UInt)
                GatherChances = BinaryPrimitives.ReverseEndianness(values[n].UInt) | BinaryPrimitives.ReverseEndianness((ulong)values[n + 1].UInt);
            else
            {
                LogUnexpectedValue(values, n);
                LogUnexpectedValue(values, n + 1);
            }

            n = 3; //item level byte array
            if (values[n].Type == ValueType.UInt && values[n + 1].Type == ValueType.UInt)
                ItemsLevels = BinaryPrimitives.ReverseEndianness(values[n].UInt) | BinaryPrimitives.ReverseEndianness((ulong)values[n + 1].UInt);
            else
            {
                LogUnexpectedValue(values, n);
                LogUnexpectedValue(values, n + 1);
            }

            //n = 5; //text "Lv."

            for (var i = 0; i < 8; i++)
            {
                n = 6 + i * 11 + 0;//enabled 1=true or false when preception reqirements are not met
                Enabled[1 << i] = values[n].Type != ValueType.Bool || values[n].Bool != false;

                n = 6 + i * 11 + 1;//Item id
                if (values[n].Type == ValueType.UInt)
                    ItemsIds[i] = values[n].UInt;
                else
                    LogUnexpectedValue(values, n);
                
                if (ItemsIds[i] == 0) continue;

                //n = 6 + i * 11 + 2;//Icon id
                //n = 6 + i * 11 + 3;//Name
                //n = 6 + i * 11 + 4;//Unknown (false)
                n = 6 + i * 11 + 5;//flags: 4=bonus 2=?always set? 1=show required perception text
                if (values[n].Type == ValueType.UInt)
                    Bonus[1 << i] = (values[n].UInt & 4) != 0;
                else
                    LogUnexpectedValue(values, n);
                //n = 6 + i * 11 + 6;//text "Requires XXX perception"
                n = 6 + i * 11 + 7;//8-bit values ?_(flags 1=gather channce green arrow; 2=boon green arrow)_boon_quantity (big-edian order)
                if (values[n].Type == ValueType.UInt)
                {
                    ItemsYields[i] = unchecked((sbyte)(values[n].UInt & 0xff));
                    BoonChances[i] = unchecked((sbyte)((values[n].UInt >> 8) & 0xff));
                }
                else
                    LogUnexpectedValue(values, n);
                //n = 6 + i * 11 + 8;//Stars
                n = 6 + i * 11 + 9;//The Giving Land buff (+? quantity)
                if (values[n].Type == ValueType.Bool)
                    RandomYield[1 << i] = values[n].Bool;
                else
                    LogUnexpectedValue(values, n);
                n = 6 + i * 11 + 10;//2=collectable
                if (values[n].Type == ValueType.UInt || !(values[n].UInt is 0 or 2))
                    Collectable[1 << i] = values[n].UInt == 2;
                else
                    LogUnexpectedValue(values, n);
            }
            //n = 94; //Unknown (Undefined)
            //n = 95; //text "-"
            //n = 96; //text "Unknown"
            //n = 97; //slot gathered on previous node (setup only)
            //n = 98; //Unknown (0)
            n = 99; //array of 8-bit flags ?_rare_?_hiden
            if (values[n].Type == ValueType.UInt)
                Flags = new(unchecked((int)values[n].UInt));
            else
                LogUnexpectedValue(values, n);

            //n = 100; //text "Qty. xxx"
            //n = 101; //1 = non-empty slot hovered; 0 = empty slot hovered (probably show "Qty. xxx" text)
            //n = 102; //Unknown (0)
            //n = 103; //normal bonus text
            //n = 104; //hidden bonus text
            //n = 105; //1 if hidden bonus is shown
            n = 106; //bool quick gathering allowed
            if (values[n].Type == ValueType.Bool)
                QuckGatheringAllowed = values[n].Bool;
            else
                LogUnexpectedValue(values, n);

            n = 107; //bool quick gathering checked
            if (values[n].Type == ValueType.Bool)
                QuckGatheringChecked = values[n].Bool;
            else
                LogUnexpectedValue(values, n);

            n = 108; //bool quick gathering in progress
            if (values[n].Type == ValueType.Bool)
                QuckGatheringInProcess = values[n].Bool;
            else
                LogUnexpectedValue(values, n);
            //n = 109; //last selected slot
            n = 110; //integrity
            if (values[n].Type == ValueType.UInt)
                Integrity = unchecked((int)values[n].UInt);
            else
                LogUnexpectedValue(values, n);

            n = 111; //max integrity
            if (values[n].Type == ValueType.UInt)
                MaxIntegrity = unchecked((int)values[n].UInt);
            else
                LogUnexpectedValue(values, n);

            //n = 112; //up arrow on integrity

            Touched = Touched || Integrity != MaxIntegrity;
            HiddenRevealed = HiddenRevealed || (Flags.Data & 0xff) != 0;
            Ready = true;

            void LogUnexpectedValue(Span<AtkValue> values, int n)
            {
                GatherBuddy.Log.Debug($"{GetType()}: unexpected value of argument {n}: {values[n].ToString()}.");
            }
        }

        private void ResetArgs()
        {
            Ready = false;
            Revisit = Touched = HiddenRevealed = false;
            //Not resetting NodeType, keeping value for revisit
            GatherChances = ItemsLevels = 0xffffffffffffffff;
            Enabled = Bonus = Flags = RandomYield = Collectable = new(0);
            for (var i = 0; i < 8; i++)
            {
                ItemsIds[i] = 0;
                ItemsYields[i] = -1;
                BoonChances[i] = -1;
            }
            QuckGatheringAllowed = QuckGatheringChecked = QuckGatheringInProcess = false;
            Integrity = MaxIntegrity = 0;
        }

        public void Dispose()
        {
            Svc.AddonLifecycle.UnregisterListener(AddonEvent.PostRefresh, "Gathering", Handler);
            Svc.AddonLifecycle.UnregisterListener(AddonEvent.PreFinalize, "Gathering", Handler);
            Svc.AddonLifecycle.UnregisterListener(AddonEvent.PostSetup, "Gathering", Handler);
        }

        public IEnumerator<ItemSlot> GetEnumerator() => Items.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
    