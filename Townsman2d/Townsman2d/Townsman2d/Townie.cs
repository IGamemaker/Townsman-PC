using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Townsman2d
{
    enum TownieProfetion
    { 
        miner,
        porter,
        worker,
        workerinbarrack,
        soldier
    }

    enum TownieState
    { 
        wait,
        work,
        go,
        gowhithres
    }

    class Townie
    {
        public Vector2 position;
        public Vector2 oldposition;
        public Vector2 startposition;
        public Vector2 target;
        public TownieProfetion profetion;
        public TownieState state;
        //public Resource[] inventory;
        public Inventory inventory;
        public ResourceType icon;
        public int building;
        public Vector2 buildingposition;
        public double wait;

        public static float speed=0.01f;

        public Townie(Vector2 pos,TownieProfetion pro,int build,Vector2 buildpos)
        {
            oldposition=startposition = target = position = pos;
            profetion = pro;
            state = TownieState.wait;
            //inventory = new Resource[0];
            inventory = new Inventory();
            icon = ResourceType.Castle;
            building = build;
            buildingposition = buildpos;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset,Texture2D[] iconset, Camera camera, Map map,double gt)
        {
            if (profetion != TownieProfetion.worker && profetion != TownieProfetion.workerinbarrack&&(position.Y >= 0 && position.Y < map.height && position.X >= 0 && position.X < map.width))
            {
                Vector2 projpos = new Vector2((position.X + position.Y) * 68, (position.X - position.Y) * 34 + 34) - new Vector2(12, 28);
                Vector2 dir = oldposition - position;
                int i = -1;
                if (dir.Y < 0)
                    i = 4;
                if (dir.Y > 0)
                    i = 0;
                if (dir.X > 0)
                    i = 6;
                if (dir.X < 0)
                    i = 2;
                if (i == -1 || (int)(gt * 5) % 2 == 0)
                    i++;
                Rectangle source = new Rectangle(24, i * 28, 24, 28);
                if (state == TownieState.go||state == TownieState.gowhithres)
                {
                    ResourceType rt = inventory.ResourceType;
                    if (profetion == TownieProfetion.soldier)
                        rt = ResourceType.Sword;
                    if (inventory.Number > 0 || profetion == TownieProfetion.soldier)
                        spriteBatch.Draw(iconset[(int)rt], new Vector2((position.X + position.Y) * 68 - 8, (position.X - position.Y) * 34 + 34 - 65) - camera.position + camera.offcet, camera.light);
                }
                if (state == TownieState.work&&map.buildings[building].productive>0)
                {
                    if (map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Lake1 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Lake2 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Lake3 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Lake4)
                    {
                        //6 32
                        source = new Rectangle(0, (((int)(gt * 5) % 2) * 32), 24, 32);//!
                        projpos = projpos - new Vector2(-6, 10);
                    }
                    else if (map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Forest1 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Forest2 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Forest3 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Forest4)
                    {
                        source = new Rectangle(48, (((int)(gt * 5) % 2) * 40)+80, 62, 40);
                        projpos = projpos - new Vector2(16, 16);
                    }
                    else if (map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Field1 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Field2 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Field3 || map.mapData[(int)position.Y, (int)position.X] == (int)MapCellType.Field4)
                    {
                        source = new Rectangle(48, (((int)(gt * 5) % 2) * 40)+160, 62, 40);
                        projpos = projpos - new Vector2(16, 16);
                    }
                    else
                    {
                        source = new Rectangle(48, (((int)(gt * 5) % 2) * 40), 62, 40);
                        projpos = projpos - new Vector2(16, 16);
                    }
                }
                spriteBatch.Draw(tileset, projpos - camera.position + camera.offcet, source, camera.light);//, 0, new Vector2(12, 28), 1, SpriteEffects.None, z);
            }
        }

        public void Update(GameTime gameTime,Map map)
        {
            
            int ty = (int)target.Y, tx = (int)target.X;
            oldposition = position;

            #region Portier
            if (profetion == TownieProfetion.porter && (map.waitTime[ty, tx] <= 0||((int)map.castleposition.X==tx&&(int)map.castleposition.Y==ty)))
            {
                if (state == TownieState.wait)
                {
                    if (map.buildings[building].outproduct)
                    {
                        if (inventory.Length != 0)
                            target = startposition;
                        else
                            target = buildingposition;
                        state = TownieState.go;
                    }
                    else if (map.buildings[building].needproduct)
                    {
                        Inventory inv = Inventory.FromResourceArray(Helper.buildingHelper[(int)map.buildings[building].type, map.buildings[building].lvl].inproduct);
                        if (map.resources > inv&&inv.Length>0)
                        {
                            inventory = inv*map.buildings[building].productive;
                            map.resources = map.resources - inv;
                            if (map.resources.FindNegative()) if (map.buildings[building].productive > 1) map.buildings[building].productive--;
                            target = buildingposition;
                            state = TownieState.gowhithres;
                        }
                    }
                }

                if (state == TownieState.go)
                {
                    if (Math.Abs(target.X - position.X) + Math.Abs(target.Y - position.Y) <= 0.015)
                    {
                        position = target;

                            if (inventory.Length != 0||inventory[(int)ResourceType.Gold]>0)
                            {
                                map.resources = map.resources + inventory;
                                if (map.buildings[0].lvl == 0) map.resources.SetMaxResourceLvl(20);
                                if (map.buildings[0].lvl == 1) if (map.buildings[0].wait <= 0) map.resources.SetMaxResourceLvl(50); else map.resources.SetMaxResourceLvl(20);
                                if (map.buildings[0].lvl == 2) if (map.buildings[0].wait <= 0) map.resources.SetMaxResourceLvl(99); else map.resources.SetMaxResourceLvl(50);
                                inventory = new Inventory();// new Resource[0];
                                state = TownieState.wait;
                            }
                            else
                            {
                                if (target == startposition) state = TownieState.wait;
                                else
                                {
                                    if (map.buildings[building].type != BuildingType.Church)
                                    {
                                        inventory[(int)map.buildings[building].production[0].type] = map.buildings[building].resources;
                                        map.buildings[building].resources = 0;
                                        map.buildings[building].outproduct = false;
                                        target = startposition;
                                        state = TownieState.go;
                                    }
                                    else
                                    {
                                        inventory[(int)ResourceType.Gold] += map.buildings[building].productive;
                                        map.buildings[building].outproduct = false;
                                        target = startposition;
                                        state = TownieState.go;
                                    }
                                }
                            }
                    }
                    else
                    {
                        Vector2 dir = Vector2.Zero;

                        if (Math.Abs(target.X - position.X) <= 0.015)
                        {
                            position.X = target.X;
                            dir.Y = Math.Sign(target.Y - position.Y);
                        }
                        else
                            dir.X = Math.Sign(target.X - position.X);

                        position += dir *speed;
                    }
                }

                if (state == TownieState.gowhithres)
                {
                    if (Math.Abs(target.X - position.X) + Math.Abs(target.Y - position.Y) <= 0.015)
                    {
                        position = target;

                        map.buildings[building].needproduct = false;
                        inventory = new Inventory();
                        target = startposition;
                        state = TownieState.go;
                    }
                    else
                    {
                        Vector2 dir = Vector2.Zero;

                        if (Math.Abs(target.X - position.X) <= 0.015)
                        {
                            position.X = target.X;
                            dir.Y = Math.Sign(target.Y - position.Y);
                        }
                        else
                            dir.X = Math.Sign(target.X - position.X);

                        position += dir * speed;
                    }
                }
            }
            #endregion

            #region Miner
            if (profetion == TownieProfetion.miner && map.waitTime[ty, tx] <= 0 && map.buildings[building].productive > 0)
            {
                if (state == TownieState.wait)
                {
                    if (inventory.Length != 0)
                        target = buildingposition;
                    else
                        target = map.GetNearResourceByBuilding(building);
                    state = TownieState.go;
                }

                if (state == TownieState.go)
                {
                    if (Math.Abs(target.X - position.X) + Math.Abs(target.Y - position.Y) <= 0.015)
                    {
                        position = target;

                        if (inventory.Length != 0)
                        {
                            //map.buildings[building].resources += map.buildings[building].production[0].number;
                            map.buildings[building].inproduct = true;
                            inventory = new Inventory();//new Resource[0];
                            state = TownieState.wait;
                        }
                        else
                        {
                            state = TownieState.work;
                            wait = 10;
                        }
                    }
                    else
                    {
                        Vector2 dir = Vector2.Zero;

                        if (Math.Abs(target.X - position.X) <= 0.015)
                        {
                            position.X = target.X;
                            dir.Y = Math.Sign(target.Y - position.Y);
                        }
                        else
                            dir.X = Math.Sign(target.X - position.X);

                        position += dir * speed;
                    }
                }

                if (state == TownieState.work)
                {
                    wait -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (wait < 0)
                    {
                        try
                        {
                            wait = 0;
                            inventory = Inventory.FromResourceArray(map.buildings[building].production) * map.buildings[building].productive;
                            state = TownieState.wait;
                            map.terrains[map.indexes[(int)position.Y, (int)position.X]].resources--;
                            if (map.terrains[map.indexes[(int)position.Y, (int)position.X]].resources < 0) map.terrains[map.indexes[(int)position.Y, (int)position.X]].resources = 0;
                        }
                        catch { state = TownieState.wait; }
                    }
                }
            }
            #endregion

            #region Worker
            if (profetion == TownieProfetion.worker && map.waitTime[ty, tx] <= 0 && map.buildings[building].productive > 0)
            {
                if (wait > 0)
                {
                    if (map.buildings[building].type == BuildingType.Mill ||
                        map.buildings[building].type == BuildingType.Forester ||
                        //map.buildings[building].type == BuildingType.Foundry || 
                        map.buildings[building].type == BuildingType.StoneQuary || map.buildings[building].type == BuildingType.Fisherman)
                        wait = 0;

                    map.buildings[building].inproduct = false;
                    wait -= gameTime.ElapsedGameTime.TotalSeconds;

                    if (wait <= 0)
                    {
                        if (map.buildings[building].type != BuildingType.Church)
                        {
                            map.buildings[building].resources += map.buildings[building].production[0].number * map.buildings[building].productive;
                            if (map.buildings[building].resources > map.buildings[building].storage) map.buildings[building].resources = map.buildings[building].storage;
                        }

                        map.buildings[building].outproduct = true;
                    }
                }
                else
                {
                    if (!map.buildings[building].needproduct && map.buildings[building].inproduct)
                        wait = 10;

                    if (wait <= 0)
                        map.buildings[building].needproduct = Inventory.FromResourceArray(Helper.buildingHelper[(int)map.buildings[building].type, map.buildings[building].lvl].inproduct).Number > 0;
                    else
                        map.buildings[building].needproduct = false;

                    if (map.buildings[building].type == BuildingType.Armourer ||
                        map.buildings[building].type == BuildingType.Blacksmith ||
                        //map.buildings[building].type == BuildingType.Foundry ||
                        map.buildings[building].type == BuildingType.Lumbermen ||
                        map.buildings[building].type == BuildingType.Farm) map.buildings[building].inproduct = true;

                    if (map.buildings[building].type == BuildingType.Church && !map.buildings[building].outproduct)
                    {
                        if (map.buildings[building].resources > 0) map.buildings[building].resources--;
                        wait = 10;
                    }
                }
            }
            #endregion

            #region WorkerInBarrack
            if (profetion == TownieProfetion.workerinbarrack && map.waitTime[ty, tx] <= 0 && map.buildings[building].productive > 0)
            {
                if (wait > 0)
                {
                    map.buildings[building].inproduct = false;
                    wait -= gameTime.ElapsedGameTime.TotalSeconds;

                    if (wait <= 0)
                    {
                        if (map.buildings[building].resources < map.buildings[building].storage)
                        {
                            map.buildings[building].resources++;
                            map.buildings[building].temp++;
                        }
                    }
                }
                else
                {
                    if (map.buildings[building].temp < map.buildings[building].storage)
                        wait = 10;
                }
            }
            #endregion

            #region Soldier
            if (profetion == TownieProfetion.soldier)
            {
                if (wait > 0)
                {
                    wait -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (state == TownieState.go)
                    {
                        if (Math.Abs(target.X - position.X) + Math.Abs(target.Y - position.Y) <= 0.015)
                        {
                            position = target;
                            target = buildingposition;
                            wait = 10;
                            state = TownieState.gowhithres;
                        }
                        else
                        {
                            Vector2 dir = Vector2.Zero;

                            if (Math.Abs(target.X - position.X) <= 0.015)
                            {
                                position.X = target.X;
                                dir.Y = Math.Sign(target.Y - position.Y);
                            }
                            else
                                dir.X = Math.Sign(target.X - position.X);

                            position += dir * speed;
                        }
                    }

                    if (state == TownieState.gowhithres)
                    {
                        if (Math.Abs(target.X - position.X) + Math.Abs(target.Y - position.Y) <= 0.015)
                        {
                            position = target;
                            state = TownieState.wait;
                            Random r =new Random();
                            for (int i = 0; i < map.GetLuck(); i++)
                            {
                                inventory[r.Next(10)] += r.Next(map.GetLuck() * 4);
                            }
                            map.resources = map.resources + inventory;
                        }
                        else
                        {
                            Vector2 dir = Vector2.Zero;

                            if (Math.Abs(target.X - position.X) <= 0.015)
                            {
                                position.X = target.X;
                                dir.Y = Math.Sign(target.Y - position.Y);
                            }
                            else
                                dir.X = Math.Sign(target.X - position.X);

                            position += dir * speed;
                        }
                    }
                }
            }
            #endregion
        }

        public float GetSing(float a)
        {
            if (a < 0) return -1;
            if (a > 0) return 1;
            return 0;
        }
    }
}
