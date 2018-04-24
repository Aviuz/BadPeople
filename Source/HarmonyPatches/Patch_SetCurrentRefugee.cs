using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace BadPeople.HarmonyPatches
{
    [HarmonyPatch(typeof(IncidentWorker_RefugeeChased), "TryExecuteWorker")]
    static class Patch_SetCurrentRefugee
    {
        static IEnumerable<CodeInstruction> Transpiler(ILGenerator gen, IEnumerable<CodeInstruction> instr)
        {
            OpCode[] opCodes =
            {
                OpCodes.Call,
                OpCodes.Stfld,
            };
            string[] operands =
            {
                "Verse.Pawn GeneratePawn(PawnGenerationRequest)",
                "Verse.Pawn refugee",
            };
            int step = 0;

            foreach (var ci in instr)
            {
                yield return ci;
                if (HPatcher.IsFragment(opCodes, operands, ci, ref step))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, ci.operand);
                    yield return new CodeInstruction(OpCodes.Call, typeof(Patch_SetCurrentRefugee).GetMethod("SetRefugee"));
                }
            }
        }

        public static void SetRefugee(Pawn refugee)
        {
            BadPeopleUtility.CurrentRefugee = refugee;
        }
    }
}
