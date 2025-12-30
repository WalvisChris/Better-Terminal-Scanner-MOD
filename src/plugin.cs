using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterTerminalScanner
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.chris.scrap.terminal";
        private const string NAME = "Scrap Terminal";
        private const string VERSION = "1.0.3";

        internal static ManualLogSource mls;
        internal static Harmony harmony = new Harmony(GUID);

        private void Awake()
        {
            mls = Logger;
            harmony.PatchAll(typeof(TerminalPatch));
            mls.LogInfo("Loaded succesfully");
        }
    }
}
