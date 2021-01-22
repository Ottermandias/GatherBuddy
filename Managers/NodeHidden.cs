using Dalamud;

namespace Gathering
{
    public class NodeHidden
    {
        private readonly Gatherable[] maps;

        private readonly (string seed, Gatherable fruit)[] seeds;

        private readonly Gatherable[] otherHidden;

        private readonly Gatherable darkMatterCluster;
        private readonly Gatherable unaspectedCrystal;
        public NodeHidden(ItemManager items)
        {
            var from = items.fromLanguage[(int)ClientLanguage.English];
            maps = new Gatherable[]
            { from["Timeworn Leather Map"    ]
            , from["Timeworn Goatskin Map"   ]
            , from["Timeworn Toadskin Map"   ]
            , from["Timeworn Boarskin Map"   ]
            , from["Timeworn Peisteskin Map" ]
            , from["Timeworn Archaeoskin Map"]
            , from["Timeworn Wyvernskin Map" ]
            , from["Timeworn Dragonskin Map" ]
            , from["Timeworn Gaganaskin Map" ]
            , from["Timeworn Gazelleskin Map"]
            , from["Timeworn Gliderskin Map" ]
            , from["Timeworn Zonureskin Map" ]
            };

            darkMatterCluster = from["Dark Matter Cluster"];
            unaspectedCrystal = from["Unaspected Crystal" ];

            seeds = new (string, Gatherable)[]
            { ("Paprika Seeds"           , from["Paprika"                   ])
            , ("Wild Onion Set"          , from["Wild Onion"                ])
            , ("Coerthan Carrot Seeds"   , from["Coerthan Carrot"           ])
            , ("La Noscean Lettuce Seeds", from["La Noscean Lettuce"        ])
            , ("Olive Seeds"             , from["Cinderfoot Olive"          ])
            , ("Popoto Set"              , from["Popoto"                    ])
            , ("Millioncorn Seeds"       , from["Millioncorn"               ])
            , ("Wizard Eggplant Seeds"   , from["Wizard Eggplant"           ])
            , ("Midland Cabbage Seeds"   , from["Midland Cabbage"           ])
            , ("La Noscean Orange Seeds" , from["La Noscean Orange"         ])
            , ("Lowland Grape Seeds"     , from["Lowland Grapes"            ])
            , ("Faerie Apple Seeds"      , from["Faerie Apple"              ])
            , ("Sun Lemon Seeds"         , from["Sun Lemon"                 ])
            , ("Pixie Plum Seeds"        , from["Pixie Plums"               ])
            , ("Blood Currant Seeds"     , from["Blood Currants"            ])
            , ("Mirror Apple Seeds"      , from["Mirror Apple"              ])
            , ("Rolanberry Seeds"        , from["Rolanberry"                ])
            , ("Garlic Cloves"           , from["Garlean Garlic"            ])
            , ("Lavender Seeds"          , from["Lavender"                  ])
            , ("Black Pepper Seeds"      , from["Black Pepper"              ])
            , ("Ala Mhigan Mustard Seeds", from["Ala Mhigan Mustard"        ])
            , ("Pearl Ginger Root"       , from["Pearl Ginger"              ])
            , ("Chamomile Seeds"         , from["Chamomile"                 ])
            , ("Linseed"                 , from["Flax"                      ])
            , ("Midland Basil Seeds"     , from["Midland Basil"             ])
            , ("Mandrake Seeds"          , from["Mandrake"                  ])
            , ("Almond Seeds"            , from["Almonds"                   ])
            // Not really seeds, but same mechanism.
            , ("Oddly Specific Fossil Dust"      , from["Oddly Specific Latex"               ])
            , ("Oddly Specific Mineral Sand"     , from["Oddly Specific Obsidian"            ])
            , ("Oddly Specific Bauble"           , from["Oddly Specific Amber"               ])
            , ("Oddly Specific Striking Stone"   , from["Oddly Specific Dark Matter"         ])
            , ("Oddly Specific Dark Chestnut Log", from["Oddly Specific Leafborne Aethersand"])
            , ("Oddly Specific Primordial Log"   , from["Oddly Specific Primordial Resin"    ])
            , ("Oddly Specific Schorl"           , from["Oddly Specific Landborne Aethersand"])
            , ("Oddly Specific Primordial Ore"   , from["Oddly Specific Primordial Asphaltum"])
            };

            otherHidden = new Gatherable[]
            { (from["Grade 1 La Noscean Topsoil"])
            , (from["Grade 1 Shroud Topsoil"    ])
            , (from["Grade 1 Thanalan Topsoil"  ])
            , (from["Grade 2 La Noscean Topsoil"])
            , (from["Grade 2 Shroud Topsoil"    ])
            , (from["Grade 2 Thanalan Topsoil"  ])
            , (from["Black Limestone"           ])
            , (from["Little Worm"               ])
            , (from["Yafaem Wildgrass"          ])
            , (from["Dark Chestnut"             ])
            , (from["Firelight Seeds"           ])
            , (from["Icelight Seeds"            ])
            , (from["Windlight Seeds"           ])
            , (from["Earthlight Seeds"          ])
            , (from["Levinlight Seeds"          ])
            , (from["Waterlight Seeds"          ])
            , (from["Mythrite Ore"              ])
            , (from["Hardsilver Ore"            ])
            , (from["Titanium Ore"              ])
            , (from["Birch Log"                 ])
            , (from["Cyclops Onion"             ])
            , (from["Emerald Beans"             ])
            };
        }

