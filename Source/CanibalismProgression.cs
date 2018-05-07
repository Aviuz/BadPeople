using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    public class CanibalismProgression : IExposable
    {
        private static Dictionary<Pawn, CanibalismProgression> LoadedClasses = new Dictionary<Pawn, CanibalismProgression>();

        private int corpseCount;
        private float fleshAmount;
        private Pawn pawn;

        public bool Locked { get; set; }

        public static CanibalismProgression For(Pawn pawn)
        {
            if (!LoadedClasses.ContainsKey(pawn))
                LoadedClasses.Add(pawn, new CanibalismProgression(pawn));
            return LoadedClasses[pawn];
        }

        public static void DeleteKey(Pawn pawn)
        {
            LoadedClasses.Remove(pawn);
        }

        public CanibalismProgression(Pawn pawn)
        {
            this.pawn = pawn;
            if (!pawn.RaceProps.Humanlike || pawn.story.traits.HasTrait(TraitDefOf.Cannibal))
                Locked = true;
            else
                Locked = false;
            fleshAmount = 0;
        }

        public void Add(float nutrition)
        {
            corpseCount++;
            fleshAmount += nutrition;
        }

        public void TryBecomeCannibal()
        {
            // Chance = 1% + Averege(CorpseCount, FullCorpseEaten) * 0,5%
            // 20% <=> 38 full human flesh meals

            // Base value
            var chance = 0.01f;
            // Count
            chance += corpseCount * 0.0025f;
            // Nutrition
            chance += fleshAmount * 0.0025f;
            // Roof to 20%
            chance = chance > 0.2f ? 0.2f : chance;

            var value = UnityEngine.Random.Range(0.0f, 1.0f);

            if (value < chance)
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Cannibal, 0, true));
                if (pawn.IsColonist)
                    Find.WindowStack.Add(new Dialog_AlteringNotification(pawn, null, AlterType.Cannibal));
                else if (pawn.IsPrisonerOfColony)
                    Messages.Message("BadPeople_GainedTrait".Translate(pawn.NameStringShort, TraitDefOf.Cannibal.degreeDatas[0].label), pawn, MessageTypeDefOf.NeutralEvent);
                Locked = true;
            }
        }

        public void RemovePawn(Pawn pawn)
        {
            if (LoadedClasses.ContainsKey(pawn))
                LoadedClasses.Remove(pawn);
        }

        public void ExposeData()
        {
            Scribe_Values.Look<float>(ref this.fleshAmount, "BadPeople_CorpseEaten", 0.0f, false);
        }
    }
}
