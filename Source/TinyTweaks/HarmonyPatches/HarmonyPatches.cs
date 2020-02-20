﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace TinyTweaks
{

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            //HarmonyInstance.DEBUG = true;
            TinyTweaks.HarmonyInstance.PatchAll();

            // Alert_ColonistNeedsTend.NeedingColonists iterator
            //TinyTweaks.HarmonyInstance.Patch(typeof(Alert_ColonistNeedsTend).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First().GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance),
            //    transpiler: new HarmonyMethod(typeof(Patch_Alert_ColonistNeedsTend.manual_get_NeedingColonists), "Transpiler"));

            // Pawn_PlayerSettings ctor
            //TinyTweaks.HarmonyInstance.Patch(typeof(Pawn_PlayerSettings).GetConstructor(new Type[] { typeof(Pawn) }),
            //    postfix: new HarmonyMethod(typeof(Patch_Pawn_PlayerSettings.manual_Ctor), "Postfix"));

            // Turret Extensions
            //if (ModCompatibilityCheck.TurretExtensions)
            //{
            //    var patchBuldingTurretGun = GenTypes.GetTypeInAnyAssembly("TurretExtensions.Patch_Building_TurretGun", "TurretExtensions");
            //    if (patchBuldingTurretGun != null)
            //    {
            //        var patchTick = patchBuldingTurretGun.GetNestedType("Patch_Tick", BindingFlags.Public | BindingFlags.Static);
            //        if (patchTick != null)
            //            TinyTweaks.HarmonyInstance.Patch(AccessTools.Method(patchTick, "Prefix"), new HarmonyMethod(typeof(Patch_TurretExtensions_Patch_Building_TurretGun.manual_Patch_Tick), "Prefix"));
            //        else
            //            Log.Error("Could not find type TurretExtensions.Patch_Building_TurretGun.Patch_Tick in Turret Extensions");
            //    }
            //    else
            //        Log.Error("Could not find type TurretExtensions.Patch_Building_TurretGun in Turret Extensions");
            //}
        }

    }

}
