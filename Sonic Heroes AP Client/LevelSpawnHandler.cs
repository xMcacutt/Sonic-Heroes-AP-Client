

using System.Numerics;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

namespace Sonic_Heroes_AP_Client;
//this is at Base + 0x3C2FC8

public class LevelSpawnHandler
{
    public bool ShouldCheckForInput = false;
    public byte LastActSelected = 0;
    public int SpawnPosIndex = 0;
    
    public readonly uint SpawnDataStart = (uint)(Mod.ModuleBase + 0x3C2FC8);
    
    public struct TeamSpawnData
    {
        public float XSpawnPos;
        public float YSpawnPos;
        public float ZSpawnPos;
        public ushort Pitch;
        public ushort PaddingShort;
        public int PaddingInt;
        public SpawnMode Mode;
        public byte PaddingByte;
        public byte PaddingByte2;
        public byte PaddingByte3;
        public ushort RunningTime;
        public byte PaddingByte4;
        public byte PaddingByte5;
    }


    private Dictionary<LevelId, int> _levelToSpawnDataIndex = new Dictionary<LevelId, int>()
    {
        { LevelId.SeasideHill, 0 },
        { LevelId.OceanPalace, 1 },
        { LevelId.GrandMetropolis, 2 },
        { LevelId.PowerPlant, 3 },
        { LevelId.CasinoPark, 4 },
        { LevelId.BingoHighway, 5 },
        { LevelId.RailCanyon, 6 },
        { LevelId.BulletStation, 7 },
        { LevelId.FrogForest, 8 },
        { LevelId.LostJungle, 9 },
        { LevelId.HangCastle, 10 },
        { LevelId.MysticMansion, 11 },
        { LevelId.EggFleet, 12 },
        { LevelId.FinalFortress, 13 },
        { LevelId.EggHawk, 14 },
        { LevelId.TeamFight1, 15 },
        { LevelId.RobotCarnival, 16 },
        { LevelId.EggAlbatross, 17 },
        { LevelId.TeamFight2, 18 },
        { LevelId.RobotStorm, 19 },
        { LevelId.EggEmperor, 20 },
        { LevelId.MetalMadness, 21 },
        { LevelId.MetalOverlord, 22 },
        { LevelId.SeaGate, 23 },
        { LevelId.BonusStage1, 24 },
        { LevelId.BonusStage2, 25 },
        { LevelId.BonusStage3, 26 },
        { LevelId.BonusStage4, 27 },
        { LevelId.BonusStage5, 28 },
        { LevelId.BonusStage6, 29 },
        { LevelId.BonusStage7, 30 },
        { LevelId.ChaotixRailCanyon, 31 },
        { LevelId.EmeraldStage1, 32 },
        { LevelId.EmeraldStage2, 33 },
        { LevelId.EmeraldStage3, 34 },
        { LevelId.EmeraldStage4, 35 },
        { LevelId.EmeraldStage5, 36 },
        { LevelId.EmeraldStage6, 37 },
        { LevelId.EmeraldStage7, 38 },
    };

    public unsafe void ChangeSpawnPos()
    {
        var levelSelectPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var levelIndex = *(int*)(levelSelectPtr + 0x194);
        if (levelIndex is < 0 or > 21)
        {
            Console.WriteLine($"Level {levelIndex} is out of range.");
            return;
        }
        
        
        
        var level = (LevelId)Mod.UserInterface!.LevelTracker.LevelMapping[levelIndex];
        var storyIndex = *(int*)(levelSelectPtr + 0x194 + 0x8C);
        
        var actPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var actIndex = *(int*)(actPtr + 0x2BC);
        
        var team = (Team)storyIndex;

        if (Mod.LevelSpawnData!.GetAllSpawnDataForLevel(team, level).Count < 1)
            return;

        if (SpawnPosIndex > Mod.LevelSpawnData!.GetAllSpawnDataForLevel(team, level).Count - 1)
            return;
        
        LevelSpawnEntry entry = Mod.LevelSpawnData!.GetAllSpawnDataForLevel(team, level)[SpawnPosIndex];
        ChangeSpawnPos(team, level, entry.Pos.X, entry.Pos.Y, entry.Pos.Z, entry.Pitch, entry.Mode, entry.RunningTime, entry.PaddingShort);
    }
    
    
    public unsafe void ChangeSpawnPos(Team team, LevelId level, float x, float y, float z, ushort pitch, SpawnMode mode, ushort runningTime, ushort paddingShort)
    {
        //Console.WriteLine($"Running ChangeSpawnPos: Team {team} Level {level}  X {x} Y {y} Z {z} Pitch {pitch} SpawnMode {mode} runningTime {runningTime}");
        TeamSpawnData* data = (TeamSpawnData*) new IntPtr(GetSpawnDataPtr(team, level));
        
        //Memory.Instance.SafeWrite(&data->XSpawnPos, BitConverter.GetBytes(x));
        
        data->XSpawnPos = x;
        data->YSpawnPos = y;
        data->ZSpawnPos = z;
        data->Mode = mode;
        data->RunningTime = runningTime;
        data->PaddingShort = paddingShort;
    }

