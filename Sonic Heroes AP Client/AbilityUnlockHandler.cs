

using System.Windows.Forms;

namespace Sonic_Heroes_AP_Client;

public class AbilityUnlockHandler
{
    
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

    public void SetAbilityUnlockForRegion(Team team, Region region, SpeedAbility speedAbility)
    {
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel = speedAbility;
        PollUpdates();
    }

    public void SetAbilityUnlockForRegion(Team team, Region region, FlyingAbility flyingAbility)
    {
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel = flyingAbility;
        PollUpdates();
    }

    public void SetAbilityUnlockForRegion(Team team, Region region, PowerAbility powerAbility)
    {
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel = powerAbility;
        PollUpdates();
    }


    public void IncrementSpeedAbilityForRegion(Team team, Region region)
    {
        SpeedAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
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
        FlyingAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
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
        PowerAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;
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
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[formationChar] = unlock;
        Console.WriteLine($"Unlocking Team {team} Character {formationChar} with {unlock}");
        ShouldOverrideState[team][formationChar] = true;
        PollUpdates();
    }
    
    public bool GetCharUnlock(Team team, FormationChar formationChar)
    {
        return Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[formationChar];
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
        
        if (!(Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel >= SpeedAbility.TriangleJumpLightAttack &&
              Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel >= FlyingAbility.Flight + shouldHaveFlowerSting &&
              Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel >= PowerAbility.Combo))
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
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
                maxChecks = (int)SpeedAbility.TriangleJumpLightAttack;
                break;
            case FormationChar.Flying:
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
                maxChecks = (int)(FlyingAbility.Flight + shouldHaveFlowerSting);
                break;
            case FormationChar.Power:
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;
                maxChecks = (int)PowerAbility.Combo;
                break;
        }

        return $"{currentChecks} / {maxChecks}";

    }

    public string GetLevelSelectUIStringForCharUnlocks(Team team)
    {
        string result = "";

        if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Speed])
        {
            result += $" {GameHandler.CharacterNames[team][FormationChar.Speed]}";
        }
        if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Flying])
        {
            result += $" {GameHandler.CharacterNames[team][FormationChar.Flying]}";
        }
        if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Power])
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
            if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].CharsUnlocked[FormationChar.Speed])
                charsUnlocked++;
            if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].CharsUnlocked[FormationChar.Flying])
                charsUnlocked++;
            if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].CharsUnlocked[FormationChar.Power])
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
            foreach (var region in Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks.Keys.Where(region => region <= Region.Train))
            {
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[region].SpeedLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[region].FlyingLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[region].PowerLevel;
                
                totalAbiltiesNeeded += 9; //4 speed 2 flying 3 power
            }
        }
        
        return $"{abilitiesUnlocked} / {totalAbiltiesNeeded}";
    }


    public void IncrementLevelUpMax(Team team, FormationChar formationChar)
    {
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar]++;

        if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar] > 3)
            Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar] = 3;
    }


    public unsafe void HandleLevelUp(Team team, Region region, FormationChar formationChar)
    {
        if (!Mod.GameHandler!.InGame())
            return;
        
        var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64C268);
        var charlevels = (byte*)(baseAddress + 0x208 + (byte)formationChar);

        if (*charlevels > Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar])
        {
            
            Console.WriteLine($"Level Up for Character {formationChar} is over max allowed value of {Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar]}");
            
            *charlevels = (byte)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[formationChar];
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

            if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData.ContainsKey(team))
            {
                Console.WriteLine($"UnlockData.ContainsKey(team)");
                if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks.ContainsKey(region))
                {
                    Console.WriteLine($"Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks.ContainsKey(region)");
                    Console.WriteLine($"{Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel}");
                }
            }
            return;
        }

        Console.WriteLine($"Setting Speed Here");
        AbilityHandler.SetSpeedProg((int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel);
        AbilityHandler.SetFlyingProg((int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel);
        AbilityHandler.SetPowerProg((int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel);
        
        AbilityHandler.SetCharState(FormationChar.Speed, Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Speed], ShouldOverrideState[team][FormationChar.Speed]);
        AbilityHandler.SetCharState(FormationChar.Flying, Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Flying], ShouldOverrideState[team][FormationChar.Flying]);
        AbilityHandler.SetCharState(FormationChar.Power, Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Power], ShouldOverrideState[team][FormationChar.Power]);

        AbilityHandler.SetCharLevel(FormationChar.Speed, (byte)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[FormationChar.Speed]);
        AbilityHandler.SetCharLevel(FormationChar.Flying, (byte)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[FormationChar.Flying]);
        AbilityHandler.SetCharLevel(FormationChar.Power, (byte)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharLevelUps[FormationChar.Power]);
        
        
        ShouldOverrideState[team][FormationChar.Speed] = false;
        ShouldOverrideState[team][FormationChar.Flying] = false;
        ShouldOverrideState[team][FormationChar.Power] = false;
        
        if (team is not Team.Sonic || levelId > LevelId.BulletStation)
            UnlockAll();
        
        

        if (Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Speed] && Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Flying] &&
            Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Power])
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
