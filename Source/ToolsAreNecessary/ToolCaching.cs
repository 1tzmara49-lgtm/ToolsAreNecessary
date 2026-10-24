using System.Collections.Generic;
using Verse;

namespace ToolsAreNecessary
{
    [StaticConstructorOnStartup]
    public static class ToolCaching
    {
        public static List<ThingDef> cachedMiningTools = new List<ThingDef>();
        public static List<ThingDef> cachedForestryTools = new List<ThingDef>();
        static ToolCaching()
        {
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if(!thingDef.HasModExtension<TANModExtension>()) continue;

                if (thingDef.GetModExtension<TANModExtension>().canMine)
                {
                    cachedMiningTools.Add(thingDef);
                }
                if (thingDef.GetModExtension<TANModExtension>().canChop)
                {
                    cachedForestryTools.Add(thingDef);
                }
            }
        }
    }
}
