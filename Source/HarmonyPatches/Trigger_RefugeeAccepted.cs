using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), "SetFaction", new Type[] { typeof(Faction), typeof(Pawn) })]
    static class Trigger_RefugeeAccepted
    {
        static void Postfix(Pawn __instance, Faction newFaction, Pawn recruiter)
        {
            if(BadPeopleUtility.CurrentRefugee != null && __instance == BadPeopleUtility.CurrentRefugee)
            {
                BadPeopleUtility.NotifyRefugeeAccepted(newFaction);
                BadPeopleUtility.CurrentRefugee = null;
            }
        }
    }
}
