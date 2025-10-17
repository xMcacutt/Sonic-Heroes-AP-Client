using System.Windows.Forms;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

namespace Sonic_Heroes_AP_Client;

public class StageObjHandler
{

    public static bool FirstTimeRunningSpawnInLevel = true;
    public static Dictionary<int, byte> ObjsDestroyedInLevel = new();
    
    public struct ObjSpawnData
    {
        public float XSpawnPos;
        public float YSpawnPos;
        public float ZSpawnPos;
        public float XSpawnRot;
        public float YSpawnRot;
        public float ZSpawnRot;
        public byte State;
        public byte Team;
        public byte AnotherState;
        public byte PaddingByte;
        public int PaddingInt;
        public long PaddingLong;

        public ushort ObjId;

        //public byte ObjId; //instead of ushort
        //public byte ObjList; //instead of ushort
        public byte LinkId;
        public byte RenderDistance;
        public int PtrVars;
        public int PaddingInt2;
        public int PtrPrevObj;
        public int PtrNextObj;
        public int PtrDynamicMem;
    }

    public static List<StageObjTypes> ObjsToNotSpawn = new List<StageObjTypes>()
    {
        StageObjTypes.Spring,
        StageObjTypes.TripleSpring,
        //StageObjTypes.RingGroup,
        //StageObjTypes.HintRing,
        StageObjTypes.Switch,
        StageObjTypes.PushPullSwitch, 
        StageObjTypes.TargetSwitch,
        StageObjTypes.DashPanel,
        StageObjTypes.DashRing,
        StageObjTypes.RainbowHoops,
        //StageObjTypes.Checkpoint,
        StageObjTypes.DashRamp,
        StageObjTypes.Cannon,
        StageObjTypes.Weight,
        StageObjTypes.BreakableWeight,
        //StageObjTypes.ItemBox,
        //StageObjTypes.ItemBalloon,
        //StageObjTypes.GoalRing,
        StageObjTypes.Pulley,
        StageObjTypes.Chao,
        StageObjTypes.CageBox,
        StageObjTypes.Propeller,
        StageObjTypes.Pole,
        StageObjTypes.PowerGong,
        StageObjTypes.Fan,
        StageObjTypes.WarpFlower,
        //StageObjTypes.BonusKey,
        
        //Seaside Hill
        //StageObjTypes.SeasideHillCementBlock,
        StageObjTypes.MovingRuinPlatform,
        StageObjTypes.SeasideHillHermitCrabChaotix,
        StageObjTypes.SmallStonePlatform,
        
        //Ocean Palace
        //StageObjTypes.CrumblingStonePillar,
        //StageObjTypes.FallingStoneStructure,
        //StageObjTypes.ScrollRingObject,
        //StageObjTypes.MovingItemBalloon,
        //StageObjTypes.OceanPalacePole,
        
        //Grand Metro
        StageObjTypes.GrandMetropolisAcceleratorRoad, //pipe? actual road obj
        StageObjTypes.GrandMetropolisFallingBridge, //great to lock out areas
        StageObjTypes.GrandMetropolisBigBridge2, //tilting bridge at end of level
        StageObjTypes.GrandMetropolisBlimpPlatform,
        //StageObjTypes.GrandMetropolisAccelerator, //collision for speed increase
        //StageObjTypes.GrandMetropolisBalloonDesign,
        //Power Plant
        StageObjTypes.PowerPlantUpwardPath, //energypipeup
        //StageObjTypes.PowerPlantEnergyColumn,
        StageObjTypes.PowerPlantElevator,
        StageObjTypes.PowerPlantLavaPlatform,
        StageObjTypes.PowerPlantGreenUpwardPath,
       
        //Casino Park
        //StageObjTypes.SmallBumper,
        //StageObjTypes.GreenFloatingBumperSpring,
        StageObjTypes.PinballFlipper,
        //StageObjTypes.TriangleBumper,
        //StageObjTypes.GlassStarBumperPanel,
        //StageObjTypes.AirGlassStarBumperPanel,
        //StageObjTypes.LargeTriangleBumper,
        //StageObjTypes.BreakableGlassFloor,
        StageObjTypes.FloatingDice,
        //StageObjTypes.TripleSlot,
        //StageObjTypes.SingleSlot,
        //StageObjTypes.CasinoParkBingoChart, //BingoPanel Chart for bingos
        //StageObjTypes.CasinoParkBingoNumber, //BingoGate chip for bingos
        //StageObjTypes.DashArrow,
        StageObjTypes.CasinoChipChaotix, //chaotix chips
        
        //Bingo Highway
        //StageObjTypes.BingoHighwayBingoChartMaybeNotUsed,
        //StageObjTypes.BingoHighwayBingoNumberMaybeNotUsed,
        
        //Rail Canyon
        StageObjTypes.SwitchableRail,
        StageObjTypes.SwitchableRailSwitch,
        //StageObjTypes.RailBooster,
        StageObjTypes.Capsule,
        StageObjTypes.PlatformWith3Rails,
        //StageObjTypes.Tunnel,
        //StageObjTypes.EngineCore,
        StageObjTypes.BigCannonGunInterior,
        //StageObjTypes.BigCannonGunTopDeco,
        //StageObjTypes.RailCanyonFan,
        StageObjTypes.RailCanyonPropeller,
        StageObjTypes.RailCanyonPulley,
        
        
        //Bullet Station
        //StageObjTypes.BulletStationFanDeco,
        
        //Frog Forest
        StageObjTypes.GreenFrog,
        //StageObjTypes.SmallGreenRainPlatform, //RainLeaf
        //StageObjTypes.BouncyMushroomSmall,
        //StageObjTypes.TallVerticalVine,
        //StageObjTypes.TallTreeWithPlatforms,
        //StageObjTypes.IvyThatGrowsAsYouGrindOnIt,
        //StageObjTypes.LargeYellowPlatform, //RainFloor
        //StageObjTypes.BouncyFruit,
        //StageObjTypes.BouncyMushroomBig,
        //StageObjTypes.SwingingVines,
        //StageObjTypes.MossyBall,
        //StageObjTypes.Alligator,
       
        //Lost Jungle
        StageObjTypes.BlackFrog,
        StageObjTypes.LostJungleBouncyFruit,
        
        //Hang Castle
        StageObjTypes.TeleporterSwitch,
        StageObjTypes.CastleFloatingPlatform,
        StageObjTypes.FlameTorch, //S11 Fire Obj used by Chaotix MM
        StageObjTypes.MansionFloatingPlatform,
        //StageObjTypes.MansionCrackedWall,
        //StageObjTypes.MansionDoor,
        StageObjTypes.ChaotixKey,
        //StageObjTypes.TriggerMusic,
        
        //Mystic Mansion
        
        //Egg Fleet
        StageObjTypes.RectangularFloatingPlatform, //EggFleet
        StageObjTypes.SquareFloatingPlatform,
        //StageObjTypes.BigMovShip,
        StageObjTypes.BigFan,
        
        //Final Fortress
        StageObjTypes.FallingPlatform,
        //StageObjTypes.SelfDestructTPSwitch,
        StageObjTypes.FinalFortressKeyChaotix,
        
        //Enemies
        //StageObjTypes.EggFlapper,
        //StageObjTypes.EggPawn,
        //StageObjTypes.Klagen,
        //StageObjTypes.Falco,
        //StageObjTypes.EggHammer,
        //StageObjTypes.Cameron,
        //StageObjTypes.RhinoLiner,
        //StageObjTypes.EggBishop,
        //StageObjTypes.E2000,
        //StageObjTypes.EggMobileObj,
        //StageObjTypes.MetalSonic1,
        //StageObjTypes.MetalSonic2,
        //StageObjTypes.MetalMadnessObj,
        //StageObjTypes.MetalOverlordObj,
        StageObjTypes.SpecialStageGroupObject,
        //StageObjTypes.SpecialStageBossAppear,
        //StageObjTypes.SpecialStageBossEnd,
        //StageObjTypes.SpecialStageBossAppearPos,
        //StageObjTypes.AppearChaosEmerald,
        StageObjTypes.SpecialStageSpring,
        StageObjTypes.SpecialStageDashPanel,
        StageObjTypes.SpecialStageDashRing,
        //StageObjTypes.SpecialStageFormationGate,
    };


