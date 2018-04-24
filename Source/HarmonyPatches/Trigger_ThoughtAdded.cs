using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[] { typeof(Thought_Memory), typeof(Pawn) })]
    static class Trigger_ThoughtAdded
    {
        static void Postfix(MemoryThoughtHandler __instance, Thought_Memory newThought, Pawn otherPawn)
        {
            BadPeopleUtility.NotifyPawnGotThought(__instance.pawn, newThought, otherPawn);
        }
    }
}
