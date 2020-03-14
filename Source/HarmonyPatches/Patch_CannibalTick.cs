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
            if (__result > 0)
            {
                bool isHumanLike = false;

                if (FoodUtility.IsHumanlikeMeat(__instance.def))
                {
                    isHumanLike = true;
                }
                else
                {
                    CompIngredients compIngredients = __instance.TryGetComp<CompIngredients>();
                    if (compIngredients != null)
                    {
                        foreach (var ing in compIngredients.ingredients)
                        {
                            if (FoodUtility.IsHumanlikeMeat(ing))
                            {
                                isHumanLike = true;
                                break;
                            }
                        }
                    }
                }

                if (isHumanLike && ingester.RaceProps.Humanlike)
                {
                    var progress = CanibalismProgression.For(ingester);
                    if (!progress.Locked)
                    {
                        progress.Add(__result);
                        progress.TryBecomeCannibal();
                    }
                }
            }
        }
    }
}