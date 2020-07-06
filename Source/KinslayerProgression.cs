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

        public static KinslayerProgression For(Pawn pawn)
        {
            if (!LoadedClasses.ContainsKey(pawn))
                LoadedClasses.Add(pawn, new KinslayerProgression(pawn));
            return LoadedClasses[pawn];
        }

        private int kinSlayed;
        private Pawn pawn;

        public bool Locked { get; set; }

        public void Increment()
        {
            kinSlayed++;
            Log.Message($"[BadPeople] Increment kinslayer for {pawn}, value {kinSlayed}");
        }

        public KinslayerProgression(Pawn pawn)
        {
            this.pawn = pawn;
            if (!pawn.RaceProps.Humanlike || pawn.story.traits.HasTrait(BPDefOf.BadPeople_Kinslayer))
                Locked = true;
            else
                Locked = false;
            kinSlayed = 0;
        }
        public void ExposeData()
        {
            Scribe_Values.Look<int>(ref this.kinSlayed, "BadPeople_KinSlayed", 0, false);
        }

        public void TryBecomeKinSlayer()
        {
            if (!Locked)
            {
                var chance = kinSlayed * 0.1f;
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
