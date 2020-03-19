using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch]
    class Patch_SetCurrentRefugee
    {
        static MethodBase TargetMethod()
        {
            return typeof(Slate).GetMethod("Set").MakeGenericMethod(typeof(Pawn));
        }
        // Quest xml file: Scripts_Utility_RewardsCore.xml, Scripts_JoinerThreatCore.xml
        static void PostFix(string name, Pawn var, bool isAbsoluteName = false)
        {
            if(name == "joiner")
            {
                BadPeopleUtility.CurrentRefugee = var;
            }
        }        
    }
}
