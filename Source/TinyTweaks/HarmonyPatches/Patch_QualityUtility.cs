﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using Harmony;

namespace TinyTweaks
{

    public static class Patch_QualityUtility
    {

        [HarmonyPatch(typeof(QualityUtility))]
        [HarmonyPatch(nameof(QualityUtility.GenerateQualityCreatedByPawn))]
        [HarmonyPatch(new Type[] { typeof(int), typeof(bool) })]
        public static class Patch_GenerateQualityCreatedByPawn
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();
                bool done = false;

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Look three instructions ahead
                    if (!done && i < instructionList.Count - 3)
                    {
                        var thirdInstructionAhead = instructionList[i + 3];

                        // Looking for 'int num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.8f);'
                        if (thirdInstructionAhead.opcode == OpCodes.Call && thirdInstructionAhead.operand == AccessTools.Method(typeof(Rand), nameof(Rand.GaussianAsymmetric)))
                        {
                            // When found, add some IL before that which modifies 'num' (the central quality level in numeric form)
                            yield return instruction; // num
                            yield return new CodeInstruction(OpCodes.Ldarg_0); // relevantSkillLevel
                            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_GenerateQualityCreatedByPawn), nameof(ChangeBaseQuality))); // ChangeBaseQuality(num, relevantSkillLevel)
                            yield return new CodeInstruction(OpCodes.Stloc_0); // num = ChangeBaseQuality(num, relevantSkillLevel)
                            instruction = new CodeInstruction(instruction.opcode, instruction.operand);
                            done = true;
                        }
                    }

                    yield return instruction;
                }
            }

            public static float ChangeBaseQuality(float originalQuality, int relevantSkillLevel)
            {
                if (TinyTweaksSettings.changeQualityDistribution)
                {
                    switch(relevantSkillLevel)
                    {
                        case 0:
                            return 0.2f; // 0.7 => 0 (Awful)
                        case 1:
                            return 0.5f; // 1.1 => 0.33
                        case 2:
                            return 0.8f; // 1.5 => 0.67
                        case 3:
                            return 1; // 1.8 => 1 (Poor)
                        case 4:
                            return 1.4f; // 2 => 1.33
                        case 5:
                            return 1.7f; // 2.2 => 1.67
                        case 6:
                            return 2; // 2.4 => 2 (Normal)
                        case 7:
                            return 2.25f; // 2.6 => 2.25
                        case 8:
                            return 2.5f; // 2.8 => 2.5
                        case 9:
                            return 2.75f; // 2.95 => 2.75
                        case 10:
                            return 3; // 3.1 => 3 (Good)
                        case 11:
                            return 3.25f; // 3.25 => 3.25
                        case 12:
                            return 3.5f; // 3.4 => 3.5
                        case 13:
                            return 3.75f; // 3.5 => 3.75
                        case 14:
                            return 4; // 3.6 => 4 (Excellent)
                        case 15:
                            return 4.15f; // 3.7 => 4.15
                        case 16:
                            return 4.3f; // 3.8 => 4.3
                        case 17:
                            return 4.45f; // 3.9 => 4.45
                        case 18:
                            return 4.6f; // 4 => 4.6
                        case 19:
                            return 4.75f; // 4.1 => 4.75
                        case 20:
                            return 5; // 4.2 => 5 (Masterwork)
                    }
                }
                return originalQuality;
            }

        }

    }

}
