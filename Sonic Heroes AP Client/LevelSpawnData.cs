
using System.Numerics;

namespace Sonic_Heroes_AP_Client;

public class LevelSpawnEntry(float x, float y, float z, ushort pitch = 0x0080, SpawnMode mode = SpawnMode.Normal, ushort runningtime = 0, bool unlocked = false, bool secret = false, bool isdefault = false)
{
    public Vector3 Pos = new Vector3(x, y, z);
    public ushort Pitch = pitch;
    public SpawnMode Mode = mode;
    public ushort RunningTime = runningtime;
    public bool Unlocked =  unlocked;
    public bool Secret = secret;
    public bool IsDefault = isdefault;

    public override string ToString()
    {
        return $"Pos: {this.Pos}, Pitch: {this.Pitch}, Mode: {this.Mode},  RunningTime: {this.RunningTime}, Unlocked: {this.Unlocked},  Secret: {this.Secret}, IsDefault: {this.IsDefault}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not LevelSpawnEntry entry)
            return false;

        return Vector3.Distance(this.Pos, entry.Pos) < 100;
    }
}

public class LevelSpawnData
{
    public Dictionary<Team, Dictionary<LevelId, List<LevelSpawnEntry>>> AllSpawnData = new ()
    {
        {
            Team.Sonic,
            new ()
            {
                {
                    LevelId.SeasideHill, new ()
                    {
                        new (0, 20, 2960, mode:SpawnMode.Running, runningtime:370, unlocked:true, isdefault:true),
                        new (1, 110, -4215),
                        new (-4509.383f, 180, -6922.28f),
                        new (-130.082f, 154.9283f, -16150.09f),
                        new (900, 600, -20605),
                        new (880.9f, 2000, -33754.08f),
                    }
                },
                {
                    LevelId.OceanPalace, new ()
                    {
                        new (200, 1300, 0, unlocked:true, isdefault:true),
                        new (750, 810, -10302),
                        new (2398.8208f, 75, -24800.24f),
                        new (2100.043f, 50, -31690.03f),
                        new (800, 1515, -36010),
                    }
                },
                {
                    LevelId.GrandMetropolis, new ()
                    {
                        //new(0, 480, 855, mode:SpawnMode.Running, runningtime:400, unlocked:true, isdefault:true),
                        new (0, -200, -2650, unlocked:true, isdefault:true),
                        new(-134.99962f, -1299.9f, -10105),
                        new(-335.00836f, -4699.9f, -20207),
                        new(3405.0002f, -4649.9f, -28195),
                        new(3400.0005f, -3359.9001f, -33430.008f),
                        new(2649.5144f, -2369.9001f, -41030.516f, secret:true), //secret OOB
                    }
                },
                {
                    LevelId.PowerPlant, new ()
                    {
                        new (0, 640, 100, unlocked:true, isdefault:true),
                        new (5772.445f, 2320, -3946.1865f),
                        new (12816.072f, 3690, -11003.363f),
                        new (16507.467f, 5745, -12811.182f),
                        new (20412.184f, 7690, -12387.666f),
                    }
                },
                {
                    LevelId.CasinoPark, new ()
                    {
                        new (0, 100, 0, unlocked:true, isdefault:true),
                        //new (-7353, -400, -980, unlocked:true, isdefault:true), //testing spawn pos here
                        new (-3190.092f, -39.9f, -2320),
                        new (-7250.528f, -649.9f, -550.4929f),
                        new (-6480, 160, 1360.049f),
                    }
                },
                {
                    LevelId.BingoHighway, new ()
                    {
                        new (0, 233, 400, unlocked:true, isdefault:true),
                        new (680, -2259.9f, -6170.08f),
                        new (680, -2689.9f, -7029.155f),
                        new (500, -5394.9f, -13793.82f),
                        new (8280.059f, -14352.9f, -18490.49f),
                    }
                },
                {
                    LevelId.MetalMadness, new ()
                    {
                        new (0, 20, 0, unlocked:true, isdefault:true),
                    }
                },
                {
                    LevelId.MetalOverlord, new ()
                    {
                        new (-999.20f, 20, -1025.00f, unlocked:true, isdefault:true)
                    }
                },
                {
                    LevelId.SeaGate, new ()
                    {
                        new (-999.20f, 20, -1025.00f, unlocked:true, isdefault:true)
                    }
                }
            }
        },
    };

