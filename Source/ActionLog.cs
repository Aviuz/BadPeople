using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
    public class ActionLog : IExposable
    {
        private static Dictionary<Pawn, ActionLog> LoadedClasses = new Dictionary<Pawn, ActionLog>();

        private Pawn pawn;

        private List<string> log;

        public static bool EnableLogging { get; set; }

        public static ActionLog For(Pawn pawn)
        {
            if (!LoadedClasses.ContainsKey(pawn))
                LoadedClasses.Add(pawn, new ActionLog(pawn));
            return LoadedClasses[pawn];
        }

        public static void DeleteKey(Pawn pawn)
        {
            LoadedClasses.Remove(pawn);
        }

        private ActionLog(Pawn pawn)
        {
            this.pawn = pawn;
            CreateArray();
        }

        public void Put(string actionName, float value)
        {
            string color = value > 0 ? "red" : "cyan";
            string date;
            if (pawn.Map != null || Find.VisibleMap != null)
            {
                int longitude = pawn.Map != null ? pawn.Map.Tile : Find.VisibleMap.Tile;
                date = GenDate.DateFullStringAt(Find.TickManager.TicksAbs, Find.WorldGrid.LongLatOf(longitude));
            }
            else
                date = "Uknown date";
            string entry = $"<color={color}><b>{date}:</b> <i>{actionName} ({value.ToStringPercent()})</i></color>";
            log.Insert(0, entry);
            if (EnableLogging)
                Log.Message($"Bad People: {pawn.Name} got entry '{entry}'");
        }

        public List<string> PickActionList()
        {
            var returnValue = new List<string>(log);
            log.Clear();
            return returnValue;
        }

        internal List<string> DebugActionLog()
        {
            return log;
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<string>(ref log, "BadPeople_ActionLog", LookMode.Value);
            CreateArray();
        }

        public void CreateArray()
        {
            if (log == null)
                log = new List<string>();
        }
    }
}
