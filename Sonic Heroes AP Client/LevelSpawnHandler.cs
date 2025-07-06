

using System.Numerics;

namespace Sonic_Heroes_AP_Client;


//this is at Base + 0x3C2FC8

public class LevelSpawnHandler
{
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
        public LevelSpawnData RailCanyonChaotix;
        public LevelSpawnData EmeraldStage1;
        public LevelSpawnData EmeraldStage2;
        public LevelSpawnData EmeraldStage3;
        public LevelSpawnData EmeraldStage4;
        public LevelSpawnData EmeraldStage5;
        public LevelSpawnData EmeraldStage6;
        public LevelSpawnData EmeraldStage7;
    }
    
    
    
    
    
    
    
}