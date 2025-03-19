﻿using Archipelago.MultiClient.Net.Models;

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
    ExtraLife,
    FiveRings,
    TenRings,
    TwentyRings,
    Shield,
    Invincibility,
    SpeedLevelUp,
    PowerLevelUp,
    FlyLevelUp,
    TeamLevelUp,
    StealthTrap = 0x100,
    FreezeTrap = 0x101,
    NoSwapTrap = 0x102,
    RingTrap = 0x103,
    CharmyTrap = 0x104,
}

public class ItemHandler
{
    private readonly Queue<SHItem> cachedItems;

    public ItemHandler()
    {
        cachedItems = new Queue<SHItem>();    
    }
    
    public void HandleItem(int index, ItemInfo item)
    {
        if (index < Mod.SaveDataHandler.CustomData.LastItemIndex)
            return;

        //Console.WriteLine($"{index} : {Mod.SaveDataHandler.CustomData.LastItemIndex}");
        Mod.SaveDataHandler.CustomData.LastItemIndex++;
        
        Console.WriteLine($"Item received: {item.ItemName}");
        
        var handled = true;
        var itemId = (SHItem)(item.ItemId - 0x93930000);
        
        unsafe
        {
            switch (itemId)
            {
                case SHItem.Emblem:
                    Mod.SaveDataHandler!.CustomData.EmblemCount++;
                    Mod.ArchipelagoHandler.SlotData.RecalculateOpenLevels();
                    SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE016);
                    break;
                case SHItem.GreenChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[0] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[3] = 1;
                    break;
                case SHItem.BlueChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[1] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[6] = 1;
                    break;
                case SHItem.YellowChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[2] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[9] = 1;
                    break;
                case SHItem.WhiteChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[3] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[12] = 1;
                    break;
                case SHItem.CyanChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[4] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[15] = 1;
                    break;
                case SHItem.PurpleChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[5] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[18] = 1;
                    break;
                case SHItem.RedChaosEmerald:
                    Mod.SaveDataHandler!.CustomData.Emeralds[6] = 1;
                    Mod.SaveDataHandler!.RedirectData->Emerald[21] = 1;
                    break;
                default:
                    handled = false;
                    break;
            }
        }
    
        if (handled)
            Mod.ArchipelagoHandler.SlotData.RecalculateOpenLevels();
        Mod.ArchipelagoHandler?.Save();
        
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
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1034);
                break;
            case SHItem.FiveRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 5);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TenRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 10);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.TwentyRings:
                Mod.GameHandler?.SetRingCount(Mod.GameHandler.GetRingCount() + 20);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1033);
                break;
            case SHItem.Shield:
                GameHandler.GiveShield((int)Mod.ModuleBase);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0x1036);
                break;
            case SHItem.SpeedLevelUp:
                GameHandler.GiveLevelUp(LevelUpType.Speed);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.PowerLevelUp:
                GameHandler.GiveLevelUp(LevelUpType.Power);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.FlyLevelUp:
                GameHandler.GiveLevelUp(LevelUpType.Flying);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
                break;
            case SHItem.TeamLevelUp: 
                GameHandler.GiveLevelUp(LevelUpType.Speed);
                GameHandler.GiveLevelUp(LevelUpType.Power);
                GameHandler.GiveLevelUp(LevelUpType.Flying);
                SoundHandler.PlaySound((int)Mod.ModuleBase, 0xE005);
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