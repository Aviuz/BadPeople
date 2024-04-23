using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace BadPeople
{
  [DefOf]
  public static class BPDefOf
  {
    public static TraitDef BadPeople_Evil;
    public static TraitDef BadPeople_Kinslayer;
    public static NeedDef BadPeople_Karma;
    public static RecordDef BadPeople_CountOfKilledRelatives;
    public static RecordDef BadPeople_CountOfEatenCorpses;
    public static RecordDef BadPeople_CountOfFleshEaten;

    // Vanilla
    public static TraitDef Cannibal;
    public static ThoughtDef KnowGuestExecuted;
    public static ThoughtDef KnowPrisonerSold;
    public static ThoughtDef KnowGuestOrganHarvested;
    public static ThoughtDef ButcheredHumanlikeCorpse;
    public static ThoughtDef KnowButcheredHumanlikeCorpse;
  }
}
