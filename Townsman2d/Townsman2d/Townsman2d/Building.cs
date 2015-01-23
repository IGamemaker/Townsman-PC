using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Townsman2d
{
    enum BuildingType
    { 
        Forester=0,
        Lumbermen=1,
        StoneQuary=2,
        Foundry=3,
        Blacksmith=4,
        Armourer=5,
        Farm=6,
        Fisherman=7,
        Market=8,
        Church=9,
        Castle=10,
        Barracs=11,
        Mill=12
    }

    class Building
    {
        public BuildingType type;

        public int productive;
        public int resources;
        public int lvl;

        public float starttime;

        public bool inproduct;
        public bool outproduct;
        public bool needproduct;

        public int temp;

        public double wait;

        public Resource[] cost { get { return Helper.buildingHelper[(int)type, lvl].buildcost; } }
        public Resource[] upgradecost { get { return Helper.buildingHelper[(int)type, lvl+1].buildcost; } }
        public Resource[] upkeep { get { return Helper.buildingHelper[(int)type, lvl].upkeepcost; } }
        public Resource[] production { get { return Helper.buildingHelper[(int)type, lvl].outproduct; } }
        public Resource[] neededresources { get { return Helper.buildingHelper[(int)type, lvl].inproduct; } }
        public int upgradetime { get { return Helper.buildingHelper[(int)type, lvl + 1].buildtime; } }
        public int buildtime { get { return Helper.buildingHelper[(int)type, lvl].buildtime; } }
        public int storage { get { int l = lvl; if (l > 0 && wait > 0)l--; return Helper.buildingHelper[(int)type, l].maxproduction; } }
        public int maxprouctive { get { if (lvl == 0)return 2; if (lvl == 1)return 3; return 5; } }

        public Building(BuildingType bt, float time)
        {
            type = bt;
            productive = 2;
            resources = 0;
            starttime = time;
            lvl = 0;
            inproduct = outproduct = needproduct = false;
            wait = 0;
        }
    }

    class LvlBuildingHelp
    {
        public Resource[] buildcost;
        public Resource[] upkeepcost;
        public Resource[] inproduct;
        public Resource[] outproduct;
        public int buildtime;
        public int maxproduction;
        public Resource[] storage;
        public LvlBuildingHelp(Resource[] c,
                            Resource[] uc,
                            Resource[] ip,
                            Resource[] op,
                            int t,
                            int mp, Resource[] st) 
        {
            buildcost = c;
            upkeepcost = uc;
            inproduct = ip;
            outproduct = op;
            buildtime = t;
            maxproduction = mp;
            storage = st;
        }
    }
}
