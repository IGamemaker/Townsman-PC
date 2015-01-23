using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Townsman2d
{
    enum MapCellType
    {
        Grass1 = 0,
        Grass2 = 1,
        Grass3 = 2,
        Grass4 = 3,
        Rock1 = 4,
        Rock2 = 5,
        Rock3 = 6,
        Rock4 = 7,
        Forest1 = 8,
        Forest2 = 9,
        Forest3 = 10,
        Forest4 = 11,
        Iron1 = 12,
        Iron2 = 13,
        Iron3 = 14,
        Iron4 = 15,
        Field1 = 16,
        Field2 = 17,
        Field3 = 18,
        Field4 = 19,
        Lake1 = 20,
        Lake2 = 21,
        Lake3 = 22,
        Lake4 = 23,
        Bog1 = 24,
        Bog2 = 25,
        Bog3 = 26,
        Bog4 = 27,
        Constuct = 28,
        Mill = 29,
        Barracs = 30,
        Blacksmith = 31,
        Castle = 32,
        Foundry = 33,
        Church = 34,
        Market = 35,
        StoneQuary = 36,
        Fisherman = 37,
        Lumberman = 38,
        Farm = 39,
        Armourer = 40,
        Forester = 41
    }

    enum MapCell
    { 
        Grass=0,
        Resource=1,
        Bog=2,
        Building=3
    }

    class Particle
    {
        public float time;
        public Vector2 pos;
        public Vector2 offcet;

        public Particle(float t, int x, int y)
        {
            time = t;
            pos = new Vector2(x,y);
            offcet = new Vector2(Game.random.Next(-60, 0), Game.random.Next(-30, 15) - 15);
        }
    }

    class Map
    {
        public int[,] mapData;
        public int[,] indexes;
        public double[,] waitTime;
        public List<int>[,] townieindexes;
        public List<Particle>[,] particles;
        public int luck;

        public Inventory resources;
        public Inventory capturedresources;

        public List<Terrain> terrains;
        public List<Building> buildings;
        public List<Townie> townies;
        public Vector2 castleposition;
        public int terraformed;
        public int raidsucces;
        
        public int width, height;

        public Map(int w = 10, int h = 10,int forestcount=0,int mountcount=0,int watercount=0,int resourcecount=0,int bogcount=-1)
        {
            width = w;
            height = h;

            mapData = new int[height, width];
            indexes = new int[height, width];
            waitTime = new double[height, width];
            particles = new List<Particle>[height, width];
            townieindexes = new List<int>[height, width];
            buildings = new List<Building>();
            terrains = new List<Terrain>();
            townies = new List<Townie>();

            luck = 1;

            Random r = new Random();

            int resmin=200, resmax=700;

            switch (resourcecount)
            {
                case -1: resmin = 2; resmax = 10; break;
                case 0: resmin = 20; resmax = 100; break;
                case 2: resmin = 1000; resmax = 10000; break;
            }

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    mapData[i, j] = r.Next(4);

                    if (r.Next(100) < (forestcount + 1) * 10)
                        mapData[i, j] += 8;
                    else if (r.Next(100) < (mountcount + 1) * 10)
                    {
                        if (r.Next(2) == 0)
                            mapData[i, j] += 4;
                        else
                            mapData[i, j] += 12;
                    }
                    else if (r.Next(100) < (watercount + 1) * 10)
                        mapData[i, j] += 20;
                    else if (r.Next(100) < (bogcount + 1) * 10)
                        mapData[i, j] += 24;

                    if (mapData[i, j] > 3)
                    {
                        indexes[i, j] = terrains.Count;
                        terrains.Add(new Terrain(r.Next(resmin, resmax)));
                    }

                    townieindexes[i, j] = new List<int>();
                    particles[i, j] = new List<Particle>();
                }

            castleposition = new Vector2(width / 2, height / 2);
            mapData[height / 2, width / 2] = (int)MapCellType.Castle;
            indexes[height / 2, width / 2] = 0;
            buildings.Add(new Building(BuildingType.Castle, 0));

            /*resources = new Resource[10];
            for (int i = 0; i < 10; i++)
                resources[i] = new Resource((ResourceType)i, 0);*/
            resources = new Inventory();
            capturedresources = new Inventory();

            resources[(int)ResourceType.Wood] = 10;
            resources[(int)ResourceType.Lumber] = 0;
            resources[(int)ResourceType.Gold] = 100;
            resources[(int)ResourceType.Meat] = 5;
            resources[(int)ResourceType.Fish] = 5;
            resources[(int)ResourceType.Corn] = 5;
            resources[(int)ResourceType.Stone] = 5;
            resources[(int)ResourceType.Iron] = 0;
            resources[(int)ResourceType.Tools] = 0;
            resources[(int)ResourceType.Sword] = 90;

            terraformed = 0;
            raidsucces = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Texture2D towniesset,Texture2D[] iconset, Texture2D cursor, Camera camera, double gameTime,SpriteFont spf,Texture2D dusttex)
        {
            for (int i = height-1; i >=0; i--)
                for (int j = 0; j < width; j++)
                {
                    spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector((MapCellType)(mapData[i, j] % 4)), GetSourceRectangle((MapCellType)(mapData[i, j] % 4), gameTime), camera.light);//, 0, GetOriginVector((MapCellType)(mapData[i, j] % 4)), 1, SpriteEffects.None, camera.GetZ((height - i) * width + (j)));
                    townieindexes[i, j].Clear();
                }

            for (int i = 0; i < townies.Count; i++)
            {
                if (townies[i].position.X - (int)townies[i].position.X > 0)
                {
                    if (townies[i].position.Y < height && townies[i].position.X < width && townies[i].position.Y >-1 && townies[i].position.X >= 0)
                        townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X].Add(i);
                    if (townies[i].position.Y < height && townies[i].position.X + 1 < width && townies[i].position.Y >-1 && townies[i].position.X + 1 >= 0)
                        townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X + 1].Add(i);
                }
                else if (townies[i].position.Y - (int)townies[i].position.Y > 0)
                {
                    if (townies[i].position.Y < height && townies[i].position.X <= width-1 && townies[i].position.Y > -1 && townies[i].position.X > -2)
                    {
                        
                        if (townies[i].position.X > -1) townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X].Add(i);
                        else townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X+1].Add(i);
                    }
                    if (townies[i].position.Y + 1 < height && townies[i].position.X <= width-1 && townies[i].position.Y + 1 > -1 && townies[i].position.X > -2)
                    {
                        //if (townies[i].position.X <= width-1) 
                        if (townies[i].position.X > -1)
                            townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X].Add(i);
                        else townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X + 1].Add(i);
                        //else townieindexes[(int)townies[i].position.Y + 1, (int)townies[i].position.X].Add(i);
                    }
                }
                else townieindexes[(int)townies[i].position.Y, (int)townies[i].position.X].Add(i);
            }

            for (int i = height -1; i >=0; i--)
                for (int j = 0; j <width; j++)
                {
                    if (mapData[i, j] > 3)
                    {
                        bool construct = waitTime[i, j] > 0 && MapCellTypeToMapCell((MapCellType)mapData[i, j]) == MapCell.Building;
                        if (construct) spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector(MapCellType.Constuct), GetSourceRectangle(MapCellType.Constuct, gameTime, i + j), camera.light);//, 0, GetOriginVector(MapCellType.Constuct), 1, SpriteEffects.None, camera.GetZ(10*((height - i) * width + j),2));
                        else spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector((MapCellType)mapData[i, j]), GetSourceRectangle((MapCellType)mapData[i, j], gameTime, i + j), camera.light);//, 0, GetOriginVector((MapCellType)mapData[i, j]), 1, SpriteEffects.None, camera.GetZ(10*((height - i) * width + j),2));
                    }

                    foreach (int id in townieindexes[i, j])
                    {
                        townies[id].Draw(spriteBatch, towniesset,iconset, camera, this, gameTime);
                    }

                    foreach (Particle p in particles[i,j])
                    {
                        spriteBatch.Draw(dusttex, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet + p.offcet,
                                         new Rectangle(0, (2 - (int)(p.time*3.99)%3) * 48, 48, 48), camera.light);
                    }

                    if (waitTime[i, j] > 0&&new Random().Next(55)==0) particles[i, j].Add(new Particle(1, j, i));
                }

            if ((int)camera.underposition.Y >= 0 && (int)camera.underposition.Y < height && (int)camera.underposition.X >= 0 && (int)camera.underposition.X < width)
                spriteBatch.Draw(cursor, new Vector2((int)camera.underposition.X * 68 + (int)camera.underposition.Y * 68, (int)camera.underposition.X * 34 - (int)camera.underposition.Y * 34) - camera.position + camera.offcet - new Vector2(68, 36), null, new Color(1,0,0,0.1f));

            if ((int)camera.mapposition.Y >= 0 && (int)camera.mapposition.Y < height && (int)camera.mapposition.X >= 0 && (int)camera.mapposition.X < width)
            {
                int i = (int)camera.mapposition.Y, j = (int)camera.mapposition.X;
                spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector((MapCellType)(mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X] % 4)), GetSourceRectangle((MapCellType)(mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X] % 4), gameTime), Color.White);//, 0, GetOriginVector((MapCellType)(mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X] % 4)), 1, SpriteEffects.None, 0.03f);
                spriteBatch.Draw(cursor, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - new Vector2(68, 36), null, Color.White);//, 0, new Vector2(68, 36), 1, SpriteEffects.None, 0.02f);
                if (mapData[i, j] > 3)
                {
                    if (waitTime[i, j] > 0 && MapCellTypeToMapCell((MapCellType)mapData[i,j]) == MapCell.Building)
                        spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector(MapCellType.Constuct), GetSourceRectangle(MapCellType.Constuct, gameTime, i +j), camera.light);//, 0, GetOriginVector(MapCellType.Constuct), 1, SpriteEffects.None, 0.01f);
                    else spriteBatch.Draw(texture, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet - GetOriginVector((MapCellType)mapData[i, j]), GetSourceRectangle((MapCellType)mapData[i,j], gameTime, i + j), Color.White);//, 0, GetOriginVector((MapCellType)mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X]), 1, SpriteEffects.None, 0.01f);
                }
                foreach (int id in GetIndexes(camera.mapposition))
                //foreach (int id in townieindexes[i,j])
                {
                    townies[id].Draw(spriteBatch, towniesset,iconset, camera, this, gameTime);
                }

                foreach (Particle p in particles[i, j])
                {
                    spriteBatch.Draw(dusttex, new Vector2(j * 68 + i * 68, j * 34 - i * 34) - camera.position + camera.offcet + p.offcet,
                                     new Rectangle(0, (2 - (int)(p.time * 3.99) % 3) * 48, 48, 48), camera.light);
                }
            }
        }

        static public Rectangle GetSourceRectangle(MapCellType terrain,double time,float offcet=0)
        {
            Rectangle r = new Rectangle();

            switch (terrain)
            {
                case MapCellType.Armourer: r = new Rectangle(952, 0, 124, 120); break;
                case MapCellType.Barracs: r = new Rectangle(240, 0, 124, 120); break;
                case MapCellType.Blacksmith: r = new Rectangle(240, 360, 116, 116); break;
                case MapCellType.Bog1: r = new Rectangle(832, 540, 120, 68); break;
                case MapCellType.Bog2: r = new Rectangle(832, 608, 120, 68); break;
                case MapCellType.Bog3: r = new Rectangle(832, 676, 120, 68); break;
                case MapCellType.Bog4: r = new Rectangle(832, 744, 120, 68); break;
                case MapCellType.Castle: r = new Rectangle(364, 0, 124, 144); break;
                case MapCellType.Church: r = new Rectangle(488, 0, 108, 132); break;
                case MapCellType.Constuct: r = new Rectangle(136, 0, 104, 68); break;
                case MapCellType.Farm: r = new Rectangle(832, 0, 120, 76); break;
                case MapCellType.Field1: r = new Rectangle(132, 416, 108, 80); break;
                case MapCellType.Field2: r = new Rectangle(132, 496, 108, 80); break;
                case MapCellType.Field3: r = new Rectangle(132, 576, 108, 80); break;
                case MapCellType.Field4: r = new Rectangle(132, 656, 108, 80); break;
                case MapCellType.Fisherman: r = new Rectangle(716, 0, 116, 92); break;
                case MapCellType.Forest1: r = new Rectangle(592, 552, 144, 130); break;
                case MapCellType.Forest2: r = new Rectangle(592, 682, 144, 130); break;
                case MapCellType.Forest3: r = new Rectangle(592, 812, 144, 130); break;
                case MapCellType.Forest4: r = new Rectangle(592, 942, 144, 130); break;
                case MapCellType.Forester: r = new Rectangle(952, 360, 124, 112); break;
                case MapCellType.Foundry: r = new Rectangle(364, 432, 124, 82); break;
                case MapCellType.Grass1: r = new Rectangle(0, 0, 136, 72); break;
                case MapCellType.Grass2: r = new Rectangle(0, 72, 136, 72); break;
                case MapCellType.Grass3: r = new Rectangle(0, 144, 136, 72); break;
                case MapCellType.Grass4: r = new Rectangle(0, 216, 136, 72); break;
                case MapCellType.Iron1: r = new Rectangle(472, 678, 120, 120); break;
                case MapCellType.Iron2: r = new Rectangle(472, 798, 120, 120); break;
                case MapCellType.Iron3: r = new Rectangle(472, 918, 120, 120); break;
                case MapCellType.Iron4: r = new Rectangle(472, 1038, 120, 120); break;
                case MapCellType.Lake1: r = new Rectangle(488, 396, 104, 56); break;
                case MapCellType.Lake2: r = new Rectangle(488, 452, 104, 56); break;
                case MapCellType.Lake3: r = new Rectangle(488, 508, 104, 56); break;
                case MapCellType.Lake4: r = new Rectangle(488, 564, 104, 56); break;
                case MapCellType.Market: r = new Rectangle(596, 0, 120, 88); break;
                case MapCellType.Mill: r = new Rectangle(136, 68, 104, 116); break;
                case MapCellType.Rock1: r = new Rectangle(0, 288, 120, 120); break;
                case MapCellType.Rock2: r = new Rectangle(0, 408, 120, 120); break;
                case MapCellType.Rock3: r = new Rectangle(0, 528, 120, 120); break;
                case MapCellType.Rock4: r = new Rectangle(0, 648, 120, 120); break;
                case MapCellType.Lumberman: r = new Rectangle(726, 276, 120, 88); break;
                case MapCellType.StoneQuary: r = new Rectangle(596, 276, 128, 92); break;
            }

            if ((int)terrain > 28)
            {
                int itime = (int)(time*1000 + offcet * 100);
                int t = itime/ 333;

                if (terrain == MapCellType.Mill) t = itime / 100;
                if (terrain == MapCellType.Lumberman) t = itime / 200;
                if (terrain == MapCellType.Castle) t = itime / 250;
                if (terrain == MapCellType.Farm) t = itime / 1000;
                if (terrain == MapCellType.Barracs) t = itime / 1000;
                if (terrain == MapCellType.Church) t = itime / 1000;

                t = t%3;

                r.Y += r.Height * t;
            }

            return r;
        }
        static public Vector2 GetOriginVector(MapCellType terrain)
        {
            Vector2 v = new Vector2();

            switch (terrain)
            {
                case MapCellType.Armourer: v = new Vector2(60, 86); break;
                case MapCellType.Barracs: v = new Vector2(62, 94); break;
                case MapCellType.Blacksmith: v = new Vector2(55, 86); break;
                case MapCellType.Bog1: v = new Vector2(59, 42); break;
                case MapCellType.Bog2: v = new Vector2(59, 42); break;
                case MapCellType.Bog3: v = new Vector2(59, 42); break;
                case MapCellType.Bog4: v = new Vector2(59, 42); break;
                case MapCellType.Castle: v = new Vector2(62, 116); break;
                case MapCellType.Church: v = new Vector2(48, 112);  break;
                case MapCellType.Constuct: v = new Vector2(52, 44); break;
                case MapCellType.Farm: v = new Vector2(58, 48); break;
                case MapCellType.Field1: v = new Vector2(54, 56); break;
                case MapCellType.Field2: v = new Vector2(54, 56); break;
                case MapCellType.Field3: v = new Vector2(54, 56); break;
                case MapCellType.Field4: v = new Vector2(54, 56); break;
                case MapCellType.Fisherman: v = new Vector2(56, 72); break;
                case MapCellType.Forest1: v = new Vector2(76, 100); break;
                case MapCellType.Forest2: v = new Vector2(76, 100); break;
                case MapCellType.Forest3: v = new Vector2(76, 100); break;
                case MapCellType.Forest4: v = new Vector2(76, 100); break;
                case MapCellType.Forester: v = new Vector2(62, 80); break;
                case MapCellType.Foundry: v = new Vector2(62, 58); break;
                case MapCellType.Grass1: v = new Vector2(68, 36); break;
                case MapCellType.Grass2: v = new Vector2(68, 36); break;
                case MapCellType.Grass3: v = new Vector2(68, 36); break;
                case MapCellType.Grass4: v = new Vector2(68, 36); break;
                case MapCellType.Iron1: v = new Vector2(58, 96); break;
                case MapCellType.Iron2: v = new Vector2(58, 96); break;
                case MapCellType.Iron3: v = new Vector2(58, 96); break;
                case MapCellType.Iron4: v = new Vector2(58, 96); break;
                case MapCellType.Lake1: v = new Vector2(54, 30); break;
                case MapCellType.Lake2: v = new Vector2(54, 30); break;
                case MapCellType.Lake3: v = new Vector2(54, 30); break;
                case MapCellType.Lake4: v = new Vector2(54, 30); break;
                case MapCellType.Market: v = new Vector2(60, 60); break;
                case MapCellType.Mill: v = new Vector2(52, 100); break;
                case MapCellType.Rock1: v = new Vector2(60, 98); break;
                case MapCellType.Rock2: v = new Vector2(60, 98); break;
                case MapCellType.Rock3: v = new Vector2(60, 98); break;
                case MapCellType.Rock4: v = new Vector2(60, 98); break;
                case MapCellType.Lumberman: v = new Vector2(58, 66); break;
                case MapCellType.StoneQuary: v = new Vector2(62, 66); break;
            }

            return v;
        }

        static public MapCell MapCellTypeToMapCell(MapCellType mct)
        {
            MapCell mc;

            if ((int)mct < 4) mc = MapCell.Grass;
            else if ((int)mct < 24) mc = MapCell.Resource;
            else if ((int)mct < 28) mc = MapCell.Bog;
            else mc = MapCell.Building;

            return mc;
        }
        static public ResourceType MapCellTypeToResource(MapCellType mct)
        {
            ResourceType rt;

            if ((int)mct < 8) rt = ResourceType.Stone;
            else if ((int)mct < 12) rt = ResourceType.Wood;
            else if ((int)mct < 16) rt = ResourceType.Iron;
            else if ((int)mct < 20) rt = ResourceType.Corn;
            else if ((int)mct < 24) rt = ResourceType.Fish;
            else if ((int)mct < 30) rt = ResourceType.Corn;
            else if ((int)mct < 31) rt = ResourceType.Sword;
            else if ((int)mct < 32) rt = ResourceType.Tools;
            else if ((int)mct < 33) rt = ResourceType.Castle;
            else if ((int)mct < 34) rt = ResourceType.Iron;
            else if ((int)mct < 35) rt = ResourceType.Will;
            else if ((int)mct < 36) rt = ResourceType.Gold;
            else if ((int)mct < 37) rt = ResourceType.Stone;
            else if ((int)mct < 38) rt = ResourceType.Fish;
            else if ((int)mct < 39) rt = ResourceType.Lumber;
            else if ((int)mct < 40) rt = ResourceType.Meat;
            else if ((int)mct < 41) rt = ResourceType.Sword;
            else rt = ResourceType.Wood;

            return rt;
        }

        public int GetResourcesNumber()
        { 
            //int o=0;

            //for (int i = 0; i < 10; i++)
            //    o += resources[i].number;
            return resources.Number-resources.gold;
            //return o;
        }
        public void Update(GameTime gameTime,ref Gui gui)
        {
            buildings[0].resources = GetResourcesNumber();

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (waitTime[i, j] > 0)
                    {
                        waitTime[i, j] -= Math.Min(gameTime.ElapsedGameTime.TotalSeconds, waitTime[i, j]);
                        if ((MapCellTypeToMapCell((MapCellType)mapData[i, j]) == MapCell.Resource || MapCellTypeToMapCell((MapCellType)mapData[i, j]) == MapCell.Grass) && waitTime[i, j] <= 0)
                            terraformed++;
                        if (MapCellTypeToMapCell((MapCellType)mapData[i, j]) == MapCell.Building)
                            buildings[indexes[i, j]].wait = waitTime[i, j];
                    }

                    for (int k = particles[i, j].Count - 1; k >= 0; k--)
                    {
                        particles[i, j][k].time -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        particles[i, j][k].offcet.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (particles[i, j][k].time <= 0) particles[i, j].RemoveAt(k);
                    }
                }

            for (int i = townies.Count - 1; i >= 0; i--)
            {
                townies[i].Update(gameTime, this);
                if (townies[i].profetion == TownieProfetion.soldier && townies[i].state == TownieState.wait)
                {
                    buildings[townies[i].building].resources++;
                    capturedresources = capturedresources + townies[i].inventory;

                    if (buildings[townies[i].building].resources == buildings[townies[i].building].temp)
                    {
                        gui.rightPanelState = RightPanelStates.Captured;
                        raidsucces++;
                        gui.rightPanelCommands.Clear();
                    }

                    townies.RemoveAt(i);
                }
            }
        }
        public int GetResources(ResourceType t)
        {
            //for (int i = 0; i < resources.Length; i++)
            //    if (resources[i].type == t) return resources[i].number;
            return resources[(int)t];
            //return -1;
        }
        public int GetCapResources(ResourceType t)
        {
            //for (int i = 0; i < resources.Length; i++)
            //    if (resources[i].type == t) return resources[i].number;
            return capturedresources[(int)t];
            //return -1;
        }
        public List<int> GetIndexes(Vector2 v)
        {
            List<int> u = new List<int>();
            Vector2 p;
            bool b;

            for (int i = 0; i < townies.Count; i++)
            {
                p = townies[i].position;
                b = p.X - v.X == 0 && p.Y - v.Y < 1 && p.Y - v.Y >= 0;
                b = b||p.X - v.X <0 && p.Y - v.Y == 0 && p.X - v.X > -1;
                if (b) u.Add(i);
            }

            return u;
        }
        public Vector2 GetVector(int i)
        {
            int y = i / (height + 1);
            int x = i - y * (height + 1);

            return new Vector2(x, y);
        }
        public Vector2 GetBuildingPosition(int b)
        { 
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (MapCellTypeToMapCell((MapCellType)mapData[i, j]) == MapCell.Building && indexes[i, j] == b) return new Vector2(j, i);
                }
            return new Vector2(-1, -1);
        }

        public Vector2 GetNearResourceByBuilding(int b)
        {
            int fid = -1;
            Vector2 bpos = GetBuildingPosition(b);
            switch (buildings[b].type)
            {
                case BuildingType.Fisherman: fid = 20; break;
                case BuildingType.Forester: fid = 8; break;
                case BuildingType.StoneQuary: fid = 4; break;
                case BuildingType.Foundry: fid = 12; break;
                case BuildingType.Mill: fid = 16; break;
            }
            if (fid == -1) return bpos;

            Vector2 res = new Vector2(-1);
            int lres = 100000000;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (mapData[i, j] - fid < 4 && mapData[i, j] - fid >= 0)
                    {
                        if (terrains[indexes[i, j]].resources > 0)
                        {
                            int l = (int)(Math.Abs(i - bpos.Y) + Math.Abs(j - bpos.X));
                            if (l < lres) { lres = l; res = new Vector2(j, i); }
                        }
                    }
                }

            if (res.X == -1)
                return bpos;

            return res;
        }

        public Vector2 GetNearGrassByPosition(Vector2 pos)
        {
            Vector2 res = new Vector2(-1);
            int lres = 100000000;

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (mapData[i, j]  < 4)
                    {
                        int l = (int)(Math.Abs(i - pos.Y) + Math.Abs(j - pos.X));
                        if (l < lres) { lres = l; res = new Vector2(j, i); }
                    }
                }

            return res;
        }

        public int CastleStorage { get { int lvl = buildings[0].lvl - (buildings[0].wait <= 0 ? 1 : 0); return lvl == 0 ? 20 : lvl == 1 ? 50 : 99; } }
        public int MarketLvl { get { foreach (Building b in buildings)if (b.type == BuildingType.Market)return b.lvl; return -1; } }

        public int IsCell(MapCellType mct, int lvl = 0)
        {
            int r = 0;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (mapData[i, j] == (int)mct && buildings[indexes[i, j]].lvl >= lvl) r++;
            return r;
        }
        public int GetLuck()
        {
            foreach (Building b in buildings)
            {
                if (b.type == BuildingType.Church)
                    return 1 + ((b.lvl + 1) * (b.lvl + 1)) + (b.resources > 0 ? (b.lvl + 3) : 0);
            }
            return 1;
        }

        public bool ForestBad()
        { return false; }
        public bool FieldBad()
        { return false; }
        public bool LakeBad()
        { return false; }
        public bool Quake()
        { return false; }

        public bool ForestNew()
        { return false; }
        public bool LakeNew()
        { return false; }
        public bool MountGood()
        { return false; }
        public bool FieldGood()
        { return false; }
        public bool BuildingGood()
        { return false; }
    }
}
