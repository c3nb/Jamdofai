using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using HarmonyLib;
using static UnityModManagerNet.UnityModManager;

namespace Jamdofai
{
    public static class Main
    {
        public static Harmony Harmony;
        public static ModEntry Mod;
        public static ModEntry.ModLogger Logger;
        public static Settings Setting;
        public static readonly MethodInfo tm_text = typeof(TextMesh).GetProperty("text").GetGetMethod(true);
        public static readonly string[] labels = new string[] { "Invert Alphabets", "Invert Alphabets Alternately" };
        public static void Load(ModEntry modEntry)
        {
            Mod = modEntry;
            Logger = modEntry.Logger;
            modEntry.OnToggle = (m, v) =>
            {
                if (v)
                {
                    Setting = ModSettings.Load<Settings>(m);
                    Harmony = new Harmony(m.Info.Id);
                    Harmony.PatchAll(Assembly.GetExecutingAssembly());
                    if (tm_text.GetMethodBody()?.GetILAsByteArray()?.Length > 0)
                        Harmony.Patch(tm_text, new HarmonyMethod(Patches.TextMeshPatch.prefix));
                }
                else
                {
                    ModSettings.Save(Setting, m);
                    Harmony.UnpatchAll(Harmony.Id);
                    Harmony = null;
                }
                return true;
            };
            modEntry.OnGUI = m =>
            {
                Setting.BreakGrammar = GUILayout.Toggle(Setting.BreakGrammar, "Break Grammar");
                Setting.Separate = GUILayout.Toggle(Setting.Separate, "Separate");
                bool[] values = new bool[] { Setting.Invert, Setting.InvertForce };
                DrawToggleGroup(values, labels);
                Setting.Invert = values[0];
                Setting.InvertForce = values[1];
            };
            modEntry.OnSaveGUI = m => ModSettings.Save(Setting, m);
        }
        public static void DrawToggleGroup(bool[] values, string[] labels)
        {
            for (int i = 0; i < values.Length; i++)
            {
                bool value = GUILayout.Toggle(values[i], labels[i]);
                if (value != values[i])
                {
                    if (values[i] = value)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            if (j == i) continue;
                            values[j] = false;
                        }
                    }                    
                }
            }
        }
    }
}
