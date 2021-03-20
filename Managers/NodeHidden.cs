using Dalamud;
using GatherBuddy.Classes;

namespace GatherBuddy.Managers
{
    public class NodeHidden
    {
        private readonly Gatherable[] _maps;

        private readonly (string seed, Gatherable fruit)[] _seeds;

        private readonly Gatherable[] _otherHidden;

        private readonly Gatherable _darkMatterCluster;
        private readonly Gatherable _unaspectedCrystal;

        public NodeHidden(ItemManager items)
        {
            // @formatter:off
            var from = items.FromLanguage[(int)ClientLanguage.English];
            _maps = new Gatherable[]
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
            , from["Timeworn Zonureskin Map" ],
            };

            _darkMatterCluster = from["Dark Matter Cluster"];
            _unaspectedCrystal = from["Unaspected Crystal" ];

            _seeds = new (string, Gatherable)[]
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
            , ("Oddly Specific Primordial Ore"   , from["Oddly Specific Primordial Asphaltum"]),
            };

            _otherHidden = new Gatherable[]
            { @from["Grade 1 La Noscean Topsoil"]
            , @from["Grade 1 Shroud Topsoil"    ]
            , @from["Grade 1 Thanalan Topsoil"  ]
            , @from["Grade 2 La Noscean Topsoil"]
            , @from["Grade 2 Shroud Topsoil"    ]
            , @from["Grade 2 Thanalan Topsoil"  ]
            , @from["Black Limestone"           ]
            , @from["Little Worm"               ]
            , @from["Yafaem Wildgrass"          ]
            , @from["Dark Chestnut"             ]
            , @from["Firelight Seeds"           ]
            , @from["Icelight Seeds"            ]
            , @from["Windlight Seeds"           ]
            , @from["Earthlight Seeds"          ]
            , @from["Levinlight Seeds"          ]
            , @from["Waterlight Seeds"          ]
            , @from["Mythrite Ore"              ]
            , @from["Hardsilver Ore"            ]
            , @from["Titanium Ore"              ]
            , @from["Birch Log"                 ]
            , @from["Cyclops Onion"             ]
            , @from["Emerald Beans"             ],
            };
            // @formatter:on
        }

        private void ApplySeeds(Node N)
        {
            foreach (var (seed, fruit) in _seeds)
                if (N.Items!.HasItems(seed))
                    N.AddItem(fruit);
        }

        // @formatter:off
        private void ApplyOtherHidden(Node N)
        {
            switch(N.Meta!.PointBaseId)
            {
                case 183: N.AddItem(_otherHidden[ 0]); return;
                case 163: N.AddItem(_otherHidden[ 1]); return;
                case 172: N.AddItem(_otherHidden[ 2]); return;
                case 193: N.AddItem(_otherHidden[ 3]); return;
                case 209: N.AddItem(_otherHidden[ 4]); return;
                case 151: N.AddItem(_otherHidden[ 5]); return;
                case 210: N.AddItem(_otherHidden[ 6]); return;
                case 177: N.AddItem(_otherHidden[ 7]); return;
                case 133: N.AddItem(_otherHidden[ 8]); return;
                case 295: N.AddItem(_otherHidden[ 9]); return;
                case  30: N.AddItem(_otherHidden[10]); return;
                case  39: N.AddItem(_otherHidden[11]); return;
                case  21: N.AddItem(_otherHidden[12]); return;
                case  31: N.AddItem(_otherHidden[13]); return;
                case  25: N.AddItem(_otherHidden[14]); return;
                case  14: N.AddItem(_otherHidden[15]); return;
                case 285: N.AddItem(_otherHidden[16]); return;
                case 353: N.AddItem(_otherHidden[17]); return;
                case 286: N.AddItem(_otherHidden[18]); return;
                case 356: N.AddItem(_otherHidden[19]); return;
                case 297: N.AddItem(_otherHidden[20]); return;
                case 298: N.AddItem(_otherHidden[21]); return;
            }
        }

        private void ApplyMaps(Node N)
        {
            if (N.Meta!.NodeType != NodeType.Regular)            
                return;

            switch(N.Meta!.Level)
            {
                case 40: N.AddItem(_maps[ 0]); return;
                case 45: N.AddItem(_maps[ 1]); return;
                case 50: N.AddItem(_maps[ 2]);
                         N.AddItem(_maps[ 3]);
                         N.AddItem(_maps[ 4]); return;
                case 55: N.AddItem(_maps[ 5]); return;
                case 60: N.AddItem(_maps[ 6]);
                         N.AddItem(_maps[ 7]); return;
                case 70: N.AddItem(_maps[ 8]);
                         N.AddItem(_maps[ 9]); return;
                case 80: N.AddItem(_maps[10]);
                         N.AddItem(_maps[11]); return;
            }
        }
        // @formatter:on

        private void ApplyDarkMatter(Node N)
        {
            if (N.Meta!.NodeType != NodeType.Unspoiled)
                return;
            if (N.Meta.Level != 50)
                return;

            N.AddItem(_darkMatterCluster);
            N.AddItem(_unaspectedCrystal);
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
