using System.Linq;
using Dalamud;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;

namespace GatherBuddy.Data
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
            var from = items.FromLanguage[(int) ClientLanguage.English];
            _maps = new[]
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

            _seeds = new[]
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

            _otherHidden = new[]
            { from["Grade 1 La Noscean Topsoil"]
            , from["Grade 1 Shroud Topsoil"    ]
            , from["Grade 1 Thanalan Topsoil"  ]
            , from["Grade 2 La Noscean Topsoil"]
            , from["Grade 2 Shroud Topsoil"    ]
            , from["Grade 2 Thanalan Topsoil"  ]
            , from["Black Limestone"           ]
            , from["Little Worm"               ]
            , from["Yafaem Wildgrass"          ]
            , from["Dark Chestnut"             ]
            , from["Firelight Seeds"           ]
            , from["Icelight Seeds"            ]
            , from["Windlight Seeds"           ]
            , from["Earthlight Seeds"          ]
            , from["Levinlight Seeds"          ]
            , from["Waterlight Seeds"          ]
            , from["Mythrite Ore"              ]
            , from["Hardsilver Ore"            ]
            , from["Titanium Ore"              ]
            , from["Birch Log"                 ]
            , from["Cyclops Onion"             ]
            , from["Emerald Beans"             ],
            };
            // @formatter:on
        }

        private void ApplySeeds(Node n)
        {
            foreach (var (seed, fruit) in _seeds)
            {
                if (n.Items!.HasItems(seed))
                    n.AddItem(fruit);
            }
        }

        // @formatter:off
        private void ApplyOtherHidden(Node n)
        {
            switch(n.Meta!.PointBaseId)
            {
                case 183: n.AddItem(_otherHidden[ 0]); return;
                case 163: n.AddItem(_otherHidden[ 1]); return;
                case 172: n.AddItem(_otherHidden[ 2]); return;
                case 193: n.AddItem(_otherHidden[ 3]); return;
                case 209: n.AddItem(_otherHidden[ 4]); return;
                case 151: n.AddItem(_otherHidden[ 5]); return;
                case 210: n.AddItem(_otherHidden[ 6]); return;
                case 177: n.AddItem(_otherHidden[ 7]); return;
                case 133: n.AddItem(_otherHidden[ 8]); return;
                case 295: n.AddItem(_otherHidden[ 9]); return;
                case  30: n.AddItem(_otherHidden[10]); return;
                case  39: n.AddItem(_otherHidden[11]); return;
                case  21: n.AddItem(_otherHidden[12]); return;
                case  31: n.AddItem(_otherHidden[13]); return;
                case  25: n.AddItem(_otherHidden[14]); return;
                case  14: n.AddItem(_otherHidden[15]); return;
                case 285: n.AddItem(_otherHidden[16]); return;
                case 353: n.AddItem(_otherHidden[17]); return;
                case 286: n.AddItem(_otherHidden[18]); return;
                case 356: n.AddItem(_otherHidden[19]); return;
                case 297: n.AddItem(_otherHidden[20]); return;
                case 298: n.AddItem(_otherHidden[21]); return;
            }
        }

        private void ApplyMaps(Node n)
        {
            if (n.Meta!.NodeType != NodeType.Regular)            
                return;

            switch(n.Meta!.PointBaseId)
            {
                case 49:
                case 180:
                case 20:
                case 137:
                case 141:
                case 140:
                    n.AddItem(_maps[0]);
                    return;
                case 46:
                case 185:
                case 186:
                case 142:
                case 143:
                    n.AddItem(_maps[1]);
                    return;
                case 146: 
                case 198: 
                case 294: 
                case 144: 
                case 197: 
                case 147: 
                case 43: 
                case 199: 
                case 149: 
                case 189: 
                case 284: 
                case 192: 
                case 210: 
                case 191: 
                case 209: 
                case 150:
                case 193:
                case 151:
                    n.AddItem(_maps[2]);
                    n.AddItem(_maps[3]);
                    n.AddItem(_maps[4]);
                    return;
                case 295: 
                case 287:
                case 297:
                case 286:
                case 298:
                case 296:
                case 288:
                case 285:
                    n.AddItem(_maps[5]);
                    return;
                case 391:
                case 356:
                case 354:
                case 358:
                case 352:
                case 359:
                case 361:
                case 360:
                case 300:
                case 351:
                case 353:
                case 355:
                    n.AddItem(_maps[6]);
                    n.AddItem(_maps[7]);
                    return;
                case 514:
                case 513:
                case 517:
                case 516:
                case 519:
                case 529:
                case 493:
                case 491:
                case 495:
                    n.AddItem(_maps[8]);
                    n.AddItem(_maps[9]);
                    return;
                case 621:
                case 620:
                case 625:
                case 623:
                case 596:
                case 648:
                case 598:
                case 600:
                case 602:
                    n.AddItem(_maps[10]);
                    n.AddItem(_maps[11]);
                    return;
            }
        }
        // @formatter:on

        private void ApplyDarkMatter(Node n)
        {
            if (n.Meta!.NodeType != NodeType.Unspoiled)
                return;
            if (n.Meta.Level != 50)
                return;

            n.AddItem(_darkMatterCluster);
            n.AddItem(_unaspectedCrystal);
        }

        public void SetHiddenItems(Node n)
        {
            ApplySeeds(n);
            ApplyMaps(n);
            ApplyDarkMatter(n);
            ApplyOtherHidden(n);
        }
    }
}
