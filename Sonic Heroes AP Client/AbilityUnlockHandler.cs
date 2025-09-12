

using System.Windows.Forms;

namespace Sonic_Heroes_AP_Client;

public class AbilityUnlockHandler
{
    private Dictionary<Team, TeamUnlockData> UnlockData =
        Enum.GetValues<Team>().ToDictionary(x => x, _ => new TeamUnlockData());
    
    public Dictionary<Team, Dictionary<FormationChar, bool>> ShouldOverrideState = new ()
    {
        {
            Team.Sonic, new Dictionary<FormationChar, bool>()
            {
                { FormationChar.Speed, false },
                { FormationChar.Flying, false },
                { FormationChar.Power, false },
            }
        },
        {
            Team.Dark, new Dictionary<FormationChar, bool>()
            {
                { FormationChar.Speed, false },
                { FormationChar.Flying, false },
                { FormationChar.Power, false },
            }
        },
        {
            Team.Rose, new Dictionary<FormationChar, bool>()
            {
                { FormationChar.Speed, false },
                { FormationChar.Flying, false },
                { FormationChar.Power, false },
            }
        },
        {
            Team.Chaotix, new Dictionary<FormationChar, bool>()
            {
                { FormationChar.Speed, false },
                { FormationChar.Flying, false },
                { FormationChar.Power, false },
            }
        },
        {
            Team.SuperHardMode, new Dictionary<FormationChar, bool>()
            {
                { FormationChar.Speed, false },
                { FormationChar.Flying, false },
                { FormationChar.Power, false },
            }
        },
    };
    
    public unsafe void ToSaveData(CustomSaveData* pointer)
    {
        UnlockData[Team.Sonic].ToSaveData(&pointer->SonicProgUpgrade);
        UnlockData[Team.Dark].ToSaveData(&pointer->DarkProgUpgrade);
        UnlockData[Team.Rose].ToSaveData(&pointer->RoseProgUpgrade);
        UnlockData[Team.Chaotix].ToSaveData(&pointer->ChaotixProgUpgrade);
        UnlockData[Team.SuperHardMode].ToSaveData(&pointer->SuperHardProgUpgrade);
    }

    public unsafe void FromSaveData(CustomSaveData* pointer)
    {
        UnlockData[Team.Sonic].FromSaveData(&pointer->SonicProgUpgrade);
        UnlockData[Team.Dark].FromSaveData(&pointer->DarkProgUpgrade);
        UnlockData[Team.Rose].FromSaveData(&pointer->RoseProgUpgrade);
        UnlockData[Team.Chaotix].FromSaveData(&pointer->ChaotixProgUpgrade);
        UnlockData[Team.SuperHardMode].FromSaveData(&pointer->SuperHardProgUpgrade);
    }

    private class TeamUnlockData()
    {
        public readonly Dictionary<Region, AbilityUnlockData> AbilityUnlocks =
            Enum.GetValues<Region>().ToDictionary(x => x, _ => new AbilityUnlockData());

        public Dictionary<FormationChar, bool> CharUnlocks = new()
        {
            { FormationChar.Speed, false },
            { FormationChar.Flying, false },
            { FormationChar.Power, false }
        };

        public Dictionary<FormationChar, int> CharLevelUps = new()
        {
            { FormationChar.Speed, 0 },
            { FormationChar.Flying, 0 },
            { FormationChar.Power, 0 }
        };

        public unsafe void ToSaveData(TeamProgUpgrade* pointer)
        {
            pointer->Speed = CharUnlocks[FormationChar.Speed];
            pointer->Flying =  CharUnlocks[FormationChar.Flying];
            pointer->Power = CharUnlocks[FormationChar.Power];
            pointer->SpeedLevelUp =  (byte)CharLevelUps[FormationChar.Speed];
            pointer->FlyingLevelUp =  (byte)CharLevelUps[FormationChar.Flying];
            pointer->PowerLevelUp =  (byte)CharLevelUps[FormationChar.Power];
            
            AbilityUnlocks[Region.Ocean].ToSaveData(&pointer->Ocean);
            AbilityUnlocks[Region.HotPlant].ToSaveData(&pointer->HotPlant);
            AbilityUnlocks[Region.Casino].ToSaveData(&pointer->Casino);
            AbilityUnlocks[Region.Train].ToSaveData(&pointer->Train);
            AbilityUnlocks[Region.BigPlant].ToSaveData(&pointer->BigPlant);
            AbilityUnlocks[Region.Ghost].ToSaveData(&pointer->Ghost);
            AbilityUnlocks[Region.Sky].ToSaveData(&pointer->Sky);
        }

