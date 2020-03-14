using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), "Destroy")]
    static class Patch_RemoveLoadedClassesWhenPawnDies
    {
        static void Postfix(Pawn __instance, DestroyMode mode)
        {
            CanibalismProgression.DeleteKey(__instance);
            ActionLog.DeleteKey(__instance);
        }
    }
}
