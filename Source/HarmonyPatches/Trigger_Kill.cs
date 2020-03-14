using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), "Kill", new Type[] { typeof(DamageInfo?), typeof(Hediff) })]
    class Trigger_Kill
    {
        static void Prefix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            BadPeopleUtility.NotifyPawnKilled(__instance, dinfo, exactCulprit);
        }
    }
}
