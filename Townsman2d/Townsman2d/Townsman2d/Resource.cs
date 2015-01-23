using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Townsman2d
{
    enum ResourceType
    { 
        Stone=0,
        Gold=1,
        Iron=2,
        Sword=3,
        Corn=4,
        Fish=5,
        Meat=6,
        Wood=7,
        Lumber=8,
        Tools=9,
        Castle=10,
        Will=11,
        Food=12,
        Soldier,
    }

    enum TerrainType
    { 
        Grass=0,
        Rocks=1,
        Forest=2,
        Iron=3,
        Field=4,
        Lake=5,
        Bog=6
    }

    class Terrain
    {
        public int resources;
        public int maxresources;

        public Terrain(int max)
        {
            maxresources = max;
            resources = max;
        }
    }

    class Resource
    {
        public ResourceType type;
        public int number;

        public Resource(ResourceType t,int n)
        {
            type = t;
            number = n;
        }
    }

    class ResourceHelp
    {
        public int lvl1buy;
        public int lvl1sell;
        public int lvl2buy;
        public int lvl2sell;
        public int lvl3buy;
        public int lvl3sell;

        public ResourceHelp(int b1, int b2, int b3, int s1, int s2, int s3)
        {
            lvl1buy = b1;
            lvl2buy = b2;
            lvl3buy = b3;
            lvl1sell = s1;
            lvl2sell = s2;
            lvl3sell = s3;
        }
    }

    class TerrainHelp
    { 
        public int terraformingtime;
        public int cost;

        public TerrainHelp(int tft,int c)
        {
            terraformingtime = tft;
            cost = c;
        }
    }

    enum IconTexture
    { 
        Attack=0,
        Load=1,
        Save=2,
        Build=3,
        Status=4,
        Down=5,
        Up=6,
        Terraforn=7,
        Upgrade=8,
        Degrade=9,
        Info=10,
        Upkeep=11,
        No=12,
        Yes=13,
        Stone=14,
        Gold=15,
        Iron=16,
        Sword=17,
        Corn=18,
        Fish=19,
        Meat=20,
        Wood=21,
        Lumber=22,
        Tooth=23,
        Inventory=24
    }
}
