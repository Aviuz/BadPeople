using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Pawn), "PreTraded")]
    static class Trigger_SellingPrisoner
    {
        static void Postfix(Pawn __instance, TradeAction action, Pawn playerNegotiator, ITrader trader)
        {
            if (action == TradeAction.PlayerSells && __instance.RaceProps.Humanlike)
                BadPeopleUtility.NotifyPawnSoldPrisoner(playerNegotiator);
        }
    }
}
