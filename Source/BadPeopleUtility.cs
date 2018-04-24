using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    public static class BadPeopleUtility
    {
        public static Pawn CurrentRefugee { get; set; }

        public static void AddPoints(Pawn pawn, float points, string entryName)
        {
            var need = pawn.needs.TryGetNeed<Need_Karma>();
            if (need != null)
            {
                if (need.CurLevel < Need_Karma.BecomeGoodLevel || need.CurLevel > Need_Karma.BecomeEvilLevel)
                    points *= 0.5f;

                need.CurLevel += points;
                need.Notify();

                ActionLog.For(pawn).Put(entryName, points);
            }
        }

        public static void NotifyPawnGotThought(Pawn pawn, Thought_Memory thought, Pawn otherPawn)
        {
            if (thought.def == ThoughtDefOf.KnowGuestExecuted)
            {
                AddPoints(pawn, PointsTable.FactionExecutedPrisoner, "BadPeople_Log_FactionExecutedPrisoner".Translate());
            }
            else if (thought.def == ThoughtDefOf.KnowColonistDied)
            {
                AddPoints(pawn, PointsTable.WitnessedColonistDeath, "BadPeople_Log_WitnessedColonistDeath".Translate());
            }
            else if (thought.def == ThoughtDefOf.RescuedMe)
            {
                if (pawn.Faction != otherPawn.Faction && !pawn.Faction.HostileTo(otherPawn.Faction))
                    AddPoints(otherPawn, PointsTable.RescuedNonColonist, "BadPeople_Log_RescuedNonColonist".Translate());
            }
            else if (thought.def == ThoughtDefOf.MyOrganHarvested)
            {
                AddPoints(pawn, PointsTable.MyOrganHarvested, "BadPeople_Log_MyOrganHarvested".Translate());
            }
        }

        public static void NotifyPawnKilled(Pawn pawn, DamageInfo? dinfo, Hediff hediff)
        {
            if (dinfo.HasValue && dinfo.Value.Instigator != null)
            {
                Pawn killer = dinfo.Value.Instigator as Pawn;

                // Executioner
                if (killer.CurJob != null && killer.jobs.curDriver is JobDriver_Execute)
                    AddPoints(killer, PointsTable.ExecutedPrisoner, "BadPeople_Log_ExecutedPrisoner".Translate());

                // Relative
                if (killer.relations.FamilyByBlood.Contains(pawn))
                    AddPoints(killer, PointsTable.KilledRelative, "BadPeople_Log_KilledRelative".Translate());

                // Friend
                if (killer.relations.OpinionOf(pawn) >= 20)
                    AddPoints(pawn, PointsTable.KilledFriend, "BadPeople_Log_KilledFriend".Translate());
            }
        }

        public static void NotifyPawnGetReleased(Pawn prisoner, Faction hostFaction)
        {
            AddPoints(prisoner, PointsTable.WasReleased, "BadPeople_Log_WasReleased".Translate());
            foreach (var pawn in prisoner.Map.mapPawns.FreeColonistsAndPrisoners)
                if (pawn != prisoner)
                    AddPoints(pawn, PointsTable.FactionReleasedPrisoner, "BadPeople_Log_ReleasingPrisoner".Translate());
        }

        public static void NotifyPawnGetCaptured(Pawn pawn)
        {
            AddPoints(pawn, PointsTable.WasCaptured, "BadPeople_Log_WasCaptured".Translate());
        }

        public static void NotifyDoctorRemovedBodyPart(Pawn doctor)
        {
            AddPoints(doctor, PointsTable.HarvestedOrgan, "BadPeople_Log_HarvestedOrgan".Translate());
        }

        public static void NotifyRefugeeAccepted(Faction faction)
        {
            foreach (var map in Find.Maps)
                foreach (var pawn in map.mapPawns.AllPawns.Where(p => p.Faction == faction))
                    AddPoints(pawn, PointsTable.AcceptedRefugee, "BadPeople_Log_AcceptedRefugee".Translate());
        }

        public static void NotifyGainedAllies(Faction faction)
        {
            foreach (var map in Find.Maps)
                foreach (var pawn in map.mapPawns.AllPawns.Where(p => p.Faction == faction))
                    AddPoints(pawn, PointsTable.GainedAllies, "BadPeople_Log_GainedAllies".Translate());
        }

        public static void NotifyLostAllies(Faction faction)
        {
            foreach (var map in Find.Maps)
                foreach (var pawn in map.mapPawns.AllPawns.Where(p => p.Faction == faction))
                    AddPoints(pawn, PointsTable.LostAllies, "BadPeople_Log_LostAllies".Translate());
        }

        public static void NotifyPawnSoldPrisoner(Pawn pawn)
        {
            AddPoints(pawn, PointsTable.SoldPrisoner, "BadPeople_Log_SoldPrisoner".Translate());
        }
    }
}
