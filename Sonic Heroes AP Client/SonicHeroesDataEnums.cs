using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Sonic_Heroes_AP_Client;

public enum Goal
{
    MetalOverlord
}

public enum MissionsActive
{
    None,
    Act1,
    Act2,
    Both
}

public enum GoalUnlockCondition
{
    LevelCompletions = 0,
    Emeralds = 1,
    LevelCompletionsandEmeralds = 2,
    //Abilities = 0,
    //AbilitiesAndEmeralds = 2,
}

public enum AbilityUnlockType
{
    AllRegionsSeparate = 0,
    EntireStory = 1
}

public enum LevelId
{
    Unk1,
    Unk2,
    SeasideHill,
    OceanPalace,
    GrandMetropolis,
    PowerPlant,
    CasinoPark,
    BingoHighway,
    RailCanyon,
    BulletStation,
    FrogForest,
    LostJungle,
    HangCastle,
    MysticMansion,
    EggFleet,
    FinalFortress,
    EggHawk,
    TeamFight1,
    RobotCarnival,
    EggAlbatross,
    TeamFight2,
    RobotStorm,
    EggEmperor,
    MetalMadness,
    MetalOverlord,
    SeaGate,
    SeasideBobsledCourse,
    CityBobsledCourse,
    CasinoBobsledCourse,
    BonusStage1,
    BonusStage2,
    BonusStage3,
    BonusStage4,
    BonusStage5,
    BonusStage6,
    BonusStage7,
    ChaotixRailCanyon,
    SeasideHillActionRace,
    GrandMetropolisActionRace,
    BingoHighwayActionRace,
    CityTopBattle,
    CasinoRingBattle,
    TurtleShellBattle,
    EggTreatRingRace,
    PinballMatchRingRace,
    HotElevatorRingRace,
    RoadRockQuickRace,
    MadExpressQuickRace,
    TerrorHallQuickRace,
    RailCanyonExpertRace,
    FrogForestExpertRace,
    EggFleetExpertRace,
    EmeraldStage1,
    EmeraldStage2,
    EmeraldStage3,
    EmeraldStage4,
    EmeraldStage5,
    EmeraldStage6,
    EmeraldStage7,
    SpecialStage1Multiplayer,
    SpecialStage2Multiplayer,
    SpecialStage3Multiplayer,
}

public enum Region
{
    Ocean,
    HotPlant,
    Casino,
    Train,
    BigPlant,
    Ghost,
    Sky,
    SpecialStage,
    Boss,
    FinalBoss
}

public enum Team
{
    Sonic,
    Dark,
    Rose,
    Chaotix,
    SuperHard,
}

public enum Mission
{
    Mission1,
    Mission2
}

public enum Rank : byte 
{
    ARank = 5,
    BRank = 4,
    CRank = 3,
    DRank = 2,
    ERank = 1,
    NoRank = 0
};

public enum SpawnMode: byte
{
    Normal,
    Running,
    Rail
}

public enum FormationChar
{
    Speed,
    Flying,
    Power
}

public enum Emerald
{
    Green,
    Blue,
    Yellow,
    White,
    Cyan,
    Purple,
    Red
}


public enum PlayableCharacter
{
    PlayableSonic,
    PlayableTails,
    PlayableKnuckles,
    PlayableShadow,
    PlayableRouge,
    PlayableOmega,
    PlayableAmy,
    PlayableCream,
    PlayableBig,
    PlayableEspio,
    PlayableCharmy,
    PlayableVector,
    PlayableSuperHardSonic,
    PlayableSuperHardTails,
    PlayableSuperHardKnuckles,
}


//use GetNames Here
//enum.tryparse to go backwards
public enum Ability
{
    HomingAttack,
    Tornado,
    RocketAccel,
    LightDash,
    TriangleJump,
    LightAttack,
    AmyHammerHover,
    Invisibility,
    Shuriken,
    Thundershoot,
    Flight,
    DummyRings,
    CheeseCannon,
    FlowerSting,
    PowerAttack,
    ComboFinisher,
    Glide,
    FireDunk,
    UltimateFireDunk,
    BellyFlop,
}



