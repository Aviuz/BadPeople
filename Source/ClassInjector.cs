﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace BadPeople
{
    static class ClassInjector
    {
        public static void Initialize()
        {
            // Make evil people immune to thoughts listed below
            List<ThoughtDef> nullifiedThoughts = new List<ThoughtDef>();
            nullifiedThoughts.Add(BPDefOf.KnowGuestExecuted);
            nullifiedThoughts.Add(ThoughtDefOf.KnowPrisonerDiedInnocent);
            nullifiedThoughts.Add(BPDefOf.KnowPrisonerSold);
            nullifiedThoughts.Add(BPDefOf.KnowGuestOrganHarvested);
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

            // Adding Debug tab to all pawns, visibility resolved internally, later.
            foreach (ThingDef t in DefDatabase<ThingDef>.AllDefs.Where(def => def.race != null && def.race.intelligence == Intelligence.Humanlike))
            {
                if (t.inspectorTabsResolved == null)
                {
                    t.inspectorTabsResolved = new List<InspectTabBase>(1);
                }
                t.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_DebugActivityLog)));
            }
            InspectPaneUtility.Reset();
        }

        private static void KinslayerInitialize()
        {
            foreach (PawnRelationDef def in DefDatabase<PawnRelationDef>.AllDefs)
            {

                if (def.familyByBloodRelation)
                {
#if DEBUG
                    Log.Message($"Patching: {def}, family by blood: {def.familyByBloodRelation}");
#endif
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
                if (def.nullifyingTraits == null)
                {
                    def.nullifyingTraits = new List<TraitDef>();
                }
                if (!def.nullifyingTraits.Contains(BPDefOf.BadPeople_Kinslayer))
                {
                    def.nullifyingTraits.Add(BPDefOf.BadPeople_Kinslayer);
                }
                def.ResolveReferences();
#if DEBUG
                Log.Message($"Patching: {def}, size: {def.nullifyingTraits.Count}");
#endif
            }

        }
        public static void EnableDevMode(bool dev)
        {
#if DEBUG
            if (dev != BPDefOf.BadPeople_Karma.showOnNeedList)
            {
                BPDefOf.BadPeople_Karma.showOnNeedList = dev;
                Log.Message($"[Bad people] dev mode: {dev}, Show karma: {BPDefOf.BadPeople_Karma.showOnNeedList}");
#endif                
            }
        }
    }
}
