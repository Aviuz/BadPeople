using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using RimWorld;

namespace BadPeople
{
    class HunamoidRaceCheck
    {
        private static readonly string modName = "Humanoid Alien Races";
        private static readonly string oldModName = "Humanoid Alien Races 2.0";
        private static bool found = false;
        private static List<ThoughtDef> cannibalThought = new List<ThoughtDef>();
        private static Dictionary<string, bool> cachedValue = new Dictionary<string, bool>();

        public static void SearchMod()
        {
            found = LoadedModManager.RunningMods.Any(m => m.Name == modName || m.Name == oldModName);
            if (found)
            {
                Initialization();
            }
        }

        private static void Initialization()
        {
            List<string> thoughtNames = new List<string> {"AteCorpse", "AteHumanlikeMeatDirect", "AteHumanlikeMeatAsIngredient", "AteHumanlikeMeatAsIngredientCannibal", "KnowButcheredHumanlikeCorpse", "ButcheredHumanlikeCorpse" };

            foreach(string name in thoughtNames)
            {
                ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.GetNamed(name, false);
                if (thoughtDef != null)
                {
                    cannibalThought.Add(thoughtDef);
                }
            }

        }

        public static bool IsCannibalByRace(string defName)
        {
            if (!found)
            {
                return false;
            }

            ThingDef thingDef = DefDatabase<ThingDef>.GetNamed(defName, false);
            if(thingDef != null && cannibalThought.Count == 6)
            {
                if(!cachedValue.TryGetValue(defName, out bool cannibalByRace))
                {
                    try
                    {
                        Traverse traverse = new Traverse(thingDef);
                        List<ThoughtDef> blockedThoughts = traverse.Field("alienRace").Field("thoughtSettings").Field("cannotReceiveThoughts").GetValue() as List<ThoughtDef>;

                        cannibalByRace = blockedThoughts != null && blockedThoughts.Intersect(cannibalThought).Count() == cannibalThought.Count;
                        Log.Message($"Returning canibalByDefault: {cannibalByRace} for race: {defName}");
                        cachedValue.Add(defName, cannibalByRace);   

                    }
                    catch (Exception e)
                    {
                        Log.Warning($"Catched error while checking race {defName}, skipping. Error: {e}");
                    }
                }
                return cannibalByRace;

            }
            return false;
        }
    }
}
