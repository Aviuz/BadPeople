using BadPeople.Settings;
using RimWorld;
using UnityEngine;
using Verse;

namespace BadPeople
{
    public class ITab_DebugActivityLog : ITab
    {
        private Vector2 scrollPosition;

        private static readonly Vector2 WinSize = new Vector2(285f + 2 * WindowGap, PreGap + OwnersWindowHeight + 94f + 2 * WindowGap + 24f);
        private const float PreGap = 10f;
        private const float WindowGap = 15f;
        private const float OwnersWindowHeight = 200f;

        public ITab_DebugActivityLog()
        {
            this.size = WinSize;
            this.labelKey = "BadPeople_DebugITabName";
        }

        public override bool IsVisible => Prefs.DevMode && BPSettings.DebugTabVisible;

        protected override void FillTab()
        {
            Rect mainRect = new Rect(0f, PreGap, WinSize.x, WinSize.y - PreGap).ContractedBy(WindowGap);
            Rect topRect = new Rect(0f, 0f, mainRect.width, 40);
            Rect middleRect = new Rect(0f, 40f, mainRect.width, mainRect.height - 48f);
            Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, 1000f);
            Text.Font = GameFont.Small;

            GUI.BeginGroup(mainRect);

            // Checkbox
            var topListing = new Listing_Standard(GameFont.Small);
            topListing.Begin(topRect);
            var loggingEnabledText = ActionLog.EnableLogging ? "Enabled" : "Disabled";
            if (topListing.ButtonTextLabeled($"Logging: {loggingEnabledText}", "Toggle"))
                ActionLog.EnableLogging = !ActionLog.EnableLogging;
            topListing.End();

            // Upper rect
            Widgets.BeginScrollView(middleRect, ref scrollPosition, viewRect);
            var logListing = new Listing_Standard(GameFont.Small);
            logListing.Begin(viewRect);
            foreach (var entry in ActionLog.For(SelPawn).DebugActionLog())
                logListing.Label(entry);
            logListing.End();
            Widgets.EndScrollView();

            GUI.EndGroup();
        }

        public override void OnOpen()
        {

        }
    }
}
