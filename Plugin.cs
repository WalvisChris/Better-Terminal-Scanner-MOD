using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TerminalScannerPlus
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "com.chris.scrap.terminal";
        private const string modName = "Scrap Terminal";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony("ScrapTerminal.mod");
        internal static Plugin Instance;
        internal static ManualLogSource mls;

        private void Awake()
        {
            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource("Terminal Scanner Plus");
            mls.LogInfo("Loaded succesfully");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch]
    internal class ScanPatcher
    {
        [HarmonyPatch(typeof(Terminal), "TextPostProcess")]
        [HarmonyPrefix]
        private static bool Prefix(ref string modifiedDisplayText, TerminalNode node, ref string __result, Terminal __instance)
        {          
            string keyword = modifiedDisplayText.Trim();

            if (keyword.Contains("[scanForItems]"))
            {
                if (!GameNetworkManager.Instance.isHostingGame) return true;

                string items = "";
                int num = 0;

                GrabbableObject[] array = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
                GrabbableObject[] sortedArray = array.Where(obj => obj.itemProperties.isScrap && !obj.isInShipRoom && !obj.isInElevator).OrderBy(obj => obj.itemProperties.itemName).ToArray();

                if (sortedArray.Length == 0)
                {
                    __result = "\n\n\nNo objects were found.\n\n";
                    return false;
                }

                foreach (var obj in sortedArray)
                {
                    items += $"\n* {obj.itemProperties.itemName} // ${obj.scrapValue}";
                    num += obj.scrapValue;
                }

                __result = $"\n\n\nThere are {sortedArray.Length} objects outside the ship, totalling a value of ${num}.\n{items}\n\n";
                return false;

            }
            return true;
        }
    }
}