    public static Dictionary<string, List<StageObjTypes>> ObjUnlockTypes = new Dictionary<string, List<StageObjTypes>>()
    {
        {"Spring", new List<StageObjTypes>() {
            StageObjTypes.Spring, 
            StageObjTypes.TripleSpring}},
        
        {"DashObjs", new List<StageObjTypes>()
        {
            StageObjTypes.DashPanel,  
            StageObjTypes.DashRing,
            StageObjTypes.RainbowHoops,
            StageObjTypes.DashRamp
        }},
        {"Cannon", new List<StageObjTypes>()
        {
            StageObjTypes.Cannon
        }},
        {"Pulley", new List<StageObjTypes>()
        {
            StageObjTypes.Pulley,
            StageObjTypes.RailCanyonPulley
        }},
        {"Pole", new List<StageObjTypes>()
        {
            StageObjTypes.Pole
        }},
        {"PowerGong", new List<StageObjTypes>()
        {
            StageObjTypes.PowerGong
        }},
        {"Fan", new List<StageObjTypes>()
        {
            StageObjTypes.Fan, 
            StageObjTypes.Propeller,
            StageObjTypes.RailCanyonFan,
            StageObjTypes.RailCanyonPropeller,
            StageObjTypes.BulletStationFanDeco,
            StageObjTypes.BigFan
        }},
        {"SpecialStage", new List<StageObjTypes>()
        {
            StageObjTypes.SpecialStageGroupObject,
            StageObjTypes.SpecialStageSpring,
            StageObjTypes.SpecialStageDashPanel,
            StageObjTypes.SpecialStageDashRing,
        }},
        
        {"Rails", new List<StageObjTypes>()
        {
            StageObjTypes.SwitchableRail,
            StageObjTypes.SwitchableRailSwitch,
        }},
        {"Chaotix",  new List<StageObjTypes>()
        {
            StageObjTypes.WarpFlower,
            StageObjTypes.SeasideHillHermitCrabChaotix,
            StageObjTypes.Chao,
            StageObjTypes.CasinoChipChaotix,
            StageObjTypes.Capsule,
            StageObjTypes.ChaotixKey,
            StageObjTypes.FlameTorch,
            StageObjTypes.FinalFortressKeyChaotix
        }}


    };
    
    
    public static unsafe void OnObjSetStateSpawned(int esi)
    {
        if (FirstTimeRunningSpawnInLevel)
            HandleLevelInit(esi);
        
        
        ObjSpawnData* spawnData = (ObjSpawnData*) new IntPtr(esi);

        if ((StageObjTypes)spawnData->ObjId is StageObjTypes.TripleSpring)
        {
            IncreaseRenderDistanceByFloat(spawnData, 5.0f);
        }


        if ((StageObjTypes)spawnData->ObjId is StageObjTypes.CageBox)
        {
            spawnData->RenderDistance = 0x00;
        }
        
        
        //if (ObjsToNotSpawn.Contains((StageObjTypes)spawnData->ObjId))
        //    HandleDestroyObj(esi);
        
        //Console.WriteLine($"OnObjSetStateSpawned called. ESI = 0x{esi:x} :: " +
        //                  $"ID = {(StageObjTypes)spawnData->ObjId}" +
        //                  $" :: SpawnCoords = {spawnData->XSpawnPos}, {spawnData->YSpawnPos}, {spawnData->ZSpawnPos}" +
        //                  $" :: DynamicMemPtr = 0x{spawnData->PtrDynamicMem:x}");
    }

