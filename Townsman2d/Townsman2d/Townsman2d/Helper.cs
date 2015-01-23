using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Townsman2d
{
    static class Helper
    {
        static public LvlBuildingHelp[,] buildingHelper;
        static public ResourceHelp[] resourceHelper;
        static public TerrainHelp[] terrainHelper;

        static public void Init()
        {
            buildingHelper = new LvlBuildingHelp[13,3];

            #region Forester
            buildingHelper[0, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Wood, 3), new Resource(ResourceType.Gold, 3) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] { },
                                                 new Resource[] { new Resource(ResourceType.Wood, 1) },
                                                 1,
                                                 4,
                                                 new Resource[] { new Resource(ResourceType.Wood, 4) });
            buildingHelper[0, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 5), new Resource(ResourceType.Stone, 5),new Resource(ResourceType.Gold,40) },
                                                new Resource[] { new Resource(ResourceType.Food, 2),new Resource(ResourceType.Tools,1),new Resource(ResourceType.Gold,2) },
                                                new Resource[] { },
                                                new Resource[] { new Resource(ResourceType.Wood, 2) },
                                                2,
                                                16,
                                                new Resource[] { new Resource(ResourceType.Wood, 16) });
            buildingHelper[0, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 25), new Resource(ResourceType.Stone, 20),new Resource(ResourceType.Tools,10), new Resource(ResourceType.Gold, 150) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Tools, 2), new Resource(ResourceType.Gold, 4) },
                                                new Resource[] { },
                                                new Resource[] { new Resource(ResourceType.Wood, 3) },
                                                3,
                                                60,
                                                new Resource[] { new Resource(ResourceType.Wood, 60) });
            #endregion
            #region Lumbermen
            buildingHelper[1, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Wood, 10), new Resource(ResourceType.Gold, 10) },
                                                 new Resource[] { new Resource(ResourceType.Food, 2) },
                                                 new Resource[] { new Resource(ResourceType.Wood,1)},
                                                 new Resource[] { new Resource(ResourceType.Lumber, 1) },
                                                 2,
                                                 4,
                                                 new Resource[] { new Resource(ResourceType.Lumber, 4) });
            buildingHelper[1, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 14), new Resource(ResourceType.Stone, 5),new Resource(ResourceType.Tools,2), new Resource(ResourceType.Gold, 70) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Tools, 1), new Resource(ResourceType.Gold, 3) },
                                                new Resource[] { new Resource(ResourceType.Wood, 1) },
                                                new Resource[] { new Resource(ResourceType.Lumber, 2) },
                                                4,
                                                20,
                                                new Resource[] { new Resource(ResourceType.Lumber, 20) });
            buildingHelper[1, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 32), new Resource(ResourceType.Stone, 15), new Resource(ResourceType.Tools, 10), new Resource(ResourceType.Gold, 160) },
                                                new Resource[] { new Resource(ResourceType.Food, 4), new Resource(ResourceType.Tools, 2), new Resource(ResourceType.Gold, 5) },
                                                new Resource[] { new Resource(ResourceType.Wood, 1), new Resource(ResourceType.Tools, 1) },
                                                new Resource[] { new Resource(ResourceType.Lumber, 3) },
                                                6,
                                                60,
                                                new Resource[] { new Resource(ResourceType.Lumber, 60) });
            #endregion
            #region StoneQuary
            buildingHelper[2, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Wood, 3),new Resource(ResourceType.Lumber,2), new Resource(ResourceType.Gold, 5) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] { },
                                                 new Resource[] { new Resource(ResourceType.Stone, 1) },
                                                 1,
                                                 4,
                                                 new Resource[] { new Resource(ResourceType.Stone, 4) });
            buildingHelper[2, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 10), new Resource(ResourceType.Stone, 5), new Resource(ResourceType.Gold, 75) },
                                                new Resource[] { new Resource(ResourceType.Food, 2), new Resource(ResourceType.Gold, 1) },
                                                new Resource[] {  },
                                                new Resource[] { new Resource(ResourceType.Stone, 2) },
                                                2,
                                                20,
                                                new Resource[] { new Resource(ResourceType.Stone, 20) });
            buildingHelper[2, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 30), new Resource(ResourceType.Stone, 10), new Resource(ResourceType.Gold, 190) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] {  },
                                                new Resource[] { new Resource(ResourceType.Stone, 3) },
                                                3,
                                                60,
                                                new Resource[] { new Resource(ResourceType.Stone, 60) });
            #endregion
            #region Foundry
            buildingHelper[3, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Wood, 2), new Resource(ResourceType.Lumber, 6), new Resource(ResourceType.Stone, 4), new Resource(ResourceType.Gold, 12) },
                                                 new Resource[] { new Resource(ResourceType.Food, 2) },
                                                 new Resource[] { new Resource(ResourceType.Wood,2) },
                                                 new Resource[] { new Resource(ResourceType.Iron, 1) },
                                                 2,
                                                 4,
                                                 new Resource[] { new Resource(ResourceType.Iron, 4) });
            buildingHelper[3, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 18), new Resource(ResourceType.Stone, 10),new Resource(ResourceType.Tools,6), new Resource(ResourceType.Gold, 80) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 1) },
                                                new Resource[] { new Resource(ResourceType.Wood, 2) },
                                                new Resource[] { new Resource(ResourceType.Iron, 2) },
                                                4,
                                                20,
                                                new Resource[] { new Resource(ResourceType.Iron, 20) });
            buildingHelper[3, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 38), new Resource(ResourceType.Stone, 20), new Resource(ResourceType.Tools, 18), new Resource(ResourceType.Gold, 160) },
                                                new Resource[] { new Resource(ResourceType.Food, 4), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { new Resource(ResourceType.Wood, 2),new Resource(ResourceType.Tools,2) },
                                                new Resource[] { new Resource(ResourceType.Iron, 4) },
                                                6,
                                                60,
                                                new Resource[] { new Resource(ResourceType.Iron, 60) });
            #endregion
            #region Blacksmith
            buildingHelper[4, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 4), new Resource(ResourceType.Stone, 4), new Resource(ResourceType.Iron, 2), new Resource(ResourceType.Gold, 10) },
                                                 new Resource[] { new Resource(ResourceType.Food, 2) },
                                                 new Resource[] { new Resource(ResourceType.Wood, 1), new Resource(ResourceType.Iron, 1) },
                                                 new Resource[] { new Resource(ResourceType.Tools, 2) },
                                                 3,
                                                 8,
                                                 new Resource[] { new Resource(ResourceType.Tools, 8) });
            buildingHelper[4, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Stone, 15), new Resource(ResourceType.Iron, 14), new Resource(ResourceType.Tools, 16), new Resource(ResourceType.Gold, 140) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 1) },
                                                new Resource[] { new Resource(ResourceType.Wood, 2), new Resource(ResourceType.Iron, 1) },
                                                new Resource[] { new Resource(ResourceType.Tools, 3) },
                                                6,
                                                25,
                                                new Resource[] { new Resource(ResourceType.Tools, 25) });
            buildingHelper[4, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Stone, 38), new Resource(ResourceType.Iron, 30), new Resource(ResourceType.Tools, 10), new Resource(ResourceType.Gold, 400) },
                                                new Resource[] { new Resource(ResourceType.Food, 4), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { new Resource(ResourceType.Wood, 2), new Resource(ResourceType.Iron, 2) },
                                                new Resource[] { new Resource(ResourceType.Tools, 6) },
                                                9,
                                                80,
                                                new Resource[] { new Resource(ResourceType.Tools, 80) });
            #endregion
            #region Armourer
            buildingHelper[5, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Stone, 16), new Resource(ResourceType.Iron, 14), new Resource(ResourceType.Tools, 14), new Resource(ResourceType.Gold, 26) },
                                                 new Resource[] { new Resource(ResourceType.Food, 2) },
                                                 new Resource[] { new Resource(ResourceType.Wood, 1), new Resource(ResourceType.Iron, 1) ,new Resource(ResourceType.Tools,1)},
                                                 new Resource[] { new Resource(ResourceType.Sword, 1) },
                                                 4,
                                                 4,
                                                 new Resource[] { new Resource(ResourceType.Sword, 4) });
            buildingHelper[5, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Stone, 28), new Resource(ResourceType.Iron, 24), new Resource(ResourceType.Tools, 20), new Resource(ResourceType.Gold, 120) },
                                                new Resource[] { new Resource(ResourceType.Food, 3),new Resource(ResourceType.Tools,1), new Resource(ResourceType.Gold, 3) },
                                                new Resource[] { new Resource(ResourceType.Wood, 1), new Resource(ResourceType.Iron, 1), new Resource(ResourceType.Tools, 1) },
                                                new Resource[] { new Resource(ResourceType.Sword, 2) },
                                                8,
                                                20,
                                                new Resource[] { new Resource(ResourceType.Sword, 20) });
            buildingHelper[5, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Stone, 45), new Resource(ResourceType.Iron, 36), new Resource(ResourceType.Tools, 38), new Resource(ResourceType.Gold, 500) },
                                                new Resource[] { new Resource(ResourceType.Food, 4),new Resource(ResourceType.Tools,2), new Resource(ResourceType.Gold, 4) },
                                                new Resource[] { new Resource(ResourceType.Wood, 1), new Resource(ResourceType.Iron, 1), new Resource(ResourceType.Tools, 2) },
                                                new Resource[] { new Resource(ResourceType.Sword, 4) },
                                                12,
                                                60,
                                                new Resource[] { new Resource(ResourceType.Sword, 60) });
            #endregion
            #region Farm
            buildingHelper[6, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 4), new Resource(ResourceType.Corn, 4), new Resource(ResourceType.Gold, 8) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1),new Resource(ResourceType.Lumber,1) },
                                                 new Resource[] { new Resource(ResourceType.Corn, 1) },
                                                 new Resource[] { new Resource(ResourceType.Meat, 2) },
                                                 1,
                                                 6,
                                                 new Resource[] { new Resource(ResourceType.Meat, 6) });
            buildingHelper[6, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 12), new Resource(ResourceType.Corn, 36), new Resource(ResourceType.Stone, 8), new Resource(ResourceType.Gold, 50) },
                                                new Resource[] { new Resource(ResourceType.Food, 2), new Resource(ResourceType.Lumber, 1), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { new Resource(ResourceType.Corn, 1) },
                                                new Resource[] { new Resource(ResourceType.Meat, 3) },
                                                2,
                                                26,
                                                new Resource[] { new Resource(ResourceType.Meat, 26) });
            buildingHelper[6, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 34), new Resource(ResourceType.Corn, 66), new Resource(ResourceType.Stone, 22), new Resource(ResourceType.Gold, 160) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Lumber, 1), new Resource(ResourceType.Gold, 3) },
                                                new Resource[] { new Resource(ResourceType.Corn, 2) },
                                                new Resource[] { new Resource(ResourceType.Meat, 4) },
                                                3,
                                                55,
                                                new Resource[] { new Resource(ResourceType.Meat, 55) });
            #endregion
            #region Fisherman
            buildingHelper[7, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Wood, 5), new Resource(ResourceType.Gold, 5) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] {  },
                                                 new Resource[] { new Resource(ResourceType.Fish, 1) },
                                                 1,
                                                 6,
                                                 new Resource[] { new Resource(ResourceType.Fish, 6) });
            buildingHelper[7, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 25), new Resource(ResourceType.Stone, 12),new Resource(ResourceType.Tools,5), new Resource(ResourceType.Gold, 50) },
                                                new Resource[] { new Resource(ResourceType.Food, 2),new Resource(ResourceType.Gold, 1) },
                                                new Resource[] {  },
                                                new Resource[] { new Resource(ResourceType.Fish, 2) },
                                                2,
                                                24,
                                                new Resource[] { new Resource(ResourceType.Fish, 24) });
            buildingHelper[7, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 48), new Resource(ResourceType.Stone, 22), new Resource(ResourceType.Tools, 18), new Resource(ResourceType.Gold, 120) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { new Resource(ResourceType.Tools, 1) },
                                                new Resource[] { new Resource(ResourceType.Fish, 4) },
                                                3,
                                                50,
                                                new Resource[] { new Resource(ResourceType.Fish, 50) });
            #endregion
            #region Market
            buildingHelper[8, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 15), new Resource(ResourceType.Stone, 10), new Resource(ResourceType.Gold, 30) },
                                                 new Resource[] { new Resource(ResourceType.Gold, 1) },
                                                 new Resource[] { },
                                                 new Resource[] {  },
                                                 4,
                                                 180,
                                                 new Resource[] {  });
            buildingHelper[8, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 25), new Resource(ResourceType.Stone, 25), new Resource(ResourceType.Gold, 80) },
                                                new Resource[] { new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { },
                                                new Resource[] {  },
                                                6,
                                                450,
                                                new Resource[] {  });
            buildingHelper[8, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 35), new Resource(ResourceType.Stone, 26), new Resource(ResourceType.Tools, 16), new Resource(ResourceType.Gold, 140) },
                                                new Resource[] { new Resource(ResourceType.Gold, 4) },
                                                new Resource[] {  },
                                                new Resource[] {  },
                                                8,
                                                900,
                                                new Resource[] {  });
            #endregion
            #region Church
            buildingHelper[9, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 6), new Resource(ResourceType.Stone, 12), new Resource(ResourceType.Gold, 18) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] { },
                                                 new Resource[] { new Resource(ResourceType.Gold, 1) },
                                                 4,
                                                 10,
                                                 new Resource[] { new Resource(ResourceType.Gold, 10) });
            buildingHelper[9, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 18), new Resource(ResourceType.Stone, 25),new Resource(ResourceType.Tools,4), new Resource(ResourceType.Gold, 48) },
                                                new Resource[] { new Resource(ResourceType.Food, 2) },
                                                new Resource[] { },
                                                new Resource[] { new Resource(ResourceType.Gold, 1) },
                                                8,
                                                20,
                                                new Resource[] { new Resource(ResourceType.Gold, 20) });
            buildingHelper[9, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 30), new Resource(ResourceType.Stone, 40), new Resource(ResourceType.Tools, 25), new Resource(ResourceType.Gold, 95) },
                                                new Resource[] { new Resource(ResourceType.Food,3 ) },
                                                new Resource[] { },
                                                new Resource[] { new Resource(ResourceType.Gold, 1) },
                                                12,
                                                30,
                                                new Resource[] { new Resource(ResourceType.Gold, 30) });
            #endregion
            #region Castle
            buildingHelper[10, 0] = new LvlBuildingHelp( new Resource[] { },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] { },
                                                 new Resource[] { },
                                                 10,
                                                 180,
                                                 new Resource[] { new Resource(ResourceType.Wood, 20), new Resource(ResourceType.Lumber, 20), new Resource(ResourceType.Corn, 20), new Resource(ResourceType.Fish, 20), new Resource(ResourceType.Meat, 20), new Resource(ResourceType.Stone, 20), new Resource(ResourceType.Iron, 20), new Resource(ResourceType.Tools, 20), new Resource(ResourceType.Sword, 20) });
            buildingHelper[10, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 18), new Resource(ResourceType.Stone, 18),new Resource(ResourceType.Gold, 90) },
                                                new Resource[] { new Resource(ResourceType.Food, 2), new Resource(ResourceType.Gold, 1) },
                                                new Resource[] { },
                                                new Resource[] { },
                                                20,
                                                450,
                                                new Resource[] { new Resource(ResourceType.Wood, 50), new Resource(ResourceType.Lumber, 50), new Resource(ResourceType.Corn, 50), new Resource(ResourceType.Fish, 50), new Resource(ResourceType.Meat, 50), new Resource(ResourceType.Stone, 50), new Resource(ResourceType.Iron, 50), new Resource(ResourceType.Tools, 50), new Resource(ResourceType.Sword, 50) });
            buildingHelper[10, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 30), new Resource(ResourceType.Stone, 24), new Resource(ResourceType.Tools, 15), new Resource(ResourceType.Gold, 160) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { },
                                                new Resource[] { },
                                                30,
                                                900,
                                                new Resource[] { new Resource(ResourceType.Wood, 99), new Resource(ResourceType.Lumber, 99), new Resource(ResourceType.Corn, 99), new Resource(ResourceType.Fish, 99), new Resource(ResourceType.Meat, 99), new Resource(ResourceType.Stone, 99), new Resource(ResourceType.Iron, 99), new Resource(ResourceType.Tools, 99), new Resource(ResourceType.Sword, 99) });
            #endregion
            #region Barracs
            buildingHelper[11, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 10), new Resource(ResourceType.Stone, 10),new Resource(ResourceType.Iron,4), new Resource(ResourceType.Gold, 30) },
                                                 new Resource[] { new Resource(ResourceType.Food, 2),new Resource(ResourceType.Sword,1),new Resource(ResourceType.Gold,1) },
                                                 new Resource[] { },
                                                 new Resource[] { },
                                                 5,
                                                 2,
                                                 new Resource[] { new Resource(ResourceType.Soldier,2)});
            buildingHelper[11, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 28), new Resource(ResourceType.Stone, 32), new Resource(ResourceType.Tools, 12), new Resource(ResourceType.Gold, 135) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Sword, 1), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] { },
                                                new Resource[] { },
                                                10,
                                                3,
                                                new Resource[] { new Resource(ResourceType.Soldier, 3) });
            buildingHelper[11, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 45), new Resource(ResourceType.Stone, 45), new Resource(ResourceType.Tools, 26), new Resource(ResourceType.Gold, 215) },
                                                new Resource[] { new Resource(ResourceType.Food, 4), new Resource(ResourceType.Sword, 2), new Resource(ResourceType.Gold, 4) },
                                                new Resource[] { },
                                                new Resource[] { },
                                                15,
                                                5,
                                                new Resource[] { new Resource(ResourceType.Soldier, 5) });
            #endregion
            #region Mill
            buildingHelper[12, 0] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 8), new Resource(ResourceType.Gold, 8) },
                                                 new Resource[] { new Resource(ResourceType.Food, 1) },
                                                 new Resource[] { },
                                                 new Resource[] { new Resource(ResourceType.Corn, 2) },
                                                 1,
                                                 6,
                                                 new Resource[] { new Resource(ResourceType.Corn, 6) });
            buildingHelper[12, 1] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 20), new Resource(ResourceType.Stone, 10), new Resource(ResourceType.Tools, 6), new Resource(ResourceType.Gold, 60) },
                                                new Resource[] { new Resource(ResourceType.Food, 2), new Resource(ResourceType.Gold, 1) },
                                                new Resource[] { },
                                                new Resource[] { new Resource(ResourceType.Corn, 3) },
                                                2,
                                                24,
                                                new Resource[] { new Resource(ResourceType.Corn, 24) });
            buildingHelper[12, 2] = new LvlBuildingHelp(new Resource[] { new Resource(ResourceType.Lumber, 38), new Resource(ResourceType.Stone, 25 ), new Resource(ResourceType.Tools, 16), new Resource(ResourceType.Gold, 40) },
                                                new Resource[] { new Resource(ResourceType.Food, 3), new Resource(ResourceType.Gold, 2) },
                                                new Resource[] {  },
                                                new Resource[] { new Resource(ResourceType.Corn, 5) },
                                                3,
                                                72,
                                                new Resource[] { new Resource(ResourceType.Corn, 72) });
            #endregion

            resourceHelper = new ResourceHelp[10] { new ResourceHelp(2, 3, 4, 1, 2, 3), 
                                                    new ResourceHelp(-1,-1,-1,-1,-1,-1),
                                                    new ResourceHelp(4,5,6,2,3,4),

                                                    new ResourceHelp(5,6,10,3,4,7), 
                                                    new ResourceHelp(1,1,1,1,1,1),
                                                    new ResourceHelp(1,1,1,1,1,1),

                                                    new ResourceHelp(1,1,1,1,1,1), 
                                                    new ResourceHelp(2,3,1,1,2,1),
                                                    new ResourceHelp(3,3,2,1,2,2),

                                                    new ResourceHelp(4,6,7,2,4,5)};
            terrainHelper = new TerrainHelp[7] { new TerrainHelp(5,50),
                                                 new TerrainHelp(-1,-1),
                                                 new TerrainHelp(10,75),
                                                 new TerrainHelp(-1,-1),
                                                 new TerrainHelp(6,20),
                                                 new TerrainHelp(15,75),
                                                 new TerrainHelp(-1,-1)};

            
        }
    }
}
