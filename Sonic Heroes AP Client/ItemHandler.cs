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
    
    ProgressiveSonicAllRegions,
    ProgressiveTailsAllRegions,
    ProgressiveKnucklesAllRegions,
    ProgressiveSonicOceanRegion,
    ProgressiveTailsOceanRegion,
    ProgressiveKnucklesOceanRegion,
    ProgressiveSonicHotPlantRegion,
    ProgressiveTailsHotPlantRegion,
    ProgressiveKnucklesHotPlantRegion,
    ProgressiveSonicCasinoRegion,
    ProgressiveTailsCasinoRegion,
    ProgressiveKnucklesCasinoRegion,
    ProgressiveSonicTrainRegion,
    ProgressiveTailsTrainRegion,
    ProgressiveKnucklesTrainRegion,
    ProgressiveSonicBigPlantRegion,
    ProgressiveTailsBigPlantRegion,
    ProgressiveKnucklesBigPlantRegion,
    ProgressiveSonicGhostRegion,
    ProgressiveTailsGhostRegion,
    ProgressiveKnucklesGhostRegion,
    ProgressiveSonicSkyRegion,
    ProgressiveTailsSkyRegion,
    ProgressiveKnucklesSkyRegion,
    
    
    //Currently Not Using these
    ProgressiveLevelUpSonic = 0x1F0,
    ProgressiveLevelUpTails,
    ProgressiveLevelUpKnuckles,
    
    
    
    ExtraLife = 0x200,
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
    
    StealthTrap = 0x300,
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
    
    //public unsafe void HandleItem(int index, NetworkItem item)
    public unsafe void HandleItem(int index, ItemInfo item)
    {
        //Console.WriteLine($"HandleSHItem {index}, {Mod.SaveDataHandler!.CustomSaveData!.LastItemIndex} :: {(SHItem)(item.Item - 0x93930000)} : 0x{item.Item:X}");
        
        //use this print below
        //Console.WriteLine($"HandleSHItem {index}, {Mod.SaveDataHandler!.CustomSaveData!.LastItemIndex} :: {item.ItemName} : 0x{item.ItemId:X}");
        
        
        if (index < Mod.SaveDataHandler!.CustomSaveData.LastItemIndex)
            return;

        //Console.WriteLine($"{index} : {Mod.SaveDataHandler.CustomData->LastItemIndex}");
        Mod.SaveDataHandler!.CustomSaveData!.LastItemIndex++;
        
        //Console.WriteLine($"Item received: {item.ItemName}");
        
        var handled = true;
        //var itemName = (SHItem)(item.Item - 0x93930000);
        //var itemId = (SHItem)(item.Item - 0x93930000);
        
        var itemName = item.ItemName;
        var itemId = (SHItem)(item.ItemId - 0x93930000);
        bool unlocked = false;

        switch (itemId)
        {
            case SHItem.Emblem:
                Mod.SaveDataHandler!.CustomSaveData.Emblems++;
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE016);
                break;
            case SHItem.GreenChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Green] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[3] = 1;
                break;
            case SHItem.BlueChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Blue] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[6] = 1;
                break;
            case SHItem.YellowChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Yellow] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[9] = 1;
                break;
            case SHItem.WhiteChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.White] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[12] = 1;
                break;
            case SHItem.CyanChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Cyan] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[15] = 1;
                break;
            case SHItem.PurpleChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Purple] = true;
                Mod.SaveDataHandler!.RedirectData->Emerald[18] = 1;
                break;
            case SHItem.RedChaosEmerald:
                Mod.SaveDataHandler!.CustomSaveData.Emeralds[Emerald.Red] = true;
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
            case SHItem.ProgressiveSonicAllRegions:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForAllRegions(Team.Sonic);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsAllRegions:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForAllRegions(Team.Sonic);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesAllRegions:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForAllRegions(Team.Sonic);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesOceanRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Ocean);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesHotPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.HotPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesCasinoRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Casino);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesTrainRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Train);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesBigPlantRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.BigPlant);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesGhostRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Ghost);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveSonicSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementSpeedAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveTailsSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementFlyingAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveKnucklesSkyRegion:
                Mod.AbilityUnlockHandler!.IncrementPowerAbilityForRegion(Team.Sonic, Region.Sky);
                Console.WriteLine($"Got Item: {itemName}");
                break;
            case SHItem.ProgressiveLevelUpSonic:
                //Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Speed);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                Mod.AbilityUnlockHandler.PollUpdates();
                break;
            case SHItem.ProgressiveLevelUpTails:
                //Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Flying);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                Mod.AbilityUnlockHandler.PollUpdates();
                break;
            case SHItem.ProgressiveLevelUpKnuckles:
                //Mod.AbilityUnlockHandler!.IncrementLevelUpMax(Team.Sonic, FormationChar.Power);
                Console.WriteLine($"Got Item: {itemName}");
                if (Mod.Configuration!.PlaySounds)
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
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1034);
                break;
            case SHItem.FiveRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 5);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TenRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 10);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TwentyRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 20);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.Shield:
                GameHandler.GiveShield((int)Mod.ModuleBase);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1036);
                break;
            case SHItem.SpeedLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Speed);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.PowerLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Power);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.FlyLevelUp:
                //GameHandler.GiveLevelUp(LevelUpType.Flying);
                if (Mod.Configuration!.PlaySounds)
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.TeamLevelUp: 
                //GameHandler.GiveLevelUp(LevelUpType.Speed);
                //GameHandler.GiveLevelUp(LevelUpType.Power);
                //GameHandler.GiveLevelUp(LevelUpType.Flying);
                if (Mod.Configuration!.PlaySounds)
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
                Mod.GameHandler!.SetRingCount(Math.Max(0, Mod.GameHandler.GetRingCount() - 50));
                if (Mod.Configuration!.PlaySounds)
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