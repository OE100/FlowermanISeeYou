using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FlowermanISeeYou.Patches
{
    [HarmonyPatch(typeof(FlowermanAI))]
    internal class FlowermanAIPatch
    {
        internal static AudioClip flowermanISeeYouSound = null;
        internal static Dictionary<FlowermanAI, int> timeTriggeredSound = new Dictionary<FlowermanAI, int>();

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPatch(FlowermanAI __instance)
        {
            if (flowermanISeeYouSound == null)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(Plugin.persistentDataPath);
                ab.LoadAsset<AudioClip>("FlowermanISeeYouSound.mp3");
            }
        }
    }
}
