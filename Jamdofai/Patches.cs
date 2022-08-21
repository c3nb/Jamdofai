using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using TMPro;

namespace Jamdofai
{
    public static class Patches
    {
        [HarmonyPatch(typeof(Text), "text", MethodType.Setter)]
        public static class TextPatch
        {
            public static void Prefix(Text __instance, ref string value)
            {
                Cache cache = __instance.GetOrAddComponent<Cache>();
                if (cache.cachedText == value || value == null) return;
                value = cache.cachedText = value.Jamdofaize();
            }
        }
        public static class TextMeshPatch
        {
            public static void Prefix(TextMesh __instance, ref string value)
            {
                Cache cache = __instance.GetOrAddComponent<Cache>();
                if (cache.cachedText == value || value == null) return;
                value = cache.cachedText = value.Jamdofaize();
            }
            public static readonly MethodInfo prefix = typeof(TextMeshPatch).GetMethod(nameof(Prefix));
        }
        [HarmonyPatch(typeof(TMP_Text), "text", MethodType.Setter)]
        public static class TextMeshProPatch
        {
            public static void Prefix(TextMeshPro __instance, ref string value)
            {
                Cache cache = __instance.GetOrAddComponent<Cache>();
                if (cache.cachedText == value || value == null) return;
                value = cache.cachedText = value.Jamdofaize();
            }
        }
        [HarmonyPatch(typeof(RDString), "GetWithCheck")]
        public static class RDStringPatch
        {
            public static void Prefix(ref string __result)
            {
                __result = __result.Jamdofaize();
            }
        }
        public static string Jamdofaize(this string s)
        {
            string result = s;
            if (Main.Setting.BreakGrammar)
                result = result.BreakGrammar();
            if (Main.Setting.Separate)
                result = result.Separate();
            if (Main.Setting.Invert)
                result = result.Invert();
            if (Main.Setting.InvertForce)
                result = result.InvertForce();
            return result;
        }
        public static T GetOrAddComponent<T>(this Component comp) where T : Component
            => comp.GetComponent<T>() ?? comp.gameObject.AddComponent<T>();
    }
}