    public static unsafe void IncreaseRenderDistanceByFloat(ObjSpawnData* _spawnData, float _distanceMultiplier)
    {
        var temp = _spawnData->RenderDistance;
        //ObjSpawnData* spawnData = (ObjSpawnData*) new IntPtr(esi);
        _spawnData->RenderDistance = byte.Max((byte)Math.Min((int)(_spawnData->RenderDistance * _distanceMultiplier), 255), 0x00);
        //Console.WriteLine($"IncreaseRenderDistanceByFloat: OldRenderDistance = 0x{temp:x} ::  " +
                          //$"NewRenderDistance = 0x{_spawnData->RenderDistance:x}");
    }

    public static unsafe void HandleLevelInit(int esi)
    {
        //do stuff here
        ObjsDestroyedInLevel.Clear();
        
        
        FirstTimeRunningSpawnInLevel = false;
    }

    public static unsafe void HandleRespawnObjType(StageObjTypes type)
    {
        foreach (var pair in ObjsDestroyedInLevel)
        {
            ObjSpawnData* spawn = (ObjSpawnData*) new IntPtr(pair.Key);
            if ((StageObjTypes)spawn->ObjId == type)
            {
                spawn->RenderDistance = pair.Value;
            }
            
        }
        ObjsToNotSpawn.Remove(type);
        
    }
    
