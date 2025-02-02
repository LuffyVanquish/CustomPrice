using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace CustomPrice;

//Given a version number MAJOR.MINOR.PATCH, increment the:
//MAJOR version when you make incompatible API changes
//MINOR version when you add functionality in a backward compatible manner
//PATCH version when you make backward compatible bug fixes

[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("com.sigurd.csync", "5.0.0")]
public class Plugin : BaseUnityPlugin
{

    internal const String modGUID = "luffyvanquish.CustomPrice";
    private const String modName = "CustomPrice";
    private const String modVersion = "1.0.3";

    private static readonly Harmony harmony = new(modGUID);

    internal new static ManualLogSource Logger { get; private set; } = null!;

    internal new static CustomConfig Config { get; private set; } = null!;

    private void Awake()
    {
        
        Logger = base.Logger;
        Config = new CustomConfig(base.Config,Logger);

       
        Config.InitialSyncCompleted += (sender, args) => {
            Logger.LogInfo("Initial sync complete!");
        };


        harmony.PatchAll(typeof(StartingCreditPatch));
        harmony.PatchAll(typeof(CustomPricePatch));

        Logger.LogInfo(modGUID+" Loaded.");

    }
}