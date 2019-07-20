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

    public static class Patch_SkillRecord
    {

        [HarmonyPatch(typeof(SkillRecord))]
        [HarmonyPatch(nameof(SkillRecord.Interval))]
        public static class Patch_Interval
        {

            public static bool Prefix(SkillRecord __instance, Pawn ___pawn)
            {
                // Delay skill decay
                if (TinyTweaksSettings.delayedSkillDecay && !___pawn.GetComp<CompSkillRecordCache>().CanDecaySkill(__instance.def))
                    return false;
                return true;
            }

        }

        [HarmonyPatch(typeof(SkillRecord))]
        [HarmonyPatch(nameof(SkillRecord.Learn))]
        public static class Patch_Learn
        {

            public static void Postfix(SkillRecord __instance, Pawn ___pawn, float xp)
            {
                // Update the pawn's CompSkillTrackerCache
                if (xp >= CompSkillRecordCache.MinExpToDelaySkillDecay || (xp > 0 && __instance.xpSinceMidnight >= CompSkillRecordCache.MinExpToDelaySkillDecay))
                    ___pawn.GetComp<CompSkillRecordCache>().NotifySubstantialExperienceGainedFor(__instance.def);
            }

        }

    }

}
