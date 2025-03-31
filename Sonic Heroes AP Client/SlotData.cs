using Newtonsoft.Json.Linq;
using Sonic_Heroes_AP_Client.Configuration;

namespace Sonic_Heroes_AP_Client;

public class Level
{
    public LevelId LevelId;
    public Team? Story;
    public bool IsBoss;
    private bool _isUnlocked;
    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            _isUnlocked = value;
            Mod.SaveDataHandler.SetLevelActive(LevelId, IsBoss, Story, value);
        }
    }
    
    public void RefreshUnlockStatus()
    {
        IsUnlocked = _isUnlocked; 
    }
    
    public Level(string levelCode)
    {
        var storyId = levelCode[0].ToString().ToLower();
        Story = storyId switch
        {
            "s" => Team.Sonic,
            "d" => Team.Dark,
            "r" => Team.Rose,
            "c" => Team.Chaotix,
            "b" => null,
            _ => Team.Sonic
        };
        LevelId = (LevelId)int.Parse(levelCode[1..]);
        IsBoss = Story == null;
    }
}

public class GateDatum
{
    private readonly SlotData _slotData;
    public int Index { get; private set; }
    public int BossCost;
    public List<Level> Levels;
    public Level BossLevel;
    private bool _isUnlocked;
    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            _isUnlocked = value;
            foreach (var level in Levels)
                level.IsUnlocked = value;
        }
    }

    public GateDatum Next()
    {
        return Index != _slotData.GateData.Count - 1 ? _slotData.GateData[Index + 1] : this;
    }

    public GateDatum Previous()
    {
        return Index != 0 ? _slotData.GateData[Index - 1] : this;
    }
    
    public void RefreshUnlockStatus()
    {
        IsUnlocked = _isUnlocked;
    }

    public GateDatum(SlotData slotData, int index, int bossCost, string[] levelIndices, string bossLevel)
    {
        _slotData = slotData;
        Index = index;
        BossCost = bossCost;
        Levels = new List<Level>();
        foreach(var levelIndex in levelIndices)
            Levels.Add(new Level(levelIndex));
        BossLevel = new Level(bossLevel);
    }
}

public class SlotData
{
    public List<GateDatum> GateData;
    public Goal Goal;
    public GoalUnlockCondition GoalUnlockCondition;
    public bool RingLinkOverlord;
    public bool SkipMetalMadness
    {
        set
        {
            GameHandler.SetSkipMadness(value);
        }
    }

    public Rank RequiredRank;
    private bool _dontLoseBonusKey;
    public bool DontLoseBonusKey    
    {
        get => _dontLoseBonusKey;
        set
        {
            _dontLoseBonusKey = value;
            GameHandler.SetDontLoseBonusKey(_dontLoseBonusKey);
        }
    }
    public Dictionary<Team, bool> StoriesActive;
    private bool _modernRingLoss;
    public bool ModernRingLoss
    {
        get => _modernRingLoss;
        set
        {
            _modernRingLoss = value;
            GameHandler.SetRingLoss(_modernRingLoss);
        }
    }
    // ACTS --------
    public bool Act1Enabled = false;
    public bool Act2Enabled = false;
    // SANITY ------
    public bool Darksanity = false;
    public int DarksanityCheckSize = 10;
    public bool Rosesanity = false;
    public int RosesanityCheckSize = 10;
    public bool Chaotixsanity = false;
    public int ChaotixsanityRingCheckSize = 10;
    // -------------
    public bool RingLink;
    public bool DeathLink;

