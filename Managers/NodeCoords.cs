using Dalamud;
using System.Linq;

namespace Gathering
{
    public static class NodeCoords
    {
        public static void SetCoords(Node N, AetheryteManager aetherytes)
        {
            bool apply(string zone, string item, double x, double y, string A, bool flag = false)
            {
                if (zone == N.placeNameEN && N.items.HasItems(item) && N.initialPos == null)
                {
                    N.initialPos = new()
                    {
                        xCoord = (int)(x * 100.0 + 0.9),
                        yCoord = (int)(y * 100.0 + 0.9),
                        prefer = flag,
                        closestAetheryte = aetherytes.aetherytes.FirstOrDefault(a => a.nameList[ClientLanguage.English] == A)
                    };
                    return true;
                }
                return false;
            }

            const GatheringType Harvesting = GatheringType.Harvesting;
            const GatheringType Mining     = GatheringType.Mining;
            const GatheringType Logging    = GatheringType.Logging;
            const GatheringType Quarrying  = GatheringType.Quarrying;

            // Diadem special
            if (N.nodes.territory.nameList[ClientLanguage.English] == "The Diadem")
            {
                N.initialPos = new()
                {
                    xCoord = 0,
                    yCoord = 0,
                    prefer = true,
                    closestAetheryte = aetherytes.aetherytes.First(a => a.nameList[ClientLanguage.English] == "Foundation")
                };
                return;
            }

            #region Ephemeral
            if (N.meta.nodeType == NodeType.Ephemeral)
            {
                switch (N.meta.level + N.meta.gatheringType)
                {
                // Shadowbringers
                case 80 + Harvesting:
                    if (apply("Weed"         , "Bog Sage"      , 25.2, 29.8, "Fort Jobb")) return;
                    if (apply("The Wild Fete", "Sweet Marjoram", 26.9, 24.3, "Fanow"    )) return;
                    if (apply("Scree"        , "White Clay"    , 13.6, 13.8, "Tomra"    )) return;
                break;
                case 80 + Mining:
                    if (apply("The Wild Fete"                , "Gale Rock" , 22.3, 18.3, "Slitherbough")) return;
                    if (apply("The Church of the First Light", "Solarite"  , 36.8, 14.9, "Fort Jobb"   )) return;
                break;
                case 80 + Quarrying:
                    if (apply("Amity", "Thunder Rock", 22.3, 18.3, "Tomra")) return;
                break;

                // Stormblood
                case 70 + Harvesting:
                    if (apply("Doma"            , "Windtea Leaves", 22.0, 13.0, "The House of the Fierce")) return;
                    if (apply("Nhaama's Retreat", "Doman Yellow"  , 15.0, 29.0, "Dhoro Iloh"             )) return;
                break;
                case 70 + Logging:
                    if (apply("Abalathia's Skull", "Torreya Branch", 28.0, 10.0, "Porta Praetoria")) return;
                break;
                case 70 + Mining:
                    if (apply("The High Bank", "Almandine", 13.0, 17.0, "Porta Praetoria")) return;
                    if (apply("Onsal Hakair" , "Schorl"   , 29.0, 15.0, "The Dawn Throne")) return;
                break;
                case 70 + Quarrying:
                    if (apply("Unseen Spirits Laughing", "Perlite", 37.0, 19.0, "Namai")) return;
                break;

                // Heavensward
                case 60 + Harvesting:
                    if (apply("Red Rim"                   , "Highland Oregano", 16.0, 32.0, "Falcon's Nest")) return;
                    if (apply("Avalonia Fallen"           , "Granular Clay"   , 10.0, 32.0, "Anyx Trine"   )) return;
                    if (apply("Greensward"                , "Granular Clay"   , 20.0, 30.0, "Zenith"       )) return;
                    if (apply("Ok' Vundu Mok"             , "Highland Oregano", 23.0, 12.0, "Ok' Zundu"    )) return;
                    if (apply("Saint Mocianne's Arboretum", "Granular Clay"   , 13.0, 19.0, "Idyllshire"   )) return;
                break;
                case 60 + Quarrying:
                    if (apply("Black Iron Bridge"  , "Lightning Moraine", 21.0, 28.0, "Falcon's Nest")) return;
                    if (apply("Avalonia Fallen"    , "Fire Moraine"     , 17.0, 27.0, "Anyx Trine"   )) return;
                    if (apply("The Makers' Quarter", "Fire Moraine"     , 26.0, 24.0, "Idyllshire"   )) return;
                    if (apply("The Lost Landlord"  , "Fire Moraine"     , 29.0, 19.0, "Moghome"      )) return;
                    if (apply("Ok' Gundu"          , "Lightning Moraine", 34.0, 30.0, "Camp Cloudtop")) return;
                break;
                }
            }
            #endregion

            #region Unspoiled
            if (N.meta.nodeType == NodeType.Unspoiled)
            {
                switch(N.meta.level + N.meta.gatheringType)
                {
                case 80 + Harvesting:
                    if (apply("The Dragging Tail"    , "Russet Popoto"        , 19.0, 16.0, "Mord Souq"                )) return;
                    if (apply("Bowrest"              , "Mist Spinach"         , 34.0, 21.0, "Fanow"                    )) return;
                    if (apply("Inviolate Witness"    , "Ethereal Cocoon"      , 27.0, 10.0, "Fort Jobb"                )) return;
                    if (apply("White Oil Falls"      , "Tender Dill"          , 28.4, 21.1, "Stilltide"                , true)) return;
                    if (apply("The Nabaath Severance", "Duskblooms"           , 32.2, 33.4, "The Inn at Journey's Head", true)) return;
                    if (apply("The Isle of Consorts" , "Raindrop Cotton Boll" , 33.7, 13.2, "Fanow"                    , true)) return;
                break;
                case 80 + Logging:
                    if (apply("Sharptongue Drip"           , "Lemonette"       , 20.0, 27.0, "Wright"               )) return;
                    if (apply("Mjrl's Regret"              , "Sandalwood Log"  , 24.0, 36.0, "Slitherbough"         )) return;
                    if (apply("Anden's Airs"               , "Merbau Log"      , 36.5, 27.3, "Lydha Lran"           , true)) return;
                    if (apply("Garik"                      , "Silver Beech Log", 16.0, 10.9, "Twine"                , true)) return;
                    if (apply("The Hour of Certain Durance", "Wattle Petribark",  5.2, 26.5, "The Ostall Imperative", true)) return;
                break;
                case 80 + Mining:
                    if (apply("Ladle"                 , "Raw Triplite" , 20.0, 29.0, "The Inn at Journey's Head")) return;
                    if (apply("Weed"                  , "Raw Petalite" , 28.0, 33.0, "Fort Jobb"                )) return;
                    if (apply("The Caliban Gorge"     , "Raw Onyx"     , 16.0, 21.0, "The Macarenses Angle"     )) return;
                    if (apply("Saint Fathric's Temple", "Prismstone"   , 30.0, 20.0, "Wolekdorf"                )) return;
                    if (apply("Where the Dry Return"  , "Tungsten Ore" , 32.0,  7.0, "The Ondo Cups"            )) return;
                    if (apply("The Isle of Ken"       , "Brashgold Ore",  4.7, 33.9, "The Ostall Imperative"    , true)) return;
                    if (apply("The Husk"              , "Dolomite"     ,  7.5, 30.1, "Slitherbough"             , true)) return;
                    if (apply("The Duergar's Tewel"   , "Hard Water"   , 36.1, 12.2, "Tomra"                    , true)) return;
                break;
                case 80 + Quarrying:
                    if (apply("Purpure"          , "Purpure Shell Chip"      , 34.4, 31.3, "The Macarenses Angle", true)) return;
                    if (apply("Mount Biran Mines", "Ashen Alumen"            , 20.1,  8.6, "Twine"               , true)) return;
                    if (apply("Voeburtenburg"    , "Solstice Stone"          , 35.6,  8.7, "Wolekdorf"           , true)) return;
                break;

                case 75 + Harvesting:
                    if (apply("Phisor Lran", "Broad Beans"         , 24.0, 36.0, "Lydha Lran")) return;
                    if (apply("Embrasure"  , "Peppermint"          , 26.0, 20.0, "Fort Jobb" )) return;
                break;
                case 75 + Mining:
                    if (apply("Sextuplet Shallow"              , "Raw Diaspore"           , 26.0, 13.0, "Wolekdorf")) return;
                    if (apply("Cleric"                         , "Raw Lazurite"           , 28.0, 33.0, "Fort Jobb")) return;
                break;

                case 70 + Harvesting:
                    if (apply("Rustrock"                    , "Hallowed Basil"     , 23.0, 16.0, "Ala Gannha"             )) return;
                    if (apply("Valley of the Fallen Rainbow", "Lotus Root"         , 28.0,  7.0, "The House of the Fierce")) return;
                    if (apply("Abalathia's Skull"           , "Jhammel Ginger"     ,  8.0,  8.0, "Porta Praetoria"        )) return;
                    if (apply("Onsal Hakair"                , "Rhea"               , 20.0,  8.0, "The Dawn Throne"        )) return;
                    if (apply("Mount Yorn"                  , "Hingan Flax"        , 24.0, 36.0, "Ala Ghiri"              )) return;
                    if (apply("The Glittering Basin"        , "Yanxian Cotton Boll", 28.0, 35.0, "Namai"                  )) return;
                break;
                case 70 + Logging:
                    if (apply("Onsal Hakair" , "Othardian Plum"  , 27.0, 17.0, "The Dawn Throne")) return;
                    if (apply("The High Bank", "Torreya Log"     , 11.0, 13.0, "Porta Praetoria")) return;
                    if (apply("Pike Falls"   , "Black Willow Log", 15.0, 21.0, "Castrum Oriens" )) return;
                    if (apply("Rustrock"     , "Urunday Log"     , 32.0, 10.0, "Ala Gannha"     )) return;
                    if (apply("Venmont Yards", "White Oak Branch", 12.0, 29.0, "Wright"         )) return;
                break;
                case 70 + Mining:
                    if (apply("Rustrock"                    , "Raw Rhodonite"    , 26.0, 12.0, "Ala Gannha")) return;
                    if (apply("Valley of the Fallen Rainbow", "Raw Imperial Jade", 29.0,  9.0, "The House of the Fierce")) return;
                    if (apply("The Towering Still"          , "Raw Azurite"      ,  5.0, 29.0, "Dhoro Iloh")) return;
                    if (apply("Wightrock"                   , "Chromite Ore"     , 16.0, 33.9, "Ala Ghiri")) return;
                    if (apply("Doma"                        , "Palladium Ore"    , 20.5, 10.4, "The House of the Fierce")) return;
                    if (apply("East Othard Coastline"       , "Nightsteel Ore"   , 11.0, 23.0, "Onokoro")) return;
                    if (apply("Nhaama's Retreat"            , "Silvergrace Ore"  , 23.0, 36.0, "Reunion")) return;
                    if (apply("Virdjala"                    , "Gyr Abanian Ore"  , 31.0, 27.0, "The Peering Stones")) return;
                    if (apply("Loch Seld"                   , "Evergleam Ore"    , 22.0, 13.0, "Porta Praetoria")) return;
                    if (apply("Governor's Row"              , "Raw Hematite"     , 33.0, 23.0, "Stilltide")) return;
                break;
                case 70 + Quarrying:
                    if (apply("Loch Seld", "Ala Mhigan Salt Crystal", 21.0, 29.0, "The Ala Mhigan Quarter")) return;
                break;

                case 65 + Logging:
                    if (apply("The Glittering Basin", "Bamboo Shoot", 28.0, 25.0, "Namai")) return;
                break;
                case 65 + Mining:
                    if (apply("East Othard Coastline", "Raw Star Spinel"                   , 15.0,  4.5, "Onokoro"           )) return;
                break;

                case 60 + Harvesting:
                    if (apply("Delta Quadrant"     , "Star Cotton Boll",  9.0, 31.0, "Helix"        )) return;
                    if (apply("Chocobo Forest"     , "Noble Sage"      , 33.0, 30.0, "Tailfeather"  )) return;
                    if (apply("Twinpools"          , "Chysahl Greens"  ,  8.0,  9.0, "Tailfeather"  )) return;
                    if (apply("Twinpools"          , "Vanilla Beans"   , 23.0, 21.0, "Falcon's Nest")) return;
                    if (apply("Greensward"         , "Seventh Heaven"  , 17.0, 36.0, "Zenith"       )) return;
                    if (apply("The Makers' Quarter", "Snurbleberry"    , 39.0, 26.0, "Anyx Trine"   )) return;
                break;
                case 60 + Logging:
                    if (apply("The Answering Quarter", "Teak Log"                  ,  6.0, 28.0, "Idyllshire"    )) return;
                    if (apply("Greensward"           , "Brown Mushroom"            , 12.0, 37.0, "Zenith"        )) return;
                    if (apply("Vundu Ok' Bendu"      , "Heavens Lemon"             , 35.0, 23.0, "Camp Cloudtop" )) return;
                    if (apply("The Ruling Quarter"   , "Cloud Banana"              , 19.0, 36.0, "Idyllshire"    )) return;
                    if (apply("The Gauntlet"         , "Honeydew Almonds"          , 24.0,  6.0, "Ok' Zundu"     )) return;
                    if (apply("Beta Quadrant"        , "Wattle Bark"               , 22.0, 10.0, "Helix"         )) return;
                    if (apply("Ohl Tahn"             , "Old-growth Camphorwood Log", 11.0, 10.0, "Zenith"        )) return;
                    if (apply("East End"             , "Beech Branch"              , 11.0, 18.0, "Castrum Oriens")) return;
                break;
                case 60 + Mining:
                    if (apply("Twinpools"         , "Tungstite"       , 10.0,  9.0, "Tailfeather"   )) return;
                    if (apply("Alpha Quadrant"    , "Luminium Ore"    ,  5.0, 17.0, "Helix"         )) return;
                    if (apply("The Ruling Quarter", "Light Kidney Ore", 34.0, 30.0, "Anyx Trine"    )) return;
                    if (apply("Beta Quadrant"     , "Adamantite Ore"  , 24.0,  6.0, "Helix"         )) return;
                    if (apply("Greensward"        , "Aurum Regis Ore" , 11.0, 38.0, "Zenith"        )) return;
                    if (apply("The Habisphere"    , "Red Alumen"      , 35.0, 16.0, "Helix"         )) return;
                    if (apply("The Gauntlet"      , "Smithsonite Ore" , 38.0, 15.0, "Ok' Zundu"     )) return;
                    if (apply("The Striped Hills" , "Raw Triphane"    , 25.0,  8.0, "Castrum Oriens")) return;
                break;
                case 60 + Quarrying:
                    if (apply("The Ruling Quarter"   , "Zeolite Ore"         , 13.0, 31.0, "Idyllshire"   )) return;
                    if (apply("The Gauntlet"         , "Abalathian Rock Salt",  7.0,  7.0, "Ok' Zundu"    )) return;
                    if (apply("Riversmeet"           , "Astral Moraine"      , 37.0, 16.0, "Falcon's Nest")) return;
                    if (apply("Voor Sian Siran"      , "Sun Mica"            , 35.0, 39.0, "Camp Cloudtop")) return;
                    if (apply("The Answering Quarter", "Blue Quartz"         , 11.0, 16.0, "Idyllshire"   )) return;
                break;
                case 55 + Harvesting:
                    if (apply("Riversmeet"    , "Pearl Sprouts"               , 31.0, 20.0, "Falcon's Nest")) return;
                break;
                case 55 + Logging:
                    if (apply("Ohl Tahn", "Porcini", 24.0,  6.0, "Zenith")) return;
                break;
                case 55 + Mining:
                    if (apply("The Smoldering Wastes", "Pyrite", 26.0, 17.0, "Tailfeather")) return;
                break;
                case 55 + Quarrying:
                    if (apply("Eil Tohm", "Green Quartz"           , 33.0, 22.0, "Moghome"      )) return;
                break;

                case 50 + Harvesting:
                    if (apply("Nine Ivies"       , "Silkworm Cocoon"         , 22.0, 26.0, "The Hawthorne Hut" )) return;
                    if (apply("The Bramble Patch", "Trillium"                , 17.0, 19.0, "The Hawthorne Hut" )) return;
                    if (apply("The Honey Yard"   , "Trillium Bulb"           , 13.0, 23.0, "The Hawthorne Hut" )) return;
                    if (apply("Bloodshore"       , "Honey Lemon"             , 26.0, 32.0, "Costa del Sol"     )) return;
                    if (apply("Raincatcher Gully", "Dzemael Tomato"          , 18.0, 26.0, "Wineport"          )) return;
                    if (apply("Drybone"          , "Black Truffle"           , 12.0, 16.0, "Camp Drybone"      )) return;
                    if (apply("Sorrel Haven"     , "Shroud Tea Leaves"       , 16.0, 20.0, "Bentbranch Meadows")) return;
                    if (apply("Quarterstone"     , "La Noscean Leek"         , 34.0, 28.0, "Swiftperch"        )) return;
                    if (apply("Cedarwood"        , "Young Cieldalaes Spinach", 32.0, 11.0, "Moraby Drydocks"   )) return;
                    if (apply("Nine Ivies"       , "Rosemary"                , 22.0, 30.0, "The Hawthorne Hut" )) return;
                    if (apply("Chocobo Forest"   , "Old World Fig"           , 26.0, 12.0, "Tailfeather"       )) return;
                break;
                case 50 + Logging:
                    if (apply("Providence Point", "Spruce Log"           , 27.0, 12.0, "Camp Dragonhead"   )) return;
                    if (apply("Dragonhead"      , "Vampire Plant"        , 27.0, 24.0, "Camp Dragonhead"   )) return;
                    if (apply("Whitebrim"       , "Thavnairian Mistletoe",  8.0, 13.0, "Camp Dragonhead"   )) return;
                    if (apply("Bloodshore"      , "Prickly Pineapple"    , 30.0, 26.0, "Costa del Sol"     )) return;
                    if (apply("North Silvertear", "Fire Cluster"         , 32.0, 11.0, "Revenant's Toll"   )) return;
                    if (apply("Alder Springs"   , "Scarlet Sap"          , 18.0, 26.0, "Fallgourd Float"   )) return;
                    if (apply("Summerford"      , "Apricot"              , 19.0, 16.0, "Summerford Farms"  )) return;
                    if (apply("Bronze Lake"     , "Blood Orange"         , 28.0, 25.0, "Camp Bronze Lake"  )) return;
                    if (apply("Upper Paths"     , "Fragrant Log"         , 18.0, 23.0, "Camp Tranquil"     )) return;
                    if (apply("Greentear"       , "Redolent Log"         , 30.0, 19.0, "Bentbranch Meadows")) return;
                    if (apply("Zephyr Drift"    , "Ebony Log"            , 23.2, 26.3, "Summerford Farms"  )) return;
                    if (apply("Bentbranch"      , "Cypress Log"          , 25.0, 29.0, "Bentbranch Meadows")) return;
                break;
                case 50 + Mining:
                    if (apply("Dragonhead"      , "Darksteel Ore"      , 27.6, 19.8, "Camp Dragonhead"          )) return;
                    if (apply("The Burning Wall", "Gold Ore"           , 28.0, 22.0, "Camp Drybone"             )) return;
                    if (apply("Raubahn's Push"  , "Ferberite"          , 16.0, 19.0, "Ceruleum Processing Plant")) return;
                    if (apply("Black Brush"     , "Native Gold"        , 24.0, 16.0, "Black Brush Station"      )) return;
                    if (apply("Moraby Bay"      , "Raw Ruby"           , 23.0, 21.0, "Moraby Drydocks"          )) return;
                    if (apply("Broken Water"    , "Platinum Ore"       , 19.0,  8.0, "Little Ala Mhigo"         )) return;
                    if (apply("Bluefog"         , "Virgin Basilisk Egg", 24.0, 26.0, "Ceruleum Processing Plant")) return;
                    if (apply("Riversmeet"      , "Yellow Copper Ore"  , 30.0, 23.0, "Falcon's Nest"            )) return;
                break;
                case 50 + Quarrying:
                    if (apply("Raincatcher Gully", "Volcanic Rock Salt"        , 21.0, 32.0, "Wineport"        )) return;
                    if (apply("Dragonhead"       , "Astral Rock"               , 23.0, 23.0, "Camp Dragonhead" )) return;
                    if (apply("Wellwick Wood"    , "Gold Sand"                 , 25.0, 22.0, "Camp Drybone"    )) return;
                    if (apply("North Silvertear" , "Fire Cluster"              , 27.0, 10.0, "Revenant's Toll" )) return;
                    if (apply("Zephyr Drift"     , "Grade 3 La Noscean Topsoil", 25.0, 27.0, "Summerford Farms")) return;
                    if (apply("Lower Paths"      , "Grade 3 Shroud Topsoil"    , 16.0, 31.0, "Camp Tranquil"   )) return;
                    if (apply("Hammerlea"        , "Grade 3 Thanalan Topsoil"  , 18.0, 27.0, "Horizon"         )) return;
                    if (apply("Wellwick Wood"    , "Antumbral Rock"            , 26.0, 19.0, "Camp Drybone"    )) return;
                    if (apply("Raincatcher Gully", "Pumice"                    , 17.0, 26.0, "Wineport"        )) return;
                break;
                }
            }
            #endregion

            #region Regular Nodes
            if (N.meta.nodeType == NodeType.Regular)
            {
                switch(N.meta.level + N.meta.gatheringType)
                {
                case 80 + Harvesting:
                    if (apply("Ashpool"                        , "Oddly Specific Amber"               , 10.6, 18.5, "Tailfeather"              )) return;
                    if (apply("Scree"                          , "Black Aethersand"                   , 12.0, 14.0, "Tomra"                    )) return;
                    if (apply("The Whale's Crown"              , "Oddly Specific Latex"               , 29.2, 23.4, "Ok' Zundu"                )) return;
                    if (apply("The Wild Fete"                  , "Sweet Alyssum"                      , 24.0, 28.0, "Slitherbough"             )) return;
                    if (apply("Weed"                           , "Tiger Lily"                         , 23.0, 34.0, "Fort Jobb"                )) return;
                    if (apply("The Fields of Amber"            , "Rarefied Night Pepper"              , 26.7, 27.5, "The Inn at Journey's Head")) return;
                break;
                case 80 + Logging:
                    if (apply("Four Arms"                      , "Oddly Specific Amber"               , 25.0, 28.2, "Moghome"                  )) return;
                    if (apply("Hare Among Giants"              , "Harcot"                             ,  8.0,  9.0, "The Ostall Imperative"    )) return;
                    if (apply("Ladle"                          , "Amber Cloves"                       , 15.0, 30.0, "Twine"                    )) return;
                    if (apply("Whilom River"                   , "Oddly Specific Latex"               , 29.7, 18.3, "Tailfeather"              )) return;
                    if (apply("Valley of the Fallen Rainbow"   , "Oddly Specific Dark Chestnut Log"   , 28.3,  9.5, "The House of the Fierce"  )) return;
                    if (apply("The Destroyer"                  , "Oddly Specific Dark Chestnut Log"   , 20.3, 18.0, "Porta Praetoria"          )) return;
                    if (apply("The Pappus Tree"                , "Oddly Specific Primordial Log"      ,  7.0, 34.2, "Helix"                    )) return;
                    if (apply("Hyperstellar Downconverter"     , "Oddly Specific Primordial Log"      ,  7.1, 15.9, "Helix"                    )) return;
                    if (apply("Antithesis"                     , "Oddly Delicate Feather"             , 11.6, 26.8, "Helix"                    )) return;
                break;
                case 80 + Mining:
                    if (apply("Mourn"                          , "Oddly Specific Dark Matter"         , 17.1, 10.8, "Anyx Trine"               )) return;
                    if (apply("Ok' Vundu Vana"                 , "Oddly Specific Obsidian"            , 26.5, 23.1, "Ok' Zundu"                )) return;
                    if (apply("Red Rim"                        , "Oddly Specific Obsidian"            , 14.8, 30.6, "Falcon's Nest"            )) return;
                    if (apply("The Church of the First Light"  , "Underground Spring Water"           , 35.0, 16.0, "Fort Jobb"                )) return;
                    if (apply("The Wild Fete"                  , "Extra Effervescent Water"           , 23.0, 28.0, "Slitherbough"             )) return;
                    if (apply("The Dragon's Struggle"          , "Oddly Specific Schorl"              , 12.5,  9.7, "The House of the Fierce"  )) return;
                    if (apply("The Pauper's Lode"              , "Oddly Specific Schorl"              ,  4.0, 26.7, "Porta Praetoria"          )) return;
                    if (apply("The Aqueduct"                   , "Oddly Specific Primordial Ore"      , 11.0, 35.2, "Helix"                    )) return;
                    if (apply("Cooling Station"                , "Oddly Specific Primordial Ore"      , 14.0,  9.8, "Helix"                    )) return;
                    if (apply("Gamma Quadrant"                 , "Oddly Delicate Adamantite Ore"      , 36.2, 27.8, "Helix"                    )) return;
                break;
                case 80 + Quarrying:
                    if (apply("Amity"                          , "Black Aethersand"                   , 22.0, 16.0, "Tomra"                    )) return;
                    if (apply("Mount Biran Mines"              , "Titancopper Sand"                   , 15.0, 12.0, "Twine"                    )) return;
                    if (apply("The Daggers"                    , "Oddly Specific Dark Matter"         , 30.9, 28.2, "Anyx Trine"               )) return;
                    if (apply("The Citia Swamps"               , "Rarefied Manasilver Sand"           , 16.0, 28.8, "Slitherbough"             )) return;
                break;
                case 75 + Harvesting:
                    if (apply("Quickspill Delta"               , "Gathering Tool Paraphernalia"       , 13.9, 32.7, "Idyllshire"               )) return;
                    if (apply("Seagazer"                       , "Upland Wheat"                       , 16.0, 36.0, "Wright"                   )) return;
                    if (apply("The Deliberating Doll"          , "Royal Grapes"                       , 16.0, 29.0, "Slitherbough"             )) return;
                    if (apply("The Forest of the Lost Shepherd", "Animal Droppings"                   , 16.0, 23.0, "The Ostall Imperative"    )) return;
                    if (apply("The Nidifice"                   , "Crafting Tool Paraphernalia"        , 35.9, 36.0, "Camp Cloudtop"            )) return;
                    if (apply("The Woolen Way"                 , "Megafauna Leftovers"                , 11.0, 24.0, "Lydha Lran"               )) return;
                    if (apply("Yuzuka Manor"                   , "Printing Paraphernalia"             , 17.2, 30.8, "Namai"                    )) return;
                    if (apply("Laxan Loft"                     , "Rarefied Bright Flax"               , 23.4, 13.1, "Fort Jobb"                )) return;
                break;
                case 75 + Logging:
                    if (apply("Hidden Tear"                    , "Handpicked Ingredients"             , 35.0,  8.5, "Ala Gannha"               )) return;
                    if (apply("The Bright Cliff"               , "White Oak Log"                      , 27.0, 22.0, "Wright"                   )) return;
                    if (apply("The Chiliad"                    , "Frantoio"                           , 36.0, 24.0, "Fort Jobb"                )) return;
                    if (apply("The Church at Dammroen Field"   , "Pixie Apple"                        , 30.0,  5.0, "Wolekdorf"                )) return;
                    if (apply("The Hundred Throes"             , "Weaving Paraphernalia"              , 34.3, 11.6, "Tailfeather"              )) return;
                    if (apply("Woven Oath"                     , "Gianthive Chip"                     , 12.0, 20.0, "Slitherbough"             )) return;
                break;
                case 75 + Mining:
                    if (apply("The Bookman's Shelves"          , "Highland Spring Water"              ,  8.0, 20.0, "Lydha Lran"               )) return;
                    if (apply("The Queen's Gardens"            , "Printing Paraphernalia"             , 29.7, 16.2, "The Ala Mhigan Quarter"   )) return;
                    if (apply("The Shattered Back"             , "Crafting Tool Paraphernalia"        , 28.9,  7.5, "Ok' Zundu"                )) return;
                    if (apply("Weed"                           , "Animal Droppings"                   , 26.0, 34.0, "Fort Jobb"                )) return;
                    if (apply("Weston Waters"                  , "Weaving Paraphernalia"              ,  8.0,  7.8, "Zenith"                   )) return;
                    if (apply("The Forest of the Lost Shepherd", "Rarefied Bluespirit Ore"            , 27.4, 21.6, "Fort Jobb"                )) return;
                break;
                case 75 + Quarrying:
                    if (apply("Lozatl's Conquest"              , "Manasilver Sand"                    , 17.0, 19.0, "Slitherbough"             )) return;
                    if (apply("Slowroad"                       , "Truegold Sand"                      , 21.0, 30.0, "Wright"                   )) return;
                    if (apply("The Convictory"                 , "Gathering Tool Paraphernalia"       , 13.4, 23.9, "Falcon's Nest"            )) return;
                    if (apply("The Kobayashi Maru"             , "Handpicked Ingredients"             , 39.4,  4.0, "Onokoro"                  )) return;
                break;
                case 70 + Harvesting:
                    if (apply("Doma"                           , "Daikon Radish"                      , 22.0, 10.0, "The House of the Fierce"  )) return;
                    if (apply("Mount Yorn"                     , "Gyr Abanian Carrot"                 , 26.0, 27.0, "Ala Ghiri"                )) return;
                    if (apply("Nhaama's Retreat"               , "Sun Cabbage"                        , 14.0, 26.0, "The Dawn Throne"          )) return;
                    if (apply("Rasen Kaikyo"                   , "Ruby Tide Kelp"                     , 11.0, 13.0, "Onokoro"                  )) return;
                    if (apply("Snitch"                         , "Animal Trace"                       , 31.0, 15.0, "Mord Souq"                )) return;
                    if (apply("Thysm Lran"                     , "Clinquant Stones"                   , 31.3, 34.2, "Lydha Lran"               )) return;
                break;
                case 70 + Logging:
                    if (apply("Abalathia's Skull"              , "Zelkova Log"                        , 26.0,  9.0, "Porta Praetoria"          )) return;
                    if (apply("Dimwold"                        , "Persimmon Leaf"                     , 10.0, 30.0, "Castrum Oriens"           )) return;
                    if (apply("Stonegazer"                     , "Raven Coal"                         , 17.2, 24.4, "Wright"                   )) return;
                break;
                case 70 + Mining:
                    if (apply("The High Bank"                  , "Molybdenum Ore"                     , 10.0, 18.0, "Porta Praetoria"          )) return;
                    if (apply("Thysm Lran"                     , "Clinquant Stones"                   , 33.3, 31.5, "Lydha Lran"               )) return;
                    if (apply("Unseen Spirits Laughing"        , "Durium Ore"                         , 36.0, 19.0, "Namai"                    )) return;
                break;
                case 70 + Quarrying:
                    if (apply("Onsal Hakair"                   , "Durium Sand"                        , 29.0, 15.0, "The Dawn Throne"          )) return;
                    if (apply("Shadow Fault"                   , "Raven Coal"                         , 11.8, 26.3, "Wright"                   )) return;
                    if (apply("The Inn at Journey's Head"      , "Animal Trace"                       , 30.0, 25.0, "The Inn at Journey's Head")) return;
                break;
                case 65 + Harvesting:
                    if (apply("Dimwold"                        , "Holy Basil"                         , 11.0, 26.0, "Castrum Oriens"           )) return;
                    if (apply("East Othard Coastline"          , "Soybeans"                           ,  7.0,  8.0, "Onokoro"                  )) return;
                    if (apply("Gorgagne Holding"               , "Phial of Thermal Fluid"             , 32.0, 15.0, "Falcon's Nest"            )) return;
                    if (apply("Ohl Tahn"                       , "Cloudkin Feather"                   ,  8.0, 15.0, "Zenith"                   )) return;
                    if (apply("Rasen Kaikyo"                   , "Gem Algae"                          , 26.0, 19.0, "Tamamizu"                 )) return;
                    if (apply("The Last Forest"                , "Mountain Popoto"                    , 13.0, 11.0, "Ala Gannha"               )) return;
                    if (apply("Mirage Creek"                   , "Rarefied Bloodhemp"                 , 29.0, 11.0, "Rhalgr's Reach"           )) return;
                break;
                case 65 + Logging:
                    if (apply("East End"                       , "Beech Log"                          , 10.0, 16.0, "Castrum Oriens"           )) return;
                    if (apply("Onokoro"                        , "Larch Log"                          , 20.0,  9.0, "Onokoro"                  )) return;
                    if (apply("The Heron's Flight"             , "Pine Resin"                         , 36.0, 15.0, "Namai"                    )) return;
                break;
                case 65 + Mining:
                    if (apply("Hells' Lid"                     , "Koppranickel Ore"                   , 25.0, 35.0, "Kugane"                   )) return;
                    if (apply("The Bed of Bones"               , "Phial of Thermal Fluid"             , 22.0, 35.0, "Falcon's Nest"            )) return;
                    if (apply("The Gensui Chain"               , "Crescent Spring Water"              , 33.0, 22.0, "Namai"                    )) return;
                    if (apply("The Striped Hills"              , "Gyr Abanian Mineral Water"          , 23.0, 13.0, "Castrum Oriens"           )) return;
                    if (apply("Pike Falls"                     , "Rarefied Gyr Abanian Mineral Water" , 18.1, 22.8, "The Peering Stones"       )) return;
                break;
                case 65 + Quarrying:
                    if (apply("Landlord Colony"                , "Cloudkin Feather"                   , 34.0, 25.0, "Moghome"                  )) return;
                    if (apply("Rasen Kaikyo"                   , "Diatomite"                          , 14.0, 16.0, "Onokoro"                  )) return;
                    if (apply("Rustrock"                       , "Stiperstone"                        , 21.0, 13.0, "Ala Gannha"               )) return;
                break;
                case 60 + Harvesting:
                    if (apply("Avalonia Fallen"                , "Coneflower"                         , 10.0, 33.0, "Anyx Trine"               )) return;
                    if (apply("East End"                       , "Bloodhemp"                          , 16.0,  7.0, "Castrum Oriens"           )) return;
                    if (apply("Four Arms"                      , "Dandelion"                          , 20.0, 31.0, "Moghome"                  )) return;
                    if (apply("The Answering Quarter"          , "Cow Bitter"                         , 14.0, 19.0, "Idyllshire"               )) return;
                    if (apply("The Blue Window"                , "Sesame Seeds"                       , 23.0, 10.0, "Ok' Zundu"                )) return;
                    if (apply("The Isle of Bekko"              , "Shishu Koban"                       , 37.0, 18.0, "Onokoro"                  )) return;
                    if (apply("The Last Forest"                , "Peaks Pigment"                      , 13.0,  7.0, "Ala Gannha"               )) return;
                    if (apply("Twinpools"                      , "Rue"                                , 12.0, 14.0, "Tailfeather"              )) return;
                break;
                case 60 + Logging:
                    if (apply("Eil Tohm"                       , "Camphorwood Log"                    , 31.0, 30.0, "Moghome"                  )) return;
                    if (apply("Voor Sian Siran"                , "Birch Branch"                       , 25.0, 34.0, "Camp Cloudtop"            )) return;
                break;
                case 60 + Mining:
                    if (apply("Avalonia Fallen"                , "Raw Opal"                           , 20.0, 26.0, "Tailfeather"              )) return;
                    if (apply("East End"                       , "Gyr Abanian Alumen"                 , 15.0, 12.0, "Castrum Oriens"           )) return;
                    if (apply("Red Rim"                        , "Raw Citrine"                        , 23.0, 30.0, "Falcon's Nest"            )) return;
                    if (apply("Sleeping Stones"                , "Peaks Pigment"                      , 35.0, 10.0, "Ala Gannha"               )) return;
                    if (apply("The Makers' Quarter"            , "Raw Chrysolite"                     , 27.0, 24.0, "Idyllshire"               )) return;
                    if (apply("The Turquoise Trench"           , "Shishu Koban"                       , 17.0, 33.0, "Tamamizu"                 )) return;
                    if (apply("Voor Sian Siran"                , "Abalathian Spring Water"            , 33.0, 31.0, "Camp Cloudtop"            )) return;
                break;
                case 60 + Quarrying:
                    if (apply("Landlord Colony"                , "Hardsilver Sand"                    , 29.0, 18.0, "Moghome"                  )) return;
                break;
                case 55 + Harvesting:
                    if (apply("Chocobo Forest"                 , "Highland Wheat"                     , 36.0, 20.0, "Tailfeather"              )) return;
                    if (apply("Landlord Colony"                , "Magma Beet"                         , 20.0, 21.0, "Zenith"                   )) return;
                    if (apply("Twinpools"                      , "Rainbow Cotton Boll"                , 17.0, 16.0, "Falcon's Nest"            )) return;
                    if (apply("Coerthas River"                 , "Rarefied Rainbow Cotton Boll"       , 24.7, 14.3, "Falcon's Nest"            )) return;
                break;
                case 55 + Logging:
                    if (apply("The Smoldering Wastes"          , "Dark Chestnut Log"                  , 25.0, 25.0, "Tailfeather"              )) return;
                break;
                case 55 + Mining:
                    if (apply("Chocobo Forest"                 , "Raw Star Ruby"                      , 30.0, 16.0, "Tailfeather"              )) return;
                    if (apply("Gorgagne Holding"               , "Raw Larimar"                        , 31.0, 12.0, "Falcon's Nest"            )) return;
                    if (apply("The Smoldering Wastes"          , "Raw Agate"                          , 27.0, 31.0, "Tailfeather"              )) return;
                break;
                case 55 + Quarrying:
                    if (apply("Twinpools"                      , "Mythrite Sand"                      , 16.0, 12.0, "Tailfeather"              )) return;
                    if (apply("Hemlock"                        , "Rarefied Mythrite Sand"             , 35.9, 23.4, "Falcon's Nest"            )) return;
                break;
                case 50 + Harvesting:
                    if (apply("Sagolii Desert"                 , "Thanalan Tea Leaves"                , 13.0, 31.0, "Forgotten Springs"        )) return;
                break;
                case 50 + Logging:
                    if (apply("Raincatcher Gully"              , "Water Shard"                        , 17.0, 32.0, "Wineport"                 )) return;
                    if (apply("Riversmeet"                     , "Cedar Log"                          , 30.0, 32.0, "Falcon's Nest"            )) return;
                    if (apply("Summerford"                     , "Fire Shard"                         , 19.0, 21.0, "Summerford Farms"         )) return;
                    if (apply("The Bramble Patch"              , "Rosewood Log"                       , 16.0, 23.0, "The Hawthorne Hut"        )) return;
                    if (apply("The Clutch"                     , "Lightning Shard"                    , 29.0, 19.0, "Black Brush Station"      )) return;
                break;
                case 50 + Mining:
                    if (apply("Bluefog"                        , "Cobalt Ore"                         , 22.0, 24.0, "Ceruleum Processing Plant")) return;
                    if (apply("Riversmeet"                     , "Dragon Obsidian"                    , 28.0, 27.0, "Falcon's Nest"            )) return;
                break;
                case 50 + Quarrying:
                    if (apply("Quarterstone"                   , "Ice Shard"                          , 34.0, 28.0, "Swiftperch"               )) return;
                    if (apply("The Bramble Patch"              , "Wind Shard"                         , 18.0, 24.0, "The Hawthorne Hut"        )) return;
                    if (apply("The Gods' Grip"                 , "Earth Shard"                        , 22.0, 34.0, "Moraby Drydocks"          )) return;
                    if (apply("The Silver Bazaar"              , "Water Shard"                        , 18.0, 28.0, "Horizon"                  )) return;
                break;
                case 45 + Harvesting:
                    if (apply("Bronze Lake"                    , "Sagolii Sage"                       , 35.0, 24.0, "Camp Bronze Lake"         )) return;
                break;
                case 45 + Logging:
                    if (apply("Whitebrim"                      , "Mirror Apple"                       , 23.0, 17.0, "Camp Dragonhead"          )) return;
                break;
                case 45 + Mining:
                    if (apply("Bronze Lake"                    , "Raw Turquoise"                      , 30.0, 25.0, "Camp Bronze Lake"         )) return;
                    if (apply("Drybone"                        , "Raw Amber"                          , 12.0, 19.0, "Camp Drybone"             )) return;
                break;
                case 45 + Quarrying:
                    if (apply("Bronze Lake"                    , "Electrum Sand"                      , 28.0, 22.0, "Camp Bronze Lake"         )) return;
                break;
                case 40 + Harvesting:
                    if (apply("Lower Paths"                    , "Thyme"                              , 21.0, 29.0, "Camp Tranquil"            )) return;
                    if (apply("Raincatcher Gully"              , "Mugwort"                            , 21.0, 29.0, "Wineport"                 )) return;
                break;
                case 40 + Logging:
                    if (apply("Raincatcher Gully"              , "Iron Acorn"                         , 19.0, 25.0, "Wineport"                 )) return;
                break;
                case 40 + Mining:
                    if (apply("Dragonhead"                     , "Raw Zircon"                         , 24.0, 19.0, "Camp Dragonhead"          )) return;
                    if (apply("Urth's Gift"                    , "Raw Spinel"                         , 28.0, 22.0, "Quarrymill"               )) return;
                break;
                case 40 + Quarrying:
                    if (apply("Bluefog"                        , "Grenade Ash"                        , 21.0, 28.0, "Ceruleum Processing Plant")) return;
                break;
                case 35 + Harvesting:
                    if (apply("Bloodshore"                     , "Midland Basil"                      , 26.0, 30.0, "Costa del Sol"            )) return;
                    if (apply("Broken Water"                   , "Aloe"                               , 20.0,  7.0, "Little Ala Mhigo"         )) return;
                    if (apply("Lower Paths"                    , "White Truffle"                      , 17.0, 28.0, "Camp Tranquil"            )) return;
                break;
                case 35 + Logging:
                    if (apply("Lower Paths"                    , "Oak Log"                            , 16.0, 30.0, "Camp Tranquil"            )) return;
                break;
                case 35 + Mining:
                    if (apply("Bloodshore"                     , "Raw Aquamarine"                     , 28.0, 27.0, "Costa del Sol"            )) return;
                    if (apply("Sagolii Desert"                 , "Raw Heliodor"                       , 25.0, 41.0, "Forgotten Springs"        )) return;
                    if (apply("Sorrel Haven"                   , "Raw Peridot"                        , 14.0, 21.0, "Bentbranch Meadows"       )) return;
                break;
                case 35 + Quarrying:
                    if (apply("The Red Labyrinth"              , "Mythril Sand"                       , 17.0, 18.0, "Little Ala Mhigo"         )) return;
                break;
                case 30 + Harvesting:
                    if (apply("Alder Springs"                  , "Jade Peas"                          , 22.0, 25.0, "Fallgourd Float"          )) return;
                break;
                case 30 + Logging:
                    if (apply("Bentbranch"                     , "Green Pigment"                      , 24.0, 30.0, "Bentbranch Meadows"       )) return;
                    if (apply("Bloodshore"                     , "Blue Pigment"                       , 28.0, 33.0, "Costa del Sol"            )) return;
                    if (apply("Peacegarden"                    , "Brown Pigment"                      , 27.0, 22.0, "Fallgourd Float"          )) return;
                    if (apply("Silent Arbor"                   , "Alligator Pear"                     , 26.0, 19.0, "Quarrymill"               )) return;
                    if (apply("Spineless Basin"                , "Purple Pigment"                     , 24.0, 31.0, "Black Brush Station"      )) return;
                    if (apply("Three-malm Bend"                , "Red Pigment"                        , 16.0, 13.0, "Summerford Farms"         )) return;
                    if (apply("Upper Paths"                    , "Grey Pigment"                       , 16.0, 21.0, "Quarrymill"               )) return;
                break;
                case 30 + Mining:
                    if (apply("Wellwick Wood"                  , "Wyvern Obsidian"                    , 26.0, 17.0, "Camp Drybone"             )) return;
                break;
                case 30 + Quarrying:
                    if (apply("Broken Water"                   , "Purple Pigment"                     , 18.0, 11.0, "Little Ala Mhigo"         )) return;
                    if (apply("Cedarwood"                      , "Brown Pigment"                      , 26.0, 15.0, "Moraby Drydocks"          )) return;
                    if (apply("Nine Ivies"                     , "Green Pigment"                      , 20.0, 27.0, "The Hawthorne Hut"        )) return;
                    if (apply("Nophica's Wells"                , "Blue Pigment"                       , 23.0, 23.0, "Horizon"                  )) return;
                    if (apply("Oakwood"                        , "Brimstone"                          , 12.0, 26.0, "Camp Bronze Lake"         )) return;
                    if (apply("Quarterstone"                   , "Grey Pigment"                       , 31.0, 28.0, "Swiftperch"               )) return;
                    if (apply("Sagolii Desert"                 , "Bomb Ash"                           , 22.0, 29.0, "Forgotten Springs"        )) return;
                    if (apply("Wellwick Wood"                  , "Red Pigment"                        , 23.0, 19.0, "Camp Drybone"             )) return;
                break;
                case 25 + Harvesting:
                    if (apply("Drybone"                        , "Button Mushroom"                    , 14.0, 20.0, "Camp Drybone"             )) return;
                    if (apply("Oakwood"                        , "Pixie Plums"                        , 14.0, 24.0, "Camp Bronze Lake"         )) return;
                break;
                case 25 + Logging:
                    if (apply("Upper Paths"                    , "Matron's Mistletoe"                 , 23.0, 21.0, "Quarrymill"               )) return;
                break;
                case 25 + Mining:
                    if (apply("Upper Paths"                    , "Silver Ore"                         , 15.0, 19.0, "Quarrymill"               )) return;
                break;
                case 25 + Quarrying:
                    if (apply("Oakwood"                        , "Fire Rock"                          , 12.0, 23.0, "Camp Bronze Lake"         )) return;
                    if (apply("Upper Paths"                    , "Earth Rock"                         , 23.0, 21.0, "Quarrymill"               )) return;
                break;
                case 20 + Harvesting:
                    if (apply("Nine Ivies"                     , "Gil Bun"                            , 18.0, 28.0, "The Hawthorne Hut"        )) return;
                    if (apply("Quarterstone"                   , "Paprika"                            , 31.0, 28.0, "Swiftperch"               )) return;
                    if (apply("Sandgate"                       , "Popoto"                             , 16.0, 27.0, "Camp Drybone"             )) return;
                break;
                case 20 + Logging:
                    if (apply("Black Brush"                    , "Nopales"                            , 21.0, 20.0, "Black Brush Station"      )) return;
                    if (apply("Cedarwood"                      , "Sun Lemon"                          , 34.0, 17.0, "Moraby Drydocks"          )) return;
                    if (apply("Nine Ivies"                     , "Faerie Apple"                       , 15.0, 27.0, "The Hawthorne Hut"        )) return;
                    if (apply("Skull Valley"                   , "Grade 1 Carbonized Matter"          , 26.0, 23.0, "Aleport"                  )) return;
                break;
                case 20 + Mining:
                    if (apply("Drybone"                        , "Raw Malachite"                      , 17.0, 20.0, "Camp Drybone"             )) return;
                    if (apply("Peacegarden"                    , "Raw Sphene"                         , 29.0, 22.0, "Fallgourd Float"          )) return;
                    if (apply("Skull Valley"                   , "Raw Danburite"                      , 29.0, 22.0, "Aleport"                  )) return;
                break;
                case 20 + Quarrying:
                    if (apply("Skull Valley"                   , "Mudstone"                           , 26.0, 24.0, "Aleport"                  )) return;
                    if (apply("Three-malm Bend"                , "Grade 1 Carbonized Matter"          , 15.0, 10.0, "Summerford Farms"         )) return;
                break;
                case 15 + Harvesting:
                    if (apply("Bentbranch"                     , "Marjoram"                           , 18.0, 19.0, "Bentbranch Meadows"       )) return;
                    if (apply("Bentbranch"                     , "Carnation"                          , 22.0, 24.0, "Bentbranch Meadows"       )) return;
                    if (apply("Horizon's Edge"                 , "Coerthan Carrot"                    , 23.0, 18.0, "Horizon"                  )) return;
                    if (apply("Moraby Bay"                     , "Cinderfoot Olive"                   , 26.0, 22.0, "Moraby Drydocks"          )) return;
                    if (apply("Nophica's Wells"                , "Garlean Garlic"                     , 23.0, 23.0, "Horizon"                  )) return;
                    if (apply("Summerford"                     , "Sunset Wheat"                       , 22.0, 19.0, "Summerford Farms"         )) return;
                    if (apply("The Clutch"                     , "Alpine Parsnip"                     , 25.0, 20.0, "Black Brush Station"      )) return;
                break;
                case 15 + Logging:
                    if (apply("Bentbranch"                     , "Elm Log"                            , 20.0, 20.0, "Bentbranch Meadows"       )) return;
                break;
                case 15 + Mining:
                    if (apply("Horizon's Edge"                 , "Iron Ore"                           , 27.0, 17.0, "Horizon"                  )) return;
                break;
                case 15 + Quarrying:
                    if (apply("Black Brush"                    , "Rock Salt"                          , 14.0, 23.0, "Black Brush Station"      )) return;
                    if (apply("Horizon's Edge"                 , "Cinnabar"                           , 24.0, 18.0, "Horizon"                  )) return;
                break;
                case 10 + Logging:
                    if (apply("Cedarwood"                      , "La Noscean Orange"                  , 32.0, 16.0, "Moraby Drydocks"          )) return;
                    if (apply("Greentear"                      , "Ash Log"                            , 25.0, 20.0, "Bentbranch Meadows"       )) return;
                    if (apply("Spineless Basin"                , "Cock Feather"                       , 22.0, 26.0, "Black Brush Station"      )) return;
                    if (apply("Treespeak"                      , "Ash Log"                            , 27.0, 24.0, "Fallgourd Float"          )) return;
                break;
                case 10 + Mining:
                    if (apply("Black Brush"                    , "Tin Ore"                            , 20.0, 22.0, "Black Brush Station"      )) return;
                    if (apply("Cedarwood"                      , "Raw Sunstone"                       , 27.0, 18.0, "Moraby Drydocks"          )) return;
                    if (apply("Hammerlea"                      , "Tin Ore"                            , 22.0, 28.0, "Horizon"                  )) return;
                    if (apply("Treespeak"                      , "Raw Lapis Lazuli"                   , 28.0, 25.0, "Fallgourd Float"          )) return;
                break;
                case  5 + Logging:
                    if (apply("Gelmorra Ruins"                 , "Earth Shard"                        , 26.5, 21.9, "Fallgourd Float"          )) return;
                    if (apply("Jadeite Thick"                  , "Latex"                              , 23.0, 18.0, "Bentbranch Meadows"       )) return;
                    if (apply("The Tangle"                     , "Lightning Shard"                    , 14.5, 14.3, "Revenant's Toll"          )) return;
                    if (apply("Treespeak"                      , "Latex"                              , 28.0, 26.0, "Fallgourd Float"          )) return;
                    if (apply("Treespeak"                      , "Maple Sap"                          , 25.0, 27.0, "Fallgourd Float"          )) return;
                    if (apply("Whitebrim"                      , "Ice Shard"                          , 26.6, 19.7, "Camp Dragonhead"          )) return;
                break;
                case  5 + Mining:
                    if (apply("Gelmorra Ruins"                 , "Earth Shard"                        , 25.3, 22.2, "Fallgourd Float"          )) return;
                    if (apply("Hammerlea"                      , "Copper Ore"                         , 26.0, 25.0, "Horizon"                  )) return;
                    if (apply("Spineless Basin"                , "Copper Ore"                         , 18.8, 25.7, "Black Brush Station"      )) return;
                    if (apply("Spineless Basin"                , "Bone Chip"                          , 24.0, 26.0, "Black Brush Station"      )) return;
                    if (apply("The Tangle"                     , "Lightning Shard"                    , 15.2, 14.7, "Revenant's Toll"          )) return;
                    if (apply("Whitebrim"                      , "Ice Shard"                          , 28.0, 16.5, "Camp Dragonhead"          )) return;
                break;
                }
            }
        }
        #endregion
    }
}