using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.TextCore;

namespace jshepler.ngu.framerate
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private void Awake()
        {
            harmony.PatchAll(typeof(Plugin));
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Character), "Start")]
        private static void SetFramerateControls()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 2)
            {
                for (var x = 1; x < args.Length - 1; x++)
                {
                    var arg = args[x].ToLowerInvariant();

                    if (arg == "-vsynccount" && int.TryParse(args[x + 1], out var count))
                        QualitySettings.vSyncCount = count;

                    else if (arg == "-targetframerate" && int.TryParse(args[x + 1], out var rate))
                    {
                        QualitySettings.vSyncCount = 0;
                        Application.targetFrameRate = rate;
                    }
                }
            }
        }
    }
}
