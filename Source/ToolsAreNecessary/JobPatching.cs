using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;

namespace ToolsAreNecessary
{
    [HarmonyPatch(typeof(WorkGiver_Miner), "JobOnThing")]
    public static class WorkGiver_Miner_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn __0)
        {   
            Pawn pawn = __0;
            // if pawn is null, run the original method
            if (pawn == null) return true;

            // nonhumans can just mine on their own (Bc pretty sure insectoids can mine and they probably use this method)
            if (!pawn.RaceProps.Humanlike) return true;

            // If any item meets the requirements (being a mining tool) run the original method. otherwise dont even bother.
            // which essentailly means if no pickaxe, dont even try to give em the job. otherwise try.
            
            if (Utility.HasMiningTool(pawn))
            {
                return true;
            }
            else
            {
                Predicate<Thing> predicate = Utility.IsMiningTool;
                Utility.TryGetTool(pawn, predicate);
            }

            return false;
        }
    }
    [HarmonyPatch(typeof(WorkGiver_PlantsCut), "JobOnThing")]
    public static class WorkGiver_PlantsCut_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Pawn __0, Thing __1)
        {
            Pawn pawn = __0;
            // if pawn is null, run the original method
            if (pawn == null) return true;

            // nonhuman check
            if (!pawn.RaceProps.Humanlike) return true;

            // if not tree, run the original method

            Thing target = __1;

            if(target.def.plant.treeCategory == TreeCategory.None)
            {
                return true;
            }


            // If any item meets the requirements (being a forestry tool) run the original method. otherwise dont even bother.
            // which essentailly means if no axe, dont even try to give em the job. otherwise try.

            if (Utility.HasForestryTool(pawn))
            {
                return true;        
            }
            else
            {
                Predicate<Thing> predicate = Utility.IsForestryTool;
                Utility.TryGetTool(pawn, predicate);
            }
            return false;
        }
    }
}
