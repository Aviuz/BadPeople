using HarmonyLib;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(Prefs), "Apply")]
    static class Patch_ToggleDevMode
    {
        static void Postfix()
        {
            ClassInjector.EnableDevMode(Prefs.DevMode);
        }
    }
}
