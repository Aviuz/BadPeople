using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    public enum ToggleMode
    {
        Disabled,
        Enabled,
        Auto,
    }

    public static class MyPrefs
    {
        private static ToggleMode _showDebugInfo;
        public static ToggleMode ShowDebugInfo { get => _showDebugInfo; set { _showDebugInfo = value; } }

        public static void ExposeData()
        {
            Scribe_Values.Look<ToggleMode>(ref _showDebugInfo, "show_debug_info", ToggleMode.Auto, true);
        }
    }
}
