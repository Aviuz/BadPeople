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

        private int corpsRecord => pawn.records.GetAsInt(BPDefOf.BadPeople_CountOfEatenCorpses);
        private float fleshRecord => pawn.records.GetValue(BPDefOf.BadPeople_CountOfFleshEaten);

        public bool Locked { get; set; }

        public static void Update()
        {
            foreach(var entry in LoadedClasses)
            {
                entry.Value.update();
            }
        }

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
            Locked = !pawn.RaceProps.Humanlike || pawn.story.traits.HasTrait(TraitDefOf.Cannibal);
            fleshAmount = 0;
        }
        public void ProgressWithTrait(float nutrition)
        {
            //update(); //compatibility
            Add(nutrition);
            if (!Locked)
            {
                TryBecomeCannibal();
            }
        }


        private void update()
        {
            if (corpsRecord < corpseCount)
            {
                var corpsToAdd = Math.Max(0, corpseCount - corpsRecord);
                pawn.records.AddTo(BPDefOf.BadPeople_CountOfEatenCorpses, corpsToAdd);
                Log.Message($"Updating pawn: {pawn.NameShortColored} CountOfEatenCorpses with value { corpsToAdd } ");
            }

            if (fleshRecord < fleshAmount)
            {
                float fleshCount = Math.Max(0, fleshAmount - fleshRecord);
                pawn.records.AddTo(BPDefOf.BadPeople_CountOfFleshEaten, fleshCount);
                Log.Message($"Updating pawn: {pawn.NameShortColored} CountOfFleshEaten with value { fleshCount} ");
            }
        }


        private void Add(float nutrition)
        {
            pawn.records.Increment(BPDefOf.BadPeople_CountOfEatenCorpses);
            pawn.records.AddTo(BPDefOf.BadPeople_CountOfFleshEaten, nutrition);
/*            corpseCount++;
            fleshAmount += nutrition;*/
        }

        private void TryBecomeCannibal()
        {
            // Chance = 1% + Averege(CorpseCount, FullCorpseEaten) * 0,5%
            // 20% <=> 38 full human flesh meals

            // Base value
            var chance = 0.01f;
            // Count
            chance += corpsRecord * 0.0025f;
            // Nutrition
            chance += fleshRecord * 0.0025f;
            // Roof to 20%
            chance = chance > 0.2f ? 0.2f : chance;

            var value = UnityEngine.Random.Range(0.0f, 1.0f);

            if (value < chance)
            {
                pawn.story.traits.GainTrait(new Trait(TraitDefOf.Cannibal, 0, true));
                if (pawn.IsColonist)
                    Find.WindowStack.Add(new Dialog_AlteringNotification(pawn, null, AlterType.Cannibal));
                else if (pawn.IsPrisonerOfColony)
                    Messages.Message("BadPeople_GainedTrait".Translate(pawn.Name.ToStringShort, TraitDefOf.Cannibal.degreeDatas[0].label), pawn, MessageTypeDefOf.NeutralEvent);
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
