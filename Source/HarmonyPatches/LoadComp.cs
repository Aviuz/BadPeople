using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Game))]
    [HarmonyPatch(nameof(Game.LoadGame))]
    class LoadComp
    {
        static void Postfix() {
            CanibalismProgression.Update();
            KinslayerProgression.Update();
        }
    }
}