    public bool ShouldIncludeSecret(Team team, LevelId level)
    {
        return false;
    }


    public unsafe void FromSaveData(CustomSaveData* pointer)
    {
        SonicSpawnDataUnlocks* ptr = &pointer->SonicSpawnDataUnlocks;
        
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.SeasideHill].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.SeasideHill][i].Unlocked = ptr->SeasideHillSpawn[i] > 0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.OceanPalace].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.OceanPalace][i].Unlocked = ptr->OceanPalaceSpawn[i] > 0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.GrandMetropolis].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.GrandMetropolis][i].Unlocked = ptr->GrandMetropolisSpawn[i] > 0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.PowerPlant].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.PowerPlant][i].Unlocked = ptr->PowerPlantSpawn[i] > 0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.CasinoPark].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.CasinoPark][i].Unlocked = ptr->CasinoParkSpawn[i] > 0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.BingoHighway].Count; i++)
        {
            AllSpawnData[Team.Sonic][LevelId.BingoHighway][i].Unlocked = ptr->BingoHighwaySpawn[i] > 0x00;
        }
    }

    public unsafe void ToSaveData(CustomSaveData* pointer)
    {
        SonicSpawnDataUnlocks* ptr = &pointer->SonicSpawnDataUnlocks;
        
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.SeasideHill].Count; i++)
        {
            ptr->SeasideHillSpawn[i] = AllSpawnData[Team.Sonic][LevelId.SeasideHill][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.OceanPalace].Count; i++)
        {
            ptr->OceanPalaceSpawn[i] = AllSpawnData[Team.Sonic][LevelId.OceanPalace][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.GrandMetropolis].Count; i++)
        {
            ptr->GrandMetropolisSpawn[i] = AllSpawnData[Team.Sonic][LevelId.GrandMetropolis][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.PowerPlant].Count; i++)
        {
            ptr->PowerPlantSpawn[i] = AllSpawnData[Team.Sonic][LevelId.PowerPlant][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.CasinoPark].Count; i++)
        {
            ptr->CasinoParkSpawn[i] = AllSpawnData[Team.Sonic][LevelId.CasinoPark][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        for (int i = 0; i < AllSpawnData[Team.Sonic][LevelId.BingoHighway].Count; i++)
        {
            ptr->BingoHighwaySpawn[i] = AllSpawnData[Team.Sonic][LevelId.BingoHighway][i].Unlocked ? (byte)0x01 : (byte)0x00;
        }
        
    }

    public List<LevelSpawnEntry> GetAllSpawnDataForLevel(Team team, LevelId level)
    {
        if (!AllSpawnData.ContainsKey(team))
        {
            Console.WriteLine($"Team {team} does not have any spawn data.");
            return new () { };
        }
        if (!AllSpawnData[team].ContainsKey(level))
        {
            Console.WriteLine($"Team {team} does not have any spawn data for Level {level}.");
            return new () { };
        }
        
        return AllSpawnData[team][level];
    }
    

    public List<LevelSpawnEntry> GetUnlockedSpawnData(Team team, LevelId level)
    {
        if (!AllSpawnData.ContainsKey(team))
        {
            Console.WriteLine($"Team {team} does not have any spawn data.");
            return new () { };
        }
        if (!AllSpawnData[team].ContainsKey(level))
        {
            Console.WriteLine($"Team {team} does not have any spawn data for Level {level}.");
            return new () { };
        }

        if (ShouldIncludeSecret(team, level))
        {
            return AllSpawnData[team][level].Where(x => x.Unlocked).ToList();
        }
        
        return AllSpawnData[team][level].Where(x => x is { Unlocked: true, Secret: false }).ToList();
    }

    public void UnlockSpawnData(Team team, LevelId level, int index, bool secret = false)
    {
        if (!AllSpawnData.ContainsKey(team))
        {
            Console.WriteLine($"Team {team} does not have any spawn data.");
            return;
        }
        if (!AllSpawnData[team].ContainsKey(level))
        {
            Console.WriteLine($"Team {team} does not have any spawn data for Level {level}.");
            return;
        }
        
        LevelSpawnEntry entry = AllSpawnData[team][level].Where(x => x.Secret == secret).ToList()[index];
        entry.Unlocked = true;
        
        Console.WriteLine($"Unlocked spawn data for Team {team} and Level {level}. Pos is {entry.Pos}, Index in List is {AllSpawnData[team][level].IndexOf(entry)}, Index is {index}");
    }


    public unsafe void HandleInput(bool up)
    {
        var levelSelectPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var levelIndex = *(int*)(levelSelectPtr + 0x194);
        if (levelIndex is < 0 or > 21)
            return;
        
        var level = (LevelId)Mod.UserInterface!.LevelTracker.LevelMapping[levelIndex];
        var storyIndex = *(int*)(levelSelectPtr + 0x194 + 0x8C);
        
        var actPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var actIndex = *(int*)(actPtr + 0x2BC);
        
        var team = (Team)storyIndex;
        
        
        var entries = GetUnlockedSpawnData(team, level);
        var allentries = GetAllSpawnDataForLevel(team, level);

        if (entries.Count < 2)
        {
            Mod.LevelSpawnHandler!.SpawnPosIndex = 0;
            return;
        }

        if (!up)
        {
            int unlockedindex = entries.IndexOf(allentries[Mod.LevelSpawnHandler!.SpawnPosIndex]);
            
            if  (unlockedindex <= 0)
                unlockedindex = entries.Count;
            
            unlockedindex--;
            Console.WriteLine($"Spawn pos Index is: {Mod.LevelSpawnHandler.SpawnPosIndex}, Unlocked index is: {unlockedindex}");
            Mod.LevelSpawnHandler.SpawnPosIndex = allentries.IndexOf(entries[unlockedindex]);
        }

        else
        {
            int unlockedindex = entries.IndexOf(allentries[Mod.LevelSpawnHandler!.SpawnPosIndex]);
            
            if  (unlockedindex >= entries.Count - 1)
                unlockedindex = -1;
            
            unlockedindex++;
            Console.WriteLine($"Spawn pos Index is: {Mod.LevelSpawnHandler.SpawnPosIndex}, Unlocked index is: {unlockedindex}");
            Mod.LevelSpawnHandler.SpawnPosIndex = allentries.IndexOf(entries[unlockedindex]);
        }
    }

    public unsafe void PrintUnlockedSpawnData()
    {
        
        var levelSelectPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var levelIndex = *(int*)(levelSelectPtr + 0x194);
        if (levelIndex is < 0 or > 21)
        {
            Console.WriteLine($"Level {levelIndex} is out of range.");
        }
        
        var level = (LevelId)Mod.UserInterface!.LevelTracker.LevelMapping[levelIndex];
        var storyIndex = *(int*)(levelSelectPtr + 0x194 + 0x8C);
        
        var actPtr = *(IntPtr*)(Mod.ModuleBase + 0x6777B4);
        var actIndex = *(int*)(actPtr + 0x2BC);
        
        var team = (Team)storyIndex;
        
        var entries = GetUnlockedSpawnData(team, level);
        Console.WriteLine($"Unlocked spawn data for Team {team} and Level {level}. ");
        foreach (var entry in entries)
        {
            Console.WriteLine($"{entry}, ");
        }
    }
    
    

    public string GetLevelSelectUiText(Team team, LevelId level)
    {
        var unlockedSpawnEntries = GetUnlockedSpawnData(team, level);

        if (unlockedSpawnEntries.Count > 1)
        {
            if (Mod.LevelSpawnHandler!.SpawnPosIndex == 0)
            {
                return "Start of Level";
            }
            else
            {
                string result = $"Checkpoint: {Mod.LevelSpawnHandler!.SpawnPosIndex}";
                
                if (GetAllSpawnDataForLevel(team, level)[Mod.LevelSpawnHandler!.SpawnPosIndex].Secret)
                    result += $" SECRET!";
                return result;
            }
            
        }
        else
        {
            return "Start of Level";
        }
        
    }
    
    
    


}