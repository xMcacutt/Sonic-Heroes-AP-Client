using System.Reflection.Emit;

namespace Sonic_Heroes_AP_Client;

public class CustomSaveData
{
    public int LastItemIndex = 1;
    public int Emblems = 0;
    
    public Dictionary<Emerald, bool> Emeralds = new ()
    {
      [Emerald.Green] = false,
      [Emerald.Blue] = false,
      [Emerald.Yellow] = false,
      [Emerald.White] = false,
      [Emerald.Cyan] = false,
      [Emerald.Purple] = false,
      [Emerald.Red] = false,
    };

    public bool[] GateBossUnlocked = new bool[8];

    public bool[] GateBossComplete = new bool[7];
    

    public Dictionary<Team, TeamProgSaveData>  UnlockSaveData = new()
    {
        [Team.Sonic] =  new TeamProgSaveData(),
        [Team.Dark] =  new TeamProgSaveData(),
        [Team.Rose] =  new TeamProgSaveData(),
        [Team.Chaotix] =  new TeamProgSaveData(),
        [Team.SuperHardMode] =  new TeamProgSaveData(),
    };

    public Dictionary<Team, Dictionary<LevelId, List<bool>>> SpawnDataUnlocks = new()
    {
        
        [Team.Sonic] = Enum.GetValues<LevelId>()
            .Where(id => ((int)id < 16 && (int)id > 1) || (int)id == 23 || (int)id == 24)
            .ToDictionary(
                id => id,
                id =>
                {
                    return new[] { true }
                        .Concat(Enumerable.Repeat(false,
                            CheckPointPriorities.AllCheckpoints.Count(x => x.Team == Team.Sonic && x.LevelId == id && !x.SuperHard)))
                        .ToList();
                }),
        [Team.Dark] = Enum.GetValues<LevelId>()
            .Where(id => ((int)id < 16 && (int)id > 1) || (int)id == 23 || (int)id == 24)
            .ToDictionary(
                id => id,
                id =>
                {
                    return new[] { true }
                        .Concat(Enumerable.Repeat(false,
                            CheckPointPriorities.AllCheckpoints.Count(x => x.Team == Team.Dark && x.LevelId == id)))
                        .ToList();
                }),
        [Team.Rose] = Enum.GetValues<LevelId>()
            .Where(id => ((int)id < 16 && (int)id > 1) || (int)id == 23 || (int)id == 24)
            .ToDictionary(
                id => id,
                id =>
                {
                    return new[] { true }
                        .Concat(Enumerable.Repeat(false,
                            CheckPointPriorities.AllCheckpoints.Count(x => x.Team == Team.Rose && x.LevelId == id)))
                        .ToList();
                }),
        [Team.Chaotix] = Enum.GetValues<LevelId>()
            .Where(id => ((int)id < 16 && (int)id > 1) || (int)id == 23 || (int)id == 24)
            .ToDictionary(
                id => id,
                id =>
                {
                    return new[] { true }
                        .Concat(Enumerable.Repeat(false,
                            CheckPointPriorities.AllCheckpoints.Count(x => x.Team == Team.Chaotix && x.LevelId == id)))
                        .ToList();
                }),
        [Team.SuperHardMode] = Enum.GetValues<LevelId>()
            .Where(id => ((int)id < 16 && (int)id > 1) || (int)id == 23 || (int)id == 24)
            .ToDictionary(
                id => id,
                id =>
                {
                    return new[] { true }
                        .Concat(Enumerable.Repeat(false,
                            CheckPointPriorities.AllCheckpoints.Count(x => x.Team == Team.Sonic && x.LevelId == id && x.SuperHard)))
                        .ToList();
                }),
    };
    
    public Dictionary<string, string> MusicRandoMapping =
        MusicShuffleData.HeroesSongs.ToDictionary(x => x.name.Split('\\').Last(), x => x.name.Split('\\').Last());
}

public class TeamProgSaveData
{
    public Dictionary<FormationChar, bool> CharsUnlocked = new()
    {
        [FormationChar.Speed] = false,
        [FormationChar.Flying] = false,
        [FormationChar.Power] = false,
    };
    
    /*
    public Dictionary<FormationChar, int> CharLevelUps = new()
    {
        [FormationChar.Speed] = 0,
        [FormationChar.Flying] = 0,
        [FormationChar.Power] = 0,
    };
    */

    public Dictionary<Region, AbilityUnlockSaveData> AbilityUnlocks = new ()
    {
        [Region.Ocean] =  new AbilityUnlockSaveData(),
        [Region.HotPlant] =  new AbilityUnlockSaveData(),
        [Region.Casino] =  new AbilityUnlockSaveData(),
        [Region.Train] =  new AbilityUnlockSaveData(),
        [Region.BigPlant] =  new AbilityUnlockSaveData(),
        [Region.Ghost] =  new AbilityUnlockSaveData(),
        [Region.Sky] =  new AbilityUnlockSaveData(),
        [Region.Boss] =  new AbilityUnlockSaveData(),
        [Region.FinalBoss] =   new AbilityUnlockSaveData(),
    };
}

public class AbilityUnlockSaveData
{
    public SpeedAbility SpeedLevel = SpeedAbility.None;
    public FlyingAbility FlyingLevel = FlyingAbility.None;
    public PowerAbility PowerLevel = PowerAbility.None;
}