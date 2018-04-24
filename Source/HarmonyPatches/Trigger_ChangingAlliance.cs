using Harmony;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Faction), "AffectGoodwillWith")]
    static class Trigger_ChangingAlliance
    {
        static void Prefix(Faction __instance, Faction other, float goodwillChange)
        {
            if (Prerequirments(__instance, other, goodwillChange))
            {
                if(!__instance.HostileTo(other) && __instance.GoodwillWith(other) < -80f)
                    BadPeopleUtility.NotifyLostAllies(other);

                if (__instance.HostileTo(other) && __instance.GoodwillWith(other) > 0f)
                    BadPeopleUtility.NotifyGainedAllies(other);
            }
        }

        static bool Prerequirments(Faction faction, Faction other, float goodwillChange)
        {
            if (faction.def.hidden || other.def.hidden)
            {
                return false;
            }
            if (goodwillChange > 0f && !faction.def.appreciative)
            {
                return false;
            }
            if (goodwillChange > 0f && ((faction.IsPlayer && SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) || (other.IsPlayer && SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))))
            {
                return false;
            }
            return true;
        }
    }
}
