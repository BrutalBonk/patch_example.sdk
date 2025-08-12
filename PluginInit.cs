using BepInEx;
using BepInEx.Logging;
using HarmonyLib.Tools;
using UnityEngine;
using HarmonyLib;
using com.hokjhas.muck_patch.sdk;

namespace com.hokjhas.muck_patch
{
    [BepInPlugin("com.hokjhas.muck_patch", "Plugin", "1.0.1")]
    
    public class Plugin : BaseUnityPlugin
    {
        Harmony Patch = new Harmony("com.hokjhas.muck_patch");
        private void Awake()
        {
            Debug.Log("Loaded");
            Patch.PatchAll();
        }
    }
}
