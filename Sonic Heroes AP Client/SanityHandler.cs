using System.Numerics;

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

    public void HandleKeySanity(int edx)
    {
        var apHandler = Mod.ArchipelagoHandler!;
        var act1StartId = 0x165d;
        var act2StartId = 0x1702;
        var noActStartId = 0x17a7;
        
        //Console.WriteLine("Running GetBonusKey Here!");
        //Console.WriteLine($"EBP is: {edx:X}");
        unsafe
        {
            var level = Mod.GameHandler!.GetCurrentLevel();
            var story = Mod.GameHandler!.GetCurrentStory();
            var act = Mod.GameHandler!.GetCurrentAct();
            
            if (apHandler!.SlotData.KeySanityDict[story] == 0)
                return;

            //var posPtr = *(int*)(Mod.ModuleBase + 0x5CE820);
            //Vector3 leaderPos = new Vector3(*(float*)(posPtr + 0xE8), *(float*)(posPtr + 0xEC), *(float*)(posPtr + 0xF0));
            
            var keyPtr = *(int*)(edx + 0x2C);
            Vector3 keyPos = new Vector3(*(float*)(keyPtr + 0x0), *(float*)(keyPtr + 0x4), *(float*)(keyPtr + 0x8));

            float minDistance = 999999f;
            
            
            var keysinlevel = from key in KeySanityPositions.AllKeyPositions
                where key.Team == story && key.LevelId == level
                select key;

            List<KeyPosition> keylist = keysinlevel.ToList();


            if (keylist.Count() == 0)
            {
                Console.WriteLine($"NO KEYS FOUND FOR TEAM LEVEL ACT: {story} {level} {act} :::: coords are: {keyPos}");
            }

            for (int i = 0; i < keylist.Count(); i++)
            {
                if (Vector3.Distance(keyPos, keylist[i].Pos) > 100.0f)
                {
                    if (Vector3.Distance(keyPos, keylist[i].Pos) < minDistance)
                    {
                        minDistance = Vector3.Distance(keyPos, keylist[i].Pos);
                    }
                    //Console.WriteLine($"Entry not matching. CurrentKeys[i].Pos is: {keylist[i].Pos} and Distance is: {Vector3.Distance(keyPos, keylist[i].Pos)}");
                    if (i == keylist.Count() - 1)
                    {
                        Console.WriteLine($"NO MATCH FOUND FOR KEY at: {story} {level} {act} with coords: {keyPos}. Smallest Distance is {minDistance}");
                    }
                    
                    continue;
                    
                }
                //Console.WriteLine($"Match Found! Index is: {i}");

                if (apHandler!.SlotData.KeySanityDict[story] == 1)
                {
                    apHandler.CheckLocation(noActStartId + KeySanityPositions.AllKeyPositions.IndexOf(keylist[i]));
                }
                
                else if (apHandler!.SlotData.KeySanityDict[story] == 2)
                {
                    if (act == Act.Act1)
                    {
                        apHandler.CheckLocation(act1StartId + KeySanityPositions.AllKeyPositions.IndexOf(keylist[i]));
                    }
                    else
                    {
                        apHandler.CheckLocation(act2StartId + KeySanityPositions.AllKeyPositions.IndexOf(keylist[i]));
                    }
                    
                }
                
                Console.WriteLine($"Got Team {story} {level} {act} Bonus Key #{i + 1}");
                //Logger.Log($"");
                
                break;
            }
            
            //Console.WriteLine($"Key Position is: {keyPos.X}, {keyPos.Y}, {keyPos.Z}");
            
        }
    }
    
    
    
    
}