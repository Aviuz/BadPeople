using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadPeople
{
    public static class PointsTable
    {
        public static float FactionExecutedPrisoner = 0.05f;
        public static float ExecutedPrisoner = 0.1f;
        public static float SoldPrisoner = 0.05f;
        public static float WasCaptured = 0.2f;
        public static float KilledRelative = 0.5f;
        public static float KilledFriend = 0.3f;
        public static float WitnessedColonistDeath = 0.03f;
        public static float MyOrganHarvested = 0.4f;
        public static float HarvestedOrgan = 0.15f;
        public static float LostAllies = 0.1f;

        public static float WasReleased = -0.1f;
        public static float FactionReleasedPrisoner = -0.1f;
        public static float AcceptedRefugee = -0.1f;
        public static float RescuedNonColonist = -0.1f;
        public static float GainedAllies = -0.1f;
    }
}