    public static unsafe void HandleDestroyObj(int esi)
    {
        ObjSpawnData* spawnData = (ObjSpawnData*) new IntPtr(esi);
        
        ObjsDestroyedInLevel[esi] = spawnData->RenderDistance;
        spawnData->RenderDistance = 0x00;
    }


    public static void ClearObjsDestroyedInLevel()
    {
        ObjsDestroyedInLevel.Clear();
        FirstTimeRunningSpawnInLevel = true;
    }


    public static unsafe void HandleObjSpawningWhenReceivingCharItem(Team team, FormationChar formationChar, bool unlock)
    {
        if (!Mod.GameHandler!.InGame())
            return;
        
        Team teamInGame = Mod.GameHandler.GetCurrentStory();
        LevelId level = Mod.GameHandler.GetCurrentLevel();
        Act act = Mod.GameHandler.GetCurrentAct();

        if (team != teamInGame)
            return;

        if (team is Team.Sonic && act is Act.Act2 or Act.Act3 && Mod.ArchipelagoHandler!.SlotData.SuperHardModeSonicAct2)
        {
            team = Team.SuperHardMode;
        }

        if (formationChar is FormationChar.Speed && team is Team.Sonic)
        {
            
            //Final Fortress Sonic Respawn Self Destruct Switches When Getting Sonic
            if (level is LevelId.FinalFortress)
            {
                var stringValue = unlock ? "Respawning" : "Despawning";
                var renderValue = unlock ? (byte)0x0F : (byte)0x00;
                
                
                //Switch 1 A85CC3
                Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 1");
                var renderDistance1 = (byte*)(Mod.ModuleBase + 0x685CC3);
                *renderDistance1 = renderValue;
                
                //Switch 2 A86403
                Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 2");
                var renderDistance2 = (byte*)(Mod.ModuleBase + 0x686403);
                *renderDistance2 = renderValue;
                
                
                //Switch 3 A86443
                Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 3");
                var renderDistance3 = (byte*)(Mod.ModuleBase + 0x686443);
                *renderDistance3 = renderValue;
            }
        }
        
        
        



    }


