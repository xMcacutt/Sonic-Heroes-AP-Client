

using System.Windows.Forms;

namespace Sonic_Heroes_AP_Client;

public class AbilityUnlockHandler
{
    private Dictionary<Team, TeamUnlockData> UnlockData =
        Enum.GetValues<Team>().ToDictionary(x => x, _ => new TeamUnlockData());

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

        public unsafe void ToSaveData(TeamProgUpgrade* pointer)
        {
            pointer->Speed = CharUnlocks[FormationChar.Speed];
            pointer->Flying =  CharUnlocks[FormationChar.Flying];
            pointer->Power = CharUnlocks[FormationChar.Power];
            
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
        if (currentProg >= SpeedAbility.TriangleJumpLightAttack)
        {
            Console.WriteLine($"Can not Increase Speed Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        SetAbilityUnlockForRegion(team, region, currentProg++);
    }
    
    public void IncrementFlyingAbilityForRegion(Team team, Region region)
    {
        FlyingAbility currentProg = UnlockData[team].AbilityUnlocks[region].FlyingLevel;
        
        int shouldHaveFlowerSting = team is Team.Chaotix ? 1 : 0;
        
        if (currentProg >= FlyingAbility.Flight + shouldHaveFlowerSting)
        {
            Console.WriteLine($"Can not Increase Flying Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        SetAbilityUnlockForRegion(team, region, currentProg++);
    }
    
    public void IncrementPowerAbilityForRegion(Team team, Region region)
    {
        PowerAbility currentProg = UnlockData[team].AbilityUnlocks[region].PowerLevel;
        if (currentProg >= PowerAbility.Combo)
        {
            Console.WriteLine($"Can not Increase Power Ability For Team {team} Region {region} as {currentProg} is the Max Value");
            return;
        }
        SetAbilityUnlockForRegion(team, region, currentProg++);
    }

    public void SetCharUnlock(Team team, FormationChar formationChar, bool unlock)
    {
        UnlockData[team].CharUnlocks[formationChar] = unlock;
        PollUpdates();
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
        
        Region region = GameHandler.LevelIdToRegion[levelId];

        if (team is Team.Sonic && act is Act.Act2 or Act.Act3
                               && Mod.ArchipelagoHandler!.SlotData.SuperHardModeSonicAct2)
            team = Team.SuperHardMode;
        

        AbilityHandler.SetSpeedProg((int)UnlockData[team].AbilityUnlocks[region].SpeedLevel);
        AbilityHandler.SetFlyingProg((int)UnlockData[team].AbilityUnlocks[region].FlyingLevel);
        AbilityHandler.SetPowerProg((int)UnlockData[team].AbilityUnlocks[region].PowerLevel);
        
        AbilityHandler.SetCharState(FormationChar.Speed, UnlockData[team].CharUnlocks[FormationChar.Speed]);
        AbilityHandler.SetCharState(FormationChar.Flying, UnlockData[team].CharUnlocks[FormationChar.Flying]);
        AbilityHandler.SetCharState(FormationChar.Power, UnlockData[team].CharUnlocks[FormationChar.Power]);
    }
}
