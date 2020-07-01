using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    static class ClassInjector
    {
        public static void Initialize()
        {
            // Make evil people immune to thoughts listed below
            List<ThoughtDef> nullifiedThoughts = new List<ThoughtDef>();
            nullifiedThoughts.Add(ThoughtDefOf.KnowGuestExecuted);
            nullifiedThoughts.Add(ThoughtDefOf.KnowPrisonerDiedInnocent);
            nullifiedThoughts.Add(ThoughtDefOf.KnowPrisonerSold);
            nullifiedThoughts.Add(ThoughtDefOf.KnowGuestOrganHarvested);
            nullifiedThoughts.Add(ThoughtDefOf.WitnessedDeathNonAlly);

            foreach (var thought in nullifiedThoughts)
            {
                thought.nullifyingTraits?.Add(BPDefOf.BadPeople_Evil);
                thought.ResolveReferences();
            }

            // Ensure psychopath can't be evil person
            var psychopathTrait = TraitDefOf.Psychopath;
            if (psychopathTrait.conflictingTraits == null)
                psychopathTrait.conflictingTraits = new List<TraitDef>();
            if (!psychopathTrait.conflictingTraits.Contains(BPDefOf.BadPeople_Evil))
                psychopathTrait.conflictingTraits.Add(BPDefOf.BadPeople_Evil);
            psychopathTrait.ResolveReferences();

            KinslayerInitialize();
        }

        private static void KinslayerInitialize()
        {
           foreach(PawnRelationDef def in DefDatabase<PawnRelationDef>.AllDefs)
           {
                if (def.familyByBloodRelation)
                {
                    addKinSlayer(def.killedThought);
                    addKinSlayer(def.killedThoughtFemale);
                    addKinSlayer(def.diedThought);
                    addKinSlayer(def.diedThoughtFemale);
                }

           }
        }

        private static void addKinSlayer(ThoughtDef def)
        {
            if (def != null)
            {
                def.nullifyingTraits?.Add(BPDefOf.BadPeople_Kinslayer);
                def.ResolveReferences();
            }

        }
        public static void EnableDevMode(bool dev)
        {
#if DEBUG
            if (dev != BPDefOf.BadPeople_Karma.showOnNeedList)
            {
                BPDefOf.BadPeople_Karma.showOnNeedList = dev;
                Log.Message($"[Bad people] dev mode: {dev}, Show karma: {BPDefOf.BadPeople_Karma.showOnNeedList}");
                foreach(ThingDef def in DefDatabase<ThingDef>.AllDefs)
                {
                    if(def.race != null && def.race.intelligence == Intelligence.Humanlike)
                    {
                        if (dev) { 
                            def.inspectorTabs.Add(typeof(ITab_DebugActivityLog));
                        }
                        else
                        {
                            def.inspectorTabs.Remove(typeof(ITab_DebugActivityLog));
                        }

                        def.ResolveReferences();

                    }
                }
/*                if (dev)
                {
                    ThingDefOf.Human.inspectorTabs.Add(typeof(ITab_DebugActivityLog));
                }
                else
                {
                    ThingDefOf.Human.inspectorTabs.Remove(typeof(ITab_DebugActivityLog));
                }
                ThingDefOf.Human.ResolveReferences();*/
            }
#endif
        }
    }
}
