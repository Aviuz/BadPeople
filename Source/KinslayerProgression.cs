using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BadPeople
{
    public class KinslayerProgression : IExposable
    {
        private static Dictionary<Pawn, KinslayerProgression> LoadedClasses = new Dictionary<Pawn, KinslayerProgression>();

        public static void DeleteKey(Pawn pawn)
        {
            LoadedClasses.Remove(pawn);
        }

        public static void Update()
        {
            foreach (var entry in LoadedClasses)
            {
                entry.Value.update();
            }
        }
        public static KinslayerProgression For(Pawn pawn)
        {
            if (!LoadedClasses.ContainsKey(pawn))
                LoadedClasses.Add(pawn, new KinslayerProgression(pawn));
            return LoadedClasses[pawn];
        }

        private int kinSlayed;
        private int actualValue => pawn.records.GetAsInt(BPDefOf.BadPeople_CountOfKilledRelatives);
        private Pawn pawn;

        private bool Locked { get; set; }

        private void update()
        {
            if (actualValue < kinSlayed)
            {
                pawn.records.AddTo(BPDefOf.BadPeople_CountOfKilledRelatives, Math.Max(0, kinSlayed - actualValue));
                Log.Message($"Updating pawn: {pawn.NameShortColored} Killed relatives with value { Math.Max(0, kinSlayed - actualValue)} ");
            }
        }

        public void ProgressWithTrait()
        {
           // update(); //compatibility
            Increment();
            TryBecomeKinSlayer();
        }
        private void Increment()
        {
            pawn.records.Increment(BPDefOf.BadPeople_CountOfKilledRelatives);
            Log.Message($"[BadPeople] Increment kinslayer for {pawn}, value {kinSlayed}");
        }

        private KinslayerProgression(Pawn pawn)
        {
            this.pawn = pawn;
            Locked = !pawn.RaceProps.Humanlike || pawn.story.traits.HasTrait(BPDefOf.BadPeople_Kinslayer);
            kinSlayed = 0;
        }
        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.kinSlayed, "BadPeople_KinSlayed", 0, false);            
        }

        private void TryBecomeKinSlayer()
        {
            if (!Locked)
            {
                var chance = actualValue * 0.1f;
                Log.Message($"[BadPeople] {pawn} try become kinslayer chance: {chance}");

                var value = UnityEngine.Random.Range(0.0f, 1.0f);

                if (value < chance)
                {
                    pawn.story.traits.GainTrait(new Trait(BPDefOf.BadPeople_Kinslayer, 0, true));
                    if (pawn.IsColonist)
                        Find.WindowStack.Add(new Dialog_AlteringNotification(pawn, null, AlterType.KinSlayer));
                    else if (pawn.IsPrisonerOfColony)
                        Messages.Message("BadPeople_GainedTrait".Translate(pawn.Name.ToStringShort, BPDefOf.BadPeople_Kinslayer.degreeDatas[0].label), pawn, MessageTypeDefOf.NeutralEvent);
                    Locked = true;
                }
            }
        }
    }

}
