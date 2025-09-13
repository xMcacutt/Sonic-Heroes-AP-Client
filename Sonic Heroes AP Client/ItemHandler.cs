using Archipelago.MultiClient.Net.Models;

namespace Sonic_Heroes_AP_Client;

public enum SHItem
{
    Emblem,
    GreenChaosEmerald,
    BlueChaosEmerald,
    YellowChaosEmerald,
    WhiteChaosEmerald,
    CyanChaosEmerald,
    PurpleChaosEmerald,
    RedChaosEmerald,
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
    ProgressiveSpeedSonicOceanRegion,
    ProgressiveFlyingSonicOceanRegion,
    ProgressivePowerSonicOceanRegion,
    ProgressiveSpeedSonicHotPlantRegion,
    ProgressiveFlyingSonicHotPlantRegion,
    ProgressivePowerSonicHotPlantRegion,
    ProgressiveSpeedSonicCasinoRegion,
    ProgressiveFlyingSonicCasinoRegion,
    ProgressivePowerSonicCasinoRegion,
    ProgressiveSpeedSonicTrainRegion,
    ProgressiveFlyingSonicTrainRegion,
    ProgressivePowerSonicTrainRegion,
    ProgressiveSpeedSonicBigPlantRegion,
    ProgressiveFlyingSonicBigPlantRegion,
    ProgressivePowerSonicBigPlantRegion,
    ProgressiveSpeedSonicGhostRegion,
    ProgressiveFlyingSonicGhostRegion,
    ProgressivePowerSonicGhostRegion,
    ProgressiveSpeedSonicSkyRegion,
    ProgressiveFlyingSonicSkyRegion,
    ProgressivePowerSonicSkyRegion,
    
    ProgressiveLevelUpSonic = 0xB0,
    ProgressiveLevelUpTails,
    ProgressiveLevelUpKnuckles,
    
    
    
    ExtraLife = 0xC0,
    FiveRings,
    TenRings,
    TwentyRings,
    Shield,
    Invincibility,
    SpeedLevelUp,
    FlyLevelUp,
    PowerLevelUp,
    TeamLevelUp,
    TeamBlastFiller,
    
    StealthTrap = 0x100,
    FreezeTrap,
    NoSwapTrap,
    RingTrap,
    CharmyTrap,
}




public class ItemHandler
{
    private readonly Queue<SHItem> cachedItems;

    public ItemHandler()
    {
        cachedItems = new Queue<SHItem>();    
    }
    
    public unsafe void HandleItem(int index, ItemInfo item)
    {
        if (index < Mod.SaveDataHandler.CustomData->LastItemIndex)
            return;

        //Console.WriteLine($"{index} : {Mod.SaveDataHandler.CustomData->LastItemIndex}");
        Mod.SaveDataHandler.CustomData->LastItemIndex++;
        
        //Console.WriteLine($"Item received: {item.ItemName}");
        
        var handled = true;
        var itemName = item.ItemName;
        var itemId = (SHItem)(item.ItemId - 0x93930000);
        bool unlocked = false;

        switch (itemId)
        {
            case SHItem.Emblem:
                Mod.SaveDataHandler!.CustomData->EmblemCount++;
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE016);
                break;
            case SHItem.GreenChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[0] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[3] = 1;
                break;
            case SHItem.BlueChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[1] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[6] = 1;
                break;
            case SHItem.YellowChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[2] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[9] = 1;
                break;
            case SHItem.WhiteChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[3] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[12] = 1;
                break;
            case SHItem.CyanChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[4] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[15] = 1;
                break;
            case SHItem.PurpleChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[5] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[18] = 1;
                break;
            case SHItem.RedChaosEmerald:
                Mod.SaveDataHandler!.CustomData->Emeralds[6] = 1;
                Mod.SaveDataHandler!.RedirectData->Emerald[21] = 1;
                break;
            case SHItem.PlayableSonic:
                unlocked = Mod.AbilityUnlockHandler!.GetCharUnlock(Team.Sonic, FormationChar.Speed);
                Mod.AbilityUnlockHandler!.SetCharUnlock(Team.Sonic, FormationChar.Speed, !unlocked);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.PlayableTails:
                unlocked = Mod.AbilityUnlockHandler!.GetCharUnlock(Team.Sonic, FormationChar.Flying);
                Mod.AbilityUnlockHandler!.SetCharUnlock(Team.Sonic, FormationChar.Flying, !unlocked);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.PlayableKnuckles:
                unlocked = Mod.AbilityUnlockHandler!.GetCharUnlock(Team.Sonic, FormationChar.Power);
                Mod.AbilityUnlockHandler!.SetCharUnlock(Team.Sonic, FormationChar.Power, !unlocked);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSpeedSonicSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveFlyingSonicSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressivePowerSonicSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveLevelUpSonic:
                Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Speed);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                Mod.AbilityUnlockHandler.PollUpdates();
                break;
            case SHItem.ProgressiveLevelUpTails:
                Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Flying);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                Mod.AbilityUnlockHandler.PollUpdates();
                break;
            case SHItem.ProgressiveLevelUpKnuckles:
                Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Power);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                Mod.AbilityUnlockHandler.PollUpdates();
                break;
            default:
                handled = false;
                break;
        }

        if (handled && Mod.ArchipelagoHandler.SlotData != null)
            Mod.ArchipelagoHandler.SlotData.RecalculateOpenLevels();
        Mod.ArchipelagoHandler!.Save();
        
        if (!Mod.GameHandler.InGame())
        {
            cachedItems.Enqueue(itemId);
            return;
        }
        
        HandleInGameItem(itemId);
    }


    public void HandleInGameItem(SHItem itemId)
    {
        switch (itemId)
        {
            case SHItem.ExtraLife:
                GameHandler.ModifyLives((int)Mod.ModuleBase, 1);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1034);
                break;
            case SHItem.FiveRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 5);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TenRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 10);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TwentyRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 20);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.Shield:
                GameHandler.GiveShield((int)Mod.ModuleBase);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1036);
                break;
            case SHItem.SpeedLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Speed);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.PowerLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Power);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.FlyLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Flying);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.TeamLevelUp: 
                //GameHandler.GiveLevelUp(LevelUpType.Speed);
                //GameHandler.GiveLevelUp(LevelUpType.Power);
                //GameHandler.GiveLevelUp(LevelUpType.Flying);
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.TeamBlastFiller:
                GameHandler.HandleTeamBlastFiller();
                break;
            case SHItem.StealthTrap:
                Mod.TrapHandler?.HandleStealthTrap();
                break;
            case SHItem.FreezeTrap:
                Mod.TrapHandler?.HandleFreezeTrap();
                break;
            case SHItem.NoSwapTrap:
                Mod.TrapHandler?.HandleNoSwapTrap();
                break;
            case SHItem.RingTrap:
                Mod.GameHandler.SetRingCount(Math.Max(0, Mod.GameHandler.GetRingCount() - 50));
                if (Mod.ArchipelagoHandler!.SlotData.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1005);
                Mod.ArchipelagoHandler.SendRing(-50);
                break;
            case SHItem.CharmyTrap:
                Mod.TrapHandler?.HandleCharmyTrap();
                break;
            default:
                break;
        }
    }

    public void HandleCachedItems()
    {
        while (cachedItems.Count > 0)
        {
            var item = cachedItems.Dequeue();
            HandleInGameItem(item);
        }
    }
}