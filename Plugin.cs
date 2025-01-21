using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace TerminalScannerPlus
{
    [BepInPlugin("TerminalScannerPlus.mod", "Terminal Scanner Plus", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony harmony = new Harmony("TerminalScannerPlus.mod");
            ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource("Terminal Scanner Plus");
            mls.LogInfo("Loaded Succesfully");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Terminal), "TextPostProcess")]
    public class TextPostProcessPatch
    {
        
        public static void Postfix(ref string __result)
        {
            if (__result.Contains("objects outside the ship, totalling at an approximate value of"))
            {
                var objects = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
                var sortedObjects = objects.Where(obj => obj.itemProperties.isScrap && !obj.isInShipRoom && !obj.isInElevator).OrderBy(obj => obj.itemProperties.itemName).ToList();
                string items = "";
                int num1 = 0;
                int num2 = 0;

                foreach (var obj in sortedObjects)
                {
                    if (obj.itemProperties.isScrap && !obj.isInShipRoom && !obj.isInElevator)
                    {
                        items += $"\n* {obj.itemProperties.itemName} // ${obj.scrapValue}";
                        num1++;
                        num2 += obj.scrapValue;
                    }
                }
                __result = $"\n\n\nThere are {num1} objects outside the ship, totalling a value of ${num2}.\n{items}\n\n";
            }
        }
    }
}