public enum StageObjTypes : ushort
    {
        None = 0x0000,
        SingleSpring = 0x0001,
        TripleSpring = 0x0002,
        Rings = 0x0003,
        HintRing = 0x0004,
        RegularSwitch = 0x0005,
        PushAndPullSwitch = 0x0006,
        TargetSwitch = 0x0007,
        DashPanel = 0x000B,
        DashRing = 0x000C,
        RainbowHoops = 0x000D,
        Checkpoint = 0x000E,
        DashRamp = 0x000F,
        Cannon = 0x0010,
        RegularWeight = 0x0013,
        BreakableWeight = 0x0014,
        SpikeBall = 0x0015,
        LaserFence = 0x0016,
        ItemBox = 0x0018,
        ItemBalloon = 0x0019,
        GoalRing = 0x001B,
        Pulley = 0x001D,
        WoodContainer = 0x0020,
        IronContainer = 0x0021,
        UnbreakableContainer = 0x0022,
        LostChao = 0x0023,
        CageBox = 0x0024,
        FormationSign = 0x0025,
        FormationChangeGate = 0x0026,
        Propeller = 0x0028,
        Pole = 0x0029,
        Gong = 0x002C,
        Fan = 0x002E,
        Case = 0x0031,
        WarpFlower = 0x0032,
        InvisibleCollisionObject = 0x0050,
        TriggerTalking = 0x0056,
        TriggerLight = 0x0059,
        TriggerRhinoLiner = 0x0060,
        TriggerDisableInput = 0x0061,
        TriggerEggHawk = 0x0062,
        TriggerFalco = 0x0063,
        TriggerHurt = 0x0064,
        TriggerKlagen = 0x0065,
        BobsledJumpCollisionObject = 0x0066,
        BonusKey = 0x0067,
        TeleportTrigger = 0x0080,
        SECollisionObject = 0x0081,
        NoOttoOttoCollisionObject = 0x0082,

        //SeasideHill
        CementBlockOnRails = 0x0102,
        CementSlidingBlock = 0x0103,
        CementBlock = 0x0104,
        MovingRuinPlatform = 0x0105,
        TriggerRuins = 0x0108,
        SeasideHillSun = 0x010A,
        HermitCrab = 0x010B,
        SeasideHillFlowerPatch = 0x0180,
        SeasideHillFlag = 0x0181,
        SeasideHillWhale = 0x0182,
        SeasideHillSeagulls = 0x0183,
        SeasideHillLargeBird = 0x0184,
        SeasideHillWhaleCollisionObject = 0x0185,
        SeasideHillWaterfallLarge = 0x0186,
        SeasideHillTidesWave = 0x0187,
        SmallStonePlatform = 0x0188,
        SeasideHillWaterfallSmall = 0x0189,
        SeasideHillParticleEffect = 0x01FF,

        //OceanPalace
        CrumblingStonePillar = 0x0200,
        FallingStoneStructure = 0x0201,
        OceanPalaceBreakableDoor = 0x0202,
        OceanPalaceBreakableBlock = 0x0203,
        Kaos = 0x0204, //big spiky boulder/ball for final section
        ScrollRingObject = 0x0205,
        MovingItemBalloon = 0x0206,
        OceanPalaceQuakeCollisionObject = 0x020A,
        OceanPalaceTriggerEventActivate = 0x020B,
        TriggerKaos = 0x020C,
        TriggerMovingLand = 0x0280,
        TurtleFeet = 0x0281,
        TurtleWave = 0x0282,
        OceanPalaceFlowingWater = 0x0283,
        OceanPalaceGreenPlant = 0x0284,
        OceanPalacePole = 0x0285,

        //GrandMetropolis
        EnergyRoadSection = 0x0300, //pipe? actual road obj
        GrandMetropolisRoadCap = 0x0302, //Egg Cap
        GrandMetropolisDoor = 0x0303,
        FallingDrawbridge = 0x0304, //great to lock out areas
        TiltingBridge = 0x0305, //tilting bridge at end of level
        GrandMetropolisFlyingCar = 0x0306,
        BlimpPlatform = 0x0307,
        EnergyRoadSpeedEffect = 0x0308, //collision for speed increase
        GrandMetropolisBalloonDesign = 0x0380,
        GrandMetropolisPlaneTrigger = 0x0381,
        GrandMetropolisTrain = 0x0382,
        GrandMetropolisPipeDesign = 0x0383,
        GrandMetropolisEnergyPiston = 0x0384,
        GrandMetropolisFlashingDoorLights = 0x0385,
        HEXAecoSignboard = 0x0386,

        //Power Plant
        EnergyRoadUpwardSection = 0x0400, //energypipeup
        EnergyColumn = 0x0401,
        Elevator = 0x0402,
        LavaPlatform = 0x0403,
        PowerPlantLavaCap = 0x0404, //Egg Cap Big
        PowerPlantFireball = 0x0406,
        PowerPlantColumnCap = 0x0408, //Egg Cap 2
        PowerPlantShutter = 0x0410,
        LiquidLava = 0x0412, //energy up
        PowerPlantElevatorCap = 0x0413, //Egg Cap Elev
        PowerPlantGlassBallCollisionObject = 0x0414,
        EnergyRoadUpwardEffect = 0x0415,
        PowerPlantElevatorSupportColumn = 0x0416, //pipe elev
        PowerPlantGlassBall = 0x0480,
        EnergyWallBackground = 0x0481,
        PowerPlantCrane = 0x0482,
        PowerPlantSatellite = 0x0483,
        HEXAecoWallLight = 0x0484,
        PowerPlantFloorLight = 0x0485,
        LavaShutter = 0x0486,

        //Casino Park
        SmallBumper = 0x0500,
        GreenFloatingBumper = 0x0501,
        PinballFlipper = 0x0502,
        SmallTriangleBumper = 0x0503,
        StarGlassPanel = 0x0504,
        StarGlassAirPanel = 0x0505,
        LargeTriangleBumper = 0x0506,
        CasinoParkXSign = 0x0507,
        LargeCasinoDoor = 0x0508,
        BreakableGlassFloor = 0x0509,
        FloatingDice = 0x050A,
        TripleSlots = 0x050B,
        SingleSlots = 0x050C,
        BingoChart = 0x050D, //BingoPanel Chart for bingos
        BingoChip = 0x050E, //BingoGate chip for bingos
        DashArrow = 0x0510,
        PotatoChip = 0x0511, //chaotix chips
        CasinoParkLightArrowSign = 0x0580,
        CasinoParkLargeFloatingArrow = 0x0581,
        CasinoParkLargeFloatingLetter = 0x0582,
        UnusedFireworks = 0x0583,
        GiantDiceDeco = 0x0584,
        GiantSlotDeco = 0x0585,
        GiantRouletteDeco = 0x0586, //not sure where glass is
        GiantCasinoChipDeco = 0x0587,
        CasinoParkSkybox = 0x0588,

        //BingoHighway
        BingoHighwayBingoChartMaybeNotUsed = 0x0600,
        BingoHighwayBingoNumberMaybeNotUsed = 0x0601,

        //RailCanyon
        SwitchableRail = 0x0700,
        RailSwitch = 0x0701,
        SwitchableArrow = 0x0702,
        RailBooster = 0x0703,
        RailCrossingRoadblock = 0x0704,
        Capsule = 0x0705,
        StationDoor = 0x0706,
        FloorGrate = 0x0707,
        RailPlatform = 0x0708,
        DestructableRail = 0x070A,
        TrainTrain = 0x071B,
        Tunnel = 0x072C,
        EngineCore = 0x072D,
        BigGunInterior = 0x073E,
        BigCannonGunTopDeco = 0x073F,
        TriggerTrainMaybeAmbience = 0x0740,
        ExplosionEffect = 0x0741,
        EggmanBase = 0x0742,
        RailCanyonBobsledCollisionObject = 0x0743,
        RailCanyonFan = 0x0780,
        RailBush = 0x0781,
        RailBarbedWireFence = 0x0782,
        RailChangeRail = 0x0783,
        RailBulletRack = 0x0785,
        RailWaterSupply = 0x0786,
        RailMechTypeABC = 0x0787,
        RailCapEN = 0x0788,
        RailCapEX = 0x0789,
        RailWideCapBlue = 0x078A,
        RailWideCapRed = 0x078B,
        RailPollExclamationMark = 0x078C,
        RailPollArrowLeft = 0x078D,
        RailPollArrowRight = 0x078E,
        RailTie = 0x078F,
        RailCanyonPropeller = 0x0790,
        Piston = 0x0791,
        Barrel = 0x0792,
        RailCanyonPulley = 0x0793,
        EggHorn = 0x0794,
        TrainAppearOnOff = 0x0795,
        CanyonBridge = 0x0796,
        AutoDoor = 0x0797,
        TrainTop = 0x0798,

        //BulletStation
        BulletStationFanDeco = 0x0800,
        MountainCannon = 0x0801,
        BulletStationTorchDeco = 0x0802,
        Wheel = 0x0803,
        WallCanyon = 0x0804,

        //FrogForest
        GreenFrog = 0x0900,
        SmallGreenRainPlatform = 0x0902, //RainLeaf
        SmallBouncyMushroom = 0x0903,
        TallVerticalVine = 0x0904,
        TallTreeWithPlatforms = 0x0905,
        IvyThatGrowsAsYouGrindOnIt = 0x0906,
        LargeYellowPlatform = 0x0907, //RainFloor
        BouncyFruit = 0x0908,
        BigBouncyMushroom = 0x0909,
        SwingingVine = 0x090B,
        MossyBall = 0x090C,
        StopRain = 0x090D,
        Alligator = 0x090E,
        RainFruitMI = 0x090F,
        IvyThatGrowsAsYouGrindOnIt2 = 0x0910,
        IvyThatGrowsAsYouGrindOnIt3 = 0x0911,
        IvyThatGrowsAsYouGrindOnItETC = 0x0912,
        RainCollisionObject = 0x0913,
        Butterflies = 0x0980,
        PinkFlower = 0x0981,
        SmallMushroomDeco = 0x0982,
        MediumPlant = 0x0983,
        SmallPlantRedLeaves = 0x0984,
        SmallPlant = 0x0985,
        Bush = 0x0986,
        YellowPlant = 0x0987,
        GreenMushroom = 0x0988, //PEPE
        Pond = 0x0989,
        Palmtree = 0x098A,
        LargeLeaf = 0x098B,
        WaterPlants = 0x098C,
        WigglingMushroom = 0x098D,
        HangingYellowFruit = 0x0991,
        TreeLeaf = 0x0992,
        MossPatchOnGround = 0x0993,
        LargeGreenThing = 0x0995,
        LargePlant = 0x0997,
        SwampWater = 0x0998,
        Powder = 0x0999,
        FloatingTrunk = 0x099A,
        Rain = 0x099B,

        //LostJungle
        BlackFrog = 0x1000,
        BouncyFallingFruit = 0x1001,
        LostJungleRain = 0x1002,
        LostJunglePond = 0x1003,
        LostJungleSwampWater = 0x1004,

        //HangCastle
        TeleporterSwitch = 0x1100,
        CastleDoor = 0x1101,
        CastleCrackedWall = 0x1102,
        CastleFloatingPlatform = 0x1103,
        FlameTorch = 0x1104, //S11 Fire Obj used by Chaotix MM
        PumpkinGhost = 0x1105,
        MansionFloatingPlatform = 0x1106,
        MansionCrackedWall = 0x1107,
        MansionDoor = 0x1108,
        CastleKey = 0x1109,
        HangCastleBobsledDummyObject = 0x110A,
        TriggerDoor = 0x110B,
        TriggerMusic = 0x110C,
        GlowEffect = 0x1180,
        CelestialSphere = 0x1181,
        CastleThunderLightning = 0x1182,
        CastleTriggerThunderLightning = 0x1183,
        SmokeScreen = 0x1184,
        Skeleton = 0x1185,
        TriggerSkeleton = 0x1186,
        SpinningSkeletonHands = 0x1187,
        CastleCurtain = 0x1188,
        GlowingSpiderSigns = 0x1189,
        CastleTree = 0x118A,
        SpikedPlant = 0x118B,
        CastleSmallPlant = 0x118C,
        SwingingAxe = 0x118D,

        //MysticMansion
        MysticMansionPumpkinGhost = 0x1200,
        MysticMansionSkeleton = 0x1201,
        MysticMansionDoor = 0x1202,
        MysticMansionFlameTorchDeco = 0x1203,

        //EggFleet
        NormalCannon = 0x1300,
        LargeCannon = 0x1301,
        HorizontalCannon = 0x1302,
        MovingCannon = 0x1303,
        RectangularFloatingPlatform = 0x1304, //EggFleet
        EggFleetDoor = 0x1305,
        SquareFloatingPlatform = 0x1306,
        EggFleetRoadblock = 0x1307,
        ConveyorBelt = 0x1308,
        BigMovShip = 0x1314,
        AnotherCannon = 0x1315,
        KanKyoHakai = 0x1320,
        BigFan = 0x1380,
        MissilePod = 0x1381,
        Screw = 0x1382,
        EggFleetDesignPipe = 0x1383,
        EggFleetUFO = 0x1384,
        Blinklight = 0x1385,
        Antenna = 0x1386,
        SenkanFar1 = 0x1387,
        SenkanFar2 = 0x1388,
        SenkanFar3 = 0x1389,
        SenkanFar4 = 0x138A,
        SenkanFar5 = 0x138B,
        SenkanFar6 = 0x138C,
        SenkanFar7 = 0x138D,
        SenkanFar8 = 0x138E,
        SenkanMiddle1 = 0x138F,
        SenkanMiddle2 = 0x1390,
        EggFleetRailCapFront = 0x1391,
        EggFleetRailCapBack = 0x1392,
        EggFleetRailArrow1 = 0x1393,
        EggFleetRailArrow3 = 0x1394,
        SenkanFarMoveTopLeft = 0x1395,
        SenkanFarMoveTopRight = 0x1396,
        SenkanFarMoveSideLeft = 0x1397,
        SenkanFarMoveSideRight = 0x1398,
        Cloud1 = 0x1399,
        Cloud2 = 0x139A,
        SenkanFarMoveBig = 0x139B,

        //FinalFortress
        FallingPlatform = 0x1400,
        HigherCannon = 0x1401,
        LaserBeam = 0x1402,
        TriggerLaserBeam = 0x1403,
        LaserBeamLightSign = 0x1404,
        SelfDestructSwitch = 0x1405,
        FinalFortressBreakableBlock = 0x1406,
        EggmanCellKey = 0x140A,
        Thunder = 0x1480,
        Thunder2 = 0x1481,
        ThunderParticle = 0x1482,
        LaserLight = 0x1483,
        RailEndSign = 0x1484,
        RedLight = 0x1485,
        RoadSideA = 0x1486,
        RoadLight = 0x1487,
        FinalFortressUFO = 0x1488,
        RedRingLight = 0x1489,
        WallNeon = 0x148A,
        WallLightSide = 0x148B,
        WallLightFront = 0x148C,
        WallNeonLeft = 0x148D,
        WallNeonRight = 0x148E,
        FinalFortressGoalNeonFloor = 0x148F,
        NeonFloor = 0x1490,
        RoadsideB = 0x1491,
        NeonFloorB = 0x1492,
        TowerNeonA = 0x1493,
        TowerNeonB = 0x1494,
        FinalFortressSearchLight = 0x1495,
        FinalFortressEggMansBase = 0x1496,
        CrushedRoof = 0x1497,
        DecoWallSide = 0x1498,

        //Enemies
        EggFlapper = 0x1500,
        EggPawn = 0x1510,
        Klagen = 0x1520,
        Falco = 0x1530,
        EggHammer = 0x1540,
        Cameron = 0x1570,
        RhinoLiner = 0x1590,
        EggBishop = 0x15C0,
        E2000 = 0x15D0,
        EggMobileObj = 0x15D1,
        MetalSonic1 = 0x15D2,
        MetalSonic2 = 0x15D3,
        MetalMadnessObj = 0x15D4,
        MetalOverlordObj = 0x15D5,
        SpecialStageOrbs = 0x15E0,
        SpecialStageBossAppear = 0x15E5,
        SpecialStageBossEnd = 0x15E6,
        SpecialStageBossAppearPos = 0x15E7,
        AppearEmerald = 0x15E8,
        SkyBobsleigh = 0x15E9,
        SkyBobsledEnd = 0x15EA,
        PutParticle = 0x15EB,
        ParticleTest = 0x15EC,
        SpecialStageEnd = 0x15ED,
        SpecialStageSpring = 0x15EE,
        SpecialStageDashPanel = 0x15F0,
        SpecialStageDashRing = 0x15F1,
        SpecialStageFormationGate = 0x15F4,

        //EggEmperor
        EggEmperorCollisionCC = 0x1600,
        EggEmperorCollisionCP = 0x1601,
        EggEmperorKingPawn = 0x1602,

        //EggHawk
        EggHawkRoadDecoBlock = 0x2080,
        EggHawkWhaleStatue = 0x2081,
        EggHawkTower = 0x2082,
        EggTreatStar = 0x2083,

        //EggAlbatross
        TriggerEggAlbatross = 0x2300,

        //MetalMadness
        TriggerMetalMadness = 0x2400,

        //MetalOverlord
        TriggerMetalOverlord = 0x2500,

        //MultiplayerBobsled
        UnbrokenBobCountObject = 0x3300,
        SeasideBobsledCourseBobsledDummyObject = 0x3301,
        CasinoCourseDiceObject = 0x3380,
        CasinoCourseRouletteObject = 0x3381,
        CasinoCourseSlotObject = 0x3382,

        //Unknown
        CustomObjectTest = 0xC040,
        SystemObject1 = 0xFFF0,
        SystemObject2 = 0xFFF1,
        SystemObject3 = 0xFFF2,
        SampleObject1 = 0xFFFE,
        SampleObject2 = 0xFFFF,
    }


public static class EnumExtension
{
    public static Dictionary<string, string> EnumDescCache = new();
    public static string? GetDescAttribute<T>(this Enum enumVal) where T : Attribute
    {
        if (EnumDescCache.TryGetValue(enumVal.ToString(), out string? result))
        {
            return result;

        }
        
        
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(string), false);
        return (attributes.Length > 0) ? (string)attributes[0] : null;
    }
}
