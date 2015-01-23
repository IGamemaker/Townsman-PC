using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Townsman2d
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int width;
        public static int height;
        public static bool fullscreen;
        public static bool cheatmode;
        public static bool music;

        Texture2D mapcursor;
        Texture2D tileset;
        Texture2D cursor;
        Texture2D backgroundlight;
        Texture2D backgrounddark;
        Texture2D outline;
        Texture2D guiset;
        Texture2D star;
        Texture2D iconset;
        Texture2D emprypixel;
        Texture2D[] icons;
        Texture2D[] resourceset;
        Texture2D timeofdayset;
        Texture2D towniesset;
        Texture2D treeup;
        Texture2D treedown;
        Texture2D treemiddle;
        Texture2D ropetexture;
        Texture2D logo;
        Texture2D dusttexture;

        Texture2D isomasktex;

        SpriteFont font;

        LangManager langManager;
        Map map;

        Gui gui;
        GameState state;

        Camera camera;

        double time;
        string debugMessage;

        int mapsize;
        int forestcount, watercount, mountcount, resourcecount;

        static float timekoef = 1;
        static float speed = 1;
        static public Random random = new Random();

        int mission;
        bool gamestarted;

        Song[] sounds;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            debugMessage = "";

            mapsize = 12;
            forestcount = 1;
            watercount = 1;
            mountcount = 1;
            resourcecount = 1;
            gamestarted=false;

            #region Config
            IniParser.FileIniDataParser parser = new IniParser.FileIniDataParser();
            IniParser.IniData data = parser.LoadFile("config.ini");
            width = Convert.ToInt32(data["Graphics"]["Width"]);
            height = Convert.ToInt32(data["Graphics"]["Height"]);
            fullscreen = Convert.ToBoolean(data["Graphics"]["Fullscreen"]);
            cheatmode = Convert.ToBoolean(data["GamePlay"]["Cheats"]);
            music = Convert.ToBoolean(data["Music"]["Sounds"]);
            string lang = data["GamePlay"]["Lang"];
            switch (lang)
            {
                case "Eng": langManager = new LangManager("Content/Lang/eng.lang"); break;
                case "Rus": langManager = new LangManager("Content/Lang/rus.lang"); break;
                default: langManager = new LangManager("Content/Lang/eng.lang"); break;
            }

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
            #endregion

        }

        protected override void Initialize()
        {
            map = new Map();
            camera = new Camera(new Vector2(width, height), new Vector2(map.width, map.height));
            state = GameState.MainMenu;

            gui = new Gui();

            gui.elements[0] = new GuiObject(new Rectangle(0, 0, width - 205, height), false, false, GameState.Game, Main, MapGuiDraw);
            gui.elements[1] = new GuiObject(new Rectangle(width - 205, 0, 205, height), false, false, GameState.Game, RightPanel, RightPanelDraw);
            gui.elements[2] = new GuiObject(new Rectangle(width - 205, 0, 205, 39), false, false, GameState.Game, GameMenuButton, GameMenuButtonDraw);
            gui.elements[3] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 - 20, 150, 30), false, false, GameState.MainMenu, StartGameButton, StandartButtonDraw, langManager.strings[(int)LangString.StartGame]);
            gui.elements[4] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 200, 150, 30), false, false, GameState.StartGameMenu, GenerateButton, StandartButtonDraw, langManager.strings[(int)LangString.Generate]);

            gui.elements[5] = new GuiObject(new Rectangle((width - 150) / 2 - 85 - 150, height / 2 - 20, 150, 30), false, false, GameState.StartGameMenu, SmallMapButton, StandartButtonDraw, langManager.strings[(int)LangString.SmallMap]);
            gui.elements[6] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 - 20, 150, 30), true, false, GameState.StartGameMenu, MediumMapButton, StandartButtonDraw, langManager.strings[(int)LangString.MediumMap]);
            gui.elements[7] = new GuiObject(new Rectangle((width - 150) / 2 + 85 + 150, height / 2 - 20, 150, 30), false, false, GameState.StartGameMenu, LargeMapButton, StandartButtonDraw, langManager.strings[(int)LangString.LargeMap]);

            gui.elements[8] = new GuiObject(new Rectangle((width - 160) / 2 - 85 - 150, height / 2 + 20, 160, 30), false, false, GameState.StartGameMenu, SmallForestButton, StandartButtonDraw, langManager.strings[(int)LangString.LittleForest]);
            gui.elements[9] = new GuiObject(new Rectangle((width - 160) / 2, height / 2 + 20, 160, 30), true, false, GameState.StartGameMenu, MediumForestButton, StandartButtonDraw, langManager.strings[(int)LangString.MediumForest]);
            gui.elements[10] = new GuiObject(new Rectangle((width - 160) / 2 + 85 + 150, height / 2 + 20, 160, 30), false, false, GameState.StartGameMenu, LargeForestButton, StandartButtonDraw, langManager.strings[(int)LangString.LargeForest]);

            gui.elements[11] = new GuiObject(new Rectangle((width - 150) / 2 - 85 - 150, height / 2 + 60, 150, 30), false, false, GameState.StartGameMenu, SmallWaterButton, StandartButtonDraw, langManager.strings[(int)LangString.LittleWater]);
            gui.elements[12] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 60, 150, 30), true, false, GameState.StartGameMenu, MediumWaterButton, StandartButtonDraw, langManager.strings[(int)LangString.MediumWater]);
            gui.elements[13] = new GuiObject(new Rectangle((width - 150) / 2 + 85 + 150, height / 2 + 60, 150, 30), false, false, GameState.StartGameMenu, LargeWaterButton, StandartButtonDraw, langManager.strings[(int)LangString.ManyWater]);

            gui.elements[14] = new GuiObject(new Rectangle((width - 150) / 2 - 85 - 150, height / 2 + 100, 150, 30), false, false, GameState.StartGameMenu, SmallRocksButton, StandartButtonDraw, langManager.strings[(int)LangString.LittleRocks]);
            gui.elements[15] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 100, 150, 30), true, false, GameState.StartGameMenu, MediumRocksButton, StandartButtonDraw, langManager.strings[(int)LangString.MediumRocks]);
            gui.elements[16] = new GuiObject(new Rectangle((width - 150) / 2 + 85 + 150, height / 2 + 100, 150, 30), false, false, GameState.StartGameMenu, LargeRocksButton, StandartButtonDraw, langManager.strings[(int)LangString.ManyRocks]);

            gui.elements[17] = new GuiObject(new Rectangle((width - 180) / 2 - 85 - 150, height / 2 + 140, 180, 30), false, false, GameState.StartGameMenu, SmallResourcesButton, StandartButtonDraw, langManager.strings[(int)LangString.SmallResources]);
            gui.elements[18] = new GuiObject(new Rectangle((width - 180) / 2, height / 2 + 140, 180, 30), true, false, GameState.StartGameMenu, MediumResourcesButton, StandartButtonDraw, langManager.strings[(int)LangString.MediumResources]);
            gui.elements[19] = new GuiObject(new Rectangle((width - 180) / 2 + 85 + 150, height / 2 + 140, 180, 30), false, false, GameState.StartGameMenu, LargeResourcesButton, StandartButtonDraw, langManager.strings[(int)LangString.ManyResources]);

            gui.elements[20] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 100, 150, 30), false, false, GameState.MainMenu, ExitButton, StandartButtonDraw, langManager.strings[(int)LangString.Exit]);
            gui.elements[21] = new GuiObject(new Rectangle(0, 0, 1, 1), false, false, GameState.Any, Nothing, LogoDraw);
            gui.elements[22] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 20, 150, 30), false, false, GameState.MainMenu, LoadGameButton, StandartButtonDraw, langManager.strings[(int)LangString.LoadGame]);
            gui.elements[23] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 60, 150, 30), false, false, GameState.MainMenu, HelpButton, StandartButtonDraw, langManager.strings[(int)LangString.Help]);
            gui.elements[24] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 - 20, 150, 30), false, false, GameState.StartGameMenuModes, MissionsButton, StandartButtonDraw, langManager.strings[(int)LangString.Missions]);
            gui.elements[25] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 20, 150, 30), false, false, GameState.StartGameMenuModes, RandomMapButton, StandartButtonDraw, langManager.strings[(int)LangString.RandomMap]);
            gui.elements[26] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 60, 150, 30), false, false, GameState.StartGameMenuModes, SandboxButton, StandartButtonDraw, langManager.strings[(int)LangString.Sandbox]);
            gui.elements[27] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 - 20, 150, 30), false, false, GameState.StartGameMenuMissions, Mission1Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 1");
            gui.elements[28] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 20, 150, 30), false, false, GameState.StartGameMenuMissions, Mission2Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 2");
            gui.elements[29] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 60, 150, 30), false, false, GameState.StartGameMenuMissions, Mission3Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 3");
            gui.elements[30] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 100, 150, 30), false, false, GameState.StartGameMenuMissions, Mission4Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 4");
            gui.elements[31] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 140, 150, 30), false, false, GameState.StartGameMenuMissions, Mission5Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 5");
            gui.elements[32] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 180, 150, 30), false, false, GameState.StartGameMenuMissions, Mission6Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 6");
            gui.elements[33] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 220, 150, 30), false, false, GameState.StartGameMenuMissions, Mission7Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 7");
            gui.elements[34] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 260, 150, 30), false, false, GameState.StartGameMenuMissions, Mission8Button, StandartButtonDraw, langManager.strings[(int)LangString.Mission] + " 8");
            gui.elements[35] = new GuiObject(new Rectangle((width - 150) / 2 - 160, height / 2 + 260, 150, 30), false, false, GameState.StartGameMenuMissions, BackButton, StandartButtonDraw, langManager.strings[(int)LangString.Back]);
            gui.elements[36] = new GuiObject(new Rectangle((width - 150) / 2 - 150 - 85, height / 2 + 200, 150, 30), false, false, GameState.StartGameMenu, BackButton, StandartButtonDraw, langManager.strings[(int)LangString.Back]);
            gui.elements[37] = new GuiObject(new Rectangle((width - 150) / 2 - 160, height / 2 + 60, 150, 30), false, false, GameState.StartGameMenuModes, BackModesButton, StandartButtonDraw, langManager.strings[(int)LangString.Back]);
            gui.elements[38] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 - 20, 150, 30), false, false, GameState.LoadMenu, LoadMissionButton, StandartButtonDraw, langManager.strings[(int)LangString.Missions]);
            gui.elements[39] = new GuiObject(new Rectangle((width - 150) / 2, height / 2 + 20, 150, 30), false, false, GameState.LoadMenu, LoadRandomButton, StandartButtonDraw, langManager.strings[(int)LangString.RandomMap]);
            gui.elements[40] = new GuiObject(new Rectangle((width - 150) / 2 - 160, height / 2 + 20, 150, 30), false, false, GameState.LoadMenu, BackModesButton, StandartButtonDraw, langManager.strings[(int)LangString.Back]);
            gui.elements[41] = new GuiObject(new Rectangle((width - 246), 0, 40, 40), false, false, GameState.Game, ToMenu, StandartButtonDraw, "<");
            gui.elements[42] = new GuiObject(new Rectangle((width - 150) / 2+160, height / 2 - 20, 150, 30), false, false, GameState.MainMenu, ToGame, StandartButtonDraw, langManager.strings[(int)LangString.Continue]);
            gui.elements[43] = new GuiObject(new Rectangle(width - 30, height - 30, 25, 25), false, false, GameState.MainMenu, Nothing, DrawAbout, "?");
            Helper.Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mapcursor = Content.Load<Texture2D>("Textures/mapcursor");
            cursor = Content.Load<Texture2D>("Textures/cursor");
            tileset = Content.Load<Texture2D>("Textures/tileset");
            towniesset = Content.Load<Texture2D>("Textures/townies");
            timeofdayset = Content.Load<Texture2D>("Textures/timeofday");
            dusttexture = Content.Load<Texture2D>("Textures/dust");
            guiset = Content.Load<Texture2D>("Gui/gui");
            iconset = Content.Load<Texture2D>("Gui/icons");
            isomasktex = Content.Load<Texture2D>("Gui/mask");
            treeup = Content.Load<Texture2D>("Gui/treeup");
            treedown = Content.Load<Texture2D>("Gui/treedown");
            treemiddle = Content.Load<Texture2D>("Gui/treemidle");
            logo = Content.Load<Texture2D>("Gui/logo");

            backgroundlight = new Texture2D(graphics.GraphicsDevice, 64, 64);
            backgrounddark = new Texture2D(graphics.GraphicsDevice, 64, 64);
            cursor = new Texture2D(graphics.GraphicsDevice, 16, 16);
            outline = new Texture2D(graphics.GraphicsDevice, 16, 16);
            star = new Texture2D(graphics.GraphicsDevice, 16, 16);
            emprypixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            ropetexture = new Texture2D(graphics.GraphicsDevice, 8, 32);
            Color[] array = new Color[64 * 64];
            guiset.GetData<Color>(0, new Rectangle(0, 64, 64, 64), array, 0, 64 * 64);
            backgroundlight.SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(64, 64, 64, 64), array, 0, 64 * 64);
            backgrounddark.SetData<Color>(array);
            array = new Color[16 * 16];
            guiset.GetData<Color>(0, new Rectangle(16, 0, 16, 16), array, 0, 16 * 16);
            outline.SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(32, 0, 16, 16), array, 0, 16 * 16);
            cursor.SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(16, 16, 16, 16), array, 0, 16 * 16);
            star.SetData<Color>(array);
            array = new Color[1];
            guiset.GetData<Color>(0, new Rectangle(32, 0, 1, 1), array, 0, 1);
            emprypixel.SetData<Color>(array);
            array = new Color[8 * 32];
            guiset.GetData<Color>(0, new Rectangle(0, 0, 8, 32), array, 0, 8*32);
            ropetexture.SetData<Color>(array);

            #region resources
            array = new Color[16 * 32];
            resourceset = new Texture2D[12];
            guiset.GetData<Color>(0, new Rectangle(80, 0, 16, 32), array, 0, 16 * 32);
            resourceset[0] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[0].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(32, 32, 16, 32), array, 0, 16 * 32);
            resourceset[1] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[1].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(0, 32, 16, 32), array, 0, 16 * 32);
            resourceset[2] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[2].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(16, 32, 16, 32), array, 0, 16 * 32);
            resourceset[3] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[3].SetData<Color>(array);

            guiset.GetData<Color>(0, new Rectangle(96, 0, 16, 32), array, 0, 16 * 32);
            resourceset[4] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[4].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(112, 0, 16, 32), array, 0, 16 * 32);
            resourceset[5] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[5].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(48, 32, 16, 32), array, 0, 16 * 32);
            resourceset[6] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[6].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(64, 0, 16, 32), array, 0, 16 * 32);
            resourceset[7] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[7].SetData<Color>(array);

            guiset.GetData<Color>(0, new Rectangle(96, 32, 16, 32), array, 0, 16 * 32);
            resourceset[8] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[8].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(112, 32, 16, 32), array, 0, 16 * 32);
            resourceset[9] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[9].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(80, 32, 16, 32), array, 0, 16 * 32);
            resourceset[10] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[10].SetData<Color>(array);
            guiset.GetData<Color>(0, new Rectangle(64, 32, 16, 32), array, 0, 16 * 32);
            resourceset[11] = new Texture2D(graphics.GraphicsDevice, 16, 32);
            resourceset[11].SetData<Color>(array);
            #endregion

            #region icons
            array = new Color[32 * 32];
            icons = new Texture2D[25];
            for (int i = 0; i < 25; i++)
            {
                icons[i] = new Texture2D(graphics.GraphicsDevice, 32, 32);
                iconset.GetData<Color>(0, new Rectangle(i % 5 * 32, i / 5 * 32, 32, 32), array, 0, 32 * 32);
                icons[i].SetData<Color>(array);
            }

            #endregion

            font = Content.Load<SpriteFont>("Fonts/debug");

            if (music)
            {
                sounds = new Song[10];
                sounds[0] = Content.Load<Song>("Sounds/menu");
                sounds[1] = Content.Load<Song>("Sounds/1");
                sounds[2] = Content.Load<Song>("Sounds/2");
                sounds[3] = Content.Load<Song>("Sounds/3");
                sounds[4] = Content.Load<Song>("Sounds/4");
                sounds[5] = Content.Load<Song>("Sounds/5");
                sounds[6] = Content.Load<Song>("Sounds/6");
                sounds[7] = Content.Load<Song>("Sounds/7");
                sounds[8] = Content.Load<Song>("Sounds/8");
                sounds[9] = Content.Load<Song>("Sounds/9");

                MediaPlayer.Play(sounds[0]);
            }
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            double oldtime;

            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            //if (ks.IsKeyDown(Keys.R)) map = new Map(mapsize, mapsize, forestcount, mountcount, watercount, resourcecount);

            if (ks.IsKeyDown(Keys.D1)) speed = 1;
            if (ks.IsKeyDown(Keys.D2)) speed = 2;
            if (ks.IsKeyDown(Keys.D3)) speed = 3;
            if (ks.IsKeyDown(Keys.D4)) speed = 4;
            if (ks.IsKeyDown(Keys.D5)) speed = 5;
            if (ks.IsKeyDown(Keys.D0)) speed = 0;

            camera.Update(Mouse.GetState());
            gui.Update(camera.mousestate, state, gameTime);
            for (int s = 0; s < speed; s++)
            {
                if (time > 10 && random.Next((int)(1000000 * gameTime.ElapsedGameTime.TotalSeconds)) == 0)
                {
                    #region Bad
                    if (random.Next() % map.GetLuck() == 0)
                    {
                        switch (random.Next() % 4)
                        {
                            case 0:
                            case 1:
                            case 2: int k = random.Next(map.terrains.Count);
                                if (k > 0)
                                {
                                    //map.terrains.RemoveAt(k);
                                    for (int i = 0; i < map.height; i++)
                                        for (int j = 0; j < map.width; j++)
                                            if (map.indexes[i, j] == k &&
                                               Map.MapCellTypeToMapCell((MapCellType)map.mapData[i, j]) == MapCell.Resource)
                                            {
                                                if (map.mapData[i, j] - (int)MapCellType.Forest1 >= 0 && map.mapData[i, j] - (int)MapCellType.Forest1 < 4)
                                                {
                                                    map.mapData[i, j] = (int)MapCellType.Bog1 + random.Next(4);
                                                    map.indexes[i, j] = -1;
                                                    gui.eventmessage = langManager.strings[(int)LangString.ForestBad];
                                                    gui.rightPanelState = RightPanelStates.Event;
                                                    gui.rightPanelCommands.Clear();
                                                }
                                                if (map.mapData[i, j] - (int)MapCellType.Lake1 >= 0 && map.mapData[i, j] - (int)MapCellType.Lake1 < 4)
                                                {
                                                    map.mapData[i, j] = (int)MapCellType.Bog1 + random.Next(4);
                                                    map.indexes[i, j] = -1;
                                                    gui.eventmessage = langManager.strings[(int)LangString.LakeBad];
                                                    gui.rightPanelState = RightPanelStates.Event;
                                                    gui.rightPanelCommands.Clear();
                                                }
                                                if (map.mapData[i, j] - (int)MapCellType.Field1 >= 0 && map.mapData[i, j] - (int)MapCellType.Field1 < 4)
                                                {
                                                    map.mapData[i, j] = (int)MapCellType.Grass1 + random.Next(4);
                                                    map.indexes[i, j] = -1;
                                                    gui.eventmessage = langManager.strings[(int)LangString.FieldBad];
                                                    gui.rightPanelState = RightPanelStates.Event;
                                                    gui.rightPanelCommands.Clear();
                                                }
                                            }
                                }
                                break;
                            case 3: int b = random.Next(map.buildings.Count);
                                if (map.buildings[b].lvl > 0)
                                {
                                    map.buildings[b].lvl--;
                                    gui.eventmessage = langManager.strings[(int)LangString.BuildingBad];
                                    gui.rightPanelState = RightPanelStates.Event;
                                    gui.rightPanelCommands.Clear();
                                }
                                break;
                        }
                    }
                    #endregion
                    #region Good
                    else
                    {
                        switch (random.Next() % 5)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3: int k = random.Next(map.terrains.Count);
                                for (int i = 0; i < map.height; i++)
                                    for (int j = 0; j < map.width; j++)
                                        if (map.indexes[i, j] == k &&
                                           Map.MapCellTypeToMapCell((MapCellType)map.mapData[i, j]) == MapCell.Resource)
                                        {
                                            if (map.terrains[k].resources == map.terrains[k].maxresources)
                                            {
                                                map.terrains[k].resources = (int)(map.terrains[k].resources * 1.5);
                                                map.terrains[k].maxresources = (int)(map.terrains[k].maxresources * 1.5);
                                            }
                                            else
                                            {
                                                map.terrains[k].resources = map.terrains[k].maxresources;
                                            }
                                            if (map.mapData[i, j] - (int)MapCellType.Forest1 >= 0 && map.mapData[i, j] - (int)MapCellType.Forest1 < 4)
                                                gui.eventmessage = langManager.strings[(int)LangString.ForestGood];
                                            if (map.mapData[i, j] - (int)MapCellType.Lake1 >= 0 && map.mapData[i, j] - (int)MapCellType.Lake1 < 4)
                                                gui.eventmessage = langManager.strings[(int)LangString.LakeGood];
                                            if (map.mapData[i, j] - (int)MapCellType.Field1 >= 0 && map.mapData[i, j] - (int)MapCellType.Field1 < 4)
                                                gui.eventmessage = langManager.strings[(int)LangString.FieldGood];
                                            if (map.mapData[i, j] - (int)MapCellType.Rock1 >= 0 && map.mapData[i, j] - (int)MapCellType.Rock1 < 4)
                                                gui.eventmessage = langManager.strings[(int)LangString.MountGood];
                                            if (map.mapData[i, j] - (int)MapCellType.Iron1 >= 0 && map.mapData[i, j] - (int)MapCellType.Iron1 < 4)
                                                gui.eventmessage = langManager.strings[(int)LangString.MountGood];
                                            gui.rightPanelState = RightPanelStates.Event;
                                            gui.rightPanelCommands.Clear();
                                        }
                                break;
                            case 4: int b = random.Next(map.buildings.Count);
                                if (map.buildings[b].lvl < 2)
                                {
                                    map.buildings[b].lvl++;
                                    gui.eventmessage = langManager.strings[(int)LangString.BuildingGood];
                                    gui.rightPanelState = RightPanelStates.Event;
                                    gui.rightPanelCommands.Clear();
                                }
                                break;
                        }
                    }
                    #endregion
                }

                oldtime = time;
                time += gameTime.ElapsedGameTime.TotalSeconds;
                if (GetDay(oldtime) != GetDay(time) && GetDay(time) % 10 == 0)
                {
                    Inventory upkeep;
                    Resource[] resupkeep;
                    int foodupkeep;

                    foreach (Building b in map.buildings)
                    {
                        if (b.productive > 0)
                        {
                            foodupkeep = 0;
                            resupkeep = b.upkeep;
                            upkeep = Inventory.FromResourceArray(resupkeep);
                            map.resources -= upkeep * b.productive;

                            foreach (Resource r in resupkeep)
                                if (r.type == ResourceType.Food)
                                    foodupkeep += r.number*b.productive;

                            if (map.resources.FindNegative()) b.productive = 0;
                            if (b.productive > 0) if (map.resources.RemoveFood(foodupkeep)) b.productive = 0;
                        }
                    }
                }
                map.Update(gameTime,ref gui);
            }

            if (mission > 0)
            {
                switch (mission)
                {
                    case 1: if (map.resources.wood >= 20 && map.resources.fish >= 20 && map.IsCell(MapCellType.Forester) > 0 && map.IsCell(MapCellType.Fisherman) > 1)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } break;
                    case 2: if (map.resources.meat >= 40 && map.IsCell(MapCellType.Lumberman) > 0 && map.IsCell(MapCellType.StoneQuary) > 0 && map.IsCell(MapCellType.Mill) > 1 && map.IsCell(MapCellType.Castle, 1) > 0 && map.IsCell(MapCellType.Farm) > 0)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } break;
                    case 3: if (map.resources.tools >= 40 && map.resources.swords >= 40 && map.IsCell(MapCellType.Foundry) > 0 && map.IsCell(MapCellType.Blacksmith) > 0 && map.IsCell(MapCellType.Armourer) > 1)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } 
                        if(GetDay(time)>300)
                        {
                            gui.rightPanelState = RightPanelStates.MissionInComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }break;
                    case 4: if (map.terraformed>9)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }
                        if (GetDay(time) > 250||map.resources.gold<100)
                        {
                            gui.rightPanelState = RightPanelStates.MissionInComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } break;
                    case 5: if (map.resources.gold >= 200 && map.IsCell(MapCellType.Barracs) > 1&&map.raidsucces>14)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }
                        if (GetDay(time) > 100)
                        {
                            gui.rightPanelState = RightPanelStates.MissionInComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } break;
                    case 6: if (map.resources.gold >= 800 && map.IsCell(MapCellType.Church,2) > 0)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }break;
                    case 7: if (map.resources.lumber >= 80 && map.resources.gold >= 1500 && map.IsCell(MapCellType.Barracs,2) > 0 &&map.raidsucces>19)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }
                        if (GetDay(time) > 650)
                        {
                            gui.rightPanelState = RightPanelStates.MissionInComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        } break;
                    case 8: if (map.resources.gold >= 1000 && map.IsCell(MapCellType.Foundry, 2) > 0 && map.IsCell(MapCellType.Blacksmith, 2) > 0 && map.IsCell(MapCellType.Armourer, 2) > 0 && map.IsCell(MapCellType.Farm, 2) > 0 && map.IsCell(MapCellType.Church, 2) > 0 && map.IsCell(MapCellType.Mill, 2) > 0 && map.IsCell(MapCellType.Lumberman, 2) > 0 && map.IsCell(MapCellType.Fisherman, 2) > 0 && map.IsCell(MapCellType.Forester, 2) > 0 && map.IsCell(MapCellType.StoneQuary, 2) > 0 && map.IsCell(MapCellType.Barracs, 2) > 0 && map.IsCell(MapCellType.Market, 2) > 0)
                        {
                            gui.rightPanelState = RightPanelStates.MissionComplate;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                        }break;
                }
            }

            if (music) if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(sounds[new Random().Next(9) + 1]);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            #region Game

            if (state == GameState.Game)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
                map.Draw(spriteBatch, tileset, towniesset, resourceset, mapcursor, camera, time, font,dusttexture);

                //string day = langManager.strings[(int)LangString.Day]+" " + GetDay(time).ToString();
                //DrawText(new Vector2(width - 220 - font.MeasureString(day).X, 130), day, Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Gold], new Vector2(0, 0), Color.White);
                DrawText(new Vector2(32, 4), map.resources.gold.ToString(),Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Fish], new Vector2(0, 32), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Corn], new Vector2(10, 32), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Meat], new Vector2(30, 32), Color.White);
                DrawText(new Vector2(64, 36), map.resources.Food.ToString(),Color.White);

                spriteBatch.DrawString(font, debugMessage, new Vector2(0, 20), Color.White);
                spriteBatch.End();
            }
            #endregion

            if (state == GameState.MainMenu || state == GameState.StartGameMenu || state == GameState.StartGameMenuMissions || state == GameState.StartGameMenuModes || state == GameState.LoadMenu)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
                DrawTexturedRect(backgroundlight, new Rectangle(0,0, width, height));
                spriteBatch.End();
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone);
            gui.Draw(outline, backgrounddark, backgroundlight, state,spriteBatch,font);
            if (state == GameState.Game)
            {
                string day = langManager.strings[(int)LangString.Day] + " " + GetDay(time).ToString();
                DrawText(new Vector2(width - 200, 50), day, Color.White);
            }
            spriteBatch.Draw(cursor, camera.mouse, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        void Nothing(ref GuiObject me) { ;}
        void Main(ref GuiObject me)
        {
            if (me.lclick)
            {
                gui.rightPanelCommands.Clear();
                Vector2 c = camera.mouse + camera.position - camera.offcet +new Vector2(34, 17);//68 34

                int x = (int)c.X ;
                int y = (int)c.Y;

                camera.mapposition.X = (x / 68 + y / 34) / 2;
                camera.mapposition.Y = x / 68 - camera.mapposition.X;

                CreateMenu();

            }
        }
        void GameMenuButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                Vector2 pos = camera.mouse - new Vector2(me.rect.X, me.rect.Y);
                if (pos.X < 40 && pos.Y < 40)
                {
                    gui.rightPanelState = RightPanelStates.Inventory;
                    gui.rightPanelCommands.Clear();
                }
                if (pos.X > 40 && pos.Y < 40 && pos.X < 80)
                {
                    gui.rightPanelCommands.Clear();
                    if (gui.rightPanelState == RightPanelStates.Quest) gui.rightPanelState = RightPanelStates.QuestGoal;
                    else gui.rightPanelState = RightPanelStates.Quest;
                }
                if (pos.X > 80 && pos.Y < 40 && pos.X < 120)
                {
                    gui.rightPanelCommands.Clear();
                    gui.rightPanelState = RightPanelStates.AllUpkeep;
                }
                if (pos.X > 120 && pos.Y < 40 && pos.X < 160)
                {
                    if (timekoef != 0)
                    {
                        Save();
                        gui.messages.Add(new ColorMessage(langManager.strings[(int)LangString.Saved], Color.White, 2));
                    }
                    else gui.messages.Add(new ColorMessage(langManager.strings[(int)LangString.CantSave], Color.White, 2));
                }
                if (pos.X > 160 && pos.Y < 40)
                {
                    if (timekoef != 0)
                    {
                        if(Load())
                            gui.messages.Add(new ColorMessage(langManager.strings[(int)LangString.Loaded], Color.White, 2));
                    }
                    else gui.messages.Add(new ColorMessage(langManager.strings[(int)LangString.CantLoad], Color.White, 2));
                }
            }
        }
        void RightPanel(ref GuiObject me)
        {
            if (me.lclick)
            {
                Rectangle r = new Rectangle(width - 205, 130 + 355, 205, 39);

                if (r.Contains((int)camera.mouse.X,(int)camera.mouse.Y) )
                {
                    int id = (int)((int)(camera.mouse.X - r.X) / r.Height);

                    if (id >= 0 && id < gui.rightPanelCommands.Count)
                    {
                        switch (gui.rightPanelCommands[id])
                        {
                            case RightPanelCommand.Help: camera.light = Color.White; gui.rightPanelState = RightPanelStates.Help; gui.rightPanelCommands.Clear(); break;
                            case RightPanelCommand.Attack: int sold = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].resources;
                                int x,y;
                                switch(new Random().Next(2))
                                {
                                    case 0:y=-1;x=new Random().Next(map.width);break;
                                        default:y=map.width+1;x=new Random().Next(map.width);break;
                                }
                                for (int i = 0; i < sold; i++)
                                {
                                    if (map.resources.swords > 0)
                                    {
                                        map.resources.swords--;
                                        map.townies.Add(new Townie(camera.mapposition, TownieProfetion.soldier, map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X], camera.mapposition));
                                        map.townies[map.townies.Count - 1].target = new Vector2(x, y);
                                        map.townies[map.townies.Count - 1].state = TownieState.go;
                                        map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].resources--;
                                        map.townies[map.townies.Count - 1].wait = 0.45f * i;
                                    }
                                }
                                break;
                            case RightPanelCommand.Build: gui.rightPanelState = RightPanelStates.Build; gui.buildterraid = 0; gui.rightPanelCommands.Clear(); 
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Up);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Down);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.No);
                                                                                                                              break;
                            case RightPanelCommand.Degrade: gui.rightPanelState = RightPanelStates.Degrade; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.Ok); gui.rightPanelCommands.Add(RightPanelCommand.No); break;
                            case RightPanelCommand.Down: if (gui.rightPanelState == RightPanelStates.Market2)
                                                            gui.resourceadd--;
                                                         else
                                                            gui.buildterraid += 11;
                                                         if (gui.rightPanelState == RightPanelStates.Productive)
                                                         {
                                                            int bid = map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                                                            map.buildings[bid].productive = map.buildings[bid].productive > 0 ? map.buildings[bid].productive - 1 : 0;
                                                            if(bid==0)Townie.speed = 0.01f + 0.002f * (map.buildings[bid].productive - 2);
                                                         }
                                                         if (gui.rightPanelState == RightPanelStates.Church)
                                                         {
                                                             int bid = map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                                                             if(map.buildings[bid].resources > 0)
                                                             {
                                                                 map.buildings[bid].resources--;
                                                                 map.resources[(int)ResourceType.Gold] = map.resources[(int)ResourceType.Gold] + 1;
                                                             }
                                                         }
                                                         break;
                            case RightPanelCommand.Ok: gui.rightPanelCommands.Clear();
                                                       if(gui.rightPanelState==RightPanelStates.Build)
                                                           AddBuilding();
                                                       else if (gui.rightPanelState == RightPanelStates.Terraform)
                                                           AddTerrain();
                                                       else if (gui.rightPanelState == RightPanelStates.Upgrade)
                                                           UpgadeBuilding();
                                                       else if (gui.rightPanelState == RightPanelStates.Degrade)
                                                           DegradeBuilding();
                                                       else if (gui.rightPanelState == RightPanelStates.MissionInComplate)
                                                           state=GameState.MainMenu;
                                                       else if (gui.rightPanelState == RightPanelStates.MissionComplate)
                                                       {
                                                           switch (mission)
                                                           {
                                                               case 1: Mission2(); break;
                                                               case 2: Mission3(); break;
                                                               case 3: Mission4(); break;
                                                               case 4: Mission5(); break;
                                                               case 5: Mission6(); break;
                                                               case 6: Mission7(); break;
                                                               case 7: Mission8(); break;
                                                               case 8: state = GameState.MainMenu; break;
                                                           }
                                                       }
                                                       else if (gui.rightPanelState == RightPanelStates.Market2)
                                                       {
                                                           int old = map.resources[gui.resourceid];
                                                           map.resources[gui.resourceid] = Math.Min(Math.Max(map.resources[gui.resourceid] + gui.resourceadd,0),map.CastleStorage);
                                                           int det = map.resources[gui.resourceid] - old;
                                                           int dg = 0;
                                                           if (det > 0)
                                                           {
                                                               switch (map.MarketLvl)
                                                               {
                                                                   case 0: dg = Helper.resourceHelper[gui.resourceid].lvl1buy * det; break;
                                                                   case 1: dg = Helper.resourceHelper[gui.resourceid].lvl2buy * det; break;
                                                                   case 2: dg = Helper.resourceHelper[gui.resourceid].lvl3buy * det; break;
                                                               }
                                                           }
                                                           if (det < 0)
                                                           {
                                                               switch (map.MarketLvl)
                                                               {
                                                                   case 0: dg = Helper.resourceHelper[gui.resourceid].lvl1sell * det; break;
                                                                   case 1: dg = Helper.resourceHelper[gui.resourceid].lvl2sell * det; break;
                                                                   case 2: dg = Helper.resourceHelper[gui.resourceid].lvl3sell * det; break;
                                                               }
                                                           }
                                                           if (dg <= map.resources[(int)ResourceType.Gold]) map.resources[(int)ResourceType.Gold] -= dg;
                                                           gui.rightPanelState = RightPanelStates.Market;
                                                       }
                                                       else gui.rightPanelState = RightPanelStates.Nothing; CreateMenu();break;
                            case RightPanelCommand.No: gui.rightPanelCommands.Clear(); CreateMenu(); break;
                            case RightPanelCommand.Productive: gui.rightPanelState = RightPanelStates.Productive; gui.rightPanelCommands.Clear();
                                                                                                                  gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                                                                                                                  gui.rightPanelCommands.Add(RightPanelCommand.Up);
                                                                                                                  gui.rightPanelCommands.Add(RightPanelCommand.Down);
                                                                                                                  gui.rightPanelCommands.Add(RightPanelCommand.No);
                                                                                                                  break;
                            case RightPanelCommand.Terraform: gui.rightPanelState = RightPanelStates.Terraform; gui.buildterraid = 0; gui.rightPanelCommands.Clear(); 
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Up);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.Down);
                                                                                                                              gui.rightPanelCommands.Add(RightPanelCommand.No);
                                                                                                                              break;
                            case RightPanelCommand.Up: if (gui.rightPanelState == RightPanelStates.Market2)
                                                            gui.resourceadd++;
                                                       else
                                                            gui.buildterraid ++; 
                                                         if (gui.rightPanelState == RightPanelStates.Productive)
                                                         {
                                                            int bid = map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                                                            map.buildings[bid].productive = map.buildings[bid].productive < map.buildings[bid].maxprouctive ? map.buildings[bid].productive + 1 : map.buildings[bid].maxprouctive;
                                                            if (bid == 0) Townie.speed = 0.01f + 0.002f * (map.buildings[bid].productive - 2);
                                                         }
                                                         if (gui.rightPanelState == RightPanelStates.Church)
                                                         {
                                                             int bid = map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                                                             if (map.buildings[bid].resources < map.buildings[bid].storage && map.resources[(int)ResourceType.Gold] > 0)
                                                             {
                                                                 map.buildings[bid].resources++;
                                                                 map.resources[(int)ResourceType.Gold] = map.resources[(int)ResourceType.Gold]-1;
                                                             }
                                                         }
                                                         break;
                            case RightPanelCommand.Upgrade: gui.rightPanelState = RightPanelStates.Upgrade; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.Ok); gui.rightPanelCommands.Add(RightPanelCommand.No); break;
                            case RightPanelCommand.Upkeep: gui.rightPanelState = RightPanelStates.Upkeep; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.Ok); break;
                            case RightPanelCommand.Market: gui.rightPanelState = RightPanelStates.Market; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.No); break;
                            case RightPanelCommand.Church: gui.rightPanelState = RightPanelStates.Church; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.Ok); gui.rightPanelCommands.Add(RightPanelCommand.Up); gui.rightPanelCommands.Add(RightPanelCommand.Down); break;
                            case RightPanelCommand.Inventory: gui.rightPanelState = RightPanelStates.Inventory; gui.rightPanelCommands.Clear(); gui.rightPanelCommands.Add(RightPanelCommand.Ok); break;
                            
                        }
                    }
                }

                if (gui.rightPanelState == RightPanelStates.Market)
                {
                    int ysize = 300;
                    int dy = ysize / 5;
                    int starty = 130+50;
                    int startx = 20;
                    int id=-1;

                    for (int i = 0; i < 9; i++)
                    {
                        if (i % 2 == 0)
                        {
                            //DrawColorRect(new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                            if (new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38).Contains((int)camera.mouse.X, (int)camera.mouse.Y))
                                id = i;
                        }
                        else
                            if (new Rectangle(width - 100 + startx - 3, starty + i / 2 * dy - 3, 38, 38).Contains((int)camera.mouse.X, (int)camera.mouse.Y))
                                id = i;
                    }

                    if (id != -1)
                    {
                        gui.buildterraid = id;
                        gui.resourceid = 0;
                        int lvl = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].productive;

                        bool ok = false;

                        switch (gui.buildterraid)
                        {
                            case 0: gui.resourceid = (int)ResourceType.Wood; ok = lvl > 0; break;
                            case 1: gui.resourceid = (int)ResourceType.Lumber; ok = lvl > 2; break;
                            case 2: gui.resourceid = (int)ResourceType.Corn; ok = lvl > 0; break;
                            case 3: gui.resourceid = (int)ResourceType.Fish; ok = lvl > 0; break;
                            case 4: gui.resourceid = (int)ResourceType.Meat; ok = lvl > 0; break;
                            case 5: gui.resourceid = (int)ResourceType.Stone; ok = lvl > 1; break;
                            case 6: gui.resourceid = (int)ResourceType.Iron; ok = lvl > 2; break;
                            case 7: gui.resourceid = (int)ResourceType.Tools; ok = lvl > 3; break;
                            case 8: gui.resourceid = (int)ResourceType.Sword; ok = lvl > 4; break;
                        }

                        if (ok)
                        {
                            gui.resourceadd = 0;
                            gui.rightPanelState = RightPanelStates.Market2;
                            gui.rightPanelCommands.Clear();
                            gui.rightPanelCommands.Add(RightPanelCommand.Ok);
                            gui.rightPanelCommands.Add(RightPanelCommand.Up);
                            gui.rightPanelCommands.Add(RightPanelCommand.Down);
                        }
                    }
                }
            }
        }
        void CreateMenu()
        {
            if ((int)camera.mapposition.Y >= 0 && (int)camera.mapposition.Y < map.height && (int)camera.mapposition.X >= 0 && (int)camera.mapposition.X < map.width)
            {
                MapCell mt = Map.MapCellTypeToMapCell((MapCellType)map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X]);

                if (mt == MapCell.Bog) { gui.rightPanelState = RightPanelStates.Bog; gui.rightPanelCommands.Add(RightPanelCommand.Terraform); }
                if (mt == MapCell.Building)
                {
                    Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                    gui.rightPanelState = RightPanelStates.Building;
                    gui.rightPanelCommands.Add(RightPanelCommand.Upkeep);
                    gui.rightPanelCommands.Add(RightPanelCommand.Productive);
                    if (b.type == BuildingType.Barracs) gui.rightPanelCommands.Add(RightPanelCommand.Attack);
                    if (b.lvl < 2) gui.rightPanelCommands.Add(RightPanelCommand.Upgrade);
                    if (!(b.lvl == 0 && b.type == BuildingType.Castle)) gui.rightPanelCommands.Add(RightPanelCommand.Degrade);
                    if (b.type == BuildingType.Market) gui.rightPanelCommands.Add(RightPanelCommand.Market);
                    if (b.type == BuildingType.Castle) gui.rightPanelCommands.Add(RightPanelCommand.Inventory);
                    if (b.type == BuildingType.Church) gui.rightPanelCommands.Add(RightPanelCommand.Church);
                }
                if (mt == MapCell.Grass)
                {
                    gui.rightPanelState = RightPanelStates.Grass;
                    gui.rightPanelCommands.Add(RightPanelCommand.Build);
                    gui.rightPanelCommands.Add(RightPanelCommand.Terraform);
                }
                if (mt == MapCell.Resource)
                {
                    MapCellType id = (MapCellType)map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                    if (id == MapCellType.Forest1 || id == MapCellType.Forest2 || id == MapCellType.Forest3 || id == MapCellType.Forest4 ||
                        id == MapCellType.Field1 || id == MapCellType.Field2 || id == MapCellType.Field3 || id == MapCellType.Field4 ||
                        id == MapCellType.Lake1 || id == MapCellType.Lake2 || id == MapCellType.Lake3 || id == MapCellType.Lake4)
                        gui.rightPanelCommands.Add(RightPanelCommand.Terraform);

                    gui.rightPanelState = RightPanelStates.Resourse;
                }

                if (map.waitTime[(int)camera.mapposition.Y, (int)camera.mapposition.X] > 0)
                    gui.rightPanelCommands.Clear();
            }
            else
                gui.rightPanelState = RightPanelStates.Nothing;
        }

        void StartGameButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.StartGameMenuModes;
                gui.elements[24].lpressed = true;
            }
        }
        void GenerateButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.Game;
                map = new Map(mapsize,mapsize,forestcount,mountcount,watercount,resourcecount);
                camera = new Camera(new Vector2(width, height), new Vector2(mapsize));
                mission = -1;
                gui.messages.Add(new ColorMessage("Map generated",Color.White,2));

                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void RandomMapButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                timekoef = 1;
                state = GameState.StartGameMenu;
            }
        }
        void SandboxButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                timekoef = 0;
                state = GameState.StartGameMenu;
            }
        }
        void MissionsButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                timekoef = 1;
                state = GameState.StartGameMenuMissions;
                gui.elements[27].lpressed = true;
            }
        }
        void LoadGameButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.LoadMenu;
                gui.elements[39].lpressed = true;
            }
        }
        void HelpButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                System.Diagnostics.Process.Start("Help.txt");
            }
        }
        void ExitButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                this.Exit();
            }
        }
        void BackButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.StartGameMenuModes;
            }
        }
        void BackModesButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.MainMenu;
            }
        }
        void LoadMissionButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mission = 1;
                if (Load())
                {
                    state = GameState.Game;
                    timekoef = 1;
                }
            }
        }
        void LoadRandomButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mission = -1;
                if (Load())
                {
                    state = GameState.Game;
                    timekoef = 1;
                }
            }
        }
        void ToMenu(ref GuiObject me)
        {
            if (me.lclick)
            {
                state = GameState.MainMenu;
                gamestarted = true;
            }
        }
        void ToGame(ref GuiObject me)
        {
            if (me.lclick&&gamestarted)
            {
                state = GameState.Game;
                
            }
        }

        #region GenerateButtons
        void SmallMapButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mapsize = 8;
                gui.elements[5].darktransparency = false;
                gui.elements[6].darktransparency = false;
                gui.elements[7].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void MediumMapButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mapsize = 12;
                gui.elements[5].darktransparency = false;
                gui.elements[6].darktransparency = false;
                gui.elements[7].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void LargeMapButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mapsize = 16;
                gui.elements[5].darktransparency = false;
                gui.elements[6].darktransparency = false;
                gui.elements[7].darktransparency = false;
                me.darktransparency = true;
            }
        }

        void SmallForestButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                forestcount = 0;
                gui.elements[8].darktransparency = false;
                gui.elements[9].darktransparency = false;
                gui.elements[10].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void MediumForestButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                forestcount = 1;
                gui.elements[8].darktransparency = false;
                gui.elements[9].darktransparency = false;
                gui.elements[10].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void LargeForestButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                forestcount = 2;
                gui.elements[8].darktransparency = false;
                gui.elements[9].darktransparency = false;
                gui.elements[10].darktransparency = false;
                me.darktransparency = true;
            }
        }

        void SmallWaterButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                watercount = 0;
                gui.elements[11].darktransparency = false;
                gui.elements[12].darktransparency = false;
                gui.elements[13].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void MediumWaterButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                watercount = 1;
                gui.elements[11].darktransparency = false;
                gui.elements[12].darktransparency = false;
                gui.elements[13].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void LargeWaterButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                watercount = 2;
                gui.elements[11].darktransparency = false;
                gui.elements[12].darktransparency = false;
                gui.elements[13].darktransparency = false;
                me.darktransparency = true;
            }
        }

        void SmallRocksButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mountcount = 0;
                gui.elements[14].darktransparency = false;
                gui.elements[15].darktransparency = false;
                gui.elements[16].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void MediumRocksButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mountcount = 1;
                gui.elements[14].darktransparency = false;
                gui.elements[15].darktransparency = false;
                gui.elements[16].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void LargeRocksButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                mountcount = 2;
                gui.elements[14].darktransparency = false;
                gui.elements[15].darktransparency = false;
                gui.elements[16].darktransparency = false;
                me.darktransparency = true;
            }
        }

        void SmallResourcesButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                resourcecount = 0;
                gui.elements[17].darktransparency = false;
                gui.elements[18].darktransparency = false;
                gui.elements[19].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void MediumResourcesButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                resourcecount = 1;
                gui.elements[17].darktransparency = false;
                gui.elements[18].darktransparency = false;
                gui.elements[19].darktransparency = false;
                me.darktransparency = true;
            }
        }
        void LargeResourcesButton(ref GuiObject me)
        {
            if (me.lclick)
            {
                resourcecount = 2;
                gui.elements[17].darktransparency = false;
                gui.elements[18].darktransparency = false;
                gui.elements[19].darktransparency = false;
                me.darktransparency = true;
            }
        }
        #endregion

        #region Mission buttons
        void Mission1Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission1();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission2Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission2();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission3Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission3();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission4Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission4();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission5Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission5();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission6Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission6();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission7Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission7();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }
        void Mission8Button(ref GuiObject me)
        {
            if (me.lclick)
            {
                Mission8();
                if (music) MediaPlayer.Play(sounds[new Random().Next(9) + 1]);
            }
        }

        void Mission1()
        {
                mission = 1;
                map = new Map(7, 7, 1, 0, 0, 2);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(7, 7));
                map.resources.gold = 200;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission2()
        {
                mission = 2;
                map = new Map(8, 8, 0, 0, 0, 2);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(8, 8));
                map.resources.gold = 120;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission3()
        {
                mission = 3;
                map = new Map(7, 7, 0, 0, 0, 2);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(7, 7));
                map.resources.gold = 150;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission4()
        {
                mission = 4;
                map = new Map(8, 8, 0, 2, 0, 1);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(8, 8));
                map.resources.gold = 200;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission5()
        {
                mission = 5;
                map = new Map(8, 8, 1, 1, -1, 2, 1);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(8, 8));
                map.resources.gold = 100;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission6()
        {
                mission = 6;
                map = new Map(8, 8, 0, 0, 0, 2, 4);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(8, 8));
                map.resources.gold = 400;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission7()
        {
                mission = 7;
                map = new Map(8, 8, 0, 0, 0, 2, 1);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(8, 8));
                map.resources.gold = 300;
                gui.rightPanelState = RightPanelStates.Quest;
        }
        void Mission8()
        {
                mission = 8;
                map = new Map(10, 10, 1, 1, 1, 2, 1);
                state = GameState.Game;
                speed = 1;
                camera = new Camera(new Vector2(width, height), new Vector2(10, 10));
                gui.rightPanelState = RightPanelStates.Quest;
        }
        #endregion

        void NothingDraw(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me) { ;}
        void MapGuiDraw( Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me)
        {
            int selectedx = (int)camera.mapposition.X, selectedy = (int)camera.mapposition.Y;

            if (selectedy >= 0 && selectedy < map.height && selectedx >= 0 && selectedx < map.width)
            {
                MapCell mc = Map.MapCellTypeToMapCell((MapCellType)map.mapData[selectedy, selectedx]);
                ResourceType rt = Map.MapCellTypeToResource((MapCellType)map.mapData[selectedy, selectedx]);
                if (map.waitTime[selectedy, selectedx] > 0)
                {
                    if (mc == MapCell.Building)
                    {
                        int id = map.indexes[selectedy, selectedx];
                        Building b = map.buildings[id];
                        float fullcoef = ((float)(map.waitTime[selectedy, selectedx])) / b.buildtime / 10;

                        Vector2 pos = new Vector2((int)camera.mapposition.X * 68 + (int)camera.mapposition.Y * 68, (int)camera.mapposition.X * 34 - (int)camera.mapposition.Y * 34) - camera.position + camera.offcet;
                        Rectangle r = new Rectangle((int)pos.X - 45, (int)pos.Y - 170, 90, 50);
                        DrawTexturedRect(darkbackground, r);
                        spriteBatch.Draw(resourceset[(int)rt], new Vector2((int)pos.X - 45 + 9, (int)pos.Y - 170 + 9), Color.White);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 16 + 9 + 4, (int)pos.Y - 170 + 21, 52, 6), Color.Black);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 16 + 9 + 5, (int)pos.Y - 170 + 22, (int)(50 * fullcoef), 4), Color.Blue);
                        DrawOutLine(line, r);
                    }
                    else//if (mc == MapCell.Resource)
                    {
                        int id = map.mapData[selectedy, selectedx];
                        TerrainType tt=TerrainType.Grass;
                        if(id<4)tt=TerrainType.Grass;
                        else if(id<12)tt=TerrainType.Forest;
                        else if(id<20)tt=TerrainType.Field;
                        else if (id < 24) tt = TerrainType.Lake;

                        float fullcoef = ((float)(map.waitTime[selectedy, selectedx])) / Helper.terrainHelper[(int)tt].terraformingtime / 10;

                        Vector2 pos = new Vector2((int)camera.mapposition.X * 68 + (int)camera.mapposition.Y * 68, (int)camera.mapposition.X * 34 - (int)camera.mapposition.Y * 34) - camera.position + camera.offcet;
                        Rectangle r = new Rectangle((int)pos.X - 45, (int)pos.Y - 170, 90, 50);
                        DrawTexturedRect(darkbackground, r);
                        if(id>3)
                        spriteBatch.Draw(resourceset[(int)rt], new Vector2((int)pos.X - 45 + 9, (int)pos.Y - 170 + 9), Color.White);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 16 + 9 + 4, (int)pos.Y - 170 + 21, 52, 6), Color.Black);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 16 + 9 + 5, (int)pos.Y - 170 + 22, (int)(50 * fullcoef), 4), Color.Blue);
                        DrawOutLine(line, r);
                    }
                }
                else
                {
                    if (mc == MapCell.Resource)
                    {
                        Vector2 pos = new Vector2((int)camera.mapposition.X * 68 + (int)camera.mapposition.Y * 68, (int)camera.mapposition.X * 34 - (int)camera.mapposition.Y * 34) - camera.position + camera.offcet;
                        Rectangle r = new Rectangle((int)pos.X - 45, (int)pos.Y - 170, 90, 50);
                        DrawTexturedRect(darkbackground, r);
                        spriteBatch.Draw(resourceset[(int)rt], new Vector2((int)pos.X - 45 + 9, (int)pos.Y - 170 + 9), Color.White);

                        float fullcoef = ((float)(map.terrains[map.indexes[selectedy, selectedx]].resources)) / map.terrains[map.indexes[selectedy, selectedx]].maxresources;
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 4, (int)pos.Y - 170 + 21, 52, 6), Color.Black);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 5, (int)pos.Y - 170 + 22, (int)(50 * fullcoef), 4), Color.Orange);

                        DrawOutLine(line, r);
                    }
                    if (mc == MapCell.Building)
                    {
                        Vector2 pos = new Vector2((int)camera.mapposition.X * 68 + (int)camera.mapposition.Y * 68, (int)camera.mapposition.X * 34 - (int)camera.mapposition.Y * 34) - camera.position + camera.offcet;
                        Rectangle r = new Rectangle((int)pos.X - 45, (int)pos.Y - 170, 90, 50);
                        DrawTexturedRect(darkbackground, r);
                        spriteBatch.Draw(resourceset[(int)rt], new Vector2((int)pos.X - 45 + 9, (int)pos.Y - 170 + 9), Color.White);

                        int id = map.indexes[selectedy, selectedx];
                        Building b = map.buildings[id];
                        float fullcoef = ((float)(b.resources)) / Helper.buildingHelper[(int)b.type, b.lvl].maxproduction;
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 4, (int)pos.Y - 170 + 21, 52, 6), Color.Black);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 5, (int)pos.Y - 170 + 22, (int)(50 * fullcoef), 4), Color.Orange);

                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 4, (int)pos.Y - 170 + 11, 52, 6), Color.Black);
                        DrawColorRect(new Rectangle((int)pos.X - 45 + 9 + 16 + 5, (int)pos.Y - 170 + 12, (int)(50 * b.productive / 5), 4), Color.Red);

                        spriteBatch.Draw(star, new Vector2((int)pos.X - 45 + 9 + 16 + 6, (int)pos.Y - 170 + 28), Color.White);
                        if (b.lvl > 0) spriteBatch.Draw(star, new Vector2((int)pos.X - 45 + 9 + 16 + 6 + 16, (int)pos.Y - 170 + 28), Color.White);
                        if (b.lvl > 1) spriteBatch.Draw(star, new Vector2((int)pos.X - 45 + 9 + 16 + 6 + 32, (int)pos.Y - 170 + 28), Color.White);

                        DrawOutLine(line, r);
                    }
                }
            }
        }
        void RightPanelDraw( Texture2D line, Texture2D darkbackground, Texture2D lightbackground,  ref GuiObject me)
        {
            DrawColorRect(new Rectangle(width-205,0,205,height),new Color(0,0,0,0.5f));
            for (int i = 0; i < height; i += 209)//size of treemidle
                spriteBatch.Draw(treemiddle, new Vector2(width - treemiddle.Width, i), Color.White);
            spriteBatch.Draw(treeup, new Vector2(width - treeup.Width, 0), Color.White);
            spriteBatch.Draw(treedown, new Vector2(width - treedown.Width, height - treedown.Height), Color.White);

            DrawTexturedRect(ropetexture, new Rectangle(width - 190, 0, 8, height));

            int h = GetTime(time) / 10;

            Rectangle r = new Rectangle(h % 3 * 200, h / 3 * 62, 200, 62);
            DrawColorRect(new Rectangle(width - 201 - 4, 50 - 4, 200 + 8, 62 + 8), new Color(17, 17, 17));
            spriteBatch.Draw(timeofdayset, new Vector2(width - 201, 50), r, Color.White);
            DrawOutLine(line, new Rectangle(width - 201 - 4, 50 - 4, 200 + 8, 62 + 8));

            int y = 130;
            #region DrawMainInfo
            if (gui.rightPanelState == RightPanelStates.Building || gui.rightPanelState == RightPanelStates.Resourse || gui.rightPanelState == RightPanelStates.Grass || gui.rightPanelState == RightPanelStates.Construct || gui.rightPanelState == RightPanelStates.Bog)
            {
                string text = "";
                int id = map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));

                if (gui.rightPanelState == RightPanelStates.Building)
                {
                    switch ((MapCellType)id)
                    {
                        case MapCellType.Armourer: text = langManager.strings[(int)LangString.Armourer]; break;
                        case MapCellType.Barracs: text = langManager.strings[(int)LangString.Barracs]; break;
                        case MapCellType.Blacksmith: text = langManager.strings[(int)LangString.Blacksmith]; break;
                        case MapCellType.Castle: text = langManager.strings[(int)LangString.Castle]; break;
                        case MapCellType.Church: text = langManager.strings[(int)LangString.Church]; break;
                        case MapCellType.Farm: text = langManager.strings[(int)LangString.Farm]; break;
                        case MapCellType.Fisherman: text = langManager.strings[(int)LangString.Fisherman]; break;
                        case MapCellType.Forester: text = langManager.strings[(int)LangString.Forester]; break;
                        case MapCellType.Foundry: text = langManager.strings[(int)LangString.Foundry]; break;
                        case MapCellType.Lumberman: text = langManager.strings[(int)LangString.Lumberman]; break;
                        case MapCellType.Market: text = langManager.strings[(int)LangString.Market]; break;
                        case MapCellType.Mill: text = langManager.strings[(int)LangString.Mill]; break;
                        case MapCellType.StoneQuary: text = langManager.strings[(int)LangString.StoneQuary]; break;
                    }

                    //if ((MapCellType)id == MapCellType.Castle)

                        spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);

                    Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                    float fullcoef = ((float)(b.resources)) / b.storage;

                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 75, y - 150 - 5 - 40 + 220, 150, 10), Color.Black);
                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 74, y - 150 - 4 - 40 + 220, b.productive * 148 / 5, 8), Color.Red);
                    spriteBatch.DrawString(font, (b.productive * 20).ToString() + "%", new Vector2(width - 100 - 75, y - 150 - 5 - 30 + 220), Color.White);

                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 75, y - 150 - 5 + 220, 150, 10), Color.Black);
                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 74, y - 150 - 4 + 220, (int)(fullcoef * 148), 8), Color.Orange);
                    spriteBatch.DrawString(font, b.resources.ToString() + "\\" + b.storage.ToString(), new Vector2(width - 100 - 75, y - 150 + 5 + 220), Color.White);

                    spriteBatch.Draw(star, new Vector2(width - 100 - 40, y + 40 + 220), Color.White);
                    if (b.lvl > 0) spriteBatch.Draw(star, new Vector2(width - 100, y + 40 + 220), Color.White);
                    if (b.lvl > 1) spriteBatch.Draw(star, new Vector2(width - 100 + 40, y + 40 + 220), Color.White);
                }

                if (gui.rightPanelState == RightPanelStates.Resourse)
                {
                    switch ((MapCellType)id)
                    {
                        case MapCellType.Field1:
                        case MapCellType.Field2:
                        case MapCellType.Field3:
                        case MapCellType.Field4: text = langManager.strings[(int)LangString.Field]; break;

                        case MapCellType.Forest1:
                        case MapCellType.Forest2:
                        case MapCellType.Forest3:
                        case MapCellType.Forest4: text = langManager.strings[(int)LangString.Forest]; break;

                        case MapCellType.Iron1:
                        case MapCellType.Iron2:
                        case MapCellType.Iron3:
                        case MapCellType.Iron4: text = langManager.strings[(int)LangString.Iron]; break;

                        case MapCellType.Lake1:
                        case MapCellType.Lake2:
                        case MapCellType.Lake3:
                        case MapCellType.Lake4: text = langManager.strings[(int)LangString.Lake]; break;

                        case MapCellType.Rock1:
                        case MapCellType.Rock2:
                        case MapCellType.Rock3:
                        case MapCellType.Rock4: text = langManager.strings[(int)LangString.Rock]; break;

                    }
                    spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);

                    float fullcoef = ((float)(map.terrains[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].resources)) / map.terrains[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].maxresources;
                    if (map.waitTime[(int)camera.mapposition.Y, (int)camera.mapposition.X] > 0)
                        fullcoef = 0;
                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 75, y - 150 - 5 + 220, 150, 10), Color.Black);
                    spriteBatch.Draw(emprypixel, new Rectangle(width - 100 - 74, y - 150 - 4 + 220, (int)(fullcoef * 148), 8), Color.Orange);
                    if (map.waitTime[(int)camera.mapposition.Y, (int)camera.mapposition.X] <= 0)
                        spriteBatch.DrawString(font, map.terrains[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]].resources.ToString(), new Vector2(width - 100 - 75, y - 150 + 5 + 220), Color.White);
                    else spriteBatch.DrawString(font, "0", new Vector2(width - 100 - 75, y - 150 + 5 + 220), Color.White);
                }

                if (gui.rightPanelState == RightPanelStates.Construct)
                {
                    text = langManager.strings[(int)LangString.Constuct];
                    spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);
                }

                if (gui.rightPanelState == RightPanelStates.Bog)
                {
                    text = langManager.strings[(int)LangString.Bog];
                    spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);
                }

                if (gui.rightPanelState == RightPanelStates.Grass)
                {
                    text = langManager.strings[(int)LangString.Grass];
                }

                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)id, time), Color.White, 0, Map.GetOriginVector((MapCellType)id), 1, SpriteEffects.None, 0);
                Vector2 size = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(width - 200, y + 60 + 220), Color.White, 0, -new Vector2(100 - (int)size.X / 2, 0), 1, SpriteEffects.None, 0);
            }
            #endregion
            #region DrawInventory

            if (gui.rightPanelState == RightPanelStates.Inventory)
            {
                int ysize = 300;
                int dy = ysize / 5;
                int starty = y+50;
                int startx = 20;

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Inventory], Color.White);

                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                        DrawColorRect(new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                    else
                        DrawColorRect(new Rectangle(width - 100 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                }

                spriteBatch.Draw(icons[(int)IconTexture.Wood], new Vector2(width - 200 + 10 + startx, starty), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5), map.GetResources(ResourceType.Wood).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Lumber], new Vector2(width - 100 + startx, starty), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5), map.GetResources(ResourceType.Lumber).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Corn], new Vector2(width - 200 + 10 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Corn).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Fish], new Vector2(width - 100 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Fish).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Meat], new Vector2(width - 200 + 10 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Meat).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Stone], new Vector2(width - 100 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Stone).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Iron], new Vector2(width - 200 + 10 + startx, starty + dy * 3), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 3), map.GetResources(ResourceType.Iron).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Tooth], new Vector2(width - 100 + startx, starty + dy * 3), Color.White);
                spriteBatch.DrawString(font, map.GetResources(ResourceType.Tools).ToString(), new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 3), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Sword], new Vector2(width - 200 + 10 + startx, starty + dy * 4), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 4), map.GetResources(ResourceType.Sword).ToString(), Color.White);

            }

            #endregion
            #region DrawCaptured

            if (gui.rightPanelState == RightPanelStates.Captured)
            {
                int ysize = 300;
                int dy = ysize / 5;
                int starty = y + 50;
                int startx = 20;

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Ride], Color.White);

                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                        DrawColorRect(new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                    else
                        DrawColorRect(new Rectangle(width - 100 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                }

                spriteBatch.Draw(icons[(int)IconTexture.Wood], new Vector2(width - 200 + 10 + startx, starty), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5), map.GetCapResources(ResourceType.Wood).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Lumber], new Vector2(width - 100 + startx, starty), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5), map.GetCapResources(ResourceType.Lumber).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Corn], new Vector2(width - 200 + 10 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy), map.GetCapResources(ResourceType.Corn).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Fish], new Vector2(width - 100 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy), map.GetCapResources(ResourceType.Fish).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Meat], new Vector2(width - 200 + 10 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 2), map.GetCapResources(ResourceType.Meat).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Stone], new Vector2(width - 100 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 2), map.GetCapResources(ResourceType.Stone).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Iron], new Vector2(width - 200 + 10 + startx, starty + dy * 3), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 3), map.GetCapResources(ResourceType.Iron).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Tooth], new Vector2(width - 100 + startx, starty + dy * 3), Color.White);
                spriteBatch.DrawString(font, map.GetCapResources(ResourceType.Tools).ToString(), new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 3), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Sword], new Vector2(width - 200 + 10 + startx, starty + dy * 4), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 4), map.GetCapResources(ResourceType.Sword).ToString(), Color.White);

            }

            #endregion
            #region DrawBuild
            if (gui.rightPanelState == RightPanelStates.Build)
            {
                string text = "";
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Build], Color.White);
                int id = 0;

                Resource[] res = new Resource[0];
                switch (gui.buildterraid % 12)
                {
                    case 0: text = langManager.strings[(int)LangString.Forester]; id = (int)MapCellType.Forester; res = Helper.buildingHelper[(int)BuildingType.Forester, 0].buildcost; break;
                    case 1: text = langManager.strings[(int)LangString.Lumberman]; id = (int)MapCellType.Lumberman; res = Helper.buildingHelper[(int)BuildingType.Lumbermen, 0].buildcost; break;
                    case 2: text = langManager.strings[(int)LangString.Mill]; id = (int)MapCellType.Mill; res = Helper.buildingHelper[(int)BuildingType.Mill, 0].buildcost; break;
                    case 3: text = langManager.strings[(int)LangString.Fisherman]; id = (int)MapCellType.Fisherman; res = Helper.buildingHelper[(int)BuildingType.Fisherman, 0].buildcost; break;
                    case 4: text = langManager.strings[(int)LangString.Farm]; id = (int)MapCellType.Farm; res = Helper.buildingHelper[(int)BuildingType.Farm, 0].buildcost; break;
                    case 5: text = langManager.strings[(int)LangString.StoneQuary]; id = (int)MapCellType.StoneQuary; res = Helper.buildingHelper[(int)BuildingType.StoneQuary, 0].buildcost; break;
                    case 6: text = langManager.strings[(int)LangString.Foundry]; id = (int)MapCellType.Foundry; res = Helper.buildingHelper[(int)BuildingType.Foundry, 0].buildcost; break;
                    case 7: text = langManager.strings[(int)LangString.Blacksmith]; id = (int)MapCellType.Blacksmith; res = Helper.buildingHelper[(int)BuildingType.Blacksmith, 0].buildcost; break;
                    case 8: text = langManager.strings[(int)LangString.Armourer]; id = (int)MapCellType.Armourer; res = Helper.buildingHelper[(int)BuildingType.Armourer, 0].buildcost; break;
                    case 9: text = langManager.strings[(int)LangString.Market]; id = (int)MapCellType.Market; res = Helper.buildingHelper[(int)BuildingType.Market, 0].buildcost; break;
                    case 10: text = langManager.strings[(int)LangString.Church]; id = (int)MapCellType.Church; res = Helper.buildingHelper[(int)BuildingType.Church, 0].buildcost; break;
                    case 11: text = langManager.strings[(int)LangString.Barracs]; id = (int)MapCellType.Barracs; res = Helper.buildingHelper[(int)BuildingType.Barracs, 0].buildcost; break;
                }

                bool left = true;
                int startx = 15;
                int starty = y + 10+30;
                int i = -1;
                int dy = 40;
                foreach (Resource re in res)
                {
                    if (left)
                    {
                        i++;
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + i * dy), re.number.ToString(), re.number <= map.resources[(int)re.type] ? Color.White : Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 100 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), re.number.ToString(), re.number <= map.resources[(int)re.type] ? Color.White : Color.Red);
                    }
                    left = !left;
                }

                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id)), 1, SpriteEffects.None, 0);
                Vector2 size = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(width - 200, y + 60 + 220), Color.White, 0, -new Vector2(100 - (int)size.X / 2, 0), 1, SpriteEffects.None, 0);
            }
            #endregion
            #region DrawTerraform
            if (gui.rightPanelState == RightPanelStates.Terraform)
            {
                string text = "";
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Terraform], Color.White);
                int id = 0;
                int tt = 0;

                switch (gui.buildterraid % 4)
                {
                    case 0: text = langManager.strings[(int)LangString.Grass]; id = (int)MapCellType.Grass1; tt = (int)TerrainType.Grass; break;
                    case 1: text = langManager.strings[(int)LangString.Forest]; id = (int)MapCellType.Forest1; tt = (int)TerrainType.Forest; break;
                    case 2: text = langManager.strings[(int)LangString.Lake]; id = (int)MapCellType.Lake1; tt = (int)TerrainType.Lake; break;
                    case 3: text = langManager.strings[(int)LangString.Field]; id = (int)MapCellType.Field1; tt = (int)TerrainType.Field; break;
                }

                int startx = 15;
                int starty = y + 40;
                spriteBatch.Draw(icons[(int)IconTexture.Gold], new Vector2(width - 200 + 10 + startx, starty), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5), Helper.terrainHelper[tt].cost.ToString(), Helper.terrainHelper[tt].cost <= map.resources[(int)ResourceType.Gold] ? Color.White : Color.Red);

                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id)), 1, SpriteEffects.None, 0);
                Vector2 size = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(width - 200, y + 60 + 220), Color.White, 0, -new Vector2(100 - (int)size.X / 2, 0), 1, SpriteEffects.None, 0);
            }
            #endregion
            #region DrawUpgrade
            if (gui.rightPanelState == RightPanelStates.Upgrade)
            {
                string text = "";
                int id = map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Upgrade], Color.White);

                switch ((MapCellType)id)
                {
                    case MapCellType.Armourer: text = langManager.strings[(int)LangString.Armourer]; break;
                    case MapCellType.Barracs: text = langManager.strings[(int)LangString.Barracs]; break;
                    case MapCellType.Blacksmith: text = langManager.strings[(int)LangString.Blacksmith]; break;
                    case MapCellType.Castle: text = langManager.strings[(int)LangString.Castle]; break;
                    case MapCellType.Church: text = langManager.strings[(int)LangString.Church]; break;
                    case MapCellType.Farm: text = langManager.strings[(int)LangString.Farm]; break;
                    case MapCellType.Fisherman: text = langManager.strings[(int)LangString.Fisherman]; break;
                    case MapCellType.Forester: text = langManager.strings[(int)LangString.Forester]; break;
                    case MapCellType.Foundry: text = langManager.strings[(int)LangString.Foundry]; break;
                    case MapCellType.Lumberman: text = langManager.strings[(int)LangString.Lumberman]; break;
                    case MapCellType.Market: text = langManager.strings[(int)LangString.Market]; break;
                    case MapCellType.Mill: text = langManager.strings[(int)LangString.Mill]; break;
                    case MapCellType.StoneQuary: text = langManager.strings[(int)LangString.StoneQuary]; break;
                }

                if ((MapCellType)id == MapCellType.Castle)

                    spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);

                Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                Resource[] res = b.upgradecost;
                bool left = true;
                int startx = 15;
                int starty = y + 40;
                int i = -1;
                int dy = 40;
                foreach (Resource re in res)
                {
                    if (left)
                    {
                        i++;
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + i * dy), re.number.ToString(), re.number <= map.resources[(int)re.type] ? Color.White : Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 100 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), re.number.ToString(), re.number <= map.resources[(int)re.type] ? Color.White : Color.Red);
                    }
                    left = !left;
                }

                spriteBatch.Draw(star, new Vector2(width - 100 - 40, y + 40 + 220), Color.White);
                spriteBatch.Draw(star, new Vector2(width - 100, y + 40 + 220), Color.White);
                if (b.lvl + 1 > 1) spriteBatch.Draw(star, new Vector2(width - 100 + 40, y + 40 + 220), Color.White);
                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)id, time), Color.White, 0, Map.GetOriginVector((MapCellType)id), 1, SpriteEffects.None, 0);
                Vector2 size = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(width - 200, y + 60 + 220), Color.White, 0, -new Vector2(100 - (int)size.X / 2, 0), 1, SpriteEffects.None, 0);
            }
            #endregion
            #region DrawDegrade
            if (gui.rightPanelState == RightPanelStates.Degrade)
            {
                string text = "";
                int id = map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Degrade], Color.White);

                switch ((MapCellType)id)
                {
                    case MapCellType.Armourer: text = langManager.strings[(int)LangString.Armourer]; break;
                    case MapCellType.Barracs: text = langManager.strings[(int)LangString.Barracs]; break;
                    case MapCellType.Blacksmith: text = langManager.strings[(int)LangString.Blacksmith]; break;
                    case MapCellType.Castle: text = langManager.strings[(int)LangString.Castle]; break;
                    case MapCellType.Church: text = langManager.strings[(int)LangString.Church]; break;
                    case MapCellType.Farm: text = langManager.strings[(int)LangString.Farm]; break;
                    case MapCellType.Fisherman: text = langManager.strings[(int)LangString.Fisherman]; break;
                    case MapCellType.Forester: text = langManager.strings[(int)LangString.Forester]; break;
                    case MapCellType.Foundry: text = langManager.strings[(int)LangString.Foundry]; break;
                    case MapCellType.Lumberman: text = langManager.strings[(int)LangString.Lumberman]; break;
                    case MapCellType.Market: text = langManager.strings[(int)LangString.Market]; break;
                    case MapCellType.Mill: text = langManager.strings[(int)LangString.Mill]; break;
                    case MapCellType.StoneQuary: text = langManager.strings[(int)LangString.StoneQuary]; break;
                }

                if ((MapCellType)id == MapCellType.Castle)

                    spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)(id % 4), time), Color.White, 0, Map.GetOriginVector((MapCellType)(id % 4)), 1, SpriteEffects.None, 0);

                Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                Resource[] res = b.cost;
                bool left = true;
                int startx = 15;
                int starty = y + 40;
                int i = -1;
                int dy = 40;
                foreach (Resource re in res)
                {
                    if (left)
                    {
                        i++;
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + i * dy), (re.number / 3 + 1).ToString(), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 100 + startx, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), (re.number / 3 + 1).ToString(), Color.White);
                    }
                    left = !left;
                }

                spriteBatch.Draw(star, new Vector2(width - 100 - 40, y + 40 + 220), Color.White);
                if (b.lvl - 1 > 0) spriteBatch.Draw(star, new Vector2(width - 100, y + 40 + 220), Color.White);
                spriteBatch.Draw(tileset, new Vector2(width - 100, y + 220), Map.GetSourceRectangle((MapCellType)id, time), Color.White, 0, Map.GetOriginVector((MapCellType)id), 1, SpriteEffects.None, 0);
                Vector2 size = font.MeasureString(text);
                spriteBatch.DrawString(font, text, new Vector2(width - 200, y + 60 + 220), Color.White, 0, -new Vector2(100 - (int)size.X / 2, 0), 1, SpriteEffects.None, 0);
            }
            #endregion
            #region DrawMarket
            if (gui.rightPanelState == RightPanelStates.Market)
            {
                int ysize = 300;
                int dy = ysize / 5;
                int starty = y + 50;
                int startx = 20;

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));

                int id = map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X];
                Building b = map.buildings[id];

                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                        DrawColorRect(new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                    else
                        DrawColorRect(new Rectangle(width - 100 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                }

                spriteBatch.Draw(icons[(int)IconTexture.Wood], new Vector2(width - 200 + 10 + startx, starty), b.productive > 0 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5), map.GetResources(ResourceType.Wood).ToString(), b.productive > 0 ? Color.White : Color.DarkGray);
                spriteBatch.Draw(icons[(int)IconTexture.Lumber], new Vector2(width - 100 + startx, starty), b.productive > 2 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5), map.GetResources(ResourceType.Lumber).ToString(), b.productive > 2 ? Color.White : Color.DarkGray);

                spriteBatch.Draw(icons[(int)IconTexture.Corn], new Vector2(width - 200 + 10 + startx, starty + dy), b.productive > 0 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Corn).ToString(), b.productive > 0 ? Color.White : Color.DarkGray);
                spriteBatch.Draw(icons[(int)IconTexture.Fish], new Vector2(width - 100 + startx, starty + dy), b.productive > 0 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Fish).ToString(), b.productive > 0 ? Color.White : Color.DarkGray);

                spriteBatch.Draw(icons[(int)IconTexture.Meat], new Vector2(width - 200 + 10 + startx, starty + dy * 2), b.productive > 0 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Meat).ToString(), b.productive > 0 ? Color.White : Color.DarkGray);
                spriteBatch.Draw(icons[(int)IconTexture.Stone], new Vector2(width - 100 + startx, starty + dy * 2), b.productive > 1 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Stone).ToString(), b.productive > 1 ? Color.White : Color.DarkGray);

                spriteBatch.Draw(icons[(int)IconTexture.Iron], new Vector2(width - 200 + 10 + startx, starty + dy * 3), b.productive > 2 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 3), map.GetResources(ResourceType.Iron).ToString(), b.productive > 2 ? Color.White : Color.DarkGray);
                spriteBatch.Draw(icons[(int)IconTexture.Tooth], new Vector2(width - 100 + startx, starty + dy * 3), b.productive > 3 ? Color.White : Color.DarkGray);
                spriteBatch.DrawString(font, map.GetResources(ResourceType.Tools).ToString(), new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 3), b.productive > 3 ? Color.White : Color.DarkGray);

                spriteBatch.Draw(icons[(int)IconTexture.Sword], new Vector2(width - 200 + 10 + startx, starty + dy * 4), b.productive > 4 ? Color.White : Color.DarkGray);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 4), map.GetResources(ResourceType.Sword).ToString(), b.productive > 4 ? Color.White : Color.DarkGray);

            }
            #endregion
            #region DrawMarket2

            if (gui.rightPanelState == RightPanelStates.Market2)
            {
                int ysize = 300;
                int dy = ysize / 5;
                int starty = y +150;
                int startx = 20;

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Market], Color.White);

                DrawColorRect(new Rectangle(width - 150 + startx - 3, starty, 38, 38), new Color(17, 17, 17));
                int tex = 0;
                switch (gui.buildterraid)
                {
                    case 0: tex = (int)IconTexture.Wood; break;
                    case 1: tex = (int)IconTexture.Lumber; break;
                    case 2: tex = (int)IconTexture.Corn; break;
                    case 3: tex = (int)IconTexture.Fish; break;
                    case 4: tex = (int)IconTexture.Meat; break;
                    case 5: tex = (int)IconTexture.Stone; break;
                    case 6: tex = (int)IconTexture.Iron; break;
                    case 7: tex = (int)IconTexture.Tooth; break;
                    case 8: tex = (int)IconTexture.Sword; break;
                }
                spriteBatch.Draw(icons[tex], new Vector2(width - 150 + startx, starty), Color.White);
                spriteBatch.DrawString(font, ((IconTexture)tex).ToString(), new Vector2(width - 150 + startx + 40, starty - 25), Color.White);
                spriteBatch.DrawString(font, gui.resourceadd != 0 ? gui.resourceadd > 0 ? langManager.strings[(int)LangString.Buy] : langManager.strings[(int)LangString.Sell] : "", new Vector2(width - 150 + startx + 40, starty - 5), Color.White);
                spriteBatch.DrawString(font, (map.resources[gui.resourceid] + gui.resourceadd).ToString(), new Vector2(width - 150 + startx + 40, starty + 20), Color.White);
            }

            #endregion
            #region DrawUpkeep
            if (gui.rightPanelState == RightPanelStates.Upkeep)
            {
                Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                Resource[] res = b.upkeep;
                bool left = true;
                int startx = 15;
                int starty = y + 50;
                int i = -1;
                int dy = 40;
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Upkeep], Color.White);
                foreach (Resource re in res)
                {
                    if (re.type == ResourceType.Food)
                    {
                        i++;
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Corn)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Fish)], new Vector2(width - 200 + 10 + startx + 32, starty + i * dy), Color.White);
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Meat)], new Vector2(width - 200 + 10 + startx + 68, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), (re.number * b.productive).ToString(), (re.number * b.productive) <= map.resources.Food ? Color.White : Color.Red);
                    }

                    else
                    {
                        if (left)
                        {
                            i++;
                            spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                            DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + i * dy), (re.number * b.productive).ToString(), (re.number * b.productive) <= map.resources[(int)re.type] ? Color.White : Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 100 + startx, starty + i * dy), Color.White);
                            DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), (re.number * b.productive).ToString(), (re.number * b.productive) <= map.resources[(int)re.type] ? Color.White : Color.Red);
                        }
                        left = !left;
                    }
                }
            }
            #endregion
            #region DrawAllUpkeep
            if (gui.rightPanelState == RightPanelStates.AllUpkeep)
            {
                Inventory allupkeep=new Inventory();
                int food=0;
                foreach (Building b in map.buildings)
                {
                    allupkeep = allupkeep + Inventory.FromResourceArray(b.upkeep)*b.productive;
                    foreach (Resource re in b.upkeep)
                        if (re.type == ResourceType.Food) food+=re.number*b.productive;
                }
                Resource[] res = Inventory.ToResourceArray(allupkeep,food);

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Upkeep], Color.White);

                bool left = true;
                int startx = 15;
                int starty = y + 50;
                int i = -1;
                int dy = 40;
                foreach (Resource re in res)
                {
                    if (re.type == ResourceType.Food)
                    {
                        i++;
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Corn)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Fish)], new Vector2(width - 200 + 10 + startx + 32, starty + i * dy), Color.White);
                        spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(ResourceType.Meat)], new Vector2(width - 200 + 10 + startx + 68, starty + i * dy), Color.White);
                        DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), (re.number).ToString(), (re.number) <= map.resources.Food ? Color.White : Color.Red);
                    }

                    else
                    {
                        if (left)
                        {
                            i++;
                            spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 200 + 10 + startx, starty + i * dy), Color.White);
                            DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + i * dy), (re.number).ToString(), (re.number) <= map.resources[(int)re.type] ? Color.White : Color.Red);
                        }
                        else
                        {
                            spriteBatch.Draw(icons[(int)ResourceTypeToIconTexture(re.type)], new Vector2(width - 100 + startx, starty + i * dy), Color.White);
                            DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + i * dy), (re.number).ToString(), (re.number) <= map.resources[(int)re.type] ? Color.White : Color.Red);
                        }
                        left = !left;
                    }
                }
            }
            #endregion
            #region DrawProductive
            if (gui.rightPanelState == RightPanelStates.Productive)
            {
                Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                //DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Productive], Color.White);
                DrawColorRect(new Rectangle(width - 115, y + 120, 30, 150), new Color(12, 12, 12));
                DrawColorRect(new Rectangle(width - 114, y + 121 + 148 / 5 * (5 - b.productive), 28, 148 / 5 * b.productive), Color.Red);

                DrawText(new Vector2(width - 185, y + 80), langManager.strings[(int)LangString.Productive] + ": " + (b.productive * 20) + "%", Color.White);
            }
            #endregion
            #region DrawChurch
            if (gui.rightPanelState == RightPanelStates.Church)
            {
                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                Building b = map.buildings[map.indexes[(int)camera.mapposition.Y, (int)camera.mapposition.X]];
                DrawColorRect(new Rectangle(width - 115, y + 120, 30, 150), new Color(12, 12, 12));
                DrawColorRect(new Rectangle(width - 114, y + 121 + 148 / b.storage * (b.storage - b.resources), 28, 148 / b.storage * b.resources), Color.Orange);

                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.Bank], Color.White);
            }
            #endregion
            #region DrawQuest
            if (gui.rightPanelState == RightPanelStates.Quest)
            {
                string txt = "";
                Rectangle rec = new Rectangle();
                if (mission > 0)
                {
                    switch(mission)
                    {
                        case 1: txt = AlignText(langManager.strings[(int)LangString.Mission1Info], 400); break;
                        case 2: txt = AlignText(langManager.strings[(int)LangString.Mission2Info], 400); break;
                        case 3: txt = AlignText(langManager.strings[(int)LangString.Mission3Info], 400); break;
                        case 4: txt = AlignText(langManager.strings[(int)LangString.Mission4Info], 400); break;
                        case 5: txt = AlignText(langManager.strings[(int)LangString.Mission5Info], 400); break;
                        case 6: txt = AlignText(langManager.strings[(int)LangString.Mission6Info], 400); break;
                        case 7: txt = AlignText(langManager.strings[(int)LangString.Mission7Info], 400); break;
                        case 8: txt = AlignText(langManager.strings[(int)LangString.Mission8Info], 400); break;
                    }
                    txt = "\n\n" + txt;
                    Vector2 v = font.MeasureString(txt);
                    rec = new Rectangle((int)(width - v.X)-4, (int)(y), (int)(v.X)+8, (int)(v.Y)+8);
                }
                else
                {
                    txt = AlignText(langManager.strings[(int)LangString.NoQuest],150);
                    txt = "\n\n" + txt;
                    rec = new Rectangle(width - 190-4, y-4, 180+8, 350+8);
                }

                DrawTexturedRect(darkbackground, rec);
                DrawOutLine(outline, rec);

                DrawCenterText(new Vector2(rec.X + rec.Width / 2, y + 10), langManager.strings[(int)LangString.Quest], Color.White);
                DrawText(new Vector2(rec.X + 8, rec.Y + 4), txt, Color.White);
            }
            #endregion
            #region DrawQuestGoal
            if (gui.rightPanelState == RightPanelStates.QuestGoal)
            {
                string txt = "";
                Rectangle rec = new Rectangle();
                if (mission > 0)
                {
                    switch (mission)
                    {
                        case 1: txt = AlignGoalText(langManager.strings[(int)LangString.Mission1Goal]); break;
                        case 2: txt = AlignGoalText(langManager.strings[(int)LangString.Mission2Goal]); break;
                        case 3: txt = AlignGoalText(langManager.strings[(int)LangString.Mission3Goal]); break;
                        case 4: txt = AlignGoalText(langManager.strings[(int)LangString.Mission4Goal]); break;
                        case 5: txt = AlignGoalText(langManager.strings[(int)LangString.Mission5Goal]); break;
                        case 6: txt = AlignGoalText(langManager.strings[(int)LangString.Mission6Goal]); break;
                        case 7: txt = AlignGoalText(langManager.strings[(int)LangString.Mission7Goal]); break;
                        case 8: txt = AlignGoalText(langManager.strings[(int)LangString.Mission8Goal]); break;
                    }
                    txt = "\n\n"+langManager.strings[(int)LangString.MissionGoal]+"\n" + txt;
                    Vector2 v = font.MeasureString(txt);
                    rec = new Rectangle((int)(width - v.X) - 12, (int)(y), (int)(v.X) + 16, (int)(v.Y) + 8);
                }
                else
                {
                    txt = AlignText(langManager.strings[(int)LangString.NoQuest], 150);
                    txt = "\n\n" + txt;
                    rec = new Rectangle(width - 190 - 4, y - 4, 180 + 8, 350 + 8);
                }

                DrawTexturedRect(darkbackground, rec);
                DrawOutLine(outline, rec);

                DrawCenterText(new Vector2(rec.X + rec.Width / 2, y + 10), langManager.strings[(int)LangString.Quest], Color.White);
                DrawText(new Vector2(rec.X + 8, rec.Y + 4), txt, Color.White);

                DrawTexturedRect(darkbackground, new Rectangle(rec.X-180,rec.Y+50,176,60));
                DrawOutLine(outline, new Rectangle(rec.X - 180, rec.Y+50, 176, 60));
                DrawText(new Vector2(rec.X - 174, rec.Y + 54), langManager.strings[(int)LangString.Terraformed] + " " + map.terraformed.ToString(), Color.White);
                DrawText(new Vector2(rec.X - 174, rec.Y + 79), langManager.strings[(int)LangString.Raids] + " " + map.raidsucces.ToString(), Color.White);
            }
            #endregion
            #region DrawMissionComplate
            if (gui.rightPanelState == RightPanelStates.MissionComplate)
            {
                int ysize = 300;
                int dy = ysize / 5;
                int starty = (height - ysize) / 2 - 20;
                int startx = 20;

                DrawTexturedRect(darkbackground, new Rectangle(width - 190, y, 180, 350));
                DrawOutLine(outline, new Rectangle(width - 190, y, 180, 350));
                DrawCenterText(new Vector2(width - 100, y + 10), langManager.strings[(int)LangString.MissionComplate], Color.White);

                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)
                        DrawColorRect(new Rectangle(width - 200 + 10 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                    else
                        DrawColorRect(new Rectangle(width - 100 + startx - 3, starty + i / 2 * dy - 3, 38, 38), new Color(17, 17, 17));
                }

                spriteBatch.Draw(icons[(int)IconTexture.Wood], new Vector2(width - 200 + 10 + startx, starty), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5), map.GetResources(ResourceType.Wood).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Lumber], new Vector2(width - 100 + startx, starty), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5), map.GetResources(ResourceType.Lumber).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Corn], new Vector2(width - 200 + 10 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Corn).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Fish], new Vector2(width - 100 + startx, starty + dy), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy), map.GetResources(ResourceType.Fish).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Meat], new Vector2(width - 200 + 10 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Meat).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Stone], new Vector2(width - 100 + startx, starty + dy * 2), Color.White);
                DrawText(new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 2), map.GetResources(ResourceType.Stone).ToString(), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Iron], new Vector2(width - 200 + 10 + startx, starty + dy * 3), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 3), map.GetResources(ResourceType.Iron).ToString(), Color.White);
                spriteBatch.Draw(icons[(int)IconTexture.Tooth], new Vector2(width - 100 + startx, starty + dy * 3), Color.White);
                spriteBatch.DrawString(font, map.GetResources(ResourceType.Tools).ToString(), new Vector2(width - 100 + 40 + startx, starty + 5 + dy * 3), Color.White);

                spriteBatch.Draw(icons[(int)IconTexture.Sword], new Vector2(width - 200 + 10 + startx, starty + dy * 4), Color.White);
                DrawText(new Vector2(width - 200 + 10 + 40 + startx, starty + 5 + dy * 4), map.GetResources(ResourceType.Sword).ToString(), Color.White);
            }
            #endregion
            #region DrawEvent
            if (gui.rightPanelState == RightPanelStates.Event)
            {
                string txt = "";
                Rectangle rec = new Rectangle();
                    txt = "\n\n" + gui.eventmessage;
                    Vector2 v = font.MeasureString(txt);
                    rec = new Rectangle((int)(width - v.X) - 4, (int)(y), (int)(v.X) + 8, (int)(v.Y) + 8);

                DrawTexturedRect(darkbackground, rec);
                DrawOutLine(outline, rec);

                DrawCenterText(new Vector2(rec.X + rec.Width / 2, y + 10), langManager.strings[(int)LangString.Event], Color.White);
                DrawText(new Vector2(rec.X + 8, rec.Y + 4), txt, Color.White);
            }
            #endregion

            #region Commands
            r = new Rectangle(width - 205, y + 355, 39, 39); r.Width = 39; r.Height = 39; r.X += 2 - r.Width - 2;

            foreach (RightPanelCommand rpc in gui.rightPanelCommands)
            {
                r.X += r.Width + 2;
                DrawColorRect(r, new Color(26, 12, 0));
                IconTexture txt = IconTexture.Yes;

                switch (rpc)
                {
                    case RightPanelCommand.Attack: txt = IconTexture.Attack; break;
                    case RightPanelCommand.Build: txt = IconTexture.Build; break;
                    case RightPanelCommand.Degrade: txt = IconTexture.Degrade; break;
                    case RightPanelCommand.Down: txt = IconTexture.Down; break;
                    case RightPanelCommand.Help: txt = IconTexture.Info; break;
                    case RightPanelCommand.Ok: txt = IconTexture.Yes; break;
                    case RightPanelCommand.Productive: txt = IconTexture.Status; break;
                    case RightPanelCommand.Terraform: txt = IconTexture.Terraforn; break;
                    case RightPanelCommand.Up: txt = IconTexture.Up; break;
                    case RightPanelCommand.Upgrade: txt = IconTexture.Upgrade; break;
                    case RightPanelCommand.Upkeep: txt = IconTexture.Upkeep; break;
                    case RightPanelCommand.Market: txt = IconTexture.Gold; break;
                    case RightPanelCommand.Church: txt = IconTexture.Gold; break;
                    case RightPanelCommand.Inventory: txt = IconTexture.Inventory; break;
                    case RightPanelCommand.No: txt = IconTexture.No; break;
                }

                spriteBatch.Draw(icons[(int)txt], new Vector2(r.X + 2 + (txt == IconTexture.Gold ? 4 : 0), r.Y + 2), Color.White);
                DrawOutLine(line, r);
            }
            #endregion

            //DrawOutLine(line, new Rectangle(me.rect.X, me.rect.Y - 8, me.rect.Width + 16, me.rect.Height + 16));

        }
        void StandartGuiDraw( Texture2D line, Texture2D darkbackground, Texture2D lightbackground,  ref GuiObject me)
        {
            if (me.darktransparency) DrawTexturedRect( darkbackground, me.rect);
            if (me.lighttransparency) DrawTexturedRect( lightbackground, me.rect);

            DrawOutLine(line, me.rect);

            if (me.text != "")
            {
                Vector2 size = font.MeasureString(me.text);
                spriteBatch.DrawString(font, me.text, new Vector2((int)(me.rect.X + me.rect.Width / 2 - size.X / 2), (int)(me.rect.Y + me.rect.Height / 2 - size.Y / 2)), Color.White);
            }
        }
        void StandartButtonDraw(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me)
        {
            //if (me.lighttransparency) DrawColorRect(me.rect, new Color(50, 50, 50));
            DrawColorRect(me.rect, new Color(53, 24, 0));
            if (me.darktransparency) DrawColorRect(me.rect, new Color(26, 12, 0));

            DrawOutLine(line, me.rect);

            if (me.text != "")
            {
                Vector2 size = font.MeasureString(me.text);
                spriteBatch.DrawString(font, me.text, new Vector2((int)(me.rect.X + me.rect.Width / 2 - size.X / 2), (int)(me.rect.Y + me.rect.Height / 2 - size.Y / 2)), Color.White);
            }
        }
        void GameMenuButtonDraw(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me)
        {
            Rectangle r = me.rect; r.Width = 39; r.Height = 39; r.X += 2;
            DrawColorRect(r, new Color(26, 12, 0));
            spriteBatch.Draw(icons[(int)IconTexture.Inventory], new Vector2(r.X, r.Y + 2), Color.White);
            DrawOutLine(line, r);

            r.X += r.Width + 2;
            DrawColorRect(r, new Color(26, 12, 0));
            spriteBatch.Draw(icons[(int)IconTexture.Attack], new Vector2(r.X + 2, r.Y + 2), Color.White);
            DrawOutLine(line, r);

            r.X += r.Width + 2;
            DrawColorRect(r, new Color(26, 12, 0));
            spriteBatch.Draw(icons[(int)IconTexture.Upkeep], new Vector2(r.X + 2, r.Y + 2), Color.White);
            DrawOutLine(line, r);

            r.X += r.Width + 2;
            DrawColorRect(r, new Color(26, 12, 0));
            spriteBatch.Draw(icons[(int)IconTexture.Save], new Vector2(r.X + 2, r.Y + 2), Color.White);
            DrawOutLine(line, r);

            r.X += r.Width + 2;
            DrawColorRect(r, new Color(26, 12, 0));
            spriteBatch.Draw(icons[(int)IconTexture.Load], new Vector2(r.X + 2, r.Y + 2), Color.White);
            DrawOutLine(line, r);
        }
        void LogoDraw(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me)
        {
            if (state == GameState.MainMenu || state == GameState.StartGameMenu || state == GameState.StartGameMenuModes || state == GameState.StartGameMenuMissions || state == GameState.LoadMenu)
            {
                spriteBatch.Draw(logo,new Vector2((width-logo.Width)/2,10),Color.White);
            }
        }

        void DrawAbout(Texture2D line, Texture2D darkbackground, Texture2D lightbackground, ref GuiObject me)
        {
            StandartButtonDraw(line,darkbackground,lightbackground,ref me);

            if (me.undercursor)
            {
                string txt=AlignText(langManager.strings[(int)LangString.Gamemaker],200);
                Vector2 v = font.MeasureString(txt);
                Rectangle rect = new Rectangle(width - (int)v.X - 8 - 8, height - 38 - (int)v.Y - 8, (int)v.X + 16, (int)v.Y + 16);
                DrawTexturedRect(darkbackground,rect);
                DrawOutLine(line, rect);
                DrawText(new Vector2(width - v.X - 8, height - 38 - v.Y),txt,Color.White);
            }
        }

        void DrawOutLine(Texture2D line, Rectangle rect)
        {
            int lw = line.Width, lh = line.Height;
            spriteBatch.Draw(line, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, lw / 2, lh / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Vector2(rect.X + rect.Width - lw / 2, rect.Y), new Rectangle(lw / 2, 0, lw / 2, lh / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Vector2(rect.X, rect.Y + rect.Height - lh / 2), new Rectangle(0, lh / 2, lw / 2, lh / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Vector2(rect.X + rect.Width - lw / 2, rect.Y + rect.Height - lh / 2), new Rectangle(lw / 2, lh / 2, lw / 2, lh / 2), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            spriteBatch.Draw(line, new Rectangle(rect.X, rect.Y + lh / 2, lw / 2, rect.Height - lh), new Rectangle(0, lh / 2, lw / 2, 0), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X + rect.Width - lw / 2, rect.Y + lh / 2, lw / 2, rect.Height - lh), new Rectangle(lw / 2, lh / 2, lw / 2, 0), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X + lw / 2, rect.Y, rect.Width - lw, lh / 2), new Rectangle(lw / 2, 0, 0, lh / 2), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X + lw / 2, rect.Y + rect.Height - lh / 2, rect.Width - lw, lh / 2), new Rectangle(lw / 2, lh / 2, 0, lh / 2), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
        void DrawSimpleOutLine(Texture2D line, Rectangle rect)
        {
            int lw = line.Width, lh = line.Height;

            spriteBatch.Draw(line, new Rectangle(rect.X, rect.Y, 0, rect.Height), new Rectangle(0, lh / 2, lw / 2, 0), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X + rect.Width - lw / 2, rect.Y , lw / 2, rect.Height), new Rectangle(lw / 2, lh / 2, lw / 2, 0), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X , rect.Y, rect.Width, lh / 2), new Rectangle(lw / 2, 0, 0, lh / 2), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(line, new Rectangle(rect.X , rect.Y + rect.Height - lh / 2, rect.Width, lh / 2), new Rectangle(lw / 2, lh / 2, 0, lh / 2), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
        void DrawColorRect(Rectangle r, Color c)
        {
            spriteBatch.Draw(emprypixel, r, c);
        }
        void DrawTexturedRect( Texture2D texture, Rectangle rect)
        {
            spriteBatch.Draw(texture, new Vector2(rect.X, rect.Y), new Rectangle(0, 0, rect.Width, rect.Height), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
        void DrawText(Vector2 pos, string text, Color color)
        {
            spriteBatch.DrawString(font, text, pos + Vector2.UnitX, Color.Black);
            spriteBatch.DrawString(font, text, pos + Vector2.UnitY, Color.Black);
            spriteBatch.DrawString(font, text, pos - Vector2.UnitX, Color.Black);
            spriteBatch.DrawString(font, text, pos - Vector2.UnitY, Color.Black);

            spriteBatch.DrawString(font, text, pos, color);
        }
        void DrawCenterText(Vector2 pos, string text, Color color)
        {
            Vector2 v = font.MeasureString(text);
            pos.X -= (int)v.X / 2;

            spriteBatch.DrawString(font, text, pos + Vector2.UnitX, Color.Black);
            spriteBatch.DrawString(font, text, pos + Vector2.UnitY, Color.Black);
            spriteBatch.DrawString(font, text, pos - Vector2.UnitX, Color.Black);
            spriteBatch.DrawString(font, text, pos - Vector2.UnitY, Color.Black);

            spriteBatch.DrawString(font, text, pos, color);
        }
        int GetTime(double gt)
        {
            double tm = gt - (((int)gt / 10) * 10);
            int hour = (int)(24 * tm);
            int min = (int)((tm - (int)tm) * 10);
            return hour  + min/10;
        }
        int GetDay(double gt)
        {
            return (int)gt / 10;
        }

        void AddBuilding()
        {
            BuildingType bt = BuildingType.Armourer;
            int id = 0;
            switch (gui.buildterraid % 12)
            {
                case 0: bt = BuildingType.Forester; id = (int)(MapCellType.Forester); break;
                case 1: bt = BuildingType.Lumbermen; id = (int)(MapCellType.Lumberman); break;
                case 2: bt = BuildingType.Mill; id = (int)(MapCellType.Mill); break;
                case 3: bt = BuildingType.Fisherman; id = (int)(MapCellType.Fisherman); break;
                case 4: bt = BuildingType.Farm; id = (int)(MapCellType.Farm); break;
                case 5: bt = BuildingType.StoneQuary; id = (int)(MapCellType.StoneQuary); break;
                case 6: bt = BuildingType.Foundry; id = (int)(MapCellType.Foundry); break;
                case 7: bt = BuildingType.Blacksmith; id = (int)(MapCellType.Blacksmith); break;
                case 8: bt = BuildingType.Armourer; id = (int)(MapCellType.Armourer); break;
                case 9: bt = BuildingType.Market; id = (int)(MapCellType.Market); break;
                case 10: bt = BuildingType.Church; id = (int)(MapCellType.Church); break;
                case 11: bt = BuildingType.Barracs; id = (int)(MapCellType.Barracs); break;
            }
            Inventory cost = Inventory.FromResourceArray(Helper.buildingHelper[(int)bt, 0].buildcost);
            if (map.resources > cost || timekoef == 0)
            {
                if (timekoef != 0) map.resources = map.resources - cost;
                int y = (int)camera.mapposition.Y, x = (int)camera.mapposition.X;
                map.indexes[y, x] = map.buildings.Count;
                map.mapData[y, x] = id;
                map.buildings.Add(new Building(bt, 0));

                map.waitTime[y, x] = map.buildings[map.indexes[y, x]].buildtime * 10 * timekoef;//(time.ElapsedGameTime.TotalSeconds * 600);

                map.townies.Add(new Townie(map.castleposition, TownieProfetion.porter, map.indexes[y, x], camera.mapposition));
                if (bt != BuildingType.Barracs)
                map.townies.Add(new Townie(camera.mapposition, TownieProfetion.worker, map.indexes[y, x], camera.mapposition));
                else map.townies.Add(new Townie(camera.mapposition, TownieProfetion.workerinbarrack, map.indexes[y, x], camera.mapposition));
                if (bt == BuildingType.Mill || bt == BuildingType.Forester || bt == BuildingType.Foundry || bt == BuildingType.StoneQuary || bt == BuildingType.Fisherman)
                    map.townies.Add(new Townie(camera.mapposition, TownieProfetion.miner, map.indexes[y, x], camera.mapposition));

                if (bt == BuildingType.Mill)
                {
                    Vector2 grasspos = map.GetNearGrassByPosition(camera.mapposition);
                    Vector2 oldpos = camera.mapposition;

                    Random r = new Random();
                    Terrain t = new Terrain(r.Next(100) + 100);
                    //TerrainType tt = TerrainType.Field;
                    int ny = (int)grasspos.Y, nx = (int)grasspos.X;
                    id = (int)MapCellType.Field1;
                    id += r.Next(4);
                    map.indexes[ny, nx] = map.terrains.Count;
                    map.mapData[ny, nx] = id;
                    map.terrains.Add(t);

                    map.waitTime[ny, nx] = map.waitTime[y, x];//time.ElapsedGameTime.TotalSeconds * 600);
                }
            }
        }
        void AddTerrain()
        {
            Random r = new Random();
            Terrain t = new Terrain(r.Next(100)+100);
            TerrainType tt = TerrainType.Grass;
            int id = 0;
            switch (gui.buildterraid % 4)
            {
                case 0: id = (int)MapCellType.Grass1; tt = TerrainType.Grass; break;
                case 1: id = (int)MapCellType.Forest1; tt = TerrainType.Forest; break;
                case 2: id = (int)MapCellType.Lake1; tt = TerrainType.Lake; break;
                case 3: id = (int)MapCellType.Field1; tt = TerrainType.Field; break;
            }
            if ((map.resources[(int)ResourceType.Gold] >= Helper.terrainHelper[(int)tt].cost || timekoef == 0) && Math.Abs(map.mapData[(int)camera.mapposition.Y, (int)camera.mapposition.X] - id) > 3)
            {
                if (timekoef != 0) map.resources[(int)ResourceType.Gold] -= Helper.terrainHelper[(int)tt].cost;
                int y = (int)camera.mapposition.Y, x = (int)camera.mapposition.X;
                if (map.mapData[y, x] > id + 3 || map.mapData[y, x] < id)
                {
                    id += r.Next(4);
                    if (id > 3)
                        map.indexes[y, x] = map.terrains.Count;
                    map.mapData[y, x] = id;
                    if (id > 3)
                        map.terrains.Add(t);

                    map.waitTime[y, x] = Helper.terrainHelper[(int)tt].terraformingtime * 10 * timekoef;//time.ElapsedGameTime.TotalSeconds * 600);
                }
            }
        }
        void UpgadeBuilding()
        {
            int y = (int)camera.mapposition.Y, x = (int)camera.mapposition.X;
            Building b = map.buildings[map.indexes[y, x]];
            Inventory cost = Inventory.FromResourceArray(b.upgradecost);
            if (map.resources > cost || timekoef == 0)
            {
                map.waitTime[y, x] = b.upgradetime * 10 * timekoef;
                b.lvl++;
                b.productive++;
                map.buildings[map.indexes[y, x]] = b;
                if (timekoef != 0) map.resources = map.resources - cost;
            }
        }
        void DegradeBuilding()
        {
            int y = (int)camera.mapposition.Y, x = (int)camera.mapposition.X;
            Building b = map.buildings[map.indexes[y, x]];
            Resource[] res = b.cost;
            foreach (Resource re in res)
            {
                if (re.number > 0)
                    re.number = re.number / 3 + 1;
            }
            map.resources = map.resources + Inventory.FromResourceArray(res);
            if (b.lvl == 0)
            {
                map.mapData[y, x] = map.mapData[y, x] % 4;
                map.waitTime[y, x] = Helper.terrainHelper[(int)TerrainType.Grass].terraformingtime * 10 * timekoef;

                for (int i = map.townies.Count - 1; i >= 0; i--)
                {
                    if (map.townies[i].building == map.indexes[y, x]) map.townies.RemoveAt(i);
                }
            }
            else 
            {
                map.buildings[map.indexes[y, x]].lvl--;
                map.buildings[map.indexes[y, x]].productive--;
                map.waitTime[y, x] = map.buildings[map.indexes[y, x]].buildtime * 10 * timekoef;
            }
        }

        bool Save()
        {
            string name = "mission.save";
            if (mission < 0)
                name = "random.save";
            name = Content.RootDirectory + "\\" + name;

            List<string> l = new List<string>();
            //Map
            l.Add(map.width.ToString());
            l.Add(map.height.ToString());
            for (int i = 0; i < map.height; i++)
                for (int j = 0; j < map.width; j++)
                {
                    l.Add(map.mapData[i, j].ToString());
                    l.Add(map.indexes[i, j].ToString());
                    l.Add(map.waitTime[i, j].ToString());
                }
            l.Add(map.terrains.Count.ToString());
            foreach (Terrain t in map.terrains)
            {
                l.Add(t.resources.ToString());
                l.Add(t.maxresources.ToString());
            }
            l.Add(map.buildings.Count.ToString());
            foreach (Building b in map.buildings)
            {
                l.Add(((int)(b.type)).ToString());
                l.Add(b.productive.ToString());
                l.Add(b.resources.ToString());
                l.Add(b.lvl.ToString());
                l.Add(b.starttime.ToString());
                l.Add(b.inproduct.ToString());
                l.Add(b.outproduct.ToString());
                l.Add(b.needproduct.ToString());
                l.Add(b.temp.ToString());
                l.Add(b.wait.ToString());
            }
            l.Add(map.townies.Count.ToString());
            foreach (Townie t in map.townies)
            {
                l.Add(t.position.X.ToString());
                l.Add(t.position.Y.ToString());
                l.Add(t.oldposition.X.ToString());
                l.Add(t.oldposition.Y.ToString());
                l.Add(t.startposition.X.ToString());
                l.Add(t.startposition.Y.ToString());
                l.Add(t.target.X.ToString());
                l.Add(t.target.Y.ToString());
                l.Add(((int)t.profetion).ToString());
                l.Add(((int)t.state).ToString());
                l.AddRange(t.inventory.ToString());
                l.Add(((int)t.icon).ToString());
                l.Add(t.building.ToString());
                l.Add(t.buildingposition.X.ToString());
                l.Add(t.buildingposition.Y.ToString());
                l.Add(t.wait.ToString());
            }
            l.Add(Townie.speed.ToString());
            l.AddRange(map.resources.ToString());
            l.Add(map.castleposition.X.ToString());
            l.Add(map.castleposition.Y.ToString());
            l.Add(time.ToString());
            l.Add(mission.ToString());
            l.Add(map.terraformed.ToString());
            l.Add(map.raidsucces.ToString());

            System.IO.File.WriteAllLines(name,l.ToArray());
            return true;
        }
        bool Load()
        {
            string name = "mission.save";
            if (mission < 0)
                name = "random.save";
            name = Content.RootDirectory + "\\" + name;

            string[] l=new string[1];
            try
            {
                l = System.IO.File.ReadAllLines(name);
            }
            catch { gui.messages.Add(new ColorMessage(langManager.strings[(int)LangString.FileNotFound], Color.Red, 2)); return false; }

            int w = int.Parse(l[0]);
            int h = int.Parse(l[1]);

            map = new Map(w,h);
            camera = new Camera(new Vector2(width, height), new Vector2(w, h));

            int k=2;
            for(int i=0;i<h;i++)
                for (int j = 0; j < w; j++)
                {
                    map.mapData[i, j] = int.Parse(l[k]);
                    map.indexes[i, j] = int.Parse(l[k+1]);
                    map.waitTime[i, j] = double.Parse(l[k+2]);
                    k += 3;
                }
            map.terrains = new List<Terrain>();
            int tc = int.Parse(l[k]); k++;
            for (int i = 0; i < tc; i++)
            {
                int r = int.Parse(l[k]);
                map.terrains.Add(new Terrain(int.Parse(l[k+1])));
                map.terrains[i].resources = r;
                k += 2;
            }
            map.buildings = new List<Building>();
            int bc = int.Parse(l[k]); k++;
            for (int i = 0; i < bc; i++)
            {
                map.buildings.Add(new Building((BuildingType)int.Parse(l[k]), 0)); k++;
                map.buildings[i].productive = int.Parse(l[k]); k++;
                map.buildings[i].resources = int.Parse(l[k]); k++;
                map.buildings[i].lvl = int.Parse(l[k]); k++;
                map.buildings[i].starttime = float.Parse(l[k]); k++;
                map.buildings[i].inproduct = bool.Parse(l[k]); k++;
                map.buildings[i].outproduct = bool.Parse(l[k]); k++;
                map.buildings[i].needproduct = bool.Parse(l[k]); k++;
                map.buildings[i].temp = int.Parse(l[k]); k++;
                map.buildings[i].wait = double.Parse(l[k]); k++;
            }
            map.townies = new List<Townie>();
            int uc = int.Parse(l[k]); k++;
            for (int i = 0; i < uc; i++)
            {
                map.townies.Add(new Townie(Vector2.Zero, TownieProfetion.miner, 0, Vector2.Zero));
                map.townies[i].position = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
                map.townies[i].oldposition = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
                map.townies[i].startposition = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
                map.townies[i].target = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
                map.townies[i].profetion = (TownieProfetion)int.Parse(l[k]); k++;
                map.townies[i].state = (TownieState)int.Parse(l[k]); k++;
                map.townies[i].inventory.FromString(l, k); k += 10;
                map.townies[i].icon = (ResourceType)int.Parse(l[k]); k++;
                map.townies[i].building = int.Parse(l[k]); k++;
                map.townies[i].buildingposition = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
                map.townies[i].wait = double.Parse(l[k]); k++;
            }
            Townie.speed = float.Parse(l[k]); k++;
            map.resources.FromString(l, k); k += 10;
            map.castleposition = new Vector2(float.Parse(l[k]), float.Parse(l[k + 1])); k += 2;
            time = double.Parse(l[k]); k++;
            mission = int.Parse(l[k]); k++;
            map.terraformed = int.Parse(l[k]); k++;
            map.raidsucces = int.Parse(l[k]); k++;
            gui.rightPanelState = RightPanelStates.Nothing;
            gui.rightPanelCommands.Clear();
            speed = 1;
            timekoef = 1;
            return true;
        }

        IconTexture ResourceTypeToIconTexture(ResourceType rt)
        {
            switch (rt)
            {
                case ResourceType.Corn: return IconTexture.Corn;
                case ResourceType.Fish: return IconTexture.Fish;
                case ResourceType.Gold: return IconTexture.Gold;
                case ResourceType.Iron: return IconTexture.Iron;
                case ResourceType.Lumber: return IconTexture.Lumber;
                case ResourceType.Meat: return IconTexture.Meat;
                case ResourceType.Stone: return IconTexture.Stone;
                case ResourceType.Tools: return IconTexture.Tooth;
                case ResourceType.Wood: return IconTexture.Wood;
            }
            return IconTexture.Attack;
        }

        string AlignText(string text, int width)
        {
            string resault = "";

            int stringsnum = (int)((font.MeasureString(text).X) / width);
            if (stringsnum == 0)
                resault = text;
            else
            {
                int nowwidth = 0;
                text.Trim();
                List<string> strings = new List<string>();
                int spacenum = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == ' ')
                    {
                        spacenum++;
                        strings.Add(text.Substring(i - nowwidth, nowwidth));
                        nowwidth = 0;
                    }
                    else
                    {
                        nowwidth++;
                    }
                }
                strings.Add(text.Substring(text.Length - 1 - nowwidth, nowwidth + 1));
                nowwidth = 0;
                for (int i = 0; i < strings.Count; i++)
                {
                    int x = (int)font.MeasureString(strings[i] + " ").X;
                    if (nowwidth + x > width)
                    {
                        nowwidth = 0; resault += "\n";
                    }
                    else nowwidth += x;
                    resault += strings[i] + " ";
                }
            }

            return resault;
        }
        string AlignGoalText(string text)
        {
            return text.Replace("-", "\n-");
        }
    }
}