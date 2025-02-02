using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSync.Lib;
using HarmonyLib;


// TODO
// ERREUR INDEX PLAYER askip >5 ~~
// PRIX VEHICULE X
// NOT IN STOCK ??



namespace CustomPrice;


internal class CustomPricePatch
{

    private static readonly CustomConfig config = Plugin.Config;
    private static Item[] buyableItemsList;
    private static BuyableVehicle[] buyableVehicles;

    private static readonly SyncedEntry<string> itemCfg = config.ItemsShopPrice;
    private static readonly SyncedEntry<string> vehicleCfg = config.VehiclesPrice;

    [HarmonyPatch(typeof(Terminal), "SetItemSales")]
    [HarmonyPostfix]
    static void ChangePrice(ref Item[] ___buyableItemsList, ref BuyableVehicle[] ___buyableVehicles)
    {
        try
        {
            // Shop items
            if (___buyableItemsList != null)
            {
                config.ShowConsole("Loading the new price of " + ___buyableItemsList.Length + " shop items.");

                foreach (Item item in ___buyableItemsList)
                {
                    string itemName = item.itemName.Trim();

                    config.AddShopItems(itemCfg, itemName, item.creditsWorth.ToString());

                    try
                    {
                        int price = config.GetShopItemPrice(itemCfg, itemName);
                        config.ShowConsole("Set " + itemName + " price to " + price + "$");
                        item.creditsWorth = price;
                    }
                    catch (KeyNotFoundException)
                    {
                        config.ShowConsole($"{itemName} not found in config.");
                    }

                }

                buyableItemsList = ___buyableItemsList;
            }
        }
        catch (Exception e) // Catch error during changing item's prices
        {
            config.ShowConsoleErr("Error during changing item's prices");
            config.ShowConsoleErr("CustomPrice - ChangePrice - buyableItemsList : ");
            config.ShowConsoleErr(e.ToString());
        }

        try
        {
            //Shop Vehicles
            if (___buyableVehicles != null)
            {
                config.ShowConsole("Loading the new price of " + ___buyableVehicles.Length + " vehicle.");


                foreach (BuyableVehicle vehicle in ___buyableVehicles)
                {
                    string vehicleName = vehicle.vehicleDisplayName.Trim();
                    config.AddShopItems(vehicleCfg, vehicleName, vehicle.creditsWorth.ToString());

                    try
                    {
                        int price = config.GetShopItemPrice(vehicleCfg, vehicleName);
                        config.ShowConsole("Set " + vehicleName + " price to " + price + "$");
                        vehicle.creditsWorth = price;
                    }
                    catch (KeyNotFoundException)
                    {
                        config.ShowConsole($"{vehicleName} not found in config.");
                    }

                }
                buyableVehicles = ___buyableVehicles;
            }
        }
        catch (Exception e) // Catch error during changing véhicle's prices
        {
            config.ShowConsoleErr("Error during changing vehicle's prices");
            config.ShowConsoleErr("CustomPrice - ChangePrice - buyableVehicles : ");
            config.ShowConsoleErr(e.ToString());
        }


    }

    [HarmonyPatch(typeof(Terminal), "LoadNewNodeIfAffordable")]
    [HarmonyPrefix]
    static void ChangePriceAfford(TerminalNode node)
    {

        if (node == null) return;
        int itemIndex;

        try { 
            if (node.buyVehicleIndex != -1) // Is vehicle
            {
                itemIndex = node.buyVehicleIndex;
                BuyableVehicle vehicle = buyableVehicles.Count() >= itemIndex  ? buyableVehicles[itemIndex] : null;

            
                if (vehicle == null) return; // check if vehicle is not null

                string vehicleName = vehicle.vehicleDisplayName;
                config.ShowConsole("("+itemIndex+") "+ vehicleName + " : " + node.itemCost);

                node.itemCost = config.GetShopItemPrice(vehicleCfg, vehicleName);

            }
        }
        catch (Exception e) // Catch error during changing afford véhicle's prices
        {
            config.ShowConsoleErr("Error during changing afford véhicle's prices");
            config.ShowConsoleErr("CustomPrice - ChangePriceAfford - buyVehicleIndex : ");
            config.ShowConsoleErr(e.ToString());
        }

        try
        {
            if (node.buyItemIndex != -1) // Is Item
            {
                itemIndex = node.buyItemIndex;
                Item item = buyableItemsList.Count() >= itemIndex ? buyableItemsList[itemIndex] : null;

                if (item == null) return; // check if item is not null

                string itemName = item.itemName;
                config.ShowConsole("(" + itemIndex + ") " + itemName + " : " + node.itemCost);

                node.itemCost = config.GetShopItemPrice(itemCfg, itemName);

            }
        }
        catch (Exception e) // Catch error during changing afford item's prices
        {
            config.ShowConsoleErr("Error during changing afford véhicle's prices");
            config.ShowConsoleErr("CustomPrice - ChangePriceAfford - buyItemIndex : ");
            config.ShowConsoleErr(e.ToString());
        }

    }



}


