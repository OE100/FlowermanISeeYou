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
        internal static Dictionary<FlowermanAI, int> timesTriggeredSound = new Dictionary<FlowermanAI, int>();

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartPatch(FlowermanAI __instance)
        {
            Plugin.log.LogInfo("Patching flowerman sounds...");
            if (flowermanISeeYouSound == null)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(Paths.PluginPath + "\\OE_Tweaks\\Sounds\\flowermansounds");
                if (ab == null)
                {
                    Plugin.log.LogError("Failed to load flowermansounds asset bundle");
                    return;
                }
                flowermanISeeYouSound = ab.LoadAsset<AudioClip>("flowerman_i_see_you.mp3");

            }
            timesTriggeredSound[__instance] = 0;
            __instance.GetComponent<AudioSource>().clip = flowermanISeeYouSound;
            __instance.GetComponent<AudioSource>().volume = 0.8f;
            __instance.GetComponent<AudioSource>().maxDistance = 50f;
            __instance.GetComponent<AudioSource>().spatialBlend = 0.5f;
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void patchFlowermanISeeYouSoundUpdate(FlowermanAI __instance)
        {
            if (flowermanISeeYouSound == null)
            {
                Plugin.log.LogWarning("Flowerman sound asset bundle not found");
                return;
            }

            if (
                timesTriggeredSound[__instance] <= 2 &&
                GameNetworkManager.Instance.localPlayerController.HasLineOfSightToPosition(__instance.transform.position + Vector3.up * 0.5f, 30f) &&
                __instance.currentBehaviourStateIndex != 2 &&
                !__instance.GetComponent<AudioSource>().isPlaying
                )
            {
                __instance.GetComponent<AudioSource>().Play();
                timesTriggeredSound[__instance]++;
            }
        }

        [HarmonyPatch(typeof(RoundManager), "GenerateNewFloor")]
        [HarmonyPrefix]
        private static void resetSavedData()
        {
            timesTriggeredSound.Clear();
        }
    }
}
