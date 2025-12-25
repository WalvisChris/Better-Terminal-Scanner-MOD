using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace TerminalScannerPlus
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatch
    {
        [HarmonyPatch("TextPostProcess")]
        [HarmonyPrefix]
        private static bool TextPostProcessPrefix(ref string modifiedDisplayText, TerminalNode node, ref string __result, Terminal __instance)
        {
            string keyword = modifiedDisplayText.Trim();
            
            if (keyword.Contains("[scanForItems]")) 
            {
                __result = Utilities.ScanDisplayText();
                return false;
            }
            return true;
        }
    }
}
