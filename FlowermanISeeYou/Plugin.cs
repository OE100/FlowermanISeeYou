using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowermanISeeYou
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(GUID);

        private const string GUID = "oe.tweaks.sounds.flowerman";
        private const string NAME = "Flowerman I See You";
        private const string VERSION = "1.0.0";

        internal static Plugin instance;

        internal static BepInEx.Logging.ManualLogSource log;

        private void Awake()
        {
            log = this.Logger;
            log.LogInfo("'Flowerman I See You' is loading...");

            if (instance == null)
                instance = this;

            harmony.PatchAll();

            log.LogInfo("'Flowerman I See You' loaded!");
        }
    }
}
