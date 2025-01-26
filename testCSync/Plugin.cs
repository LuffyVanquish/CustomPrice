using System;
using BepInEx;
using BepInEx.Logging;

namespace TestCSync.ExamplePlugin;

[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("com.sigurd.csync", "5.0.0")]
public class Plugin : BaseUnityPlugin
{

    internal const String modGUID = "TestCSync.testMod";
    private const String modName = "testCSync";
    private const String modVersion = "0.0.0.1";

    internal new static ManualLogSource Logger { get; private set; } = null!;

    internal new static CustomConfig Config { get; private set; } = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Config = new CustomConfig(base.Config);

       
        Config.InitialSyncCompleted += (sender, args) => {
            Logger.LogInfo("Initial sync complete!");
        };


        Logger.LogMessage("TEST MOD Loaded.");
    }
}