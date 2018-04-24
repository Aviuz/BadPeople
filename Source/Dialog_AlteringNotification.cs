using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BadPeople
{
    public class Dialog_AlteringNotification : Window
    {
        private static readonly Vector2 WinSize = new Vector2(WinWidth + 2 * WindowGap, WinHeight + 2 * WindowGap);
        private const int WinWidth = 500;
        private const int WinHeight = 165;
        private const int WindowGap = 15;

        private Pawn pawn;
        private List<string> actionList;

        public override Vector2 InitialSize => WinSize;

        public Dialog_AlteringNotification(Pawn pawn)
        {
            this.forcePause = true;
            this.absorbInputAroundWindow = true;
            this.pawn = pawn;
            actionList = ActionLog.For(pawn).PickActionList();
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect mainRect = new Rect(0, 0, inRect.width, inRect.height).ContractedBy(WindowGap);
            Rect imageRect = new Rect(0, 0, 100, mainRect.height - 30);
            Rect textRect = new Rect(100, 0, mainRect.width - 100, mainRect.height - 30);

            GUI.BeginGroup(mainRect);

            GUI.BeginGroup(imageRect);
            var texture = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize);
            float scalledWidth = imageRect.height / ColonistBarColonistDrawer.PawnTextureSize.y * ColonistBarColonistDrawer.PawnTextureSize.x;
            var scalledRect = new Rect(imageRect.width - scalledWidth, 0, scalledWidth, imageRect.height);
            GUI.DrawTexture(new Rect(18, 0, imageRect.width - 36, imageRect.height), texture);
            GUI.EndGroup();

            var listing = new Listing_Standard(GameFont.Small);
            listing.Begin(textRect);
            string message = pawn.story.traits.HasTrait(BPDefOf.BadPeople_Evil) ? "BadPeople_TurnBadMessage" : "BadPeople_TurnGoodMessage";
            listing.Label(message.Translate(pawn.NameStringShort));
            listing.End();

            if (Widgets.ButtonText(new Rect(0, mainRect.height - 30, 200, 30), "BadPeople_Button_Details".Translate()))
                Find.WindowStack.Add(new Dialog_ActionList(actionList));

            if (Widgets.ButtonText(new Rect(mainRect.width - 200, mainRect.height - 30, 200, 30), "OK".Translate()))
                Close();

            GUI.EndGroup();
        }
    }
}