    public SlotData(Dictionary<string, object> slotDict)
    {
        GateData = new List<GateDatum>();

        
        foreach (var x in slotDict)
        {
            Console.WriteLine($"{x.Key} {x.Value}");
        }
        
        var gateLevelCounts = ((JArray)slotDict["GateLevelCounts"]).ToObject<int[]>();
        var gateEmblemCosts = ((JArray)slotDict["GateEmblemCosts"]).ToObject<int[]>();
        var shuffledLevels = ((JArray)slotDict["ShuffledLevels"]).ToObject<string[]>();
        var shuffledBosses = ((JArray)slotDict["ShuffledBosses"]).ToObject<string[]>();
        var version = slotDict["ModVersion"].ToString();
        if (version != Mod.ModConfig.ModVersion)
            Logger.Log("Your apworld is out of date. This might be fine but please check for updates.");
        var runningLevelCount = 0;
        for (var gateIndex = 0; gateIndex < gateEmblemCosts.Length; gateIndex++)
        {
            var gateLevelStrings = shuffledLevels.Skip(runningLevelCount).Take(gateLevelCounts[gateIndex]).ToArray();
            var bossLevelString = shuffledBosses[gateIndex];
            GateData.Add(new GateDatum(
                this,
                gateIndex,
                gateEmblemCosts[gateIndex],
                gateLevelStrings,
                bossLevelString
            ));
            if (gateIndex == 0)
                GateData[gateIndex].IsUnlocked = true;
            runningLevelCount += gateLevelCounts[gateIndex];
        }
        Goal = (Goal)(int)(long)slotDict["Goal"];
        GoalUnlockCondition = (GoalUnlockCondition)(int)(long)slotDict["GoalUnlockCondition"];
        SkipMetalMadness = (long)slotDict["SkipMetalMadness"] == 1;
        RequiredRank = (Rank)(int)(long)slotDict["RequiredRank"];
        DontLoseBonusKey = (long)slotDict["DontLoseBonusKey"] == 1;
        StoriesActive = new Dictionary<Team, bool>()
        {
            { Team.Sonic, (long)slotDict["SonicStory"] == 1 },
            { Team.Dark, (long)slotDict["DarkStory"] == 1 },
            { Team.Rose, (long)slotDict["RoseStory"] == 1 },
            { Team.Chaotix, (long)slotDict["ChaotixStory"] == 1 }
        };
        ModernRingLoss = (long)slotDict["ModernRingLoss"] == 1;
        
        Darksanity = (long)slotDict["DarkSanity"] == 1;
        DarksanityCheckSize = (int)(long)slotDict["DarkSanityEnemyInterval"];
        Rosesanity = (long)slotDict["RoseSanity"] == 1;
        RosesanityCheckSize = (int)(long)slotDict["RoseSanityRingInterval"];
        Chaotixsanity = (long)slotDict["ChaotixSanity"] == 1;
        ChaotixsanityRingCheckSize = (int)(long)slotDict["ChaotixSanityRingInterval"];

        Act1Enabled = (long)slotDict["EnableMissionA"] == 1;
        Act2Enabled = (long)slotDict["EnableMissionB"] == 1;

        RingLinkOverlord = (long)slotDict["RingLinkOverlord"] == 1;
        
        List<string> tags = new List<string>();
        DeathLink = Mod.Configuration?.DeathLink switch
        {
            Config.DeathLinkTag.UseYaml => (long)slotDict["DeathLink"] == 1,
            Config.DeathLinkTag.OverrideOn => true,
            Config.DeathLinkTag.OverrideOff => false,
            _ => false
        };
        if (DeathLink)
            tags.Add("DeathLink");
        RingLink = Mod.Configuration?.RingLink switch
        {
            Config.RingLinkTag.UseYaml => (long)slotDict["RingLink"] == 1,
            Config.RingLinkTag.OverrideOn => true,
            Config.RingLinkTag.OverrideOff => false,
            _ => false
        };
        if (RingLink)
            tags.Add("RingLink");
        Mod.ArchipelagoHandler.UpdateTags(tags);
    }
    
    public void RecalculateOpenLevels()
    {
        unsafe
        {
            Mod.SaveDataHandler.SaveData->EmblemCount = (byte)Mod.SaveDataHandler.CustomData->EmblemCount;
            
            var finalGate = GateData.First(x => x.BossLevel.LevelId == LevelId.MetalMadness);
            var hasEmeralds = true;
            var hasEmblemsForMetal = true;
            if (GoalUnlockCondition is GoalUnlockCondition.Emeralds or GoalUnlockCondition.EmblemsEmeralds)
                for (var emeraldIndex = 0; emeraldIndex < 7; emeraldIndex++)
                    if (Mod.SaveDataHandler.CustomData->Emeralds[emeraldIndex] == 0)
                        hasEmeralds = false;
            if (GoalUnlockCondition is GoalUnlockCondition.Emblems or GoalUnlockCondition.EmblemsEmeralds)
                if (Mod.SaveDataHandler.CustomData->EmblemCount < finalGate.BossCost)
                    hasEmblemsForMetal = false;

            finalGate.BossLevel.IsUnlocked = (hasEmblemsForMetal && hasEmeralds && finalGate.IsUnlocked) 
                                             || (GoalUnlockCondition is GoalUnlockCondition.Emeralds && hasEmeralds);
            Mod.SaveDataHandler.CustomData->GateBossUnlocked[finalGate.Index] = finalGate.BossLevel.IsUnlocked ? (byte)1 : (byte)0;

            foreach (var gate in GateData.Where(gate => Mod.SaveDataHandler.CustomData->GateBossComplete[gate.Index] == 1))
                gate.Next().IsUnlocked = true;
                
            foreach (var gate in GateData.Where(gate => gate.IsUnlocked && 
                                                        Mod.SaveDataHandler.CustomData->EmblemCount >= gate.BossCost &&
                                                        gate.BossLevel.LevelId != LevelId.MetalMadness))
            {
                gate.BossLevel.IsUnlocked = true;
                Mod.SaveDataHandler.CustomData->GateBossUnlocked[gate.Index] = 1;
                Mod.ArchipelagoHandler.Save();
            }
        }
        foreach (var gate in GateData)
        {
            gate.RefreshUnlockStatus();
            gate.BossLevel.RefreshUnlockStatus();
        }
    }

    public int? FindGateForLevel(LevelId levelId, Team storyId)
    {
        foreach (var gate in GateData.Where(gate => gate.Levels.Any(x => x.LevelId == levelId && x.Story == storyId || gate.BossLevel.LevelId == levelId)))
            return gate.Index;
        return null;
    }
}