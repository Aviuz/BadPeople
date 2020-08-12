using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BadPeople.Settings
{

    [StaticConstructorOnStartup]
    public class SettingsMenu : Mod
    {

        private static bool showDebugTab;

        public SettingsMenu(ModContentPack content) : base(content)
        {
        }

        public static void Init()
        {
            showDebugTab = BPSettings.DebugTabVisible;
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing_options = new Listing_Standard();

            listing_options.Begin(inRect);

            listing_options.CheckboxLabeled("BadPeople_ShowDebugTab".Translate(), ref showDebugTab,
                "BadPeople_ShowDebugTabDesc".Translate());

            listing_options.End();

        }

        public override string SettingsCategory()
        {
            return "Bad People";
        }

        public override void WriteSettings()
        {
            BPSettings.DebugTabVisible = showDebugTab;
            BPSettings.Save();
            Log.Message("Bad People settings saved");
        }

    }
}
