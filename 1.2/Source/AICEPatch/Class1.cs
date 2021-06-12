using AwesomeInventory.Jobs;
using CombatExtended;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AICEPatch
{
    [StaticConstructorOnStartup]
    internal static class HarmonyInit
    {
        static HarmonyInit()
        {
            var harmony = new Harmony("AICEPatch.HarmonyPatches");
            harmony.PatchAll();
        }
    }

    //[HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    //public class StartJobPatch
    //{
    //    private static void Postfix(Pawn_JobTracker __instance, Pawn ___pawn, Job newJob, JobTag? tag)
    //    {
    //        {
    //            Log.Message(___pawn + " is starting " + newJob);
    //        }
    //    }
    //}
    //
    //
    //[HarmonyPatch(typeof(Pawn_JobTracker), "EndCurrentJob")]
    //public class EndCurrentJobPatch
    //{
    //    private static void Prefix(Pawn_JobTracker __instance, Pawn ___pawn, JobCondition condition, ref bool startNewJob, bool canReturnToPool = true)
    //    {
    //        {
    //            Log.Message(___pawn + " is ending " + ___pawn.CurJob);
    //        }
    //    }
    //}
    //
    //[HarmonyPatch(typeof(ThinkNode_JobGiver), "TryIssueJobPackage")]
    //public class TryIssueJobPackage
    //{
    //    private static void Postfix(ThinkNode_JobGiver __instance, ThinkResult __result, Pawn pawn, JobIssueParams jobParams)
    //    {
    //        {
    //            Log.Message(pawn + " gets " + __result.Job + " from " + __instance);
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(JobGiver_AwesomeInventory_FindItems), "TryGiveJob")]
    public class JobGiver_AwesomeInventory_FindItems_Patch
    {
        private static void Postfix(ref Job __result, Pawn pawn)
        {
            if (__result != null)
            {
                CompInventory inventory = pawn.TryGetComp<CompInventory>();
                if (inventory.CanFitInInventory(__result.targetA.Thing, out int count))
                {
                    Log.Message(__result + " - can fit " + count + " - " + __result.count);
                    if (__result.count > count)
                    {
                        __result.count = count;
                    }
                }
                else
                {
                    Log.Message("Can't fit, failing");
                    __result = null;
                }
            }
        }
    }
}
