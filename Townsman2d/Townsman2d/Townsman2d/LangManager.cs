﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Townsman2d
{
    enum LangString
    {
        Grass,
        Rock,
        Forest,
        Iron,
        Field,
        Lake,
        Bog,
        Constuct,
        Mill,
        Barracs,
        Blacksmith,
        Castle,
        Foundry,
        Church,
        Market,
        StoneQuary,
        Fisherman,
        Lumberman,
        Farm,
        Armourer,
        Forester,
        StartGame,
        Generate,
        SmallMap,
        MediumMap,
        LargeMap,
        LittleForest,
        MediumForest,
        LargeForest,
        LittleWater,
        MediumWater,
        ManyWater,
        LittleRocks,
        MediumRocks,
        ManyRocks,
        SmallResources,
        MediumResources,
        ManyResources,
        Exit,
        Day,
        Buy,
        Sell,
        Productive,
        Bank,
        Inventory,
        Ride,
        Build,
        Terraform,
        Upgrade,
        Degrade,
        Upkeep,
        Missions,
        Mission,
        RandomMap,
        Sandbox,
        LoadGame,
        Help,
        Back,
        Saved,
        Loaded,
        CantSave,
        CantLoad,
        FileNotFound,
        Quest,
        NoQuest,
        MissionGoal,
        Mission1Info,
        Mission1Goal,
        Mission2Info,
        Mission2Goal,
        Mission3Info,
        Mission3Goal,
        Mission4Info,
        Mission4Goal,
        Mission5Info,
        Mission5Goal,
        Mission6Info,
        Mission6Goal,
        Mission7Info,
        Mission7Goal,
        Mission8Info,
        Mission8Goal,
        MissionComplate,
        Terraformed,
        Raids,
        Continue,
        Gamemaker,
        Event,
        ForestBad,
        LakeBad,
        FieldBad,
        BuildingBad,
        ForestGood,
        LakeGood,
        FieldGood,
        MountGood,
        BuildingGood
    }

    class LangManager
    {
        public string[] strings;

        public LangManager(string path) { strings=System.IO.File.ReadAllLines(path);}
    }
}