    public static unsafe void HandleInitSetGenerator()
    {
        //Obj Table is loaded into memory already
        //look into making changes to Objs like coords and the like

        Team team = Mod.GameHandler.GetCurrentStory();
        LevelId level = Mod.GameHandler.GetCurrentLevel();
        Act act = Mod.GameHandler.GetCurrentAct();

        if (team is Team.Sonic && act is Act.Act2 or Act.Act3 && Mod.ArchipelagoHandler!.SlotData.SuperHardModeSonicAct2)
        {
            team = Team.SuperHardMode;
        }
        
        //Console.WriteLine($"Running HandleInitSetGenerator: Team: {team} Level: {level} Act: {act}");
        
        
        //Casino Park Sonic
        //A8B5C3 
        //change to 0
        if (team is Team.Sonic && level == LevelId.CasinoPark && Mod.ArchipelagoHandler.SlotData.RemoveCasinoParkVIPTableLaserGate)
        {
            Console.WriteLine($"Removing Casino Park Sonic VIP Table Laser Gate");
            var renderDistance = (byte*)(Mod.ModuleBase + 0x68B5C3);
            *renderDistance = 0x00;
        }
        
        
        
        
        
        //Rail Canyon Sonic
        //A9151C
        //change to 12620
        if (team is Team.Sonic && level == LevelId.RailCanyon)
        {
            Console.WriteLine($"Rail Canyon Sonic Bonus Key 3 moving down");
            var bonusKey3YCoord = (float*)(Mod.ModuleBase + 0x69151C);
            *bonusKey3YCoord = 12620f;
        }
        
        //Hang Castle Sonic
        //A9125C
        //change to -1755
        if (team is Team.Sonic && level == LevelId.HangCastle)
        {
            Console.WriteLine($"Hang Castle Sonic Bonus Key 3 moving down");
            var bonusKey3YCoord = (float*)(Mod.ModuleBase + 0x69125C);
            *bonusKey3YCoord = -1755f;
        }
        
        //Mystic Mansion Sonic
        //A8A8D8 (coords)
        //change to 15420 -8878 -39730
        if (team is Team.Sonic && level == LevelId.MysticMansion)
        {
            Console.WriteLine($"Mystic Mansion Sonic Bonus Key 3 moving down");
            var bonusKey3XCoord = (float*)(Mod.ModuleBase + 0x68A8D8);
            var bonusKey3YCoord = (float*)(Mod.ModuleBase + 0x68A8D8 + 4);
            var bonusKey3ZCoord = (float*)(Mod.ModuleBase + 0x68A8D8 + 8);
            *bonusKey3XCoord = 15420f;
            *bonusKey3YCoord = -8878f;
            *bonusKey3ZCoord = -39730f;
        }
        
        
        //Final Fortress Sonic
        //A8945C
        //change to 5400 (y)
        if (team is Team.Sonic && level == LevelId.FinalFortress)
        {
            Console.WriteLine($"Final Fortress Sonic Bonus Key 2 moving down");
            var bonusKey2YCoord = (float*)(Mod.ModuleBase + 0x68945C);
            *bonusKey2YCoord = 5400;
            
            
            //UnSpawn Self Destruct Switches if no Sonic
            var spawnSwitches = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic]
                .CharsUnlocked[FormationChar.Speed];
            var stringValue = spawnSwitches ? "Spawning" : "Despawning";
            var renderValue = spawnSwitches ? (byte)0x0F : (byte)0x00;
            
            //Switch 1 A85CC3
            Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 1");
            var renderDistance1 = (byte*)(Mod.ModuleBase + 0x685CC3);
            *renderDistance1 = renderValue;
            
            //Switch 2 A86403
            Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 2");
            var renderDistance2 = (byte*)(Mod.ModuleBase + 0x686403);
            *renderDistance2 = renderValue;
            
            //Switch 3 A86443
            Console.WriteLine($"Final Fortress Sonic {stringValue} SelfDestruct Switch 3");
            var renderDistance3 = (byte*)(Mod.ModuleBase + 0x686443);
            *renderDistance3 = renderValue;
            
        }
        
    }
    
}