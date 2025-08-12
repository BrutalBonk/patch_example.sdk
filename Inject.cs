using System;
using System.Collections.Generic;
using System.Reflection;
using com.hokjhas.muck_patch.sdk;
using HarmonyLib;
using UnityEngine;

namespace com.hokjhas.muck_patch
{
    [HarmonyPatch]
    internal class ChestsChamsPatch
    {
        private static Material _chamsMaterial;
        private static Dictionary<Renderer, Material> _originalMaterials = new Dictionary<Renderer, Material>();

        static MethodBase TargetMethod()
        {
            return Worker.GetPrivateMethod<MoveCamera>("LateUpdate");
        }

        static bool Prefix()
        {
            _chamsMaterial = Materials.LoadFromBundle("chamsbundle", "chams");

            if (_chamsMaterial.HasProperty("_ColorBehind"))
                _chamsMaterial.SetColor("_ColorBehind", Color.cyan);
            if (_chamsMaterial.HasProperty("_ColorVisible"))
                _chamsMaterial.SetColor("_ColorVisible", Color.white);

            foreach (Renderer rend in renderers)
            {
                if (rend.gameObject.name.IndexOf("chest", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!_originalMaterials.ContainsKey(rend))
                        _originalMaterials[rend] = rend.sharedMaterial;

                    rend.sharedMaterial = true
                        ? _chamsMaterial
                        : _originalMaterials[rend];
                }
            }
            

            return true;
        }
    }
}

