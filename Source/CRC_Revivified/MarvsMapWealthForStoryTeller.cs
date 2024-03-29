﻿using Verse;

namespace CRC_Reintegrated;

public static class MarvsMapWealthForStoryTeller
{
    public static bool Prefix(Map __instance, ref float __result)
    {
        if (!__instance.IsPlayerHome)
        {
            return true;
        }

        if (Find.Storyteller.difficulty.fixedWealthMode)
        {
            return true;
        }

        ArmouryUtility.GetStorytellerArmouryPoints(__instance, out var armouryPoints);
        var buildings = (float)(__instance.wealthWatcher.WealthBuildings *
            (double)CRC_Loader.settings.percentOfValueForBuildings / 100.0);


        __result = armouryPoints + buildings;
        return false;

        // Should not be used as this prefix should return colony wealth, not raidpoints

        //var num1 = 0.0f;
        //var num2 = 0.0f;
        //var labelCap = __instance.Parent.LabelCap;
        //if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        //{
        //    Log.Message("CRC: begin output");
        //    Log.Message($"CE is loaded: {ModLister.HasActiveModWithName("Combat Extended")}");
        //}

        //ArmouryUtility.GetStorytellerArmouryPoints(__instance, out armouryPoints);
        //if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        //{
        //    Log.Message($"Armoury Points(pre-curve) for map {labelCap}: {armouryPoints}");
        //}

        //num1 = MarvsStoryTellerUtility.PointsPerWealthCurve.Evaluate(armouryPoints);
        //if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        //{
        //    Log.Message($"Armoury Points(post-curve) for map {labelCap}: {num1}");
        //}

        //var x = (float)(__instance.wealthWatcher.WealthBuildings *
        //    (double)CRC_Loader.settings.percentOfValueForBuildings / 100.0);
        //if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        //{
        //    Log.Message(
        //        $"Building Points(pre-curve) for map {labelCap}: {__instance.wealthWatcher.WealthBuildings}(at {CRC_Loader.settings.percentOfValueForBuildings}%)");
        //}

        //num2 = MarvsStoryTellerUtility.PointsPerWealthCurve.Evaluate(x);
        //if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        //{
        //    Log.Message($"Building Points(post-curve) for map {labelCap}: {num2}");
        //}

        //__result = num1 + num2;
        //return false;
    }
}