using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ToolsAreNecessary
{
    // the doodad on the right that says "miner needs tools"
    public class Alert_NeedMiningTool : Alert
    {
        private static List<Designation> tmpDesignations = new List<Designation>();
        private static List<Pawn> targetPawns = new List<Pawn>();

        // tbd: add translation (no idea how to do that but sometime.)
        public Alert_NeedMiningTool()
        {
            defaultLabel = "Miner missing tools";
            defaultExplanation = "You have marked some spots for mining, but at least one of your miners lacks a tool to mine them.";
            defaultPriority = AlertPriority.High;
        }

        public override AlertReport GetReport()
        {
            // get all maps. iterate over all
            List<Map> maps = Find.Maps;
            for (int i = 0; i < maps.Count; i++)
            {
                Map map = maps[i];
                if (!map.IsPlayerHome)
                {
                    continue;
                }
                // get all mining designations.
                tmpDesignations.Clear();
                tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.Mine]);
                tmpDesignations.AddRange(map.designationManager.designationsByDef[DesignationDefOf.MineVein]);

                // if no mining designations, check the next map.
                if (tmpDesignations.NullOrEmpty())
                {
                    continue;
                }

                targetPawns.Clear();
                bool flag = false;
                foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfPlayer))
                {
                    if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.IsFreeColonist)
                    {
                        // if miner && no tools = displlay

                        // might change this to a check where there's gotta be at least 1 miner with a tool, not if some miner lacks a tool.
                        if (!Utility.HasMiningTool(pawn) && pawn.workSettings?.GetPriority(WorkTypeDefOf.Mining) > 0)
                        {
                            flag = true;
                            targetPawns.Add(pawn);
                        }
                    }
                }
                if (flag)
                {
                    // show the culprit as the pawn's position.
                    return AlertReport.CulpritIs(new GlobalTargetInfo(targetPawns[0].Position, map));
                }
            }
            return false;
        }
    }
}