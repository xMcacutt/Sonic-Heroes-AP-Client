namespace Sonic_Heroes_AP_Client;

public class SanityHandler
{
    public int[] DarkChecksCompleted;
    public int[] RoseChecksCompleted;
    public int[] ChaotixChecksCompleted;
    
    public SanityHandler()
    {
        DarkChecksCompleted = new int[14];
        RoseChecksCompleted = new int[14];
        ChaotixChecksCompleted = new int[28];
    }
    
    public void CheckRingSanity(int newCount)
    {
        if (!Mod.GameHandler.InGame())
            return;
        if (!(Mod.ArchipelagoHandler.SlotData.IsRosesanityActive || Mod.ArchipelagoHandler.SlotData.IsChaotixsanityActive))
            return;
        var levelId = Mod.GameHandler.GetCurrentLevel();
        if (!Enum.IsDefined(typeof(LevelId), levelId) || (int)levelId > 15)
            return;
        var storyId = Mod.GameHandler.GetCurrentStory();
        var act = Mod.GameHandler.GetCurrentAct();
        if (storyId == Team.Rose && act != Act.Act2
            || storyId != Team.Rose && (storyId != Team.Chaotix || levelId != LevelId.CasinoPark))
            return;

        var maxRingCheck = storyId == Team.Rose || act == Act.Act1 ? 200 : 500;
        if (newCount > maxRingCheck)
            newCount = maxRingCheck;
        int previousCount;
        if (storyId == Team.Rose)
        {
            previousCount = RoseChecksCompleted[(int)levelId - 2];
            if (previousCount >= newCount)
                return;
            RoseChecksCompleted[(int)levelId - 2] = newCount;
        }
        else
        {
            previousCount = ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)];
            if (previousCount >= newCount)
                return;
            ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)] = newCount;
        }
        var checkSize = storyId == Team.Rose
            ? Mod.ArchipelagoHandler.SlotData.RosesanityCheckSize
            : Mod.ArchipelagoHandler.SlotData.ChaotixsanityRingCheckSize;
        var levelOffset = ((int)levelId - 2) * 200;
        if (storyId == Team.Chaotix && levelId == LevelId.CasinoPark)
            levelOffset = act == Act.Act1 ? 0xBC0 : 0xC88;
        for (var i = previousCount + 1; i <= newCount; i++)
            if (i % checkSize == 0)
                Mod.ArchipelagoHandler.CheckLocation(0x6C7 + levelOffset + i);
    }

    public void HandleCountIncreased(int newCount)
    {
        if (!Mod.GameHandler.InGame())
            return;
        if (!Mod.ArchipelagoHandler.SlotData.IsChaotixsanityActive)
            return;
        var storyId = Mod.GameHandler.GetCurrentStory();
        if (storyId != Team.Chaotix)
            return;
        var levelId = Mod.GameHandler.GetCurrentLevel();
        var act = Mod.GameHandler.GetCurrentAct();
        switch (levelId)
        {
            case LevelId.SeasideHill:
                HandleChaotixsanity(act == Act.Act1 ? 0x11B7 : 0x11C1, 
                    act == Act.Act1 ? 10 : 20 , newCount, levelId, act);
                break;
            case LevelId.BingoHighway:
                HandleChaotixsanity(act == Act.Act1 ? 0x1543 : 0x154D, 
                    act == Act.Act1 ? 10 : 20 , newCount, levelId, act);
                break;
            case LevelId.LostJungle:
                HandleChaotixsanity(act == Act.Act1 ? 0x15B1 : 0x15BB, 
                    act == Act.Act1 ? 10 : 20, newCount, levelId, act);
                break;
            case LevelId.HangCastle:
                HandleChaotixsanity(act == Act.Act1 ? 0x15CF : 0x15D9, 
                    10, newCount, levelId, act);
                break;
            case LevelId.MysticMansion:
                HandleChaotixsanity(act == Act.Act1 ? 0x15E3 : 0x161F, 
                    act == Act.Act1 ? 60 : 46, newCount, levelId, act);
                break;
            case LevelId.FinalFortress:
                HandleChaotixsanity(act == Act.Act1 ? 0x164D : 0x1652, 
                    act == Act.Act1 ? 5 : 10, newCount, levelId, act);
                break;
            default:
                return;
        }
    }

    public void HandleChaotixsanity(int levelOffset, int maxCount, int newCount, LevelId levelId, Act act)
    {
        if (ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)] < newCount)
            ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)] = newCount;
        else
            return;
        if (newCount > maxCount)
            return;
        //Console.WriteLine($"Check Index {newCount}");
        //Console.WriteLine((levelOffset + newCount).ToString("X"));
        Mod.ArchipelagoHandler.CheckLocation(levelOffset + newCount);
    }

    public void CheckEnemyCount(int newCount)
    {
        if (!Mod.GameHandler.InGame())
            return;
        if (!(Mod.ArchipelagoHandler.SlotData.IsDarksanityActive || Mod.ArchipelagoHandler.SlotData.IsChaotixsanityActive))
            return;
        var levelId = Mod.GameHandler.GetCurrentLevel();
        if (!Enum.IsDefined(typeof(LevelId), levelId) || (int)levelId > 15)
            return;
        var storyId = Mod.GameHandler.GetCurrentStory();
        var act = Mod.GameHandler.GetCurrentAct();
        if ((storyId != Team.Dark && storyId != Team.Chaotix) ||
            (storyId == Team.Dark && act != Act.Act2) ||
            (storyId == Team.Chaotix && levelId != LevelId.GrandMetropolis))
            return;
        var maxEnemyCheck = storyId == Team.Dark ? 100 : 85;
        if (newCount > maxEnemyCheck)
            newCount = maxEnemyCheck;

        int previousCount;
        if (storyId == Team.Dark)
        {
            previousCount = DarkChecksCompleted[(int)levelId - 2];
            if (previousCount >= newCount)
                return;
            DarkChecksCompleted[(int)levelId - 2] = newCount;
        }
        else
        {
            previousCount = ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)];
            if (previousCount >= newCount)
                return;
            ChaotixChecksCompleted[((int)levelId - 2) + (14 * (int)act)] = newCount;
        }

        var checkSize = storyId == Team.Dark
            ? Mod.ArchipelagoHandler.SlotData.DarksanityCheckSize
            : 1;
        var levelOffset = ((int)levelId - 2) * 100;
        if (storyId == Team.Chaotix && levelId == LevelId.GrandMetropolis)
            levelOffset = act == Act.Act1 ? 0x1086 : 0x10DB;

        // Loop through all enemy counts that were skipped (or reached in succession)
        for (var i = previousCount + 1; i <= newCount; i++)
            if (i % checkSize == 0)
                Mod.ArchipelagoHandler.CheckLocation(0x14F + levelOffset + i);
    }

    public void HandleBSCapsuleCountIncreased(int newCount)
    {
        if (!Mod.GameHandler.InGame())
            return;
        if (!Mod.ArchipelagoHandler.SlotData.IsChaotixsanityActive)
            return;
        var storyId = Mod.GameHandler.GetCurrentStory();
        if (storyId != Team.Chaotix)
            return;
        var levelId = Mod.GameHandler.GetCurrentLevel();
        var act = Mod.GameHandler.GetCurrentAct();
        if (levelId != LevelId.BulletStation)
            return; 
        HandleChaotixsanity(act == Act.Act1 ? 0x1561 : 0x157F, 
            act == Act.Act1 ? 30 : 50 , newCount, levelId, act);
    }

    public void HandleGoldBeetleCountIncreased(int newCount)
    {
        if (!Mod.GameHandler.InGame())
            return;
        if (!Mod.ArchipelagoHandler.SlotData.IsChaotixsanityActive)
            return;
        var storyId = Mod.GameHandler.GetCurrentStory();
        if (storyId != Team.Chaotix)
            return;
        var levelId = Mod.GameHandler.GetCurrentLevel();
        var act = Mod.GameHandler.GetCurrentAct();
        if (levelId != LevelId.PowerPlant)
            return; 
        HandleChaotixsanity(act == Act.Act1 ? 0x127F : 0x1282, 
            act == Act.Act1 ? 3 : 5, newCount, levelId, act);
    }
}