        public unsafe void FromSaveData(TeamProgUpgrade* pointer)
        {
            CharUnlocks[FormationChar.Speed] = pointer->Speed;
            CharUnlocks[FormationChar.Flying] = pointer->Flying;
            CharUnlocks[FormationChar.Power] = pointer->Power;
            CharLevelUps[FormationChar.Speed] = pointer->SpeedLevelUp;
            CharLevelUps[FormationChar.Flying] = pointer->FlyingLevelUp;
            CharLevelUps[FormationChar.Power] = pointer->PowerLevelUp;
            
            AbilityUnlocks[Region.Ocean].FromSaveData(&pointer->Ocean);
            AbilityUnlocks[Region.HotPlant].FromSaveData(&pointer->HotPlant);
            AbilityUnlocks[Region.Casino].FromSaveData(&pointer->Casino);
            AbilityUnlocks[Region.Train].FromSaveData(&pointer->Train);
            AbilityUnlocks[Region.BigPlant].FromSaveData(&pointer->BigPlant);
            AbilityUnlocks[Region.Ghost].FromSaveData(&pointer->Ghost);
            AbilityUnlocks[Region.Sky].FromSaveData(&pointer->Sky);
        }
    }

    private class AbilityUnlockData()
    {
        public SpeedAbility SpeedLevel = SpeedAbility.None;
        public FlyingAbility FlyingLevel = FlyingAbility.None;
        public PowerAbility PowerLevel = PowerAbility.None;

        public unsafe void ToSaveData(ProgUpgrade* pointer)
        {
            pointer->ProgSpeed = (byte)SpeedLevel;
            pointer->ProgFlying = (byte)FlyingLevel;
            pointer->ProgPower = (byte)PowerLevel;
        }

        public unsafe void FromSaveData(ProgUpgrade* pointer)
        {
            SpeedLevel = (SpeedAbility)pointer->ProgSpeed;
            FlyingLevel = (FlyingAbility)pointer->ProgFlying;
            PowerLevel = (PowerAbility)pointer->ProgPower;
        }
        
    }

    public void SetAbilityUnlockForRegion(Team team, Region region, SpeedAbility speedAbility)
    {
        UnlockData[team].AbilityUnlocks[region].SpeedLevel = speedAbility;
        PollUpdates();
    }

    public void SetAbilityUnlockForRegion(Team team, Region region, FlyingAbility flyingAbility)
    {
        UnlockData[team].AbilityUnlocks[region].FlyingLevel = flyingAbility;
        PollUpdates();
    }

    public void SetAbilityUnlockForRegion(Team team, Region region, PowerAbility powerAbility)
    {
        UnlockData[team].AbilityUnlocks[region].PowerLevel = powerAbility;
        PollUpdates();
    }


