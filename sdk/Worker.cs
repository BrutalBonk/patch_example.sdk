using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace com.hokjhas.muck_patch.sdk
{
    public static class Worker
    {
        static readonly string PluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Application.dataPath;
        static readonly string ConfigPath = Path.Combine(PluginDir, "cfg.json");

        public static FieldInfo GetPrivateVar<T>(string variable)
        {
            return typeof(T).GetField(variable, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetPrivateMethod<T>(string method)
        {
            return AccessTools.Method(typeof(T), method);
        }
    }
}
