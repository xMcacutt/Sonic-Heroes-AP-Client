using System.Data;
using System.Globalization;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Sonic_Heroes_AP_Client.SoundHandler;

namespace Sonic_Heroes_AP_Client;

public class ArchipelagoHandler
{
    private const string GAME_NAME = "Sonic Heroes";
    public SlotData SlotData;
    private ArchipelagoSession _session;
    private LoginSuccessful _loginSuccessful;
    private string Server { get; set; }
    private int Port { get; set; }
    private string Slot { get; set; }
    private string? Seed { get; set; }
    private string Password { get; set; }
    private double SlotInstance { get; set; }
    public static int TempIndex;
    
    public static bool IsConnected;
    public static bool IsConnecting;

    public ArchipelagoHandler(string server, int port, string slot, string password)
    {
        Server = server;
        Port = port;
        Slot = slot;
        Password = password;
        CreateSession();
    }

    private void CreateSession()
    {
        TempIndex = 0;
        SlotInstance = DateTime.Now.ToUnixTimeStamp();
        _session = ArchipelagoSessionFactory.CreateSession(Server, Port);
        _session.MessageLog.OnMessageReceived += OnMessageReceived;
        _session.Socket.SocketClosed += OnSocketClosed;
        _session.Items.ItemReceived += ItemReceived;
        _session.Socket.PacketReceived += PacketReceived;
    }

    private void OnSocketClosed(string reason)
    {
        Console.WriteLine($"Connection closed ({reason}) Attempting reconnect...");
        IsConnected = false;
    }
    
    public void InitConnect()
    {
        IsConnecting = true;
        CreateSession();
        IsConnected = Connect();
        IsConnecting = false;
    }

    private bool Connect()
    {
        LoginResult result;

        try
        {
            Mod.GameHandler = new GameHandler();
            Mod.SaveDataHandler = new SaveDataHandler();
            Mod.SanityHandler = new SanityHandler();
            Mod.TrapHandler = new TrapHandler();
            Seed = _session.ConnectAsync()?.Result?.SeedName;
            Console.WriteLine(Seed + Slot);
            if (Seed != null)
                Mod.SaveDataHandler.LoadSaveData(Seed, Slot);
            result = _session.LoginAsync(
                game: GAME_NAME, 
                name: Slot,
                itemsHandlingFlags: ItemsHandlingFlags.AllItems, 
                version: new Version(1, 0, 0),
                tags: new string[] {},
                password: Password
            ).Result;
        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
        }

        if (result.Successful)
        {
            _loginSuccessful = (LoginSuccessful)result;
            SlotData = new SlotData(_loginSuccessful.SlotData);
            Mod.InitOnConnect();
            return true;
        }
        var failure = (LoginFailure)result;
        var errorMessage = $"Failed to Connect to {Server}:{Port} as {Slot}:";
        errorMessage = failure.Errors.Aggregate(errorMessage, (current, error) => current + $"\n    {error}");
        errorMessage = failure.ErrorCodes.Aggregate(errorMessage, (current, error) => current + $"\n    {error}");
        Console.WriteLine(errorMessage);
        Console.WriteLine($"Attempting reconnect...");
        return false;
    }

    private void ItemReceived(ReceivedItemsHelper helper)
    {
        var item = helper.DequeueItem();
        var name = item.ItemName;
        Mod.ItemHandler?.HandleItem(helper.Index, item);
    }

    private void PacketReceived(ArchipelagoPacketBase packet)
    {
        switch (packet)
        {
            case BouncePacket bouncePacket:
                BouncePacketReceived(bouncePacket);
                break;
        }
    }

    private void HandleDeathLink(string source, string cause)
    {
        if (!SlotData.DeathLink)
            return;
        if (source == Slot)
            return;
        Console.WriteLine($"{source} {cause}");
        Mod.GameHandler.Kill();
    }

