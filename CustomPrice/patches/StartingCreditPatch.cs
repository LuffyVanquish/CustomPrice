using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using CSync;
using HarmonyLib;


namespace CustomPrice;

[HarmonyPatch(typeof(Terminal))]
internal class StartingCreditPatch
{

    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    static void  SetStartingCredit(ref int ___groupCredits) {
        
        CustomConfig config = Plugin.Config;

        int startMoney = config.StartCredit.Value;

        config.ShowConsole("Start money set to " + startMoney);
            
        ___groupCredits = startMoney;

    }
}


