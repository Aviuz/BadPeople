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
    //(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
    [HarmonyPatch(typeof(Faction), "TryAffectGoodwillWith")]
    static class Trigger_ChangingAlliance
    {
        static void Prefix(Faction __instance, Faction other, int goodwillChange, bool canSendMessage, bool canSendHostilityLetter, string reason, GlobalTargetInfo? lookTarget)
        {
            if (Prerequirments(__instance, other, goodwillChange))
            {
                if(!__instance.HostileTo(other) && __instance.GoodwillWith(other) < -80)
                    BadPeopleUtility.NotifyLostAllies(other);

                if (__instance.HostileTo(other) && __instance.GoodwillWith(other) > 0)
                    BadPeopleUtility.NotifyGainedAllies(other);
            }
        }

        static bool Prerequirments(Faction faction, Faction other, float goodwillChange)
        {
            if (faction.def.hidden || other.def.hidden)
            {
                return false;
            }
            if (goodwillChange > 0f && faction.def.permanentEnemy)
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
