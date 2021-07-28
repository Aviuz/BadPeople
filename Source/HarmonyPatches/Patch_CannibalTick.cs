using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Thing), "Ingested")]
    class Patch_CannibalTick
    {
        static void Postfix(Thing __instance, float __result, Pawn ingester, float nutritionWanted)
        {
            if (ingester.RaceProps.Humanlike && __result > 0)
            {
                bool isHumanLike = false;

                if (__instance.def != null && __instance.def.IsIngestible)
                {
                    isHumanLike = FoodUtility.IsHumanlikeCorpseOrHumanlikeMeatOrIngredient(__instance);
                }
               

                if (isHumanLike)
                {
                    var progress = CanibalismProgression.For(ingester);
                    progress.ProgressWithTrait(__result);
                }
            }
        }
    }
}