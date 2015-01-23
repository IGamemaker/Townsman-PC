using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Townsman2d
{
    class Inventory
    {
        public int wood;
        public int lumber;
        public int corn;
        public int fish;
        public int meat;
        public int stone;
        public int iron;
        public int tools;
        public int swords;
        public int gold;

        public Inventory() { ;}

        public int Length 
        {
            get
            {
                return wood > 0 ? 1 : 0 +
                       lumber > 0 ? 1 : 0 +
                       corn > 0 ? 1 : 0 +
                       fish > 0 ? 1 : 0 +
                       meat > 0 ? 1 : 0 +
                       stone > 0 ? 1 : 0 +
                       iron > 0 ? 1 : 0 +
                       tools > 0 ? 1 : 0 +
                       swords > 0 ? 1 : 0;
            }
        }
        public int Number { get { return wood + lumber + corn + fish + meat + stone + iron + tools + swords+gold; } }
        public int Food { get { return meat + fish + corn; } }
        public ResourceType ResourceType
        {
            get
            {
                if (Length > 1)
                    return ResourceType.Castle;
                if (wood > 0) return ResourceType.Wood;
                if (lumber > 0) return ResourceType.Lumber;
                if (corn > 0) return ResourceType.Corn;
                if (fish > 0) return ResourceType.Fish;
                if (meat > 0) return ResourceType.Meat;
                if (stone > 0) return ResourceType.Stone;
                if (iron > 0) return ResourceType.Iron;
                if (tools > 0) return ResourceType.Tools;
                if (swords > 0) return ResourceType.Sword;
                if (gold > 0) return ResourceType.Gold;
                return ResourceType.Castle;
            }
        }
        public bool SetMaxResourceLvl(int lvl)
        {
            bool r = false;
            if (wood > lvl) { wood = lvl; r = true; }
            if (lumber > lvl) { lumber = lvl; r = true; }
            if (corn > lvl) { corn = lvl; r = true; }
            if (fish > lvl) { fish = lvl; r = true; }
            if (meat > lvl) { meat = lvl; r = true; }
            if (stone > lvl) { stone = lvl; r = true; }
            if (iron > lvl) { iron = lvl; r = true; }
            if (tools > lvl) { tools = lvl; r = true; }
            if (swords > lvl) { swords = lvl; r = true; }

            return r;
        }

        public int this[int i]
        {
            get 
            { 
                switch(i)
                {
                    case (int)ResourceType.Wood: return wood;
                    case (int)ResourceType.Lumber: return lumber;
                    case (int)ResourceType.Corn: return corn;
                    case (int)ResourceType.Fish: return fish;
                    case (int)ResourceType.Meat: return meat;
                    case (int)ResourceType.Stone: return stone;
                    case (int)ResourceType.Iron: return iron;
                    case (int)ResourceType.Tools: return tools;
                    case (int)ResourceType.Sword: return swords;
                    case (int)ResourceType.Gold: return gold;
                }
                return 0;
            }

            set
            {
                switch (i)
                {
                    case (int)ResourceType.Wood: wood = value; break;
                    case (int)ResourceType.Lumber: lumber = value; break;
                    case (int)ResourceType.Corn: corn = value; break;
                    case (int)ResourceType.Fish: fish = value; break;
                    case (int)ResourceType.Meat: meat = value; break;
                    case (int)ResourceType.Stone: stone = value; break;
                    case (int)ResourceType.Iron: iron = value; break;
                    case (int)ResourceType.Tools: tools = value; break;
                    case (int)ResourceType.Sword: swords = value; break;
                    case (int)ResourceType.Gold: gold = value; break;
                }
            }
        }

        static public Inventory operator +(Inventory inv1, Inventory inv2)
        {
            inv1.wood += inv2.wood;
            inv1.lumber += inv2.lumber;
            inv1.corn += inv2.corn;
            inv1.fish += inv2.fish;
            inv1.meat += inv2.meat;
            inv1.stone += inv2.stone;
            inv1.iron += inv2.iron;
            inv1.tools += inv2.tools;
            inv1.swords += inv2.swords;
            inv1.gold += inv2.gold;

            return inv1;
        }
        static public Inventory operator -(Inventory inv1, Inventory inv2)
        {
            inv1.wood -= inv2.wood;
            inv1.lumber -= inv2.lumber;
            inv1.corn -= inv2.corn;
            inv1.fish -= inv2.fish;
            inv1.meat -= inv2.meat;
            inv1.stone -= inv2.stone;
            inv1.iron -= inv2.iron;
            inv1.tools -= inv2.tools;
            inv1.swords -= inv2.swords;
            inv1.gold -= inv2.gold;

            return inv1;
        }
        static public Inventory operator *(Inventory inv1,int a)
        {
            inv1.wood *= a;
            inv1.lumber *= a;
            inv1.corn *= a;
            inv1.fish *= a;
            inv1.meat *= a;
            inv1.stone *= a;
            inv1.iron *= a;
            inv1.tools *= a;
            inv1.swords *= a;
            inv1.gold *= a;

            return inv1;
        }
        static public bool operator >(Inventory inv1, Inventory inv2)
        {
            return inv1.wood >= inv2.wood &&
                inv1.lumber >= inv2.lumber &&
                inv1.corn >= inv2.corn &&
                inv1.fish >= inv2.fish &&
                inv1.meat >= inv2.meat &&
                inv1.stone >= inv2.stone &&
                inv1.iron >= inv2.iron &&
                inv1.tools >= inv2.tools &&
                inv1.gold >= inv2.gold;
        }
        static public bool operator <(Inventory inv1, Inventory inv2)
        {
            return inv1.wood < inv2.wood &&
                inv1.lumber < inv2.lumber &&
                inv1.corn < inv2.corn &&
                inv1.fish < inv2.fish &&
                inv1.meat < inv2.meat &&
                inv1.stone < inv2.stone &&
                inv1.iron < inv2.iron &&
                inv1.tools < inv2.tools &&
                inv1.gold < inv2.gold;
        }
        static public Inventory FromResourceArray(Resource[] res)
        {
            Inventory inv=new Inventory();
            foreach (Resource r in res)
            {
                switch (r.type)
                {
                    case ResourceType.Wood: inv.wood += r.number; break;
                    case ResourceType.Lumber: inv.lumber += r.number; break;
                    case ResourceType.Corn: inv.corn += r.number; break;
                    case ResourceType.Fish: inv.fish += r.number; break;
                    case ResourceType.Meat: inv.meat += r.number; break;
                    case ResourceType.Stone: inv.stone += r.number; break;
                    case ResourceType.Iron: inv.iron += r.number; break;
                    case ResourceType.Tools: inv.tools += r.number; break;
                    case ResourceType.Sword: inv.swords += r.number; break;
                    case ResourceType.Gold: inv.gold += r.number; break;
                }
            }

            return inv;
        }
        static public Resource[] ToResourceArray(Inventory inv,int food=0)
        {
            List<Resource> res = new List<Resource>();


            if (inv.wood > 0) res.Add(new Resource(ResourceType.Wood, inv.wood));
            if (inv.lumber > 0) res.Add(new Resource(ResourceType.Lumber, inv.lumber));
            if (inv.fish > 0) res.Add(new Resource(ResourceType.Fish, inv.fish));
            if (inv.meat > 0) res.Add(new Resource(ResourceType.Meat, inv.meat));
            if (inv.stone > 0) res.Add(new Resource(ResourceType.Stone, inv.stone));
            if (inv.iron > 0) res.Add(new Resource(ResourceType.Iron, inv.iron));
            if (inv.tools > 0) res.Add(new Resource(ResourceType.Tools, inv.tools));
            if (inv.swords > 0) res.Add(new Resource(ResourceType.Sword, inv.swords));
            if (inv.corn > 0) res.Add(new Resource(ResourceType.Corn, inv.corn));
            if (inv.gold > 0) res.Add(new Resource(ResourceType.Gold, inv.gold));

            if (food > 0) res.Add(new Resource(ResourceType.Food,food));

            return res.ToArray();
            
        }

        public bool FindNegative()
        {
            bool r = false;
            if (wood < 0) { wood = 0; r = true; }
            if (lumber < 0) { lumber = 0; r = true; }
            if (corn < 0) { corn = 0; r = true; }
            if (fish < 0) { fish = 0; r = true; }
            if (meat < 0) { meat = 0; r = true; }
            if (stone < 0) { stone = 0; r = true; }
            if (iron < 0) { iron = 0; r = true; }
            if (tools < 0) { tools = 0; r = true; }
            if (swords < 0) { swords = 0; r = true; }

            return r;
        }
        public bool RemoveFood2(int food)
        {
            if (food > Food)
            {
                fish = 0;
                corn = 0;
                meat = 0;
                return true;
            }
            if (food >= meat)
            {
                food -= meat;
                meat = 0;
            }
            else { meat -= food; food = 0; }
            if (food >= fish)
            {
                food -= fish;
                fish = 0;
            }
            else { fish -= food; food = 0; }
            if (food >= corn)
            {
                corn = 0;
            }
            else { corn -= food; food = 0; }
            return false;
                
        }
        public bool RemoveFood(int food)
        {
            double fishpr = (double)fish / Food;
            double cornpr = (double)corn / Food;
            double meatpr = 1-fishpr-cornpr;
            int startfood=food;

            int corndt = Math.Min((int)(cornpr * startfood), corn);
            corn -= corndt;
            food -= corndt;

            int fishdt = Math.Min((int)(fishpr * startfood), fish);
            fish -= fishdt;
            food -= fishdt;

            int meatdt = Math.Min(startfood - fishdt - corndt, meat);
            meat -= meatdt;
            food -= meatdt;

            if (food <= fish) { fish -= food; food = 0; }
            else { food -= fish; fish = 0; }

            if (food <= corn) { corn -= food; food = 0; }
            else { food -= corn; corn = 0; }

            return food>0;
        }

        new public List<string> ToString()
        {
            List<string> l = new List<string>();

            l.Add(wood.ToString());
            l.Add(lumber.ToString());
            l.Add(corn.ToString());
            l.Add(fish.ToString());
            l.Add(meat.ToString());
            l.Add(stone.ToString());
            l.Add(iron.ToString());
            l.Add(tools.ToString());
            l.Add(swords.ToString());
            l.Add(gold.ToString());

            return l;
        }
        public void FromString(string[] l,int k=0)
        {
            if (l.Length-k > 9)
            {
                wood = int.Parse(l[k]); k++;
                lumber = int.Parse(l[k]); k++;
                corn = int.Parse(l[k]); k++;
                fish = int.Parse(l[k]); k++;
                meat = int.Parse(l[k]); k++;
                stone = int.Parse(l[k]); k++;
                iron = int.Parse(l[k]); k++;
                tools = int.Parse(l[k]); k++;
                swords = int.Parse(l[k]); k++;
                gold = int.Parse(l[k]); k++;
            }
        }
    }
}
