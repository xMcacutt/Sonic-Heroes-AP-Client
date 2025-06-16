using System.Windows.Forms;

namespace Sonic_Heroes_AP_Client;

public class StageObjHandler
{
    
    public struct ObjSpawnData
    {
        public float XSpawnPos;
        public float YSpawnPos;
        public float ZSpawnPos;
        public float XSpawnRot;
        public float YSpawnRot;
        public float ZSpawnRot;
        public byte State;
        public byte Team;
        public byte AnotherState;
        public byte PaddingByte;
        public int PaddingInt;
        public long PaddingLong;
        public byte ObjId;
        public byte ObjList;
        public byte LinkId;
        public byte RenderDistance;
        public int PtrVars;
        public int PaddingInt2;
        public int PtrPrevObj;
        public int PtrNextObj;
        public int PtrDynamicMem;
    }
    public enum StageObjTypes
    {
        None = 0,
        Spring = 1,
        TripleSpring = 2,
        RingGroup = 3,
        HintRing = 4,
        Switch = 5,
        PushPullSwitch = 6,
        TargetSwitch = 7,
        DashPanel = 11,
        DashRing = 12,
        RainbowHoops = 13,
        Checkpoint = 14,
        DashRamp = 15,
        Cannon = 16,
        SpikeBall = 21,
        LaserFence = 22,
        ItemBox = 24,
        ItemBalloon = 25,
        GoalRing = 27,
        Pulley = 29,
        Chao = 35,
        CageBox = 36,
        FormationSign = 37,
        FormationChangeGate = 38,
        Pole = 41,
        PowerGong = 44,
        Fan = 46,
        Case = 49,
        WarpFlower = 50,
        BonusKey = 103,
        //SeasideHill
        SeasideHillCementBlock = 260,
        MovingRuinPlatform = 261,
        SmallStonePlatform = 392, 
        //OceanPalace
        CrumblingStonePillar = 512,
        FallingStoneStructure = 513,
        BreakableDoor = 514,
        BreakableBlockOceanPalace = 515,
        Kaos = 516,
        //Casino Park
        SmallBumper = 1280,
        GreenFloatingBumperSpring = 1281,
        PinballFlipper = 1282,
        TriangleBumper = 1283,
        GlassStarBumperPanel = 1284,
        AirGlassStarBumperPanel = 1285,
        LargeTriangleBumper = 1286,
        LargeCasinoDoor = 1288,
        BreakableGlassFloor = 1289,
        FloatingDice = 1290,
        TripleSlot = 1291,
        SingleSlot = 1292,
        DashArrow = 1296,
        //Enemies
        EggFlapper = 5376,
        EggPawn = 5392,
        Klagen = 5408,
        Cameron = 5488,
    }

    public static Dictionary<StageObjTypes, byte[]> StageObjTypeBytes = new Dictionary<StageObjTypes, byte[]>
    {
        { StageObjTypes.Spring, [0x01, 0x00] },
        { StageObjTypes.TripleSpring, [0x02, 0x00] },
        { StageObjTypes.RingGroup, [0x03, 0x00] },
        { StageObjTypes.HintRing, [0x04, 0x00] }, 
        { StageObjTypes.Switch, [0x05, 0x00] },
        { StageObjTypes.PushPullSwitch, [0x06, 0x00] },
        { StageObjTypes.TargetSwitch, [0x07, 0x00] },
        { StageObjTypes.DashPanel, [0x0B, 0x00] },
        { StageObjTypes.DashRing, [0x0C, 0x00] },
        { StageObjTypes.RainbowHoops, [0x0D, 0x00] },
        { StageObjTypes.Checkpoint, [0x0E, 0x00] },
        { StageObjTypes.DashRamp, [0x0F, 0x00] },
        { StageObjTypes.Cannon, [0x10, 0x00] },
        { StageObjTypes.SpikeBall, [0x15, 0x00] },
        { StageObjTypes.LaserFence, [0x16, 0x00] },
        { StageObjTypes.ItemBox, [0x18, 0x00] },
        { StageObjTypes.ItemBalloon, [0x19, 0x00] },
        { StageObjTypes.GoalRing, [0x1B, 0x00] },
        { StageObjTypes.Pulley, [0x1D, 0x00] },
        { StageObjTypes.Chao, [0x23, 0x00] },
        { StageObjTypes.CageBox, [0x24, 0x00] },
        { StageObjTypes.FormationSign, [0x25, 0x00] },
        { StageObjTypes.FormationChangeGate, [0x26, 0x00] },
        { StageObjTypes.Pole, [0x29, 0x00] },
        { StageObjTypes.PowerGong, [0x2C, 0x00] },
        { StageObjTypes.Fan, [0x2E, 0x00] },
        { StageObjTypes.Case, [0x31, 0x00] },
        { StageObjTypes.WarpFlower, [0x32, 0x00] },
        { StageObjTypes.BonusKey, [0x67, 0x00] },
        { StageObjTypes.SeasideHillCementBlock, [0x04, 0x01] },
        { StageObjTypes.MovingRuinPlatform, [0x05, 0x01] },
        { StageObjTypes.SmallStonePlatform, [0x88, 0x01] },
        { StageObjTypes.CrumblingStonePillar, [0x00, 0x02] },
        { StageObjTypes.FallingStoneStructure, [0x01, 0x02] },
        { StageObjTypes.BreakableDoor, [0x02, 0x02] },
        { StageObjTypes.BreakableBlockOceanPalace, [0x03, 0x02] },
        { StageObjTypes.Kaos, [0x04, 0x02] },
        { StageObjTypes.SmallBumper, [0x00, 0x05] },
        { StageObjTypes.GreenFloatingBumperSpring, [0x01, 0x05] },
        { StageObjTypes.PinballFlipper, [0x02, 0x05] },
        { StageObjTypes.TriangleBumper, [0x03, 0x05] },
        { StageObjTypes.GlassStarBumperPanel, [0x04, 0x05] },
        { StageObjTypes.AirGlassStarBumperPanel, [0x05, 0x05] },
        { StageObjTypes.LargeTriangleBumper, [0x06, 0x05] },
        { StageObjTypes.LargeCasinoDoor, [0x08, 0x05] },
        { StageObjTypes.BreakableGlassFloor, [0x09, 0x05] },
        { StageObjTypes.FloatingDice, [0x0A, 0x05] },
        { StageObjTypes.TripleSlot, [0x0B, 0x05] },
        { StageObjTypes.SingleSlot, [0x0C, 0x05] },
        { StageObjTypes.DashArrow, [0x10, 0x05] },
        { StageObjTypes.EggFlapper, [0x00, 0x15] },
        { StageObjTypes.EggPawn, [0x10, 0x15] },
        { StageObjTypes.Klagen, [0x20, 0x15] },
        { StageObjTypes.Cameron, [0x70, 0x15] },
    };
    
    public static Dictionary<string, StageObjTypes> StageObjTypeBytesReversed = StageObjTypeBytes.ToDictionary
        (x => $"{x.Value[0]}{x.Value[1]}", x => x.Key);

    public static unsafe void OnObjSetStateSpawned(int esi)
    {
        Console.WriteLine($"OnObjSetStateSpawned called. ESI = {esi:x}");
    }
    
}