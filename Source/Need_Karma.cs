using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BadPeople
{
    public class Need_Karma : Need
    {
        public const float degredation = 0.0001f;
        public const float BecomeGoodLevel = 0.5f;
        public const float NeutralLevel = 1.5f;
        public const float BecomeEvilLevel = 2.5f;

        private bool readyForChange = true;

        public override float MaxLevel => 3f;

        public override int GUIChangeArrow => (int)(NeutralLevel - CurLevel);

        public Need_Karma(Pawn pawn) : base(pawn)
        {

        }

        public override void SetInitialLevel()
        {
            CurLevel = UnityEngine.Random.Range(BecomeGoodLevel, BecomeEvilLevel);
        }

        public override void NeedInterval()
        {
            if (CurLevel > NeutralLevel)
                CurLevel -= degredation;
            else if (CurLevel < NeutralLevel)
                CurLevel += degredation;

            if (!readyForChange)
            {
                if (CurLevel > BecomeGoodLevel && CurLevel < BecomeEvilLevel)
                    readyForChange = true;
            }
            else
            {
                if (CurLevel <= BecomeGoodLevel)
                {
                    if (pawn.story.traits.HasTrait(BPDefOf.BadPeople_Evil))
                    {
                        pawn.story.traits.allTraits.RemoveAll(trait => trait.def == BPDefOf.BadPeople_Evil);
                        if (pawn.IsColonist || pawn.IsPrisonerOfColony)
                            Find.WindowStack.Add(new Dialog_AlteringNotification(pawn));
                    }
                    readyForChange = false;
                }
                else if (CurLevel >= BecomeEvilLevel)
                {
                    if (!pawn.story.traits.HasTrait(BPDefOf.BadPeople_Evil) && !pawn.story.traits.HasTrait(TraitDefOf.Psychopath))
                    {
                        pawn.story.traits.GainTrait(new Trait(BPDefOf.BadPeople_Evil, 0, true));
                        if (pawn.IsColonist || pawn.IsPrisonerOfColony)
                            Find.WindowStack.Add(new Dialog_AlteringNotification(pawn));
                    }
                    readyForChange = false;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
        }

        public override string GetTipString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.GetTipString());
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Pawn will become good at {BecomeGoodLevel.ToStringPercent()}");
            stringBuilder.AppendLine($"Pawn will become evil at {BecomeEvilLevel.ToStringPercent()}");
            return stringBuilder.ToString();
        }

        public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
        {
            if (threshPercents == null)
                threshPercents = new List<float>();
            threshPercents.Clear();
            threshPercents.Add(BecomeGoodLevel / MaxLevel);
            threshPercents.Add(NeutralLevel / MaxLevel);
            threshPercents.Add(BecomeEvilLevel / MaxLevel);
            base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
        }

        public void Notify()
        {
            readyForChange = true;
        }
    }
}
