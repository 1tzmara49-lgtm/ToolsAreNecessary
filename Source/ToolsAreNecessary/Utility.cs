using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ToolsAreNecessary
{
    public static class Utility
    {
        // method to queue the job + reserve for a pawn
        public static void TryGetTool(Pawn pawn, Predicate<Thing> predicate)
        {
            // guards to prevent the 20+ queue bs. essentially just "if job, then no do"
            if (pawn.jobs.curJob != null && (pawn.jobs.curJob.def == JobDefOf.Equip || pawn.jobs.curJob.def == JobDefOf.TakeCountToInventory))
            {
                return;
            }
            if (pawn.jobs.jobQueue != null && pawn.jobs.jobQueue.Any(j => j.job.def == JobDefOf.Equip || j.job.def == JobDefOf.TakeCountToInventory))
            {
                return;
            }
            Thing toolToEquip = GetClosestThing(pawn, predicate);

            if (toolToEquip == null) return;
            if (pawn.equipment.Primary == null && !pawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                Job equipJob = JobMaker.MakeJob(JobDefOf.Equip, toolToEquip, pawn);
                equipJob.count = 1;
                pawn.Map.reservationManager.Reserve(pawn, equipJob, toolToEquip);
                pawn.jobs.jobQueue.EnqueueFirst(equipJob);
            }
            else
            {
                if (pawn.inventory.innerContainer.CanAcceptAnyOf(toolToEquip))
                {
                    Job pikupJob = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, toolToEquip);
                    pikupJob.count = 1;
                    pawn.Map.reservationManager.Reserve(pawn, pikupJob, toolToEquip);
                    pawn.jobs.jobQueue.EnqueueFirst(pikupJob);
                }
            }

        }

        // utiliyt checks
        public static bool IsMiningTool(Thing tool)
        {
            ThingDef def = tool.def;
            if (def.HasModExtension<TANModExtension>())
            {
                return (def.GetModExtension<TANModExtension>().canMine);
            }
            return false;
        }
        public static bool IsForestryTool(Thing tool)
        {
            ThingDef def = tool.def;
            if (def.HasModExtension<TANModExtension>())
            {
                return (def.GetModExtension<TANModExtension>().canChop);
            }
            return false;
        }

        // find the closest tool that fits the validator. validator is passed from the job patch
        public static Thing GetClosestThing(Pawn pawn, Predicate<Thing> validator)
        {
            if (pawn == null) return null;

            return GenClosest.ClosestThingReachable(
                pawn.Position,
                pawn.Map,
                ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways),
                Verse.AI.PathEndMode.InteractionCell,
                TraverseParms.For(pawn),
                80f,
                thing =>
                {
                    if (thing.IsForbidden(pawn)) return false;
                    if (!thing.IsInAnyStorage()) return false;
                    if (!pawn.CanReserve(thing)) return false;
                    if (!validator(thing)) return false;
                    return true;
                }
                );
        }
        
        // more utility checks.
        public static bool HasMiningTool(Pawn pawn)
        {
            // inventory
            if (pawn.inventory != null && pawn.inventory.innerContainer != null)
            { 
                foreach (Thing thing in pawn.inventory.innerContainer)
                {
                    if (ToolCaching.cachedMiningTools.Contains(thing.def)) return true;
                }
            }

            //equipped
            if (pawn.equipment != null)
            {
                foreach (Thing thing in pawn.equipment.AllEquipmentListForReading)
                {
                    if (ToolCaching.cachedMiningTools.Contains(thing.def)) return true;
                }
            }

            return false;
        }

        public static bool HasForestryTool(Pawn pawn)
        {
            if (pawn == null || pawn.Dead || !pawn.Spawned)
            {
                return false;
            }
            // inventory
            if (pawn.inventory != null && pawn.inventory.innerContainer != null)
            {
                foreach (Thing thing in pawn.inventory.innerContainer)
                {
                    if (ToolCaching.cachedForestryTools.Contains(thing.def)) return true;
                }
            }


            //equipped
            if (pawn.equipment != null)
            {
                foreach (Thing thing in pawn.equipment.AllEquipmentListForReading)
                {

                    if (ToolCaching.cachedForestryTools.Contains(thing.def)) return true;
                }
            }


            return false;
        }
    }
}
