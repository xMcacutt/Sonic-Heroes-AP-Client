﻿using System.Numerics;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X86;

namespace Sonic_Heroes_AP_Client;

public enum LevelUpType
{
    Speed,
    Flying,
    Power
}

public enum Act
{
    Act1,
    Act2,
    Act3
}

public struct Stage
{
    private readonly LevelId _level;
    private readonly Team _story;
    private readonly Act _act;

    public override bool Equals(object? obj)
    {
        if (obj is not Stage stage)
            return false;
        return (_level == stage._level
                && _story == stage._story
                && _act == stage._act);
    }

    public Stage(LevelId level, Team story, Act act)
    { 
        _level = level;
        _story = story;
        _act = act;
    }
}

public class GameHandler
{
    public static int SuperHardModeId = 0x2300; //154F
    
    
    [DllImport("SHAP-NativeCaller.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ModifyLives(int moduleBase, int amount);
    
    [DllImport("SHAP-NativeCaller.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int RestartLevel(int moduleBase);
    
    [DllImport("SHAP-NativeCaller.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GiveShield(int moduleBase);

    public static void SetRingLoss(bool modern)
    {
        var bytes = modern ? new byte[] { 0xB9, 0x14, 0x0, 0x0, 0x0, 0x90, 0x90 } 
            : new byte[] { 0x8B, 0xC, 0x85, 0xC, 0xD7, 0x9D, 0x0 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A446D, bytes);
    }

    public static void GiveLevelUp(LevelUpType type)
    {
        unsafe
        {
           var baseAddress = *(int*)((int)Mod.ModuleBase + 0x64C268);
           var ptr = (byte*)(baseAddress + 0x208 + (int)type);
           var currentValue = *ptr;
           if (currentValue > 2)
               return;
           *ptr = (byte)(currentValue + 1);
        }
    }
    
    public static void SetDontLoseBonusKey(bool value)
    {
        var writeValue1 = value ? new byte[] { 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0xC6, 0x40, 0x26, 0x1 };
        var writeValue2 = value ? new byte[] { 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0xC6, 0x46, 0x26, 0x1 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4B4E, writeValue1);
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x1A43D3, writeValue2);
    }


    public static void SetCheckPointPriorityWrite(bool value)
    {
        if (!value) 
            return;
        var bytes = new byte[] { 0x90, 0x90 };
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x23996, bytes);

    }

    public void ModifyInstructions()
    {
        // Makes all menu options display visually
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x504A3, new byte[] { 0x90, 0x90 });
        // Removes emblem update
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x22F344, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x24255B, new byte[] { 0x90 });
    }

    public void Kill()
    {
        if (!InGame()) 
            return;
        ModifyLives((int)Mod.ModuleBase, -1);
        RestartLevel((int)Mod.ModuleBase);
    }
    
    public bool InGame()
    {
        unsafe
        {
            return *(int*)(Mod.ModuleBase + 0x4D66F0) == 5 && *(int*)((int)Mod.ModuleBase + 0x64C268) != 0;;
        }
    }
    
    public LevelId GetCurrentLevel() {
        unsafe
        {
            var level = *(int*)(Mod.ModuleBase + 0x4D6720);
            if (level == 36)
                level = 8;
            return (LevelId)level;
        }
    }
    
    public Team GetCurrentStory() {
        unsafe
        {
            return (Team)(*(int*)(Mod.ModuleBase + 0x4D6920));
        }
    }

    public Act GetCurrentAct()
    {
        unsafe
        {
            var baseAddress = *(int*)((int)Mod.ModuleBase + 0x6777E4);
            return (Act)(*(byte*)(baseAddress + 0x28));
        }
    }
    
    public void SetRingCount(int amount)
    {
        unsafe
        {
            *(int*)(Mod.ModuleBase + 0x5DD70C) = amount;
        }
    }
    
    public int GetRingCount()
    {
        unsafe
        {
            return *(int*)(Mod.ModuleBase + 0x5DD70C);
        }
    }

    private static List<IAsmHook> _asmHooks;
    private static IReverseWrapper<CompleteLevel> _reverseWrapOnCompleteLevel;
    private static IReverseWrapper<GoLevelSelect> _reverseWrapOnGoLevelSelect;
    private static IReverseWrapper<SetRings> _reverseWrapOnSetRings;
    private static IReverseWrapper<Die> _reverseWrapOnDie;
    private static IReverseWrapper<IncrementCount> _reverseWrapOnIncrementCount;
    private static IReverseWrapper<IncrementEnemyCount> _reverseWrapOnMoveEnemyCount;
    private static IReverseWrapper<IncrementBSCapsuleCount> _reverseWrapOnIncrementBSCapsuleCount;
    private static IReverseWrapper<IncrementGoldBeetleCount> _reverseWrapOnIncrementGoldBeetleCount;
    private static IReverseWrapper<AssignRings> _reverseWrapOnCheckRings;
    private static IReverseWrapper<CompleteEmeraldStage> _reverseWrapOnCompleteEmeraldStage;
    private static IReverseWrapper<SetStateInGame> _reverseWrapOnSetStateInGame;
    private static IReverseWrapper<StartCompleteStage> _reverseWrapOnStartCompleteStage;
    private static IReverseWrapper<GetBonusKey> _reverseWrapOnGetBonusKey;
    private static IReverseWrapper<GetCheckPoint> _reverseWrapOnGetCheckPoint;
    public void SetupHooks(IReloadedHooks hooks)
    {
        _asmHooks = new List<IAsmHook>();
        string[] goMenuHook = {
            "use32",
            "mov dword[esi+0x438],0x0",
            "mov dword[esi+0x43C],0x1",
            "mov dword[esi+0x440],0x0",
            "mov dword[esi+0x444],0x0"
        };
        _asmHooks.Add(hooks.CreateAsmHook(goMenuHook, (int)(Mod.ModuleBase + 0x50436), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] completeLevelHook = {
            "use32",
            "pushad",
            "pushfd",
            "push esi",
            "push ebx",
            "push edx",
            "push ecx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnCompleteLevel, out _reverseWrapOnCompleteLevel)}",
            "pop ecx",
            "pop edx",
            "pop ebx",
            "pop esi",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(completeLevelHook, (int)(Mod.ModuleBase + 0x22EEC0), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] goLevelSelectHook = {
            "use32",
            "pushad",
            "pushfd",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnGoLevelSelect, out _reverseWrapOnGoLevelSelect)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(goLevelSelectHook, (int)(Mod.ModuleBase + 0x4F440), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] setRings = {
            "use32",
            "pushad",
            "pushfd",
            "push edx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnSetRings, out _reverseWrapOnSetRings)}",
            "pop edx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(setRings, (int)(Mod.ModuleBase + 0x23AA0), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] die = {
            "use32",
            "pushad",
            "pushfd",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnDie, out _reverseWrapOnDie)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(die, (int)(Mod.ModuleBase + 0x452B), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] incrementCount =
        {
            "use32",
            "pushad",
            "pushfd",
            "push ecx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnIncrementCount, out _reverseWrapOnIncrementCount)}",
            "pop ecx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(incrementCount, (int)(Mod.ModuleBase + 0x1B4901), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] moveEnemyCount =
        {
            "use32",
            "pushad",
            "pushfd",
            "push ebx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnMoveEnemyCount, out _reverseWrapOnMoveEnemyCount)}",
            "pop ebx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(moveEnemyCount, (int)(Mod.ModuleBase + 0x1DDFD7), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] incrementBSCapsuleCount =
        {
            "use32",
            "pushad",
            "pushfd",
            "push eax",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnIncrementBSCapsuleCount, out _reverseWrapOnIncrementBSCapsuleCount)}",
            "pop eax",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(incrementBSCapsuleCount, (int)(Mod.ModuleBase + 0xD4B76), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] incrementGoldBeetleCount =
        {
            "use32",
            "pushad",
            "pushfd",
            "push edx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnIncrementGoldBeetleCount, out _reverseWrapOnIncrementGoldBeetleCount)}",
            "pop edx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(incrementGoldBeetleCount, (int)(Mod.ModuleBase + 0x1FA390), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] checkRings =
        {
            "use32",
            "pushad",
            "pushfd",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnCheckRings, out _reverseWrapOnCheckRings)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(checkRings, (int)(Mod.ModuleBase + 0x1A9DB2), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] completeEmeraldStage =
        {
            "use32",
            "pushad",
            "pushfd",
            "push eax",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnCompleteEmeraldStage, out _reverseWrapOnCompleteEmeraldStage)}",
            "pop eax",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(completeEmeraldStage, (int)(Mod.ModuleBase + 0x22F498), AsmHookBehaviour.DoNotExecuteOriginal).Activate());
        
        string[] startCompleteStage =
        {
            "use32",
            "pushad",
            "pushfd",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnStartCompleteStage, out _reverseWrapOnStartCompleteStage)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(startCompleteStage, (int)(Mod.ModuleBase + 0x4454), AsmHookBehaviour.ExecuteFirst).Activate());
        
        string[] setStateInGame =
        {
            "use32",
            "pushad",
            "pushfd",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnSetStateInGame, out _reverseWrapOnSetStateInGame)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(setStateInGame, (int)(Mod.ModuleBase + 0x2774), AsmHookBehaviour.ExecuteAfter).Activate());
        _asmHooks.Add(hooks.CreateAsmHook(setStateInGame, (int)(Mod.ModuleBase + 0x41FC), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] getBonusKey =
        {
            "use32",
            "pushad",
            "pushfd",
            "mov edx,ebp",
            "push edx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnGetBonusKey, out _reverseWrapOnGetBonusKey)}",
            "pop edx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(getBonusKey, (int)(Mod.ModuleBase + 0x7B325), AsmHookBehaviour.ExecuteAfter).Activate());
        
        string[] getCheckPoint = 
        {
            "use32",
            "pushad",
            "pushfd",
            "mov edx,eax",
            "push edx",
            "push ecx",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnGetCheckPoint, out _reverseWrapOnGetCheckPoint)}",
            "pop ecx",
            "pop edx",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(getCheckPoint, (int)(Mod.ModuleBase + 0x23990), AsmHookBehaviour.ExecuteFirst).Activate());

        
        string[] setAct =
        {
            "use32",
            "pushad",
            "pushfd",
            "mov eax,esi",
            $"{hooks.Utilities.GetAbsoluteCallMnemonics(OnSetAct, out _reverseWrapOnSetAct)}",
            "popfd",
            "popad"
        };
        _asmHooks.Add(hooks.CreateAsmHook(setAct, (int)(Mod.ModuleBase + 0x4B659), AsmHookBehaviour.ExecuteAfter).Activate());
    }
    private static IReverseWrapper<SetAct> _reverseWrapOnSetAct;
    
    public void SetCurrentAct(Act act)
    {
        unsafe
        {
            var baseAddress = *(int*)((int)Mod.ModuleBase + 0x6777E4);
            *(byte*)(baseAddress + 0x28) = (byte)act;
        }
    }

    public void SetBonusKey(bool value)
    {
        unsafe
        {
            //Bonus Key Byte
            var baseAddress = *(int*)((int)Mod.ModuleBase + 0x6777E4);
            *(byte*)(baseAddress + 0x26) = value ? (byte)0 : (byte)1;
            
            //Visual Bonus Key Byte Here (yellow key on UI)
            //baseAddress = *(int*)((int)Mod.ModuleBase + 0x5DD4E4);
            //*(byte*)(baseAddress + 0x48) = value ? (byte)1 : (byte)0;
        }
    }
    
    [Function(new FunctionAttribute.Register[] { },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int SetAct();
    private static int OnSetAct()
    {
        //return 0;
        unsafe
        {
            var baseAddress = *(int*)((int)Mod.ModuleBase + 0x6777B4);
            var team = *(int*)(baseAddress + 0x220);
            
            if (Mod.ArchipelagoHandler.SlotData.SuperHardModeSonicAct2 &&
                (Team)team == Team.Sonic &&
                Mod.GameHandler.GetCurrentAct() == Act.Act2)
                Mod.GameHandler.SetCurrentAct(Act.Act3);
            return 0;
        }
    }
    
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.edx}, 
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int GetBonusKey(int edx);
    private static int OnGetBonusKey(int edx)
    {
        Mod.SanityHandler!.HandleKeySanity(edx);
        return 0;
    }
    
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.ecx, FunctionAttribute.Register.edx}, 
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int GetCheckPoint(int priority, int pointer);
    private static int OnGetCheckPoint(int ecx, int edx)
    {
        Mod.SanityHandler!.HandleCheckPointSanity(ecx, edx);
        return 0;
    }
    
    [UnmanagedFunctionPointer(CallingConvention.FastCall)]
    private delegate void LifeSetFunc(int ecx, int edx);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void RestartFunc();

    [Function(new[] {
        FunctionAttribute.Register.ecx, 
        FunctionAttribute.Register.edx, 
        FunctionAttribute.Register.ebx, 
        FunctionAttribute.Register.esi 
    },FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int CompleteLevel(int isMission2, int levelIndex, int rank, int story);
    private static int OnCompleteLevel(int ecx, int edx, int ebx, int esi)
    {
        var isMission2 = ecx; 
        var levelIndex = edx;
        var story = (Team)esi;
        var rank = (Rank)ebx;
        var apHandler = Mod.ArchipelagoHandler!;
        var slotData = apHandler.SlotData;

        if (levelIndex == 36)
            levelIndex = 8;
        
        //Console.WriteLine($"OnCompleteLevel Here. IsAct2: {isMission2},  LevelIndex: {levelIndex}, Rank: {rank}, Story: {story}");
        
        
        if (levelIndex > 25)
            return 0;
        
        if (rank <= slotData.RequiredRank) {
            Logger.Log("Did not reach the required rank.");
            Console.WriteLine($"Did not reach the required rank. {rank} is not the required {slotData.RequiredRank}");
            return 0;
        }

        if ((LevelId)levelIndex == LevelId.MetalOverlord) {
            Logger.Log("Victory!");
            apHandler.Release();
            return 1;
        }
        
        if (Mod.ArchipelagoHandler.SlotData.StoriesActive[story] is MissionsActive.None)
            return 0;
        if (levelIndex < 16 && Mod.ArchipelagoHandler.SlotData.StoriesActive[story] is not MissionsActive.Both)
        {
            if (Mod.ArchipelagoHandler.SlotData.StoriesActive[story] is MissionsActive.Act1 && isMission2 == 1)
                return 0;
            if (Mod.ArchipelagoHandler.SlotData.StoriesActive[story] is MissionsActive.Act2 && isMission2 == 0)
                return 0;
        }
        //Console.WriteLine($"Story: {(int)story} Level: {levelIndex} Rank: {(int)rank} IsMission2: {isMission2}");

        var locationId = 0xA0 + (int)story * 42 + (levelIndex - 2) * 2 + isMission2;

        if (Mod.ArchipelagoHandler.SlotData.SuperHardModeSonicAct2 && story == Team.Sonic && isMission2 == 1)
        {
            //hardcoded SuperHard ID here
            locationId = SuperHardModeId + (levelIndex - 2);
            //Console.WriteLine($"OnCompleteLevel Here. IsAct2: {isMission2},  LevelIndex: {levelIndex}, Rank: {rank}, Story: {story}");

        }
        
        
        if (levelIndex is >= 16 and < 25)
        {
            for (var gateIndex = 0; gateIndex < slotData.GateData.Count - 1; gateIndex++)
            {
                if (slotData.GateData[gateIndex].BossLevel.LevelId == (LevelId)levelIndex)
                {
                    slotData.GateData[gateIndex + 1].IsUnlocked = true;
                    slotData.RecalculateOpenLevels();
                    unsafe
                    {
                        Mod.SaveDataHandler.CustomData->GateBossComplete[gateIndex] = 1;
                    }
                }
                Mod.ArchipelagoHandler?.Save();
                locationId = 0xA0 + (levelIndex - 2) * 2;
                foreach (var team in slotData.StoriesActive.Where(team => team.Value != MissionsActive.None))
                    apHandler.CheckLocation(locationId + 42 * (int)team.Key);
            }
            return 1;
        }

        //Console.WriteLine($"Checking Mission Completion Location Here: Id = {(0x93930000 + locationId):X}");
        apHandler.CheckLocation(locationId);
        return 1;
    }
    

    [Function(new FunctionAttribute.Register[] { }, 
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int GoLevelSelect();
    private static int OnGoLevelSelect()
    {
        Mod.ArchipelagoHandler!.SlotData.RecalculateOpenLevels();
        return 1;
    }
    
    [Function(new FunctionAttribute.Register[] { }, 
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int SetStateInGame();
    private static int OnSetStateInGame()
    {
        Mod.ItemHandler.HandleCachedItems();
        if (Mod.GameHandler.GetCurrentAct() == Act.Act3)
        {
            Mod.GameHandler.SetCurrentAct(Act.Act2);
            Mod.GameHandler.SetBonusKey(true);
            
            
        }
        return 1;
    }

    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.edx },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int SetRings(int amount);
    private static int OnSetRings(int amount)
    {
        if (!Mod.ArchipelagoHandler.SlotData.RingLink) 
            return 0;
        if (Mod.ArchipelagoHandler.SlotData.RingLinkOverlord 
            || Mod.GameHandler.GetCurrentLevel() != LevelId.MetalOverlord)
            Mod.ArchipelagoHandler.SendRing(amount);
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int AssignRings();
    private static int OnCheckRings()
    {
        unsafe
        {
            var newCount = *(int*)(Mod.ModuleBase + 0x5DD70C);
            Mod.SanityHandler.CheckRingSanity(newCount);
        }
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int StartCompleteStage();
    private static int OnStartCompleteStage()
    {
        Mod.SanityHandler!.CheckEnemyCount(100);
        Mod.SanityHandler.CheckRingSanity(500);
        if (Mod.GameHandler.GetCurrentAct() == Act.Act3)
            Mod.GameHandler.SetBonusKey(true);
        return 0;
    }

    [Function(new FunctionAttribute.Register[] { },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int Die();

    public static bool SomeoneElseDied;
    private static int OnDie()
    {
        if (SomeoneElseDied)
        {
            SomeoneElseDied = false;
            return 1;
        }
        if (Mod.ArchipelagoHandler.SlotData.DeathLink)
            Mod.ArchipelagoHandler.SendDeath();
        return 0;
    }

    private static List<Stage> stealthStages =  new List<Stage>
    {
        new (LevelId.OceanPalace, Team.Chaotix, Act.Act2),
        new (LevelId.FrogForest, Team.Chaotix, Act.Act1),
        new (LevelId.FrogForest, Team.Chaotix, Act.Act2),
        new (LevelId.EggFleet, Team.Chaotix, Act.Act1),
        new (LevelId.EggFleet, Team.Chaotix, Act.Act2),
        new (LevelId.HangCastle, Team.Chaotix, Act.Act2)
    };

    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.ecx },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int IncrementCount(int newCount);

    private static int OnIncrementCount(int newCount)
    {
        Mod.SanityHandler.HandleCountIncreased(newCount);
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.ebx },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int IncrementEnemyCount(int newCount);

    private static int OnMoveEnemyCount(int newCount)
    {
        Mod.SanityHandler.CheckEnemyCount(newCount);
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.edx },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int IncrementGoldBeetleCount(int newCount);

    private static int OnIncrementGoldBeetleCount(int newCount)
    {
        Mod.SanityHandler.HandleGoldBeetleCountIncreased(newCount);
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.eax },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int IncrementBSCapsuleCount(int ptr);
    private static int OnIncrementBSCapsuleCount(int ptr)
    {
        unsafe
        {
            var newCount = *(int*)(ptr + 0x23C);
            Mod.SanityHandler.HandleBSCapsuleCountIncreased(newCount);
        }
        return 0;
    }
    
    [Function(new FunctionAttribute.Register[] { FunctionAttribute.Register.eax },
        FunctionAttribute.Register.eax, FunctionAttribute.StackCleanup.Callee)]
    public delegate int CompleteEmeraldStage(int emeraldAddressOffset);
    private static int OnCompleteEmeraldStage(int emeraldAddressOffset)
    {
        var locationId = 0x148 + (emeraldAddressOffset - 21) / 3;
        Mod.ArchipelagoHandler.CheckLocation(locationId);
        return 0;
    }

    public void RedirectSaveData(IntPtr redirectAddress)
    {
        //Console.WriteLine($"addr: {redirectAddress}");
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BAD7, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0xB)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BAF4, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0x23)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BB11, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0x33)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BB2E, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0x43)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BB63, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0x383)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4BB4A, 
            BitConverter.GetBytes((int)(redirectAddress + 0x4C + 0x4D3)));
        Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4B823, 
            BitConverter.GetBytes((int)(redirectAddress + 0x624)));
    }

    public static void SetSkipMadness(bool value)
    {
        unsafe
        {
            var addr = (char*)(Mod.ModuleBase + 0x343898 + (0x15 * 4));
            var levelId = value ? (int)LevelId.MetalOverlord : (int)LevelId.MetalMadness;
            Memory.Instance.SafeWrite((UIntPtr)addr, new byte[] {(byte)levelId});
        }
    }
}