using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace com.hokjhas.muck_patch.sdk
{
    internal static class Materials
    {
        static Material cachedMat;
        public enum MaterialVariants
        {
            Glow,
            OutlineGlow,
            Glass,
            GlassGlow,
            Shiny
        }

        public static Material GetMaterial(MaterialVariants variant)
        {
            switch (variant)
            {
                case MaterialVariants.Glow:
                    return CreateGlow(Color.cyan, 2f);

                case MaterialVariants.OutlineGlow:
                    return CreateOutlineGlow(Color.yellow);

                case MaterialVariants.Glass:
                    return CreateGlass(new Color(0.5f, 0.7f, 1f, 0.3f));

                case MaterialVariants.GlassGlow:
                    return CreateGlassGlow(Color.cyan, 1.5f);

                case MaterialVariants.Shiny:
                    return CreateShiny(Color.gray);

                default:
                    return null;
            }
        }

        private static Material CreateGlow(Color glowColor, float intensity)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = glowColor * 0.1f; // Dim base
            mat.SetFloat("_Glossiness", 0.6f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", glowColor * intensity);
            return mat;
        }

        private static Material CreateOutlineGlow(Color outlineColor)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = outlineColor * 0.2f;
            mat.SetFloat("_Glossiness", 0.3f);
            mat.SetFloat("_Metallic", 0.2f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", outlineColor * 1.2f);
            // Simulated "outline" by high rim lighting via fresnel effect
            mat.SetFloat("_SpecularHighlights", 1f);
            return mat;
        }

        private static Material CreateGlass(Color tint)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = tint;
            mat.SetFloat("_Glossiness", 0.95f);
            mat.SetFloat("_Metallic", 0.1f);
            mat.SetFloat("_Mode", 3); // Transparent
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            return mat;
        }

        private static Material CreateGlassGlow(Color glowColor, float intensity)
        {
            Material mat = CreateGlass(new Color(glowColor.r, glowColor.g, glowColor.b, 0.3f));
            mat.SetFloat("_Glossiness", 1f);
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", glowColor * intensity);
            return mat;
        }

        private static Material CreateShiny(Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            mat.SetFloat("_Metallic", 1f);     // Max metallic
            mat.SetFloat("_Glossiness", 1f);   // Mirror shine
            mat.SetFloat("_SpecularHighlights", 1f);
            mat.SetFloat("_Parallax", 0.05f);  // Adds a subtle depth effect
            return mat;
        }

        public static Material LoadFromBundle(string BundleName, string Name)
        {
            if (cachedMat != null) return cachedMat;

            string pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundlePath = Path.Combine(pluginDir, BundleName, BundleName);
            if (!File.Exists(bundlePath))
            {
                Debug.LogError("Bundle not founded");
                return null;
            }

            byte[] bytes = File.ReadAllBytes(bundlePath);
            var ab = AssetBundle.LoadFromMemory(bytes);
            cachedMat = ab.LoadAsset<Material>(Name);

            return cachedMat;
        }
    }
}
