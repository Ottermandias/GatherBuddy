using System.Linq;
using Dalamud;
using GatherBuddy.Classes;
using GatherBuddy.Enums;
using GatherBuddy.Game;
using GatherBuddy.Managers;
using GatherBuddy.Nodes;

namespace GatherBuddy.Data
{
    public static class NodeCoords
    {
        public static void SetCoords(Node node, AetheryteManager aetherytes)
        {
            bool Apply(string zone, string item, double x, double y, string A, bool flag = false)
            {
                if (zone != node.PlaceNameEn || !node.Items!.HasItems(item) || node.InitialPos != null)
                    return false;

                node.InitialPos = new InitialNodePosition()
                {
                    XCoordIntegral   = (int) (x * 100.0 + 0.9),
                    YCoordIntegral   = (int) (y * 100.0 + 0.9),
                    Prefer           = flag,
                    ClosestAetheryte = aetherytes.Aetherytes.FirstOrDefault(a => a.Name == A),
                };
                return true;
            }

            // Diadem special
            if (node.Nodes!.Territory!.Name == "The Diadem")
            {
                node.InitialPos = new InitialNodePosition()
                {
                    XCoordIntegral   = 0,
                    YCoordIntegral   = 0,
                    Prefer           = true,
                    ClosestAetheryte = aetherytes.Aetherytes.First(a => a.Name == "Foundation"),
                };
                return;
            }

            // @formatter:off
            if (node.Meta!.NodeType == NodeType.Ephemeral)
                switch (node.Meta.Level + node.Meta.GatheringType)
                {
                    // Shadowbringers
                    case 80 + GatheringType.Harvesting:
                        if (Apply("Weed"         , "Bog Sage"      , 25.2, 29.8, "Fort Jobb")) return;
                        if (Apply("The Wild Fete", "Sweet Marjoram", 26.9, 24.3, "Fanow"    )) return;
                        if (Apply("Scree"        , "White Clay"    , 13.6, 13.8, "Tomra"    )) return;
                        break;
                    case 80 + GatheringType.Mining:
                        if (Apply("The Wild Fete"                , "Gale Rock" , 22.3, 18.3, "Slitherbough")) return;
                        if (Apply("The Church of the First Light", "Solarite"  , 36.8, 14.9, "Fort Jobb"   )) return;
                        break;
                    case 80 + GatheringType.Quarrying:
                        if (Apply("Amity", "Thunder Rock", 22.3, 18.3, "Tomra")) return;
                        break;

                    // Stormblood
                    case 70 + GatheringType.Harvesting:
                        if (Apply("Doma"            , "Windtea Leaves", 22.0, 13.0, "The House of the Fierce")) return;
                        if (Apply("Nhaama's Retreat", "Doman Yellow"  , 15.0, 29.0, "Dhoro Iloh"             )) return;
                        break;
                    case 70 + GatheringType.Logging:
                        if (Apply("Abalathia's Skull", "Torreya Branch", 28.0, 10.0, "Porta Praetoria")) return;
                        break;
                    case 70 + GatheringType.Mining:
                        if (Apply("The High Bank", "Almandine", 13.0, 17.0, "Porta Praetoria")) return;
                        if (Apply("Onsal Hakair" , "Schorl"   , 29.0, 15.0, "The Dawn Throne")) return;
                        break;
                    case 70 + GatheringType.Quarrying:
                        if (Apply("Unseen Spirits Laughing", "Perlite", 37.0, 19.0, "Namai")) return;
                        break;

                    // Heavensward
                    case 60 + GatheringType.Harvesting:
                        if (Apply("Red Rim"                   , "Highland Oregano", 25.7, 29.4, "Falcon's Nest")) return;
                        if (Apply("Avalonia Fallen"           , "Granular Clay"   , 17.5, 32.0, "Anyx Trine"   )) return;
                        if (Apply("Greensward"                , "Granular Clay"   , 10.3, 37.0, "Zenith"       )) return;
                        if (Apply("Ok' Vundu Mok"             , "Highland Oregano", 13.9, 20.8, "Ok' Zundu"    )) return;
                        if (Apply("Saint Mocianne's Arboretum", "Granular Clay"   ,  6.9, 27.8, "Idyllshire"   )) return;
                        break;
                    case 60 + GatheringType.Quarrying:
                        if (Apply("Black Iron Bridge"  , "Lightning Moraine", 21.0, 28.0, "Falcon's Nest")) return;
                        if (Apply("Avalonia Fallen"    , "Fire Moraine"     , 17.0, 27.0, "Anyx Trine"   )) return;
                        if (Apply("The Makers' Quarter", "Fire Moraine"     , 26.0, 24.0, "Idyllshire"   )) return;
                        if (Apply("The Lost Landlord"  , "Fire Moraine"     , 29.0, 19.0, "Moghome"      )) return;
                        if (Apply("Ok' Gundu"          , "Lightning Moraine", 34.0, 30.0, "Camp Cloudtop")) return;
                        break;
                }

            if (node.Meta!.NodeType == NodeType.Unspoiled)
                switch(node.Meta.Level + node.Meta.GatheringType)
                {
                    case 80 + GatheringType.Harvesting:
                        if (Apply("The Dragging Tail"    , "Russet Popoto"        , 19.0, 16.0, "Mord Souq"                )) return;
                        if (Apply("Bowrest"              , "Mist Spinach"         , 34.0, 21.0, "Fanow"                    )) return;
                        if (Apply("Inviolate Witness"    , "Ethereal Cocoon"      , 27.0, 10.0, "Fort Jobb"                )) return;
                        if (Apply("White Oil Falls"      , "Tender Dill"          , 28.4, 21.1, "Stilltide"                , true)) return;
                        if (Apply("The Nabaath Severance", "Duskblooms"           , 32.2, 33.4, "The Inn at Journey's Head", true)) return;
                        if (Apply("The Isle of Consorts" , "Raindrop Cotton Boll" , 33.7, 13.2, "Fanow"                    , true)) return;
                        break;
                    case 80 + GatheringType.Logging:
                        if (Apply("Sharptongue Drip"           , "Lemonette"             , 20.0, 27.0, "Wright"               )) return;
                        if (Apply("Mjrl's Regret"              , "Sandalwood Log"        , 24.0, 36.0, "Slitherbough"         )) return;
                        if (Apply("Anden's Airs"               , "Merbau Log"            , 36.5, 27.3, "Lydha Lran"           , true)) return;
                        if (Apply("Garik"                      , "Silver Beech Log"      , 16.0, 10.9, "Twine"                , true)) return;
                        if (Apply("The Hour of Certain Durance", "Wattle Petribark"      , 5.2,  26.5, "The Ostall Imperative", true)) return;
                        if (Apply("The Central Hills of Amber" , "Rarefied Sandteak Log" , 18.6, 20.5, "Twine"                )) return;
                        if (Apply("The Norvrandt Slope"        , "Rarefied Kelp"         , 37.5, 11.7, "The Ondo Cups"        )) return;
                        if (Apply("Heather Falls"              , "Rarefied Urunday Log"  , 30.6, 6.9,  "Ala Gannha"           )) return;
                        break;
                    case 80 + GatheringType.Mining:
                        if (Apply("Ladle"                 , "Raw Triplite"               , 20.0, 29.0, "The Inn at Journey's Head")) return;
                        if (Apply("Weed"                  , "Raw Petalite"               , 28.0, 33.0, "Fort Jobb"                )) return;
                        if (Apply("The Caliban Gorge"     , "Raw Onyx"                   , 16.0, 21.0, "The Macarenses Angle"     )) return;
                        if (Apply("Saint Fathric's Temple", "Prismstone"                 , 30.0, 20.0, "Wolekdorf"                )) return;
                        if (Apply("Where the Dry Return"  , "Tungsten Ore"               , 32.0,  7.0, "The Ondo Cups"            )) return;
                        if (Apply("The Isle of Ken"       , "Brashgold Ore"              ,  4.7, 33.9, "The Ostall Imperative"    , true)) return;
                        if (Apply("The Husk"              , "Dolomite"                   ,  7.5, 30.1, "Slitherbough"             , true)) return;
                        if (Apply("The Duergar's Tewel"   , "Hard Water"                 , 36.1, 12.2, "Tomra"                    , true)) return;
                        if (Apply("The Ondo Cups"         , "Rarefied Reef Rock"         , 32.7, 20.6, "The Ondo Cups"            )) return;
                        if (Apply("The Norvrandt Slope"   , "Rarefied Sea Salt"          , 25.1,  4.5, "The Ondo Cups"            )) return;
                        if (Apply("Virdjala"              , "Rarefied Gyr Abanian Alumen", 31.6, 31.4, "The Peering Stones"       )) return;
                        break;
                    case 80 + GatheringType.Quarrying:
                        if (Apply("Purpure"          , "Purpure Shell Chip"      , 34.4, 31.3, "The Macarenses Angle", true)) return;
                        if (Apply("Mount Biran Mines", "Ashen Alumen"            , 20.1, 8.6,  "Twine"               , true)) return;
                        if (Apply("Voeburtenburg"    , "Solstice Stone"          , 35.6, 8.7,  "Wolekdorf"           , true)) return;
                        break;

                    case 75 + GatheringType.Logging:
                        if (Apply("The Chisel"    , "Rarefied White Oak Log", 28.0, 32.8, "Stilltide" )) return;
                        if (Apply("The Woolen Way", "Rarefied Pixie Apple",   4.1,  23.1, "Lydha Lran")) return;
                        break;
                    case 75 + GatheringType.Harvesting:
                        if (Apply("Phisor Lran", "Broad Beans"         , 24.0, 36.0, "Lydha Lran")) return;
                        if (Apply("Embrasure"  , "Peppermint"          , 26.0, 20.0, "Fort Jobb" )) return;
                        break;
                    case 75 + GatheringType.Mining:
                        if (Apply("Sextuplet Shallow"              , "Raw Diaspore"            , 26.0, 13.0, "Wolekdorf"   )) return;
                        if (Apply("Cleric"                         , "Raw Lazurite"            , 25.0, 34.0, "Slitherbough")) return;
                        if (Apply("The Belt"                       , "Rarefied Titancopper Ore", 31.3, 24.2, "Fort Jobb"   )) return;
                        if (Apply("Cleric"                         , "Rarefied Raw Lazurite"   , 16.5, 18.2, "Slitherbough"   )) return;
                        break;

                    case 70 + GatheringType.Harvesting:
                        if (Apply("Rustrock"                    , "Hallowed Basil"     , 23.0, 16.0, "Ala Gannha"             )) return;
                        if (Apply("Valley of the Fallen Rainbow", "Lotus Root"         , 28.0, 7.0,  "The House of the Fierce")) return;
                        if (Apply("Abalathia's Skull"           , "Jhammel Ginger"     , 8.0,  8.0,  "Porta Praetoria"        )) return;
                        if (Apply("Onsal Hakair"                , "Rhea"               , 20.0, 8.0,  "The Dawn Throne"        )) return;
                        if (Apply("Mount Yorn"                  , "Hingan Flax"        , 24.0, 36.0, "Ala Ghiri"              )) return;
                        if (Apply("The Glittering Basin"        , "Yanxian Cotton Boll", 28.0, 35.0, "Namai"                  )) return;
                        break;
                    case 70 + GatheringType.Logging:
                        if (Apply("Onsal Hakair" , "Othardian Plum"     , 27.0, 17.0, "The Dawn Throne"        )) return;
                        if (Apply("The High Bank", "Torreya Log"        , 11.0, 13.0, "Porta Praetoria"        )) return;
                        if (Apply("Pike Falls"   , "Black Willow Log"   , 15.0, 21.0, "Castrum Oriens"         )) return;
                        if (Apply("Rustrock"     , "Urunday Log"        , 32.0, 10.0, "Ala Gannha"             )) return;
                        if (Apply("Venmont Yards", "White Oak Branch"   , 12.0, 29.0, "Wright"                 )) return;
                        if (Apply("Doma"         , "Rarefied Pine Resin", 18.7, 14.4, "The House of the Fierce")) return;
                        break;
                    case 70 + GatheringType.Mining:
                        if (Apply("Rustrock"                    , "Raw Rhodonite"       , 26.0, 12.0, "Ala Gannha"             )) return;
                        if (Apply("Valley of the Fallen Rainbow", "Raw Imperial Jade"   , 29.0, 9.0,  "The House of the Fierce")) return;
                        if (Apply("The Towering Still"          , "Raw Azurite"         , 5.0,  29.0, "Dhoro Iloh"             )) return;
                        if (Apply("Wightrock"                   , "Chromite Ore"        , 16.0, 33.9, "Ala Ghiri"              )) return;
                        if (Apply("Doma"                        , "Palladium Ore"       , 20.5, 10.4, "The House of the Fierce")) return;
                        if (Apply("East Othard Coastline"       , "Nightsteel Ore"      , 11.0, 23.0, "Onokoro"                )) return;
                        if (Apply("Nhaama's Retreat"            , "Silvergrace Ore"     , 23.0, 36.0, "Reunion"                )) return;
                        if (Apply("Virdjala"                    , "Gyr Abanian Ore"     , 31.0, 27.0, "The Peering Stones"     )) return;
                        if (Apply("Loch Seld"                   , "Evergleam Ore"       , 22.0, 13.0, "Porta Praetoria"        )) return;
                        if (Apply("Governor's Row"              , "Raw Hematite"        , 33.0, 23.0, "Stilltide"              )) return;
                        if (Apply("GThe Sea of Blades"          , "Rarefied Raw Azurite", 36.0, 26.4, "Reunion"                )) return;
                        break;
                    case 70 + GatheringType.Quarrying:
                        if (Apply("Loch Seld", "Ala Mhigan Salt Crystal", 21.0, 29.0, "The Ala Mhigan Quarter")) return;
                        break;

                    case 65 + GatheringType.Logging:
                        if (Apply("The Glittering Basin", "Bamboo Shoot"              , 28.0, 25.0, "Namai"  )) return;
                        if (Apply("Isari"               , "Rarefied Larch Log"        , 6.0,  15.8, "Onokoro")) return;
                        if (Apply("Shoal Rock"          , "Rarefied Shiitake Mushroom", 33.1, 9.2,  "Onokoro", true)) return; // Aetheryte
                        break;
                    case 65 + GatheringType.Mining:
                        if (Apply("East Othard Coastline", "Raw Star Spinel"                   , 15.0,  4.5, "Onokoro"           )) return;
                        if (Apply("Mirage Creek"         , "Rarefied Raw Triphane"             , 29.6, 12.9, "Rhalgr's Reach"    , true)) return; // Aetheryte
                        if (Apply("Pike Falls"           , "Rarefied Gyr Abanian Mineral Water", 18.1, 22.8, "The Peering Stones")) return;
                        if (Apply("The Crab Pots"        , "Rarefied Raw Star Spinel"          , 21.2, 34.4, "Kugane"            , true)) return; // Aetheryte
                        break;

                    case 60 + GatheringType.Harvesting:
                        if (Apply("Delta Quadrant"     , "Star Cotton Boll", 9.0,  31.0, "Helix"        )) return;
                        if (Apply("Chocobo Forest"     , "Noble Sage"      , 33.0, 30.0, "Tailfeather"  )) return;
                        if (Apply("Twinpools"          , "Chysahl Greens"  , 8.0,  9.0,  "Tailfeather"  )) return;
                        if (Apply("Twinpools"          , "Vanilla Beans"   , 23.0, 21.0, "Falcon's Nest")) return;
                        if (Apply("Greensward"         , "Seventh Heaven"  , 17.0, 36.0, "Zenith"       )) return;
                        if (Apply("The Makers' Quarter", "Snurbleberry"    , 39.0, 26.0, "Anyx Trine"   )) return;
                        break;
                    case 60 + GatheringType.Logging:
                        if (Apply("The Answering Quarter", "Teak Log"                  , 6.0,  28.0, "Idyllshire"    )) return;
                        if (Apply("Greensward"           , "Brown Mushroom"            , 12.0, 37.0, "Zenith"        )) return;
                        if (Apply("Vundu Ok' Bendu"      , "Heavens Lemon"             , 35.0, 23.0, "Camp Cloudtop" )) return;
                        if (Apply("The Ruling Quarter"   , "Cloud Banana"              , 19.0, 36.0, "Idyllshire"    )) return;
                        if (Apply("The Gauntlet"         , "Honeydew Almonds"          , 24.0, 6.0,  "Ok' Zundu"     )) return;
                        if (Apply("Beta Quadrant"        , "Wattle Bark"               , 22.0, 10.0, "Helix"         )) return;
                        if (Apply("Ohl Tahn"             , "Old-growth Camphorwood Log", 11.0, 10.0, "Zenith"        )) return;
                        if (Apply("East End"             , "Beech Branch"              , 11.0, 18.0, "Castrum Oriens")) return;
                        if (Apply("Avalonia Fallen"      , "Rarefied Dark Chestnut"    , 16.2, 35.5, "Anyx Trine"    )) return;
                        break;
                    case 60 + GatheringType.Mining:
                        if (Apply("Twinpools"         , "Tungstite"                       , 10.0, 9.0,  "Tailfeather"   )) return;
                        if (Apply("Alpha Quadrant"    , "Luminium Ore"                    , 5.0,  17.0, "Helix"         )) return;
                        if (Apply("The Ruling Quarter", "Light Kidney Ore"                , 34.0, 30.0, "Anyx Trine"    )) return;
                        if (Apply("Beta Quadrant"     , "Adamantite Ore"                  , 24.0, 6.0,  "Helix"         )) return;
                        if (Apply("Greensward"        , "Aurum Regis Ore"                 , 11.0, 38.0, "Zenith"        )) return;
                        if (Apply("The Habisphere"    , "Red Alumen"                      , 35.0, 16.0, "Helix"         )) return;
                        if (Apply("The Gauntlet"      , "Smithsonite Ore"                 , 38.0, 15.0, "Ok' Zundu"     )) return;
                        if (Apply("The Striped Hills" , "Raw Triphane"                    , 25.0, 8.0,  "Castrum Oriens")) return;
                        if (Apply("The Blue Window"   , "Rarefied Abalathian Spring Water", 20.6, 11.6, "Ok' Zundu"      )) return;
                        break;
                    case 60 + GatheringType.Quarrying:
                        if (Apply("The Ruling Quarter"   , "Zeolite Ore"         , 13.0, 31.0, "Idyllshire"   )) return;
                        if (Apply("The Gauntlet"         , "Abalathian Rock Salt", 7.0,  7.0,  "Ok' Zundu"    )) return;
                        if (Apply("Riversmeet"           , "Astral Moraine"      , 37.0, 16.0, "Falcon's Nest")) return;
                        if (Apply("Voor Sian Siran"      , "Sun Mica"            , 35.0, 39.0, "Camp Cloudtop")) return;
                        if (Apply("The Answering Quarter", "Blue Quartz"         , 11.0, 16.0, "Idyllshire"   )) return;
                        break;
                    case 55 + GatheringType.Harvesting:
                        if (Apply("Riversmeet"    , "Pearl Sprouts"               , 31.0, 20.0, "Falcon's Nest")) return;
                        break;
                    case 55 + GatheringType.Logging:
                        if (Apply("Ohl Tahn"             , "Porcini"                   , 24.0, 6.0,  "Zenith"     )) return;
                        if (Apply("The Smoldering Wastes", "Rarefied Dark Chestnut Sap", 29.2, 30.1, "Tailfeather")) return;
                        break;
                    case 55 + GatheringType.Mining:
                        if (Apply("The Smoldering Wastes", "Pyrite"         , 26.0, 17.0, "Tailfeather")) return;
                        if (Apply("The Smoldering Wastes", "Rarefied Pyrite", 30.7, 32.2, "Tailfeather")) return;
                        break;
                    case 55 + GatheringType.Quarrying:
                        if (Apply("Eil Tohm", "Green Quartz"           , 33.0, 22.0, "Moghome"      )) return;
                        break;

                    case 50 + GatheringType.Harvesting:
                        if (Apply("Nine Ivies"       , "Silkworm Cocoon"         , 22.0, 26.0, "The Hawthorne Hut" )) return;
                        if (Apply("The Bramble Patch", "Trillium"                , 17.0, 19.0, "The Hawthorne Hut" )) return;
                        if (Apply("The Honey Yard"   , "Trillium Bulb"           , 13.0, 23.0, "The Hawthorne Hut" )) return;
                        if (Apply("Bloodshore"       , "Honey Lemon"             , 26.0, 32.0, "Costa del Sol"     )) return;
                        if (Apply("Raincatcher Gully", "Dzemael Tomato"          , 18.0, 26.0, "Wineport"          )) return;
                        if (Apply("Drybone"          , "Black Truffle"           , 12.0, 16.0, "Camp Drybone"      )) return;
                        if (Apply("Sorrel Haven"     , "Shroud Tea Leaves"       , 16.0, 20.0, "Bentbranch Meadows")) return;
                        if (Apply("Quarterstone"     , "La Noscean Leek"         , 34.0, 28.0, "Swiftperch"        )) return;
                        if (Apply("Cedarwood"        , "Young Cieldalaes Spinach", 32.0, 11.0, "Moraby Drydocks"   )) return;
                        if (Apply("Nine Ivies"       , "Rosemary"                , 22.0, 30.0, "The Hawthorne Hut" )) return;
                        if (Apply("Chocobo Forest"   , "Old World Fig"           , 26.0, 12.0, "Tailfeather"       )) return;
                        break;
                    case 50 + GatheringType.Logging:
                        if (Apply("Providence Point", "Spruce Log"           , 27.0, 12.0, "Camp Dragonhead"   )) return;
                        if (Apply("Dragonhead"      , "Vampire Plant"        , 27.0, 24.0, "Camp Dragonhead"   )) return;
                        if (Apply("Whitebrim"       , "Thavnairian Mistletoe", 8.0,  13.0, "Camp Dragonhead"   )) return;
                        if (Apply("Bloodshore"      , "Prickly Pineapple"    , 30.0, 26.0, "Costa del Sol"     )) return;
                        if (Apply("North Silvertear", "Fire Cluster"         , 32.0, 11.0, "Revenant's Toll"   )) return;
                        if (Apply("Alder Springs"   , "Scarlet Sap"          , 18.0, 26.0, "Fallgourd Float"   )) return;
                        if (Apply("Summerford"      , "Apricot"              , 19.0, 16.0, "Summerford Farms"  )) return;
                        if (Apply("Bronze Lake"     , "Blood Orange"         , 28.0, 25.0, "Camp Bronze Lake"  )) return;
                        if (Apply("Upper Paths"     , "Fragrant Log"         , 18.0, 23.0, "Camp Tranquil"     )) return;
                        if (Apply("Greentear"       , "Redolent Log"         , 30.0, 19.0, "Bentbranch Meadows")) return;
                        if (Apply("Zephyr Drift"    , "Ebony Log"            , 23.2, 26.3, "Summerford Farms"  )) return;
                        if (Apply("Bentbranch"      , "Cypress Log"          , 25.0, 29.0, "Bentbranch Meadows")) return;
                        break;
                    case 50 + GatheringType.Mining:
                        if (Apply("Dragonhead"      , "Darksteel Ore"      , 27.6, 19.8, "Camp Dragonhead"          )) return;
                        if (Apply("The Burning Wall", "Gold Ore"           , 28.0, 22.0, "Camp Drybone"             )) return;
                        if (Apply("Raubahn's Push"  , "Ferberite"          , 16.0, 19.0, "Ceruleum Processing Plant")) return;
                        if (Apply("Black Brush"     , "Native Gold"        , 24.0, 16.0, "Black Brush Station"      )) return;
                        if (Apply("Moraby Bay"      , "Raw Ruby"           , 23.0, 21.0, "Moraby Drydocks"          )) return;
                        if (Apply("Broken Water"    , "Platinum Ore"       , 19.0, 8.0,  "Little Ala Mhigo"         )) return;
                        if (Apply("Bluefog"         , "Virgin Basilisk Egg", 24.0, 26.0, "Ceruleum Processing Plant")) return;
                        if (Apply("Riversmeet"      , "Yellow Copper Ore"  , 30.0, 23.0, "Falcon's Nest"            )) return;
                        break;
                    case 50 + GatheringType.Quarrying:
                        if (Apply("Raincatcher Gully", "Volcanic Rock Salt"        , 21.0, 32.0, "Wineport"        )) return;
                        if (Apply("Dragonhead"       , "Astral Rock"               , 23.0, 23.0, "Camp Dragonhead" )) return;
                        if (Apply("Wellwick Wood"    , "Gold Sand"                 , 25.0, 22.0, "Camp Drybone"    )) return;
                        if (Apply("North Silvertear" , "Fire Cluster"              , 27.0, 10.0, "Revenant's Toll" )) return;
                        if (Apply("Zephyr Drift"     , "Grade 3 La Noscean Topsoil", 25.0, 27.0, "Summerford Farms")) return;
                        if (Apply("Lower Paths"      , "Grade 3 Shroud Topsoil"    , 16.0, 31.0, "Camp Tranquil"   )) return;
                        if (Apply("Hammerlea"        , "Grade 3 Thanalan Topsoil"  , 18.0, 27.0, "Horizon"         )) return;
                        if (Apply("Wellwick Wood"    , "Antumbral Rock"            , 26.0, 19.0, "Camp Drybone"    )) return;
                        if (Apply("Raincatcher Gully", "Pumice"                    , 17.0, 26.0, "Wineport"        )) return;
                        break;
                }

            if (node.Meta!.NodeType == NodeType.Regular)
                switch(node.Meta.Level + node.Meta.GatheringType)
                {
                    case 80 + GatheringType.Harvesting:
                        if (Apply("Ashpool"                        , "Oddly Specific Amber"               , 10.6, 18.5, "Tailfeather"              )) return;
                        if (Apply("Scree"                          , "Black Aethersand"                   , 12.0, 14.0, "Tomra"                    )) return;
                        if (Apply("The Whale's Crown"              , "Oddly Specific Latex"               , 29.2, 23.4, "Ok' Zundu"                )) return;
                        if (Apply("The Wild Fete"                  , "Sweet Alyssum"                      , 24.0, 28.0, "Slitherbough"             )) return;
                        if (Apply("Weed"                           , "Tiger Lily"                         , 23.0, 34.0, "Fort Jobb"                )) return;
                        if (Apply("The Fields of Amber"            , "Rarefied Night Pepper"              , 26.7, 27.5, "The Inn at Journey's Head")) return;
                        break;
                    case 80 + GatheringType.Logging:
                        if (Apply("Four Arms"                      , "Oddly Specific Amber"               , 25.0, 28.2, "Moghome"                  )) return;
                        if (Apply("Hare Among Giants"              , "Harcot"                             ,  8.0,  9.0, "The Ostall Imperative"    )) return;
                        if (Apply("Ladle"                          , "Amber Cloves"                       , 15.0, 30.0, "Twine"                    )) return;
                        if (Apply("Whilom River"                   , "Oddly Specific Latex"               , 29.7, 18.3, "Tailfeather"              )) return;
                        if (Apply("Valley of the Fallen Rainbow"   , "Oddly Specific Dark Chestnut Log"   , 28.3,  9.5, "The House of the Fierce"  )) return;
                        if (Apply("The Destroyer"                  , "Oddly Specific Dark Chestnut Log"   , 20.3, 18.0, "Porta Praetoria"          )) return;
                        if (Apply("The Pappus Tree"                , "Oddly Specific Primordial Log"      ,  7.0, 34.2, "Helix"                    )) return;
                        if (Apply("Hyperstellar Downconverter"     , "Oddly Specific Primordial Log"      ,  7.1, 15.9, "Helix"                    )) return;
                        if (Apply("Antithesis"                     , "Oddly Delicate Feather"             , 11.6, 26.8, "Helix"                    )) return;
                        break;
                    case 80 + GatheringType.Mining:
                        if (Apply("Mourn"                          , "Oddly Specific Dark Matter"         , 17.1, 10.8, "Anyx Trine"               )) return;
                        if (Apply("Ok' Vundu Vana"                 , "Oddly Specific Obsidian"            , 26.5, 23.1, "Ok' Zundu"                )) return;
                        if (Apply("Red Rim"                        , "Oddly Specific Obsidian"            , 14.8, 30.6, "Falcon's Nest"            )) return;
                        if (Apply("The Church of the First Light"  , "Underground Spring Water"           , 35.0, 16.0, "Fort Jobb"                )) return;
                        if (Apply("The Wild Fete"                  , "Extra Effervescent Water"           , 23.0, 28.0, "Slitherbough"             )) return;
                        if (Apply("The Dragon's Struggle"          , "Oddly Specific Schorl"              , 12.5,  9.7, "The House of the Fierce"  )) return;
                        if (Apply("The Pauper's Lode"              , "Oddly Specific Schorl"              ,  4.0, 26.7, "Porta Praetoria"          )) return;
                        if (Apply("The Aqueduct"                   , "Oddly Specific Primordial Ore"      , 11.0, 35.2, "Helix"                    )) return;
                        if (Apply("Cooling Station"                , "Oddly Specific Primordial Ore"      , 14.0,  9.8, "Helix"                    )) return;
                        if (Apply("Gamma Quadrant"                 , "Oddly Delicate Adamantite Ore"      , 36.2, 27.8, "Helix"                    )) return;
                        break;
                    case 80 + GatheringType.Quarrying:
                        if (Apply("Amity"                          , "Black Aethersand"                   , 22.0, 16.0, "Tomra"                    )) return;
                        if (Apply("Mount Biran Mines"              , "Titancopper Sand"                   , 15.0, 12.0, "Twine"                    )) return;
                        if (Apply("The Daggers"                    , "Oddly Specific Dark Matter"         , 30.9, 28.2, "Anyx Trine"               )) return;
                        if (Apply("The Citia Swamps"               , "Rarefied Manasilver Sand"           , 16.0, 28.8, "Slitherbough"             )) return;
                        break;
                    case 75 + GatheringType.Harvesting:
                        if (Apply("Quickspill Delta"               , "Gathering Tool Paraphernalia"       , 13.9, 32.7, "Idyllshire"               )) return;
                        if (Apply("Seagazer"                       , "Upland Wheat"                       , 16.0, 36.0, "Wright"                   )) return;
                        if (Apply("The Deliberating Doll"          , "Royal Grapes"                       , 16.0, 29.0, "Slitherbough"             )) return;
                        if (Apply("The Forest of the Lost Shepherd", "Animal Droppings"                   , 16.0, 23.0, "The Ostall Imperative"    )) return;
                        if (Apply("The Nidifice"                   , "Crafting Tool Paraphernalia"        , 35.9, 36.0, "Camp Cloudtop"            )) return;
                        if (Apply("The Woolen Way"                 , "Megafauna Leftovers"                , 11.0, 24.0, "Lydha Lran"               )) return;
                        if (Apply("Yuzuka Manor"                   , "Printing Paraphernalia"             , 17.2, 30.8, "Namai"                    )) return;
                        if (Apply("Laxan Loft"                     , "Rarefied Bright Flax"               , 23.4, 13.1, "Fort Jobb"                )) return;
                        break;
                    case 75 + GatheringType.Logging:
                        if (Apply("Hidden Tear"                    , "Handpicked Ingredients"             , 35.0,  8.5, "Ala Gannha"               )) return;
                        if (Apply("The Bright Cliff"               , "White Oak Log"                      , 27.0, 22.0, "Wright"                   )) return;
                        if (Apply("The Chiliad"                    , "Frantoio"                           , 36.0, 24.0, "Fort Jobb"                )) return;
                        if (Apply("The Church at Dammroen Field"   , "Pixie Apple"                        , 30.0,  5.0, "Wolekdorf"                )) return;
                        if (Apply("The Hundred Throes"             , "Weaving Paraphernalia"              , 34.3, 11.6, "Tailfeather"              )) return;
                        if (Apply("Woven Oath"                     , "Gianthive Chip"                     , 12.0, 20.0, "Slitherbough"             )) return;
                        break;
                    case 75 + GatheringType.Mining:
                        if (Apply("The Bookman's Shelves"          , "Highland Spring Water"              ,  8.0, 20.0, "Lydha Lran"               )) return;
                        if (Apply("The Queen's Gardens"            , "Printing Paraphernalia"             , 29.7, 16.2, "The Ala Mhigan Quarter"   )) return;
                        if (Apply("The Shattered Back"             , "Crafting Tool Paraphernalia"        , 28.9,  7.5, "Ok' Zundu"                )) return;
                        if (Apply("Weed"                           , "Animal Droppings"                   , 26.0, 34.0, "Fort Jobb"                )) return;
                        if (Apply("Weston Waters"                  , "Weaving Paraphernalia"              ,  8.0,  7.8, "Zenith"                   )) return;
                        if (Apply("The Forest of the Lost Shepherd", "Rarefied Bluespirit Ore"            , 27.4, 21.6, "Fort Jobb"                )) return;
                        break;
                    case 75 + GatheringType.Quarrying:
                        if (Apply("Lozatl's Conquest"              , "Manasilver Sand"                    , 17.0, 19.0, "Slitherbough"             )) return;
                        if (Apply("Slowroad"                       , "Truegold Sand"                      , 21.0, 30.0, "Wright"                   )) return;
                        if (Apply("The Convictory"                 , "Gathering Tool Paraphernalia"       , 13.4, 23.9, "Falcon's Nest"            )) return;
                        if (Apply("The Kobayashi Maru"             , "Handpicked Ingredients"             , 39.4,  4.0, "Onokoro"                  )) return;
                        break;
                    case 70 + GatheringType.Harvesting:
                        if (Apply("Doma"                           , "Daikon Radish"                      , 22.0, 10.0, "The House of the Fierce"  )) return;
                        if (Apply("Mount Yorn"                     , "Gyr Abanian Carrot"                 , 26.0, 27.0, "Ala Ghiri"                )) return;
                        if (Apply("Nhaama's Retreat"               , "Sun Cabbage"                        , 14.0, 26.0, "The Dawn Throne"          )) return;
                        if (Apply("Rasen Kaikyo"                   , "Ruby Tide Kelp"                     , 11.0, 13.0, "Onokoro"                  )) return;
                        if (Apply("Snitch"                         , "Animal Trace"                       , 31.0, 15.0, "Mord Souq"                )) return;
                        if (Apply("Thysm Lran"                     , "Clinquant Stones"                   , 31.3, 34.2, "Lydha Lran"               )) return;
                        break;
                    case 70 + GatheringType.Logging:
                        if (Apply("Abalathia's Skull"              , "Zelkova Log"                        , 26.0,  9.0, "Porta Praetoria"          )) return;
                        if (Apply("Dimwold"                        , "Persimmon Leaf"                     , 10.0, 30.0, "Castrum Oriens"           )) return;
                        if (Apply("Stonegazer"                     , "Raven Coal"                         , 17.2, 24.4, "Wright"                   )) return;
                        break;
                    case 70 + GatheringType.Mining:
                        if (Apply("The High Bank"                  , "Molybdenum Ore"                     , 10.0, 18.0, "Porta Praetoria"          )) return;
                        if (Apply("Thysm Lran"                     , "Clinquant Stones"                   , 33.3, 31.5, "Lydha Lran"               )) return;
                        if (Apply("Unseen Spirits Laughing"        , "Durium Ore"                         , 36.0, 19.0, "Namai"                    )) return;
                        break;
                    case 70 + GatheringType.Quarrying:
                        if (Apply("Onsal Hakair"                   , "Durium Sand"                        , 29.0, 15.0, "The Dawn Throne"          )) return;
                        if (Apply("Shadow Fault"                   , "Raven Coal"                         , 11.8, 26.3, "Wright"                   )) return;
                        if (Apply("The Inn at Journey's Head"      , "Animal Trace"                       , 30.0, 25.0, "The Inn at Journey's Head")) return;
                        break;
                    case 65 + GatheringType.Harvesting:
                        if (Apply("Dimwold"                        , "Holy Basil"                         , 11.0, 26.0, "Castrum Oriens"           )) return;
                        if (Apply("East Othard Coastline"          , "Soybeans"                           ,  7.0,  8.0, "Onokoro"                  )) return;
                        if (Apply("Gorgagne Holding"               , "Phial of Thermal Fluid"             , 32.0, 15.0, "Falcon's Nest"            )) return;
                        if (Apply("Ohl Tahn"                       , "Cloudkin Feather"                   ,  8.0, 15.0, "Zenith"                   )) return;
                        if (Apply("Rasen Kaikyo"                   , "Gem Algae"                          , 26.0, 19.0, "Tamamizu"                 )) return;
                        if (Apply("The Last Forest"                , "Mountain Popoto"                    , 13.0, 11.0, "Ala Gannha"               )) return;
                        if (Apply("Mirage Creek"                   , "Rarefied Bloodhemp"                 , 29.0, 11.0, "Rhalgr's Reach"           )) return;
                        break;
                    case 65 + GatheringType.Logging:
                        if (Apply("East End"                       , "Beech Log"                          , 10.0, 16.0, "Castrum Oriens"           )) return;
                        if (Apply("Onokoro"                        , "Larch Log"                          , 20.0,  9.0, "Onokoro"                  )) return;
                        if (Apply("The Heron's Flight"             , "Pine Resin"                         , 36.0, 15.0, "Namai"                    )) return;
                        break;
                    case 65 + GatheringType.Mining:
                        if (Apply("Hells' Lid"                     , "Koppranickel Ore"                   , 25.0, 35.0, "Kugane"                   )) return;
                        if (Apply("The Bed of Bones"               , "Phial of Thermal Fluid"             , 22.0, 35.0, "Falcon's Nest"            )) return;
                        if (Apply("The Gensui Chain"               , "Crescent Spring Water"              , 33.0, 22.0, "Namai"                    )) return;
                        if (Apply("The Striped Hills"              , "Gyr Abanian Mineral Water"          , 23.0, 13.0, "Castrum Oriens"           )) return;
                        if (Apply("Pike Falls"                     , "Rarefied Gyr Abanian Mineral Water" , 18.1, 22.8, "The Peering Stones"       )) return;
                        break;
                    case 65 + GatheringType.Quarrying:
                        if (Apply("Landlord Colony"                , "Cloudkin Feather"                   , 34.0, 25.0, "Moghome"                  )) return;
                        if (Apply("Rasen Kaikyo"                   , "Diatomite"                          , 14.0, 16.0, "Onokoro"                  )) return;
                        if (Apply("Rustrock"                       , "Stiperstone"                        , 21.0, 13.0, "Ala Gannha"               )) return;
                        break;
                    case 60 + GatheringType.Harvesting:
                        if (Apply("Avalonia Fallen"                , "Coneflower"                         , 10.0, 33.0, "Anyx Trine"               )) return;
                        if (Apply("East End"                       , "Bloodhemp"                          , 16.0,  7.0, "Castrum Oriens"           )) return;
                        if (Apply("Four Arms"                      , "Dandelion"                          , 20.0, 31.0, "Moghome"                  )) return;
                        if (Apply("The Answering Quarter"          , "Cow Bitter"                         , 14.0, 19.0, "Idyllshire"               )) return;
                        if (Apply("The Blue Window"                , "Sesame Seeds"                       , 23.0, 10.0, "Ok' Zundu"                )) return;
                        if (Apply("The Isle of Bekko"              , "Shishu Koban"                       , 37.0, 18.0, "Onokoro"                  )) return;
                        if (Apply("The Last Forest"                , "Peaks Pigment"                      , 13.0,  7.0, "Ala Gannha"               )) return;
                        if (Apply("Twinpools"                      , "Rue"                                , 12.0, 14.0, "Tailfeather"              )) return;
                        break;
                    case 60 + GatheringType.Logging:
                        if (Apply("Eil Tohm"                       , "Camphorwood Log"                    , 31.0, 30.0, "Moghome"                  )) return;
                        if (Apply("Voor Sian Siran"                , "Birch Branch"                       , 25.0, 34.0, "Camp Cloudtop"            )) return;
                        break;
                    case 60 + GatheringType.Mining:
                        if (Apply("Avalonia Fallen"                , "Raw Opal"                           , 20.0, 26.0, "Tailfeather"              )) return;
                        if (Apply("East End"                       , "Gyr Abanian Alumen"                 , 15.0, 12.0, "Castrum Oriens"           )) return;
                        if (Apply("Red Rim"                        , "Raw Citrine"                        , 23.0, 30.0, "Falcon's Nest"            )) return;
                        if (Apply("Sleeping Stones"                , "Peaks Pigment"                      , 35.0, 10.0, "Ala Gannha"               )) return;
                        if (Apply("The Makers' Quarter"            , "Raw Chrysolite"                     , 27.0, 24.0, "Idyllshire"               )) return;
                        if (Apply("The Turquoise Trench"           , "Shishu Koban"                       , 17.0, 33.0, "Tamamizu"                 )) return;
                        if (Apply("Voor Sian Siran"                , "Abalathian Spring Water"            , 33.0, 31.0, "Camp Cloudtop"            )) return;
                        break;
                    case 60 + GatheringType.Quarrying:
                        if (Apply("Landlord Colony"                , "Hardsilver Sand"                    , 29.0, 18.0, "Moghome"                  )) return;
                        break;
                    case 55 + GatheringType.Harvesting:
                        if (Apply("Chocobo Forest"                 , "Highland Wheat"                     , 36.0, 20.0, "Tailfeather"              )) return;
                        if (Apply("Landlord Colony"                , "Magma Beet"                         , 20.0, 21.0, "Zenith"                   )) return;
                        if (Apply("Twinpools"                      , "Rainbow Cotton Boll"                , 17.0, 16.0, "Falcon's Nest"            )) return;
                        if (Apply("Coerthas River"                 , "Rarefied Rainbow Cotton Boll"       , 24.7, 14.3, "Falcon's Nest"            )) return;
                        break;
                    case 55 + GatheringType.Logging:
                        if (Apply("The Smoldering Wastes"          , "Dark Chestnut Log"                  , 25.0, 25.0, "Tailfeather"              )) return;
                        break;
                    case 55 + GatheringType.Mining:
                        if (Apply("Chocobo Forest"                 , "Raw Star Ruby"                      , 30.0, 16.0, "Tailfeather"              )) return;
                        if (Apply("Gorgagne Holding"               , "Raw Larimar"                        , 31.0, 12.0, "Falcon's Nest"            )) return;
                        if (Apply("The Smoldering Wastes"          , "Raw Agate"                          , 27.0, 31.0, "Tailfeather"              )) return;
                        break;
                    case 55 + GatheringType.Quarrying:
                        if (Apply("Twinpools"                      , "Mythrite Sand"                      , 16.0, 12.0, "Tailfeather"              )) return;
                        if (Apply("Hemlock"                        , "Rarefied Mythrite Sand"             , 35.9, 23.4, "Falcon's Nest"            )) return;
                        break;
                    case 50 + GatheringType.Harvesting:
                        if (Apply("Sagolii Desert"                 , "Thanalan Tea Leaves"                , 13.0, 31.0, "Forgotten Springs"        )) return;
                        break;
                    case 50 + GatheringType.Logging:
                        if (Apply("Raincatcher Gully"              , "Water Shard"                        , 17.0, 32.0, "Wineport"                 )) return;
                        if (Apply("Riversmeet"                     , "Cedar Log"                          , 30.0, 32.0, "Falcon's Nest"            )) return;
                        if (Apply("Summerford"                     , "Fire Shard"                         , 19.0, 21.0, "Summerford Farms"         )) return;
                        if (Apply("The Bramble Patch"              , "Rosewood Log"                       , 16.0, 23.0, "The Hawthorne Hut"        )) return;
                        if (Apply("The Clutch"                     , "Lightning Shard"                    , 29.0, 19.0, "Black Brush Station"      )) return;
                        break;
                    case 50 + GatheringType.Mining:
                        if (Apply("Bluefog"                        , "Cobalt Ore"                         , 22.0, 24.0, "Ceruleum Processing Plant")) return;
                        if (Apply("Riversmeet"                     , "Dragon Obsidian"                    , 28.0, 27.0, "Falcon's Nest"            )) return;
                        break;
                    case 50 + GatheringType.Quarrying:
                        if (Apply("Quarterstone"                   , "Ice Shard"                          , 34.0, 28.0, "Swiftperch"               )) return;
                        if (Apply("The Bramble Patch"              , "Wind Shard"                         , 18.0, 24.0, "The Hawthorne Hut"        )) return;
                        if (Apply("The Gods' Grip"                 , "Earth Shard"                        , 22.0, 34.0, "Moraby Drydocks"          )) return;
                        if (Apply("The Silver Bazaar"              , "Water Shard"                        , 18.0, 28.0, "Horizon"                  )) return;
                        break;
                    case 45 + GatheringType.Harvesting:
                        if (Apply("Bronze Lake"                    , "Sagolii Sage"                       , 35.0, 24.0, "Camp Bronze Lake"         )) return;
                        break;
                    case 45 + GatheringType.Logging:
                        if (Apply("Whitebrim"                      , "Mirror Apple"                       , 23.0, 17.0, "Camp Dragonhead"          )) return;
                        break;
                    case 45 + GatheringType.Mining:
                        if (Apply("Bronze Lake"                    , "Raw Turquoise"                      , 30.0, 25.0, "Camp Bronze Lake"         )) return;
                        if (Apply("Drybone"                        , "Raw Amber"                          , 12.0, 19.0, "Camp Drybone"             )) return;
                        break;
                    case 45 + GatheringType.Quarrying:
                        if (Apply("Bronze Lake"                    , "Electrum Sand"                      , 28.0, 22.0, "Camp Bronze Lake"         )) return;
                        break;
                    case 40 + GatheringType.Harvesting:
                        if (Apply("Lower Paths"                    , "Thyme"                              , 21.0, 29.0, "Camp Tranquil"            )) return;
                        if (Apply("Raincatcher Gully"              , "Mugwort"                            , 21.0, 29.0, "Wineport"                 )) return;
                        break;
                    case 40 + GatheringType.Logging:
                        if (Apply("Raincatcher Gully"              , "Iron Acorn"                         , 19.0, 25.0, "Wineport"                 )) return;
                        break;
                    case 40 + GatheringType.Mining:
                        if (Apply("Dragonhead"                     , "Raw Zircon"                         , 24.0, 19.0, "Camp Dragonhead"          )) return;
                        if (Apply("Urth's Gift"                    , "Raw Spinel"                         , 28.0, 22.0, "Quarrymill"               )) return;
                        break;
                    case 40 + GatheringType.Quarrying:
                        if (Apply("Bluefog"                        , "Grenade Ash"                        , 21.0, 28.0, "Ceruleum Processing Plant")) return;
                        break;
                    case 35 + GatheringType.Harvesting:
                        if (Apply("Bloodshore"                     , "Midland Basil"                      , 26.0, 30.0, "Costa del Sol"            )) return;
                        if (Apply("Broken Water"                   , "Aloe"                               , 20.0,  7.0, "Little Ala Mhigo"         )) return;
                        if (Apply("Lower Paths"                    , "White Truffle"                      , 17.0, 28.0, "Camp Tranquil"            )) return;
                        break;
                    case 35 + GatheringType.Logging:
                        if (Apply("Lower Paths"                    , "Oak Log"                            , 16.0, 30.0, "Camp Tranquil"            )) return;
                        break;
                    case 35 + GatheringType.Mining:
                        if (Apply("Bloodshore"                     , "Raw Aquamarine"                     , 28.0, 27.0, "Costa del Sol"            )) return;
                        if (Apply("Sagolii Desert"                 , "Raw Heliodor"                       , 25.0, 41.0, "Forgotten Springs"        )) return;
                        if (Apply("Sorrel Haven"                   , "Raw Peridot"                        , 14.0, 21.0, "Bentbranch Meadows"       )) return;
                        break;
                    case 35 + GatheringType.Quarrying:
                        if (Apply("The Red Labyrinth"              , "Mythril Sand"                       , 17.0, 18.0, "Little Ala Mhigo"         )) return;
                        break;
                    case 30 + GatheringType.Harvesting:
                        if (Apply("Alder Springs"                  , "Jade Peas"                          , 22.0, 25.0, "Fallgourd Float"          )) return;
                        break;
                    case 30 + GatheringType.Logging:
                        if (Apply("Bentbranch"                     , "Green Pigment"                      , 24.0, 30.0, "Bentbranch Meadows"       )) return;
                        if (Apply("Bloodshore"                     , "Blue Pigment"                       , 28.0, 33.0, "Costa del Sol"            )) return;
                        if (Apply("Peacegarden"                    , "Brown Pigment"                      , 27.0, 22.0, "Fallgourd Float"          )) return;
                        if (Apply("Silent Arbor"                   , "Alligator Pear"                     , 26.0, 19.0, "Quarrymill"               )) return;
                        if (Apply("Spineless Basin"                , "Purple Pigment"                     , 24.0, 31.0, "Black Brush Station"      )) return;
                        if (Apply("Three-malm Bend"                , "Red Pigment"                        , 16.0, 13.0, "Summerford Farms"         )) return;
                        if (Apply("Upper Paths"                    , "Grey Pigment"                       , 16.0, 21.0, "Quarrymill"               )) return;
                        break;
                    case 30 + GatheringType.Mining:
                        if (Apply("Wellwick Wood"                  , "Wyvern Obsidian"                    , 26.0, 17.0, "Camp Drybone"             )) return;
                        break;
                    case 30 + GatheringType.Quarrying:
                        if (Apply("Broken Water"                   , "Purple Pigment"                     , 18.0, 11.0, "Little Ala Mhigo"         )) return;
                        if (Apply("Cedarwood"                      , "Brown Pigment"                      , 26.0, 15.0, "Moraby Drydocks"          )) return;
                        if (Apply("Nine Ivies"                     , "Green Pigment"                      , 20.0, 27.0, "The Hawthorne Hut"        )) return;
                        if (Apply("Nophica's Wells"                , "Blue Pigment"                       , 23.0, 23.0, "Horizon"                  )) return;
                        if (Apply("Oakwood"                        , "Brimstone"                          , 12.0, 26.0, "Camp Bronze Lake"         )) return;
                        if (Apply("Quarterstone"                   , "Grey Pigment"                       , 31.0, 28.0, "Swiftperch"               )) return;
                        if (Apply("Sagolii Desert"                 , "Bomb Ash"                           , 22.0, 29.0, "Forgotten Springs"        )) return;
                        if (Apply("Wellwick Wood"                  , "Red Pigment"                        , 23.0, 19.0, "Camp Drybone"             )) return;
                        break;
                    case 25 + GatheringType.Harvesting:
                        if (Apply("Drybone"                        , "Button Mushroom"                    , 14.0, 20.0, "Camp Drybone"             )) return;
                        if (Apply("Oakwood"                        , "Pixie Plums"                        , 14.0, 24.0, "Camp Bronze Lake"         )) return;
                        break;
                    case 25 + GatheringType.Logging:
                        if (Apply("Upper Paths"                    , "Matron's Mistletoe"                 , 23.0, 21.0, "Quarrymill"               )) return;
                        break;
                    case 25 + GatheringType.Mining:
                        if (Apply("Upper Paths"                    , "Silver Ore"                         , 15.0, 19.0, "Quarrymill"               )) return;
                        break;
                    case 25 + GatheringType.Quarrying:
                        if (Apply("Oakwood"                        , "Fire Rock"                          , 12.0, 23.0, "Camp Bronze Lake"         )) return;
                        if (Apply("Upper Paths"                    , "Earth Rock"                         , 23.0, 21.0, "Quarrymill"               )) return;
                        break;
                    case 20 + GatheringType.Harvesting:
                        if (Apply("Nine Ivies"                     , "Gil Bun"                            , 18.0, 28.0, "The Hawthorne Hut"        )) return;
                        if (Apply("Quarterstone"                   , "Paprika"                            , 31.0, 28.0, "Swiftperch"               )) return;
                        if (Apply("Sandgate"                       , "Popoto"                             , 16.0, 27.0, "Camp Drybone"             )) return;
                        break;
                    case 20 + GatheringType.Logging:
                        if (Apply("Black Brush"                    , "Nopales"                            , 21.0, 20.0, "Black Brush Station"      )) return;
                        if (Apply("Cedarwood"                      , "Sun Lemon"                          , 34.0, 17.0, "Moraby Drydocks"          )) return;
                        if (Apply("Nine Ivies"                     , "Faerie Apple"                       , 15.0, 27.0, "The Hawthorne Hut"        )) return;
                        if (Apply("Skull Valley"                   , "Grade 1 Carbonized Matter"          , 26.0, 23.0, "Aleport"                  )) return;
                        break;
                    case 20 + GatheringType.Mining:
                        if (Apply("Drybone"                        , "Raw Malachite"                      , 17.0, 20.0, "Camp Drybone"             )) return;
                        if (Apply("Peacegarden"                    , "Raw Sphene"                         , 29.0, 22.0, "Fallgourd Float"          )) return;
                        if (Apply("Skull Valley"                   , "Raw Danburite"                      , 29.0, 22.0, "Aleport"                  )) return;
                        break;
                    case 20 + GatheringType.Quarrying:
                        if (Apply("Skull Valley"                   , "Mudstone"                           , 26.0, 24.0, "Aleport"                  )) return;
                        if (Apply("Three-malm Bend"                , "Grade 1 Carbonized Matter"          , 15.0, 10.0, "Summerford Farms"         )) return;
                        break;
                    case 15 + GatheringType.Harvesting:
                        if (Apply("Bentbranch"                     , "Marjoram"                           , 18.0, 19.0, "Bentbranch Meadows"       )) return;
                        if (Apply("Bentbranch"                     , "Carnation"                          , 22.0, 24.0, "Bentbranch Meadows"       )) return;
                        if (Apply("Horizon's Edge"                 , "Coerthan Carrot"                    , 23.0, 18.0, "Horizon"                  )) return;
                        if (Apply("Moraby Bay"                     , "Cinderfoot Olive"                   , 26.0, 22.0, "Moraby Drydocks"          )) return;
                        if (Apply("Nophica's Wells"                , "Garlean Garlic"                     , 23.0, 23.0, "Horizon"                  )) return;
                        if (Apply("Summerford"                     , "Sunset Wheat"                       , 22.0, 19.0, "Summerford Farms"         )) return;
                        if (Apply("The Clutch"                     , "Alpine Parsnip"                     , 25.0, 20.0, "Black Brush Station"      )) return;
                        break;
                    case 15 + GatheringType.Logging:
                        if (Apply("Bentbranch"                     , "Elm Log"                            , 20.0, 20.0, "Bentbranch Meadows"       )) return;
                        break;
                    case 15 + GatheringType.Mining:
                        if (Apply("Horizon's Edge"                 , "Iron Ore"                           , 27.0, 17.0, "Horizon"                  )) return;
                        break;
                    case 15 + GatheringType.Quarrying:
                        if (Apply("Black Brush"                    , "Rock Salt"                          , 14.0, 23.0, "Black Brush Station"      )) return;
                        if (Apply("Horizon's Edge"                 , "Cinnabar"                           , 24.0, 18.0, "Horizon"                  )) return;
                        break;
                    case 10 + GatheringType.Logging:
                        if (Apply("Cedarwood"                      , "La Noscean Orange"                  , 32.0, 16.0, "Moraby Drydocks"          )) return;
                        if (Apply("Greentear"                      , "Ash Log"                            , 25.0, 20.0, "Bentbranch Meadows"       )) return;
                        if (Apply("Spineless Basin"                , "Cock Feather"                       , 22.0, 26.0, "Black Brush Station"      )) return;
                        if (Apply("Treespeak"                      , "Ash Log"                            , 27.0, 24.0, "Fallgourd Float"          )) return;
                        break;
                    case 10 + GatheringType.Mining:
                        if (Apply("Black Brush"                    , "Tin Ore"                            , 20.0, 22.0, "Black Brush Station"      )) return;
                        if (Apply("Cedarwood"                      , "Raw Sunstone"                       , 27.0, 18.0, "Moraby Drydocks"          )) return;
                        if (Apply("Hammerlea"                      , "Tin Ore"                            , 22.0, 28.0, "Horizon"                  )) return;
                        if (Apply("Treespeak"                      , "Raw Lapis Lazuli"                   , 28.0, 25.0, "Fallgourd Float"          )) return;
                        break;
                    case  5 + GatheringType.Logging:
                        if (Apply("Gelmorra Ruins"                 , "Earth Shard"                        , 26.5, 21.9, "Fallgourd Float"          )) return;
                        if (Apply("Jadeite Thick"                  , "Latex"                              , 23.0, 18.0, "Bentbranch Meadows"       )) return;
                        if (Apply("The Tangle"                     , "Lightning Shard"                    , 14.5, 14.3, "Revenant's Toll"          )) return;
                        if (Apply("Treespeak"                      , "Latex"                              , 28.0, 26.0, "Fallgourd Float"          )) return;
                        if (Apply("Treespeak"                      , "Maple Sap"                          , 25.0, 27.0, "Fallgourd Float"          )) return;
                        if (Apply("Whitebrim"                      , "Ice Shard"                          , 26.6, 19.7, "Camp Dragonhead"          )) return;
                        break;
                    case  5 + GatheringType.Mining:
                        if (Apply("Gelmorra Ruins"                 , "Earth Shard"                        , 25.3, 22.2, "Fallgourd Float"          )) return;
                        if (Apply("Hammerlea"                      , "Copper Ore"                         , 26.0, 25.0, "Horizon"                  )) return;
                        if (Apply("Spineless Basin"                , "Copper Ore"                         , 18.8, 25.7, "Black Brush Station"      )) return;
                        if (Apply("Spineless Basin"                , "Bone Chip"                          , 24.0, 26.0, "Black Brush Station"      )) return;
                        if (Apply("The Tangle"                     , "Lightning Shard"                    , 15.2, 14.7, "Revenant's Toll"          )) return;
                        if (Apply("Whitebrim"                      , "Ice Shard"                          , 28.0, 16.5, "Camp Dragonhead"          )) return;
                        break;
                }
            // @formatter:on
        }
    }
}
