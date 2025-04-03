using System.ComponentModel;
using Sonic_Heroes_AP_Client.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;

namespace Sonic_Heroes_AP_Client.Configuration;

public class Config : Configurable<Config>
{
    /*
        User Properties:
            - Please put all of your configurable properties here.

        By default, configuration saves as "Config.json" in mod user config folder.
        Need more config files/classes? See Configuration.cs

        Available Attributes:
        - Category
        - DisplayName
        - Description
        - DefaultValue

        // Technically Supported but not Useful
        - Browsable
        - Localizable

        The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
    */

    [DisplayName("Host IP")]
    [Description("Host address of the Archipelago server")]
    [DefaultValue("Archipelago.gg")]
    public string Server { get; set; } = "Archipelago.gg";
    
    [DisplayName("Port")]
    [Description("Port open for the Archipelago server")]
    [DefaultValue("55555")]
    public int Port { get; set; } = 55555;

    [DisplayName("Slot")]
    [Description("Slot user name used to connect to the Archipelago server")]
    [DefaultValue("Sonic")]
    public string Slot { get; set; } = "Sonic";

    [DisplayName("Password")]
    [Description("Password for the Archipelago server")]
    [DefaultValue("")]
    public string Password { get; set; } = "";

    [DisplayName("Death Link")]
    [Description("Set how death link will work in the Archipelago session")]
    [DefaultValue(DeathLinkTag.UseYaml)]
    public DeathLinkTag DeathLink { get; set; } = DeathLinkTag.UseYaml;

    public enum DeathLinkTag
    {
        UseYaml,
        OverrideOn,
        OverrideOff
    }
    
    [DisplayName("Ring Link")]
    [Description("Set how ring link will work in the Archipelago session")]
    [DefaultValue(RingLinkTag.UseYaml)]
    public RingLinkTag RingLink { get; set; } = RingLinkTag.UseYaml;

    public enum RingLinkTag
    {
        UseYaml,
        OverrideOn,
        OverrideOff
    }
    
    [DisplayName("Log Message Time")]
    [Description("Set how long each message will display for in seconds in the log")]
    [DefaultValue(5)]
    public int LogMessageTime { get; set; } = 5;

    [DisplayName("Log Message Count")]
    [Description("Set how many messages can be displayed in the log at once.")]
    [DefaultValue(6)]
    public int LogMessageCount { get; set; } = 6;

    /*
    [DisplayName("Int Slider")]
    [Description("This is a int that uses a slider control similar to a volume control slider.")]
    [DefaultValue(100)]
    [SliderControlParams(
        minimum: 0.0,
        maximum: 100.0,
        smallChange: 1.0,
        largeChange: 10.0,
        tickFrequency: 10,
        isSnapToTickEnabled: false,
        tickPlacement: SliderControlTickPlacement.BottomRight,
        showTextField: true,
        isTextFieldEditable: true,
        textValidationRegex: "\\d{1-3}")]
    public int IntSlider { get; set; } = 100;

    [DisplayName("Double Slider")]
    [Description("This is a double that uses a slider control without any frills.")]
    [DefaultValue(0.5)]
    [SliderControlParams(minimum: 0.0, maximum: 1.0)]
    public double DoubleSlider { get; set; } = 0.5;

    [DisplayName("File Picker")]
    [Description("This is a sample file picker.")]
    [DefaultValue("")]
    [FilePickerParams(title: "Choose a File to load from")]
    public string File { get; set; } = "";

    [DisplayName("Folder Picker")]
    [Description("Opens a file picker but locked to only allow folder selections.")]
    [DefaultValue("")]
    [FolderPickerParams(
        initialFolderPath: Environment.SpecialFolder.Desktop,
        userCanEditPathText: false,
        title: "Custom Folder Select",
        okButtonLabel: "Choose Folder",
        fileNameLabel: "ModFolder",
        multiSelect: true,
        forceFileSystem: true)]
    public string Folder { get; set; } = "";
    */
}

/// <summary>
/// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
/// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
/// </summary>
public class ConfiguratorMixin : ConfiguratorMixinBase
{
    // 
}