    public void IncrementSpeedAbilityForRegion(Team team, Region region)
    {
        SpeedAbility currentProg = UnlockData[team].AbilityUnlocks[region].SpeedLevel;
        SpeedAbility nextProg = currentProg + 1;
        
        if (currentProg >= SpeedAbility.TriangleJumpLightAttack)
        {
            Console.WriteLine($"Can not Increase Speed Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        Console.WriteLine($"Incrementing Speed Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }
    
    public void IncrementFlyingAbilityForRegion(Team team, Region region)
    {
        FlyingAbility currentProg = UnlockData[team].AbilityUnlocks[region].FlyingLevel;
        FlyingAbility nextProg = currentProg + 1;
        
        int shouldHaveFlowerSting = team is Team.Chaotix ? 1 : 0;
        
        if (currentProg >= FlyingAbility.Flight + shouldHaveFlowerSting)
        {
            Console.WriteLine($"Can not Increase Flying Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        Console.WriteLine($"Incrementing Flying Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }
    
    public void IncrementPowerAbilityForRegion(Team team, Region region)
    {
        PowerAbility currentProg = UnlockData[team].AbilityUnlocks[region].PowerLevel;
        PowerAbility nextProg = currentProg + 1;
        
        if (currentProg >= PowerAbility.Combo)
        {
            Console.WriteLine($"Can not Increase Power Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        Console.WriteLine($"Incrementing Power Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }

    public void SetCharUnlock(Team team, FormationChar formationChar, bool unlock)
    {
        UnlockData[team].CharUnlocks[formationChar] = unlock;
        Console.WriteLine($"Unlocking Team {team} Character {formationChar} with {unlock}");
        ShouldOverrideState[team][formationChar] = true;
        PollUpdates();
    }
    
    public bool GetCharUnlock(Team team, FormationChar formationChar)
    {
        return UnlockData[team].CharUnlocks[formationChar];
    }

    public bool HasAllAbilitiesandCharsForTeam(Team team)
    {
        if (!(GetCharUnlock(team, FormationChar.Speed) && GetCharUnlock(team, FormationChar.Flying) &&
              GetCharUnlock(team, FormationChar.Power)))
        {
            Console.WriteLine($"Final Boss Requires All Characters");
            return false;
        }

        if (!(HasAllAbilitiesForRegion(team, Region.Ocean) && HasAllAbilitiesForRegion(team, Region.HotPlant) && HasAllAbilitiesForRegion(team, Region.Casino)))
            return false;
        return true;
    }

    public bool HasAllAbilitiesForRegion(Team team, Region region)
    {
        int shouldHaveFlowerSting = team is Team.Chaotix ? 1 : 0;
        
        if (!(UnlockData[team].AbilityUnlocks[region].SpeedLevel >= SpeedAbility.TriangleJumpLightAttack &&
              UnlockData[team].AbilityUnlocks[region].FlyingLevel >= FlyingAbility.Flight + shouldHaveFlowerSting &&
              UnlockData[team].AbilityUnlocks[region].PowerLevel >= PowerAbility.Combo))
        {
            Console.WriteLine($"Final Boss Requires All Abilities for Team {team} and Region {region}");
            return false;
        }

        return true;
    }


    public string GetLevelSelectUIStringForAbilityUnlocks(Team team, Region region, FormationChar formationChar)
    {
        int currentChecks = 0;
        int maxChecks = 0;
        
        int shouldHaveFlowerSting = team is Team.Chaotix ? 1 : 0;

        switch (formationChar)
        {
            case FormationChar.Speed:
                currentChecks = (int)UnlockData[team].AbilityUnlocks[region].SpeedLevel;
                maxChecks = (int)SpeedAbility.TriangleJumpLightAttack;
                break;
            case FormationChar.Flying:
                currentChecks = (int)UnlockData[team].AbilityUnlocks[region].FlyingLevel;
                maxChecks = (int)(FlyingAbility.Flight + shouldHaveFlowerSting);
                break;
            case FormationChar.Power:
                currentChecks = (int)UnlockData[team].AbilityUnlocks[region].PowerLevel;
                maxChecks = (int)PowerAbility.Combo;
                break;
        }

        return $"{currentChecks} / {maxChecks}";

    }

    public string GetLevelSelectUIStringForCharUnlocks(Team team)
    {
        string result = "";

        if (UnlockData[team].CharUnlocks[FormationChar.Speed])
        {
            result += $" {GameHandler.CharacterNames[team][FormationChar.Speed]}";
        }
        if (UnlockData[team].CharUnlocks[FormationChar.Flying])
        {
            result += $" {GameHandler.CharacterNames[team][FormationChar.Flying]}";
        }
        if (UnlockData[team].CharUnlocks[FormationChar.Power])
        {
            result += $" {GameHandler.CharacterNames[team][FormationChar.Power]}";
        }
        
        return result;
    }


    public string GetLevelSelectUIStringForFinalBossCharUnlocks()
    {
        int charsUnlocked = 0;
        int totalCharsNeeded = 0;

        if (Mod.ArchipelagoHandler!.SlotData.StoriesActive[Team.Sonic] > 0)
        {
            if (UnlockData[Team.Sonic].CharUnlocks[FormationChar.Speed])
                charsUnlocked++;
            if (UnlockData[Team.Sonic].CharUnlocks[FormationChar.Flying])
                charsUnlocked++;
            if (UnlockData[Team.Sonic].CharUnlocks[FormationChar.Power])
                charsUnlocked++;
            
            totalCharsNeeded += 3;
        }
        
        return $"{charsUnlocked} / {totalCharsNeeded}";
    }


    public string GetLevelSelectUIStringForFinalBossAbilityUnlocks()
    {
        int abilitiesUnlocked = 0;
        int totalAbiltiesNeeded = 0;

        if (Mod.ArchipelagoHandler!.SlotData.StoriesActive[Team.Sonic] > 0)
        {
            foreach (var region in UnlockData[Team.Sonic].AbilityUnlocks.Keys.Where(region => region <= Region.Train))
            {
                abilitiesUnlocked += (int)UnlockData[Team.Sonic].AbilityUnlocks[region].SpeedLevel;
                abilitiesUnlocked += (int)UnlockData[Team.Sonic].AbilityUnlocks[region].FlyingLevel;
                abilitiesUnlocked += (int)UnlockData[Team.Sonic].AbilityUnlocks[region].PowerLevel;
                
                totalAbiltiesNeeded += 9; //4 speed 2 flying 3 power
            }
        }
        
        return $"{abilitiesUnlocked} / {totalAbiltiesNeeded}";
    }


    public void IncrementLevelUpMax(Team team, FormationChar formationChar)
    {
        UnlockData[team].CharLevelUps[formationChar]++;

        if (UnlockData[team].CharLevelUps[formationChar] > 3)
            UnlockData[team].CharLevelUps[formationChar] = 3;
    }


    public unsafe void HandleLevelUp(Team team, Region region, FormationChar formationChar)
    {
        if (!Mod.GameHandler!.InGame())
            return;
        
        var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64C268);
        var charlevels = (byte*)(baseAddress + 0x208 + (byte)formationChar);

        if (*charlevels > UnlockData[team].CharLevelUps[formationChar])
        {
            
            Console.WriteLine($"Level Up for Character {formationChar} is over max allowed value of {UnlockData[team].CharLevelUps[formationChar]}");
            
            *charlevels = (byte)UnlockData[team].CharLevelUps[formationChar];
        }
    }
    
    

    public void PollUpdates()
    {
        if (!Mod.GameHandler!.InGame())
            return;

        Team team = Mod.GameHandler.GetCurrentStory();
        Act act = Mod.GameHandler.GetCurrentAct();
        LevelId levelId = Mod.GameHandler.GetCurrentLevel();

        if (!GameHandler.LevelIdToRegion.ContainsKey(levelId))
            return;
        
        Console.WriteLine($"Running Poll Updates");
        
        Region region = GameHandler.LevelIdToRegion[levelId];

        if (team is Team.Sonic && act is Act.Act2 or Act.Act3
                               && Mod.ArchipelagoHandler!.SlotData.SuperHardModeSonicAct2)
        {
            team = Team.SuperHardMode;
            Console.WriteLine($"Team is Super Hard");

            if (UnlockData.ContainsKey(team))
            {
                Console.WriteLine($"UnlockData.ContainsKey(team)");
                if (UnlockData[team].AbilityUnlocks.ContainsKey(region))
                {
                    Console.WriteLine($"UnlockData[team].AbilityUnlocks.ContainsKey(region)");
                    Console.WriteLine($"{UnlockData[team].AbilityUnlocks[region].SpeedLevel}");
                }
            }
            return;
        }

        Console.WriteLine($"Setting Speed Here");
        AbilityHandler.SetSpeedProg((int)UnlockData[team].AbilityUnlocks[region].SpeedLevel);
        AbilityHandler.SetFlyingProg((int)UnlockData[team].AbilityUnlocks[region].FlyingLevel);
        AbilityHandler.SetPowerProg((int)UnlockData[team].AbilityUnlocks[region].PowerLevel);
        
        AbilityHandler.SetCharState(FormationChar.Speed, UnlockData[team].CharUnlocks[FormationChar.Speed], ShouldOverrideState[team][FormationChar.Speed]);
        AbilityHandler.SetCharState(FormationChar.Flying, UnlockData[team].CharUnlocks[FormationChar.Flying], ShouldOverrideState[team][FormationChar.Flying]);
        AbilityHandler.SetCharState(FormationChar.Power, UnlockData[team].CharUnlocks[FormationChar.Power], ShouldOverrideState[team][FormationChar.Power]);

        AbilityHandler.SetCharLevel(FormationChar.Speed, (byte)UnlockData[team].CharLevelUps[FormationChar.Speed]);
        AbilityHandler.SetCharLevel(FormationChar.Flying, (byte)UnlockData[team].CharLevelUps[FormationChar.Flying]);
        AbilityHandler.SetCharLevel(FormationChar.Power, (byte)UnlockData[team].CharLevelUps[FormationChar.Power]);
        
        
        ShouldOverrideState[team][FormationChar.Speed] = false;
        ShouldOverrideState[team][FormationChar.Flying] = false;
        ShouldOverrideState[team][FormationChar.Power] = false;
        
        if (team is not Team.Sonic || levelId > LevelId.BulletStation)
            UnlockAll();
        
        

        if (UnlockData[team].CharUnlocks[FormationChar.Speed] && UnlockData[team].CharUnlocks[FormationChar.Flying] &&
            UnlockData[team].CharUnlocks[FormationChar.Power])
        {
            Console.WriteLine($"Team Blast is allowed");
            Mod.ArchipelagoHandler!.SlotData.TeamBlastWrite = true;
        }
        else
        {
            Console.WriteLine($"Team Blast is not allowed");
            Mod.ArchipelagoHandler!.SlotData.TeamBlastWrite = false;
        }
    }

    public void UnlockAll()
    {
        AbilityHandler.SetSpeedProg(4);
        AbilityHandler.SetFlyingProg(3);
        AbilityHandler.SetPowerProg(3);
        
        AbilityHandler.SetCharState(FormationChar.Speed, true, true);
        AbilityHandler.SetCharState(FormationChar.Flying, true, true);
        AbilityHandler.SetCharState(FormationChar.Power, true, true);
        
        AbilityHandler.SetCharLevel(FormationChar.Speed, 3);
        AbilityHandler.SetCharLevel(FormationChar.Flying, 3);
        AbilityHandler.SetCharLevel(FormationChar.Power, 3);
    }
    
    
}
