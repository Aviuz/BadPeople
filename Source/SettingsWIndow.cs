using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BadPeople
{
    public class SettingsWindow : Mod
    {
        

        public SettingsWindow(ModContentPack content) : base(content)
        {
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);
            switch(MyPrefs.ShowDebugInfo)
            {
                case ToggleMode.Disabled:
                    if (listing.ButtonTextLabeled("Settings_ShowDebug", "Disabled"))
                        MyPrefs.ShowDebugInfo = ToggleMode.Enabled;
                    break;
                case ToggleMode.Enabled:
                    if (listing.ButtonTextLabeled("Settings_ShowDebug", "Enabled"))
                        MyPrefs.ShowDebugInfo = ToggleMode.Auto;
                    break;
                case ToggleMode.Auto:
                    if (listing.ButtonTextLabeled("Settings_ShowDebug", "Auto"))
                        MyPrefs.ShowDebugInfo = ToggleMode.Disabled;
                    break;
            }
            listing.End();
        }

        public override string SettingsCategory()
        {
            return "Bad People";
        }

        public override string ToString()
        {
            return "Bad People";
        }

        public override void WriteSettings()
        {
            MyPrefs.ExposeData();
        }
    }
}
