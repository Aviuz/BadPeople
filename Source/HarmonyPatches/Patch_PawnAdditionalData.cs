using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), "ExposeData", new Type[]{})]
    class Patch_PawnAdditionalData
    {
        static void Postfix(Pawn __instance)
        {
            CanibalismProgression.For(__instance).ExposeData();
            ActionLog.For(__instance).ExposeData();
        }
    }
}