    public unsafe TeamSpawnData* GetSpawnDataPtr(Team team, LevelId level)
    {
        //Console.WriteLine($"Running GetSpawnPos: Team {team} Level {level}");
        if (team is Team.SuperHard)
            team = Team.Sonic;
        
        if (team == Team.Chaotix && level == LevelId.RailCanyon)
            level = LevelId.ChaotixRailCanyon;

        if (!_levelToSpawnDataIndex.ContainsKey(level))
        {
            //Console.WriteLine($"Level {level} does not save Spawn Pos. Defaulting to Sea Gate.");
            level = LevelId.SeaGate;
        }
        int leveloffset = _levelToSpawnDataIndex[level];

        var ptr = SpawnDataStart + leveloffset * 0x90 + 4 + (int)team * 0x1C;
        TeamSpawnData* data = (TeamSpawnData*)ptr;
        
        //Console.WriteLine($"Spawn Pos Ptr Here: 0x{ptr:x}");

        return data;
    }



    /*
     *
     * public struct TeamSpawnData
       {
           public float XSpawnPos;
           public float YSpawnPos;
           public float ZSpawnPos;
           public ushort Pitch;
           public ushort PaddingShort;
           public int PaddingInt;
           public SpawnMode Mode;
           public byte PaddingByte;
           public byte PaddingByte2;
           public byte PaddingByte3;
           public ushort RunningTime;
           public byte PaddingByte4;
           public byte PaddingByte5;
       }

       public struct LevelSpawnData
       {
           public int LevelIndex;
           public TeamSpawnData SonicSpawn;
           public TeamSpawnData DarkSpawn;
           public TeamSpawnData RoseSpawn;
           public TeamSpawnData ChaotixSpawn;
           public TeamSpawnData UnusedTeamSpawn;
       }

       public struct AllLevelSpawnData
       {
           public LevelSpawnData SeasideHill;
           public LevelSpawnData OceanPalace;
           public LevelSpawnData GrandMetropolis;
           public LevelSpawnData PowerPlant;
           public LevelSpawnData CasinoPark;
           public LevelSpawnData BingoHighway;
           public LevelSpawnData RailCanyon;
           public LevelSpawnData BulletStation;
           public LevelSpawnData FrogForest;
           public LevelSpawnData LostJungle;
           public LevelSpawnData HangCastle;
           public LevelSpawnData MysticMansion;
           public LevelSpawnData EggFleet;
           public LevelSpawnData FinalFortress;
           public LevelSpawnData EggHawk;
           public LevelSpawnData TeamFight1;
           public LevelSpawnData RobotCarnival;
           public LevelSpawnData EggAlbatross;
           public LevelSpawnData TeamFight2;
           public LevelSpawnData RobotStorm;
           public LevelSpawnData EggEmperor;
           public LevelSpawnData MetalMadness;
           public LevelSpawnData MetalOverlord;
           public LevelSpawnData SeaGate;
           public LevelSpawnData BonusStage1;
           public LevelSpawnData BonusStage2;
           public LevelSpawnData BonusStage3;
           public LevelSpawnData BonusStage4;
           public LevelSpawnData BonusStage5;
           public LevelSpawnData BonusStage6;
           public LevelSpawnData BonusStage7;
           public LevelSpawnData ChaotixRailCanyon;
           public LevelSpawnData EmeraldStage1;
           public LevelSpawnData EmeraldStage2;
           public LevelSpawnData EmeraldStage3;
           public LevelSpawnData EmeraldStage4;
           public LevelSpawnData EmeraldStage5;
           public LevelSpawnData EmeraldStage6;
           public LevelSpawnData EmeraldStage7;
       }
     */
    
    
}