    private void HandleRingLink(string source, string amountStr)
    {
        if (!SlotData.RingLink)
            return;
        if (!Mod.ArchipelagoHandler.SlotData.RingLinkOverlord
            && Mod.GameHandler.GetCurrentLevel() == LevelId.MetalOverlord)
            return;
        if (source == SlotInstance.ToString(CultureInfo.InvariantCulture))
            return;
        if (!int.TryParse(amountStr, out var amount))
            return;
        var ringCount = Mod.GameHandler.GetRingCount();
        var newAmount = Math.Max(Math.Min(ringCount + amount, 999), 0);
        if (Mod.GameHandler.InGame())
        {
            switch (amount)
            {
                case 1:
                    PlaySound((int)Mod.ModuleBase, 0x1004);
                    break;
                case > 1:
                    PlaySound((int)Mod.ModuleBase, 0x1033);
                    break;
                case < 0:
                    PlaySound((int)Mod.ModuleBase, 0x1005);
                    break;
            }
        }
        Mod.GameHandler.SetRingCount(newAmount);
    }

    private string lastDeath;
    private string lastRing;
    private void BouncePacketReceived(BouncePacket packet)
    {
        if (SlotData.DeathLink)
            ProcessBouncePacket(packet, "DeathLink", ref lastDeath, (source, data) =>
                HandleDeathLink(source, data["cause"]?.ToString() ?? "Unknown")); 

        if (SlotData.RingLink)
            ProcessBouncePacket(packet, "RingLink", ref lastRing, (source, data) =>
                HandleRingLink(source, data["amount"]?.ToString() ?? "0"));
    }

    private static void ProcessBouncePacket(BouncePacket packet, string tag, ref string lastTime, Action<string, Dictionary<string, JToken>> handler)
    {
        if (!packet.Tags.Contains(tag)) return;
        if (tag == "DeathLink")
            GameHandler.SomeoneElseDied = true;
        if (!packet.Data.TryGetValue("time", out var timeObj)) 
            return;
        if (lastTime == timeObj.ToString())
            return;
        lastTime = timeObj.ToString();
        if (!packet.Data.TryGetValue("source", out var sourceObj)) 
            return;
        var source = sourceObj?.ToString() ?? "Unknown";
        handler(source, packet.Data);
    }

    public void SendRing(int amount)
    {
        BouncePacket packet = new BouncePacket();
        var now = DateTime.Now;
        packet.Tags = new List<string>();
        packet.Tags.Add("RingLink");
        packet.Data = new Dictionary<string, JToken>();
        packet.Data.Add("time", now.ToUnixTimeStamp());
        packet.Data.Add("source", SlotInstance);
        packet.Data.Add("amount", amount);
        _session.Socket.SendPacket(packet);
    }
    
    public void SendDeath()
    {
        BouncePacket packet = new BouncePacket();
        var now = DateTime.Now;
        packet.Tags = new List<string>();
        packet.Tags.Add("DeathLink");
        packet.Data = new Dictionary<string, JToken>();
        packet.Data.Add("time", now.ToUnixTimeStamp());
        packet.Data.Add("source", Slot);
        packet.Data.Add("cause", "died");
        _session.Socket.SendPacket(packet);
    }
    
    public void Release()
    {
        _session.SetGoalAchieved();
        _session.SetClientState(ArchipelagoClientState.ClientGoal);
    }

    public void CheckLocations(Int64[] ids)
    {
        _session.Locations.CompleteLocationChecks(ids.Select(x => x + 0x93930000).ToArray());
    }
    
    public void CheckLocation(Int64 id)
    {
        _session.Locations.CompleteLocationChecks(id + 0x93930000);
    }

    public void UpdateTags(List<string> tags)
    {
        var packet = new ConnectUpdatePacket
        {
            Tags = tags.ToArray(),
            ItemsHandling = ItemsHandlingFlags.AllItems
        };
        _session.Socket.SendPacket(packet);
    }
    
    static void OnMessageReceived(LogMessage message)
    {
        Console.WriteLine(message.ToString());
    }

    public void Save()
    {
        Mod.SaveDataHandler?.SaveGame(Seed, Slot);
    }
    
}