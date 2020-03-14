using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Recipe_Surgery), "CheckSurgeryFail")]
    //ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
    static class Patch_Trigger_DoctorRemovedBodypart
    {
        static void Postfix(bool __result, Recipe_Surgery __instance, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
        {
            if (!__result && IsClean(patient, part) && __instance.GetType().Name == "Recipe_RemoveBodyPart" && patient.RaceProps.Humanlike)
            {
                BadPeopleUtility.NotifyDoctorRemovedBodyPart(surgeon);
            }
        }

        private static bool IsClean(Pawn pawn, BodyPartRecord part)
        {
            return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
                                   where x.Part == part
                                   select x).Any<Hediff>();
        }
    }
}
