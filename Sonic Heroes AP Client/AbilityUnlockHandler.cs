

using System.Windows.Forms;

namespace Sonic_Heroes_AP_Client;

public class AbilityUnlockHandler
{
    
    public readonly Dictionary<Team, Dictionary<FormationChar, bool>> ShouldOverrideState = new ()
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


    public bool CanTeamBlast(Team team, Region region, bool force)
    {
        if (force)
            return true;
        
        bool hasChars = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Speed] && Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Flying] && Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Power];

        bool hasAbilities = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel >= Enum.GetValues<SpeedAbility>().Max() && Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel >= Enum.GetValues<FlyingAbility>().Max() && Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel >= Enum.GetValues<PowerAbility>().Max();
        
        return hasChars && hasAbilities;
    }

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


    public void IncrementSpeedAbilityForAllRegions(Team team)
    {
        IncrementSpeedAbilityForRegion(team, Region.Ocean);
        IncrementSpeedAbilityForRegion(team, Region.HotPlant);
        IncrementSpeedAbilityForRegion(team, Region.Casino);
        IncrementSpeedAbilityForRegion(team, Region.Train);
        IncrementSpeedAbilityForRegion(team, Region.BigPlant);
        IncrementSpeedAbilityForRegion(team, Region.Ghost);
        IncrementSpeedAbilityForRegion(team, Region.Sky);
    }


    public void IncrementSpeedAbilityForRegion(Team team, Region region)
    {
        SpeedAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
        if (currentProg >= Enum.GetValues<SpeedAbility>().Max())
        {
            Console.WriteLine($"Can not Increase Speed Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        SpeedAbility nextProg = currentProg + 1;
        Console.WriteLine($"Incrementing Speed Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }
    
    public void IncrementFlyingAbilityForAllRegions(Team team)
    {
        IncrementFlyingAbilityForRegion(team, Region.Ocean);
        IncrementFlyingAbilityForRegion(team, Region.HotPlant);
        IncrementFlyingAbilityForRegion(team, Region.Casino);
        IncrementFlyingAbilityForRegion(team, Region.Train);
        IncrementFlyingAbilityForRegion(team, Region.BigPlant);
        IncrementFlyingAbilityForRegion(team, Region.Ghost);
        IncrementFlyingAbilityForRegion(team, Region.Sky);
    }
    
    public void IncrementFlyingAbilityForRegion(Team team, Region region)
    {
        FlyingAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
        
        if (currentProg >= Enum.GetValues<FlyingAbility>().Max())
        {
            Console.WriteLine($"Can not Increase Flying Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        FlyingAbility nextProg = currentProg + 1;
        Console.WriteLine($"Incrementing Flying Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }
    
    public void IncrementPowerAbilityForAllRegions(Team team)
    {
        IncrementPowerAbilityForRegion(team, Region.Ocean);
        IncrementPowerAbilityForRegion(team, Region.HotPlant);
        IncrementPowerAbilityForRegion(team, Region.Casino);
        IncrementPowerAbilityForRegion(team, Region.Train);
        IncrementPowerAbilityForRegion(team, Region.BigPlant);
        IncrementPowerAbilityForRegion(team, Region.Ghost);
        IncrementPowerAbilityForRegion(team, Region.Sky);
    }
    
    public void IncrementPowerAbilityForRegion(Team team, Region region)
    {
        PowerAbility currentProg = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;
        if (currentProg >= Enum.GetValues<PowerAbility>().Max())
        {
            Console.WriteLine($"Can not Increase Power Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        PowerAbility nextProg = currentProg + 1;
        Console.WriteLine($"Incrementing Power Ability. Current is: {currentProg} New is {nextProg} Team {team} Region {region}");
        SetAbilityUnlockForRegion(team, region, nextProg);
    }

    public void SetCharUnlock(Team team, FormationChar formationChar, bool unlock)
    {
        Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[formationChar] = unlock;
        Console.WriteLine($"Unlocking Team {team} Character {formationChar} with {unlock}");
        ShouldOverrideState[team][formationChar] = true;
        PollUpdates();
        StageObjHandler.HandleObjSpawningWhenReceivingCharItem(Team.Sonic, FormationChar.Speed, unlock);
    }
    
    public bool GetCharUnlock(Team team, FormationChar formationChar)
    {
        return Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[formationChar];
    }

    public bool HasAllAbilitiesandCharsandLevelUpsForTeam(Team team)
    {
        if (!(GetCharUnlock(team, FormationChar.Speed) && GetCharUnlock(team, FormationChar.Flying) &&
              GetCharUnlock(team, FormationChar.Power)))
        {
            Console.WriteLine($"Final Boss Requires All Characters");
            return false;
        }

        foreach (var reg in Enum.GetValues<Region>().Where(reg => reg < Region.Sky))
        {
            if (!HasAllAbilitiesForRegion(team, reg))
            {
                Console.WriteLine($"Final Boss Requires All Abilities for Team {team} and Region {reg}");
                return false;
            }
           
        }
        return true;
    }

    public bool HasAllAbilitiesForRegion(Team team, Region region)
    {
        
        if (!(Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel >= (SpeedAbility)Enum.GetValues(typeof(SpeedAbility)).Cast<int>().Max() &&
              Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel >= (FlyingAbility)Enum.GetValues(typeof(FlyingAbility)).Cast<int>().Max() &&
              Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel >= (PowerAbility)Enum.GetValues(typeof(PowerAbility)).Cast<int>().Max()))
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

        switch (formationChar)
        {
            case FormationChar.Speed:
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
                maxChecks = Enum.GetValues(typeof(SpeedAbility)).Cast<int>().Max();
                break;
            case FormationChar.Flying:
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
                maxChecks = Enum.GetValues(typeof(FlyingAbility)).Cast<int>().Max();
                break;
            case FormationChar.Power:
                currentChecks = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;
                maxChecks = Enum.GetValues(typeof(PowerAbility)).Cast<int>().Max();
                break;
        }

        if (Mod.ArchipelagoHandler!.SlotData.AbilityUnlocks is AbilityUnlockType.AllRegionsSeparate)
        {
            return $"Progressive {GameHandler.CharacterNames[team][formationChar]} {region} Region: {currentChecks} / {maxChecks}";

        }
        
        return $"Progressive {GameHandler.CharacterNames[team][formationChar]}: {currentChecks} / {maxChecks}";
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

        if (Mod.ArchipelagoHandler.SlotData.AbilityUnlocks is AbilityUnlockType.AllRegionsSeparate)
        {

            foreach (var team in Mod.ArchipelagoHandler.SlotData.StoriesActive.Keys)
            {
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[team] <= 0) 
                    continue;
                foreach (var region in Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks.Keys.Where(region => region <= Region.Ghost))
                {
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
                
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
                
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;
                
                    totalAbiltiesNeeded += 9; //3 speed 3 flying 3 power
                }

            }

            //Handle SuperHard Here
            if (Mod.ArchipelagoHandler.SlotData.SuperHardModeSonicAct2 &&
                Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Sonic] is MissionsActive.Act2 or MissionsActive.Both)
            {
                foreach (var region in Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks.Keys.Where(region => region <= Region.Ghost))
                {
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[region].SpeedLevel;
                
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[region].FlyingLevel;
                
                    abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[region].PowerLevel;
                
                    totalAbiltiesNeeded += 9;
                }
            }
        }


        else
        {
            
            foreach (var team in Mod.ArchipelagoHandler.SlotData.StoriesActive.Keys)
            {
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[team] <= 0) 
                    continue;

                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[Region.Ocean].SpeedLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[Region.Ocean].FlyingLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.Sonic].AbilityUnlocks[Region.Ocean].PowerLevel;
                
                totalAbiltiesNeeded += 9;
                

            }
            //Handle SuperHard Here
            if (Mod.ArchipelagoHandler.SlotData.SuperHardModeSonicAct2 &&
                Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Sonic] is MissionsActive.Act2 or MissionsActive.Both)
            {
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[Region.Ocean].SpeedLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[Region.Ocean].FlyingLevel;
                
                abilitiesUnlocked += (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[Team.SuperHardMode].AbilityUnlocks[Region.Ocean].PowerLevel;
                
                totalAbiltiesNeeded += 9;
            }
        }
        
        return $"{abilitiesUnlocked} / {totalAbiltiesNeeded}";
    }


    public unsafe void HandleLevelUp(Team team, Region region, FormationChar formationChar)
    {
        if (!Mod.GameHandler!.InGame())
            return;
        
        var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64C268);
        var charlevels = (byte*)(baseAddress + 0x208 + (byte)formationChar);
        
        switch (formationChar)
        {
            case FormationChar.Speed:
            {
                var speedMax = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;

                if (*charlevels > speedMax)
                {
                    Console.WriteLine($"Level Up for Character {formationChar} is over max allowed value of {Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel}");
                    *charlevels = (byte)speedMax;
                }

                break;
            }
            case FormationChar.Flying:
            {
                var flyingMax = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;

                if (*charlevels > flyingMax)
                {
                    Console.WriteLine($"Level Up for Character {formationChar} is over max allowed value of {Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel}");
                    *charlevels = (byte)flyingMax;
                }

                break;
            }
            case FormationChar.Power:
            {
                var powerMax = (int)Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;

                if (*charlevels > powerMax)
                {
                    Console.WriteLine($"Level Up for Character {formationChar} is over max allowed value of {Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel}");
                    *charlevels = (byte)powerMax;
                }

                break;
            }
        }
    }
    
    

    public void PollUpdates()
    {
        if (!Mod.GameHandler!.InGame())
            return;

        if (!ArchipelagoHandler.IsConnected)
        {
            Console.WriteLine($"Not Connected in PollUpdates. Aborting");
            return;
        }
        
        Team team = Mod.GameHandler.GetCurrentStory();
        Act act = Mod.GameHandler.GetCurrentAct();
        LevelId levelId = Mod.GameHandler.GetCurrentLevel();
        
        Console.WriteLine($"Running Poll Updates");

        if (!GameHandler.LevelIdToRegion.ContainsKey(levelId))
        {
            Console.WriteLine($"LevelId {levelId} does not exist in Region Mapping");
            return;
        }
        
        Region region = GameHandler.LevelIdToRegion[levelId];
        
        

        if (team is Team.Sonic && act is Act.Act2 or Act.Act3
                               && Mod.ArchipelagoHandler!.SlotData.SuperHardModeSonicAct2)
        {
            team = Team.SuperHardMode;
            Console.WriteLine($"Team is Super Hard");
        }
        Console.WriteLine($"Poll Updates is Updating Game Here");


        bool speedChar = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Speed];
        bool flyingChar = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Flying];
        bool powerChar = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].CharsUnlocked[FormationChar.Power];


        SpeedAbility speedLevel = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].SpeedLevel;
        FlyingAbility flyingLevel = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].FlyingLevel;
        PowerAbility powerLevel = Mod.SaveDataHandler!.CustomSaveData!.UnlockSaveData[team].AbilityUnlocks[region].PowerLevel;

        bool forceTeamBlastEnable = false;


        if (team is not Team.Sonic || region > Region.Sky)
        {
            //UnlockAll();
            speedChar = true;
            flyingChar = true;
            powerChar = true;
            
            speedLevel = Enum.GetValues<SpeedAbility>().Max();
            flyingLevel = Enum.GetValues<FlyingAbility>().Max();
            powerLevel = Enum.GetValues<PowerAbility>().Max();
            
            ShouldOverrideState[team][FormationChar.Speed] = true;
            ShouldOverrideState[team][FormationChar.Flying] = true;
            ShouldOverrideState[team][FormationChar.Power] = true;
            
            forceTeamBlastEnable = true;
        }

        if (team is Team.Sonic && region is Region.Sky)
        {
            speedLevel = Enum.GetValues<SpeedAbility>().Max();
            flyingLevel = Enum.GetValues<FlyingAbility>().Max();
            powerLevel = Enum.GetValues<PowerAbility>().Max();
        }
        
        AbilityHandler.HandleSpeedProg(speedLevel);
        AbilityHandler.HandleFlyingProg(flyingLevel);
        AbilityHandler.HandlePowerProg(powerLevel);
        
        AbilityHandler.SetCharState(FormationChar.Speed, speedChar, ShouldOverrideState[team][FormationChar.Speed]);
        AbilityHandler.SetCharState(FormationChar.Flying, flyingChar, ShouldOverrideState[team][FormationChar.Flying]);
        AbilityHandler.SetCharState(FormationChar.Power, powerChar, ShouldOverrideState[team][FormationChar.Power]);

        if (CanTeamBlast(team, region, forceTeamBlastEnable))
        {
            Console.WriteLine($"Team Blast is allowed");
            Mod.ArchipelagoHandler!.SlotData.TeamBlastWrite = true;
        }
        else
        {
            Console.WriteLine($"Team Blast is not allowed");
            Mod.ArchipelagoHandler!.SlotData.TeamBlastWrite = false;
        }
        
        ShouldOverrideState[team][FormationChar.Speed] = false;
        ShouldOverrideState[team][FormationChar.Flying] = false;
        ShouldOverrideState[team][FormationChar.Power] = false;
    }
}