        private void ApplySeeds(Node N)
        {
            foreach ((string seed, Gatherable fruit) in seeds)
                if (N.items.HasItems(seed))
                    N.AddItem(fruit);
        }

        private void ApplyOtherHidden(Node N)
        {
            switch(N.meta.pointBaseId)
            {
                case 183: N.AddItem(otherHidden[ 0]); return;
                case 163: N.AddItem(otherHidden[ 1]); return;
                case 172: N.AddItem(otherHidden[ 2]); return;
                case 193: N.AddItem(otherHidden[ 3]); return;
                case 209: N.AddItem(otherHidden[ 4]); return;
                case 151: N.AddItem(otherHidden[ 5]); return;
                case 210: N.AddItem(otherHidden[ 6]); return;
                case 177: N.AddItem(otherHidden[ 7]); return;
                case 133: N.AddItem(otherHidden[ 8]); return;
                case 295: N.AddItem(otherHidden[ 9]); return;
                case  30: N.AddItem(otherHidden[10]); return;
                case  39: N.AddItem(otherHidden[11]); return;
                case  21: N.AddItem(otherHidden[12]); return;
                case  31: N.AddItem(otherHidden[13]); return;
                case  25: N.AddItem(otherHidden[14]); return;
                case  14: N.AddItem(otherHidden[15]); return;
                case 285: N.AddItem(otherHidden[16]); return;
                case 353: N.AddItem(otherHidden[17]); return;
                case 286: N.AddItem(otherHidden[18]); return;
                case 356: N.AddItem(otherHidden[19]); return;
                case 297: N.AddItem(otherHidden[20]); return;
                case 298: N.AddItem(otherHidden[21]); return;
            }
        }

        private void ApplyMaps(Node N)
        {
            if (N.meta.nodeType != NodeType.Regular)            
                return;

            switch(N.meta.level)
            {
                case 40: N.AddItem(maps[ 0]); return;
                case 45: N.AddItem(maps[ 1]); return;
                case 50: N.AddItem(maps[ 2]);
                         N.AddItem(maps[ 3]);
                         N.AddItem(maps[ 4]); return;
                case 55: N.AddItem(maps[ 5]); return;
                case 60: N.AddItem(maps[ 6]);
                         N.AddItem(maps[ 7]); return;
                case 70: N.AddItem(maps[ 8]);
                         N.AddItem(maps[ 9]); return;
                case 80: N.AddItem(maps[10]);
                         N.AddItem(maps[11]); return;
            }
        }

        private void ApplyDarkMatter(Node N)
        {
            if (N.meta.nodeType != NodeType.Unspoiled)
                return;
            if (N.meta.level != 50)
                return;
            N.AddItem(darkMatterCluster);
            N.AddItem(unaspectedCrystal);
        }
        public void SetHiddenItems(Node N)
        {
            ApplySeeds(N);
            ApplyMaps(N);
            ApplyDarkMatter(N);
            ApplyOtherHidden(N);
        }
    }
}