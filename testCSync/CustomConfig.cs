using BepInEx;
using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;

namespace TestCSync.ExamplePlugin;

public class CustomConfig : SyncedConfig2<CustomConfig>
{

    public ConfigEntry<bool> Debug { get; private set; }

    [field: SyncedEntryField]
    public SyncedEntry<float> TestValue { get; private set; }

    [field: SyncedEntryField]
    public SyncedEntry<int> TestValue2 { get; private set; }

    public CustomConfig(ConfigFile cfg) : base(Plugin.modGUID)
    {

        Debug = cfg.Bind(
            new ConfigDefinition("Debug", "Show Debug"),
            true,
            new ConfigDescription("Show debuging information in the console.")
            );


        TestValue = SyncedBindingExtensions.BindSyncedEntry<float>(
             cfg, 
             "test", 
             "test float value", 
             3.9f, 
             "test value descr."
             );

        TestValue2 = SyncedBindingExtensions.BindSyncedEntry<int>(
             cfg,
             "test2",
             "test2 int value",
             5,
             "test2 value descr."
             );


        Plugin.Logger.LogInfo("TestValue value : ");
        TestValue.Changed += (sender, args) =>
        {
            Plugin.Logger.LogInfo($"The old value was {args.OldValue}");
            Plugin.Logger.LogInfo($"The new value is {args.NewValue}");
        };

        Plugin.Logger.LogInfo("TestValue2 value : ");
        TestValue2.Changed += (sender, args) =>
        {
            Plugin.Logger.LogInfo($"The old value was {args.OldValue}");
            Plugin.Logger.LogInfo($"The new value is {args.NewValue}");
        };
        
        

        ConfigManager.Register<CustomConfig>((SyncedConfig2<CustomConfig>)(object)this);
    }
}