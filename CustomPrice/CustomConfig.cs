using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CSync.Extensions;
using CSync.Lib;
using DunGen;
using System.Collections.Generic;
using System.Text;

namespace CustomPrice;

public class CustomConfig : SyncedConfig2<CustomConfig>
{


    // LOCAL VAR /////////////////////////////////////////////////////////////

    private ManualLogSource logger;
    private ConfigFile cfg;


    // LOCAL CONFIG //////////////////////////////////////////////////////////
    private ConfigEntry<bool> Debug { get; set; }


    // SYNCED CONFIG //////////////////////////////////////////////////////////
    [field: SyncedEntryField]
    public SyncedEntry<int> StartCredit { get; private set; }

    [field: SyncedEntryField]
    public SyncedEntry<string> ItemsShopPrice { get; private set; }

    [field: SyncedEntryField]
    public SyncedEntry<string> VehiclesPrice { get; private set; }



    ///////////////////////////////////////////////////////////////////////////

    public CustomConfig(ConfigFile cfg, ManualLogSource logger) : base(Plugin.modGUID)
    {
        // Init Local var
        this.logger = logger;
        this.cfg = cfg;



        // Var showing console Debug
        Debug = cfg.Bind(
            new ConfigDefinition("Debug", "ShowDebug"),
            false,
            new ConfigDescription("Show debuging information in the console.")
            );


        // Var Starting money
        StartCredit = SyncedBindingExtensions.BindSyncedEntry<int>(
             cfg,
             "Start Credit",
             "StartCredit", 
             60,
             "Starting money."
             );

        // Var Shop item
        ItemsShopPrice = SyncedBindingExtensions.BindSyncedEntry<string>(
             cfg,
             "Items",
             "ItemsPrice",
             "",
             "Price ofr each item"
             );

        // Var Vehicle
        VehiclesPrice = SyncedBindingExtensions.BindSyncedEntry<string>(
            cfg,
            "Vehicles",
            "VehiclesPrice",
            "",
            "Price of each Vehicle"
            );

   
        // Listener synced config
        StartCredit.Changed += (sender, args) =>
        {
            logger.LogInfo("StartCredit value : ");
            logger.LogInfo($"The old value was {args.OldValue}");
            logger.LogInfo($"The new value is {args.NewValue}");
        };


        ConfigManager.Register<CustomConfig>((SyncedConfig2<CustomConfig>)(object)this);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /**
     * Text to show in console if Debug is activate in config file
     * String Text : Text to show 
     */
    public void ShowConsole(string text)
    {
        if (Debug.Value)
        {
            this.logger.LogMessage(text);
        }
    }


    public void ShowConsoleErr(string text)
    {
        if (Debug.Value)
        {
            this.logger.LogError(text);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /**
     * Register new shop item
     */
    public void AddShopItems(SyncedEntry<string> entryCfg , string itemName, string itemPrice)
    {
        string item = itemName + ":" + itemPrice;
        string currentItems = entryCfg.LocalValue; // type : "a:1,b:2,c:3" TODO : transfo string -> dictionnary


        if (currentItems.IsNullOrWhiteSpace()) // Add first item 
        {
            entryCfg.LocalValue = item;
        }else if(!currentItems.Contains(itemName+":")) // Add new item to config List if is not already register
        {

            Dictionary<string, int> items = StringToDict(currentItems);

            items.Add(itemName, int.Parse(itemPrice));
            entryCfg.LocalValue = DictToString(items);

        }

    }

    /**
     * Get shop item's price
     */
    public int GetShopItemPrice(SyncedEntry<string> entryCfg, string itemName)
    {
        Dictionary<string, int> items = StringToDict(entryCfg.Value);
        
        return items[itemName];
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /**
     * Convert String to Dictionnary
     */
    private Dictionary<string,int> StringToDict(string str)
    {
        Dictionary<string, int> keyValuePairs = new();

        foreach (string item in str.Trim().Split(','))
        {
            string[] keyVal = item.Split(':');
            keyValuePairs.Add(keyVal[0],int.Parse(keyVal[1]));
        }

        return keyValuePairs;

    }

    /**
     * Convert Dictionnary to String
     */
    private string DictToString<T,K>(Dictionary<K, T> dict)
    {
        StringBuilder keyValuePairsString = new();

        foreach(K key in dict.Keys)
        {
            keyValuePairsString.Append(key + ":" + dict[key] + ",");
        }

        keyValuePairsString.Length--;

        return keyValuePairsString.ToString();

    }




}