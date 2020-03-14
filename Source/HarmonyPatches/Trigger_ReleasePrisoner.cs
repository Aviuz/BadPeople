using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(GenGuest), "PrisonerRelease")]
    static class Trigger_ReleasePrisoner
    {
        static void Prefix(Pawn p)
        {
            BadPeopleUtility.NotifyPawnGetReleased(p, p.guest.HostFaction);
        }
    }
}
