using Verse;
using HarmonyLib;

namespace ToolsAreNecessary
{
    // harmony patch applier.
    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmony = new Harmony("1tzMara.toolsAreNecessary");
            harmony.PatchAll();

            Log.Message("[Tools Are Necessary] Harmony patches applied");
        }
    }
}
