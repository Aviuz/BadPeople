using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    [StaticConstructorOnStartup]
    static class Initialization
    {
        static Initialization()
        {
            HarmonyPatches.HPatcher.Init();
            ClassInjector.Initialize();

            ClassInjector.EnableDevMode(Prefs.DevMode);
        }
    }
}
