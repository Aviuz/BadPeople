using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BadPeople
{
    public class Dialog_ActionList : Window
    {
        private static readonly Vector2 WinSize = new Vector2(WinWidth + 2 * WindowGap, WinHeight + 2 * WindowGap);
        private const int WinWidth = 600;
        private const int WinHeight = 300;
        private const int WindowGap = 15;

        private static readonly int lineHeight = (int)GetFixedHeight(Text.fontStyles[(int)GameFont.Tiny]);

        private List<string> actionList;

        private Vector2 scrollPosition;

        public override Vector2 InitialSize => WinSize;

        public Dialog_ActionList(List<string> actionList) : base()
        {
            this.forcePause = true;
            this.absorbInputAroundWindow = true;
            this.actionList = actionList;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect mainRect = new Rect(0, 0, inRect.width, inRect.height - 50).ContractedBy(WindowGap);
            Rect viewRect = new Rect(0, 0, mainRect.width - 16f, Text.fontStyles[(int)GameFont.Tiny].lineHeight * actionList.Count + WindowGap);

            Widgets.BeginScrollView(mainRect, ref scrollPosition, viewRect);
            Text.Font = GameFont.Tiny;
            int curY = 0;
            foreach (var action in actionList)
            {
                var rect = new Rect(4, curY, viewRect.width, lineHeight);
                Widgets.Label(rect, action);
                curY += lineHeight;
            }
            Widgets.EndScrollView();

            if (Widgets.ButtonText(new Rect((int)(inRect.width * 0.5) - 100, inRect.height - 45, 200, 30), "CloseButton".Translate()))
                Close();
        }

        public static float GetFixedHeight(GUIStyle style)
        {
            float num1 = style.lineHeight;
            float num2 = style.CalcHeight(new GUIContent("ABCDEFGHIJKLMNOPRSTUWXYZQabcdefghijklmnoprstuwxyzq"), 3000);
            return num1 > num2 ? num1 : num2;
        }
    }
}
