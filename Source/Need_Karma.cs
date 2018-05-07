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
        // Will degradate at rate 10% every 3 days, or 100% every 30 days.
        public const double Degradation = 1f / (30f * GenDate.TicksPerDay / 150f); // Need.NeedInterval() is triggered every 150 ticks 
        public const float BecomeGoodLevel = 0.5f;
        public const float NeutralLevel = 1.5f;
        public const float BecomeEvilLevel = 2.5f;

        private bool readyForChange = true;
        private double _curLevel;

        public override float MaxLevel => 3f;

        public override int GUIChangeArrow => !Extremist ? NeutralLevel.CompareTo(CurLevel) : 0;

        public bool Extremist => CurLevel < BecomeGoodLevel || CurLevel > BecomeEvilLevel ? true : false;

        public override float CurLevel { get => (float)_curLevel; set => _curLevel = value; }

        public Need_Karma(Pawn pawn) : base(pawn)
        {
            _curLevel = CurLevel;
        }

        public override void SetInitialLevel()
        {
            CurLevel = UnityEngine.Random.Range(BecomeGoodLevel, BecomeEvilLevel);
        }

        public override void NeedInterval()
        {
            if (!Extremist)
            {
                if (_curLevel > NeutralLevel)
                    _curLevel -= Degradation;
                else if (_curLevel < NeutralLevel)
                    _curLevel += Degradation;
            }

            if (!readyForChange)
            {
                if (CurLevel > BecomeGoodLevel && CurLevel < BecomeEvilLevel)
                    readyForChange = true;
            }
            else
            {
                // Become good
                if (CurLevel <= BecomeGoodLevel)
                {
                    if (pawn.story.traits.HasTrait(BPDefOf.BadPeople_Evil))
                    {
                        pawn.story.traits.allTraits.RemoveAll(trait => trait.def == BPDefOf.BadPeople_Evil);
                        var actionLog = ActionLog.For(pawn).PickActionList();
                        if (pawn.IsColonist)
                            Find.WindowStack.Add(new Dialog_AlteringNotification(pawn, actionLog, AlterType.Good));
                        else if (pawn.IsPrisonerOfColony)
                            Messages.Message("BadPeople_LostTrait".Translate(pawn.NameStringShort, BPDefOf.BadPeople_Evil.degreeDatas[0].label), pawn, MessageTypeDefOf.NeutralEvent);
                    }
                    readyForChange = false;
                }
                // Become evil
                else if (CurLevel >= BecomeEvilLevel)
                {
                    if (!pawn.story.traits.HasTrait(BPDefOf.BadPeople_Evil) && !pawn.story.traits.HasTrait(TraitDefOf.Psychopath))
                    {
                        // Remove kind trait
                        if (pawn.story.traits.HasTrait(TraitDefOf.Kind))
                            pawn.story.traits.allTraits.RemoveAll(trait => trait.def == TraitDefOf.Kind);

                        pawn.story.traits.GainTrait(new Trait(BPDefOf.BadPeople_Evil, 0, true));
                        var actionLog = ActionLog.For(pawn).PickActionList();
                        if (pawn.IsColonist)
                            Find.WindowStack.Add(new Dialog_AlteringNotification(pawn, actionLog, AlterType.Bad));
                        else if (pawn.IsPrisonerOfColony)
                            Messages.Message("BadPeople_GainedTrait".Translate(pawn.NameStringShort, BPDefOf.BadPeople_Evil.degreeDatas[0].label), pawn, MessageTypeDefOf.NeutralEvent);
                    }
                    readyForChange = false;
                }
            }
        }

        public override void ExposeData()
        {
            base.CurLevel = CurLevel;
            base.ExposeData();
        }

        public override string GetTipString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{LabelCap}: {(CurLevel - NeutralLevel).ToStringPercent()}");
            stringBuilder.AppendLine(def.description);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Pawn will become good at {(BecomeGoodLevel - NeutralLevel).ToStringPercent()}");
            stringBuilder.AppendLine($"Pawn will become evil at {(BecomeEvilLevel - NeutralLevel).ToStringPercent()}");
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
