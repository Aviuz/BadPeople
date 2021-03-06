﻿using BadPeople.Settings;
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
            try
            {
                HarmonyPatches.HPatcher.Init();
                ClassInjector.Initialize();
                BPSettings.Init();
                SettingsMenu.Init();
                HunamoidRaceCheck.SearchMod();
                ClassInjector.EnableDevMode(Prefs.DevMode);
            }
            catch (Exception e)
            {
                Log.Error($"BadPeople: Failed to initialize: {e.Message}");
            }
        }
    }
}
