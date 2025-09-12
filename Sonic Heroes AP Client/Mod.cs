using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Heroes.Controller.Hook.Interfaces;
using Reloaded.Hooks.Definitions;
using Reloaded.Imgui.Hook;
using Reloaded.Imgui.Hook.Direct3D9.Definitions;
using Reloaded.Imgui.Hook.Implementations;
using Reloaded.Mod.Interfaces;
using SharpDX.Direct3D9;
using Sonic_Heroes_AP_Client.Template;
using Sonic_Heroes_AP_Client.Configuration;

namespace Sonic_Heroes_AP_Client;

public class Mod : ModBase // <= Do not Remove.
{
    private readonly IModLoader _modLoader;
    private static WeakReference<IControllerHook> _controllerHook;
    private static IReloadedHooks? _hooks;
    private readonly ILogger _logger;
    private readonly IMod _owner;
    public static IModConfig ModConfig;
    public static Config? Configuration { get; private set; }

    public static ArchipelagoHandler? ArchipelagoHandler;
    public static GameHandler? GameHandler;
    public static SaveDataHandler? SaveDataHandler;
    public static ItemHandler? ItemHandler;
    public static SanityHandler? SanityHandler;
    public static TrapHandler? TrapHandler;
    public static StageObjHandler? StageObjHandler;
    public static AbilityUnlockHandler? AbilityUnlockHandler;
    public static LevelSpawnData? LevelSpawnData;
    public static LevelSpawnHandler? LevelSpawnHandler;
    public static Controller? Controller;
    
    public static UIntPtr ModuleBase;
    public static UserInterface? UserInterface;
    public static DXHook? DxHook;
    
    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _controllerHook = _modLoader.GetController<IControllerHook>();
        _logger = context.Logger;
        _owner = context.Owner;
        ModConfig = context.ModConfig;
        Configuration = context.Configuration;
        //DxHook = new DXHook(_hooks)
        SDK.Init(_hooks);
        UserInterface = new UserInterface();
        ModuleBase = (UIntPtr)Process.GetCurrentProcess().MainModule!.BaseAddress;
        LevelSpawnHandler = new LevelSpawnHandler();
        Controller = new Controller(_controllerHook, 0);
        AbilityHandler.SetAllAbilities(false);
        Console.WriteLine($"Module Base Here: 0x{ModuleBase:x}");
        
        if (Configuration == null)
            return;
        ArchipelagoHandler = new ArchipelagoHandler(Configuration.Server, Configuration.Port, Configuration.Slot, Configuration.Password);
        var t = new Thread(start: () =>
        {
            while (true)
            {
                if (!ArchipelagoHandler.IsConnecting && !ArchipelagoHandler.IsConnected)
                {
                    ItemHandler = new ItemHandler();
                    ArchipelagoHandler.InitConnect();
                }
                Thread.Sleep(2500);
            }
        });
        t.Start();
    }
    
    #region Standard Overrides

    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        Configuration = configuration;
        _logger.WriteLine($"[{ModConfig.ModId}] Config Updated: Applying");
    }
    
    public static void InitOnConnect()
    {
        GameHandler?.ModifyInstructions();
        if (_hooks != null) 
            GameHandler?.SetupHooks(_hooks);
    }

    #endregion

    #region For Exports, Serialization etc.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod()
    {
    }
#pragma warning restore CS8618

    #endregion
}