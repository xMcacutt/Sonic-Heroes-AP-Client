using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Reloaded.Memory;
using Reloaded.Memory.Interfaces;

namespace Sonic_Heroes_AP_Client;

public struct TimeData {
    public byte Minutes;
    public byte Seconds;
    public byte Milliseconds;
};

[StructLayout(LayoutKind.Explicit)]
public struct LongData {
    [FieldOffset(0x0)]
    public short Rings;
    [FieldOffset(0x4)]
    public int Score;
    [FieldOffset(0x8)]
    public TimeData Time;
    [FieldOffset(0xB)]
    public Rank Rank;
};

public struct ShortData {
    public TimeData Time;
    public Rank Rank;
};

public struct SonicLevelData {
    public LongData Mission1;
    public LongData Mission2;
};

public struct DarkLevelData {
    public LongData Mission1;
    public ShortData Mission2;
};

public struct RoseLevelData {
    public LongData Mission1;
    public ShortData Mission2;
};

public struct ChaotixLevelData {
    public LongData Mission1;
    public LongData Mission2;
};

public struct LevelData {
    public SonicLevelData Sonic;
    public DarkLevelData Dark;
    public RoseLevelData Rose;
    public ChaotixLevelData Chaotix;
};

public struct BossData {
    public ShortData SonicBoss;
    public ShortData DarkBoss;
    public ShortData RoseBoss;
    public ShortData ChaotixBoss;
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct SaveData
{
    private fixed byte padding1[0x22];
    public byte EmblemCount;
    private fixed byte padding2[0x29];
    private fixed byte levelsBuffer[14 * 0x50];
    private fixed byte bossesBuffer[7 * 0x10];
    public ShortData MetalMadness;
    private fixed byte padding3[0x14C];
    public fixed int Emerald[25];
    
    public LevelData* Levels
    {
        get
        {
            fixed (byte* ptr = levelsBuffer)
                return (LevelData*)ptr;
        }
    }

    public BossData* Bosses
    {
        get
        {
            fixed (byte* ptr = bossesBuffer)
                return (BossData*)ptr;
        }
    }
}

public unsafe struct CustomSaveData {
    public fixed byte Emeralds[7];
    public int EmblemCount;
    public fixed byte GateBossUnlocked[8];
    public fixed byte GateBossComplete[7];
    public int LastItemIndex;
};

public class SaveDataHandler
{
    public unsafe SaveData* SaveData;
    public unsafe SaveData* RedirectData;
    public unsafe CustomSaveData* CustomData;
    
    private unsafe bool LoadFromFile(string filePath)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var buffer = new byte[Marshal.SizeOf<CustomSaveData>()];
        var success = fs.Read(buffer, 0, buffer.Length);
        fixed (byte* pBuffer = buffer)
            Unsafe.CopyBlock(CustomData, pBuffer, (uint)sizeof(CustomSaveData));
        return success != 0;
    }
    
    public unsafe bool LoadSaveData(string seed, string slot) 
    {
        CustomData = (CustomSaveData*)Marshal.AllocHGlobal(sizeof(CustomSaveData));
        Unsafe.InitBlock(CustomData, 0, (uint)sizeof(CustomSaveData));
        var filePath = "./Saves/" + seed + slot;
        if (!Directory.Exists("./Saves"))
            Directory.CreateDirectory("./Saves");
        if (File.Exists(filePath)) {
            if (LoadFromFile(filePath))
                Logger.Log("Save loaded successfully!");
            else {
                Logger.Log("Error: Unable to read save.");
                return false;
            }
        }
        else {
            Logger.Log("Creating a new save file.");
            SaveToFile(filePath);
            Logger.Log("Save file created");
        }
        unsafe
        { 
            SaveData = (SaveData*)(Mod.ModuleBase + 0x6A4228);
            RedirectData = (SaveData*)Marshal.AllocHGlobal(sizeof(SaveData)).ToPointer();
            var redirectAddress = (IntPtr)RedirectData;
            var empty = new SaveData();
            Marshal.StructureToPtr(empty, (IntPtr)RedirectData, false);
            Mod.GameHandler.RedirectSaveData(redirectAddress);
            SaveData->EmblemCount = (byte)CustomData->EmblemCount;
            RedirectData->Emerald[3] = CustomData->Emeralds[0];
            RedirectData->Emerald[6] = CustomData->Emeralds[1];
            RedirectData->Emerald[9] = CustomData->Emeralds[2];
            RedirectData->Emerald[12] = CustomData->Emeralds[3];
            RedirectData->Emerald[15] = CustomData->Emeralds[4];
            RedirectData->Emerald[18] = CustomData->Emeralds[5];
            RedirectData->Emerald[21] = CustomData->Emeralds[6];
            Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4B6E3, new byte[] { 0xA1 });
            Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4B6E4, 
                BitConverter.GetBytes((int)((IntPtr)CustomData + 8)));
            Memory.Instance.SafeWrite(Mod.ModuleBase + 0x4B6E8, new byte[] { 0x90, 0x90 });
        } 
        return true;
    }

    public void SaveGame(string seed, string slot)
    {
        var filePath = "./Saves/" + seed + slot;
        SaveToFile(filePath);
    }

    public void SaveToFile(string filePath)
    {
        unsafe
        {
            var buffer = new byte[Marshal.SizeOf<CustomSaveData>()];
            fixed (byte* pBuffer = buffer)
                Unsafe.CopyBlock(pBuffer, CustomData, (uint)sizeof(CustomSaveData));
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(buffer, 0, buffer.Length);
        }
    }

    public void SetLevelActive(LevelId level, bool isBoss, Team? story, bool value)
    {
        if (Mod.ArchipelagoHandler.SlotData == null)
            return;
        unsafe
        {
            var rank = value ? Rank.ERank : Rank.NoRank;
            if (level == LevelId.MetalMadness)
            {
                //Logger.Log($"Setting boss: {level} to {rank}");
                RedirectData->MetalMadness.Rank = rank;
                return;
            }
            if (isBoss)
            { 
                //Logger.Log($"Setting boss: {level} to {rank}");
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Sonic])
                    RedirectData->Bosses[(int)level - 16].SonicBoss.Rank = rank;
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Dark])
                    RedirectData->Bosses[(int)level - 16].DarkBoss.Rank = rank;
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Rose])
                    RedirectData->Bosses[(int)level - 16].RoseBoss.Rank = rank;
                if (Mod.ArchipelagoHandler.SlotData.StoriesActive[Team.Chaotix])
                    RedirectData->Bosses[(int)level - 16].ChaotixBoss.Rank = rank;
                return;
            }
            //Logger.Log($"Setting {story}'s {level} to {rank}");
            if (story == Team.Sonic)
                RedirectData->Levels[(int)level - 2].Sonic.Mission1.Rank = rank;
            if (story == Team.Dark)
                RedirectData->Levels[(int)level - 2].Dark.Mission1.Rank = rank;
            if (story == Team.Rose)
                RedirectData->Levels[(int)level - 2].Rose.Mission1.Rank = rank;
            if (story == Team.Chaotix)
                RedirectData->Levels[(int)level - 2].Chaotix.Mission1.Rank = rank;
        }
            
    }
}