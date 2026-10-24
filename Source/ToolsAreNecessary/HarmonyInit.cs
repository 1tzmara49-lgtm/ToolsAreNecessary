using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace ToolsAreNecessary
{
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        // by god this sucked
        static HarmonyInit()
        {
            Harmony harmony = new Harmony("1tzMara.toolsAreNecessary");


            // patching everything in JobPatching.cs (except for the one class without a [] harmony doodad)
            harmony.PatchAll(typeof(HarmonyInit).Assembly);
            
            //get mod with id.
            var mod = ModLister.GetActiveModWithIdentifier(
                "Mlie.SeparateTreeChoppingPriority"
            );

            if (mod == null)
            {
                return;
            }

            Log.Message(
                "[Tools Are Necessary] Mlie.SeparateTreeChoppingPriority found. Trying patches."
            );

            // get type via name since coupling by "using Namespace" fucks everything over.
            Type type = AccessTools.TypeByName("TreesOnly.WorkGiver_TreesChop");

            // if type not found, throw an error.
            if (type == null)
            {
                Log.Error(
                    "[Tools Are Necessary] failed to find the type. dm @1tzMara for an update"
                );
                return;
            }

            //searching for the "jobonthing" method
            MethodInfo original = AccessTools.Method(type, "JobOnThing");

            if (original == null)
            {
                Log.Error("[Tools Are Necessary] Could not find JobOnThing in " + type.FullName);
                return;
            }

            MethodInfo prefix = AccessTools.Method(typeof(WorkGiver_TreesChopPatch), "Prefix");

  
            if (prefix == null)
            {
                Log.Error("[Tools Are Necessary] Prefix not found.");
                return;
            }
            // final patch
            harmony.Patch(original, prefix: new HarmonyMethod(prefix));

            Log.Message("[Tools Are Necessary] Patched " + original.DeclaringType.FullName + "." + original.Name);
            Log.Message("[Tools Are Necessary] Harmony patches applied");
        }
    }
}