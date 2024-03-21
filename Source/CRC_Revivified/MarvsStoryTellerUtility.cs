using RimWorld;
using UnityEngine;
using Verse;

namespace CRC_Reintegrated;

public static class MarvsStoryTellerUtility
{
    private const float GlobalMaxPoints = 10000f;
    private const float GlobalMaxPointsMercilesss = 20000f;
    private const float GlobalMinPoints = 35f;

    public static readonly SimpleCurve PointsPerWealthCurve =
    [
        new CurvePoint(0.0f, 0.0f),
        new CurvePoint(100f, 0.0f),
        new CurvePoint(1000f, 90f),
        new CurvePoint(14000f, 350f),
        new CurvePoint(30000f, 550f),
        new CurvePoint(100000f, 850f),
        new CurvePoint(200000f, 950f),
        new CurvePoint(300000f, 2200f),
        new CurvePoint(400000f, 2550f),
        new CurvePoint(500000f, 2800f),
        new CurvePoint(600000f, 2950f),
        new CurvePoint(700000f, 3600f),
        new CurvePoint(800000f, 3800f),
        new CurvePoint(900000f, 3900f)
    ];

    private static readonly SimpleCurve PointsCurve =
    [
        new CurvePoint(0.0f, 35f),
        new CurvePoint(100f, 35f),
        new CurvePoint(200f, 80f),
        new CurvePoint(500f, 220f),
        new CurvePoint(1000f, 380f),
        new CurvePoint(1500f, 400f),
        new CurvePoint(2000f, 750f),
        new CurvePoint(3000f, 1200f),
        new CurvePoint(4000f, 1550f),
        new CurvePoint(5000f, 1600f),
        new CurvePoint(6000f, 1710f),
        new CurvePoint(9000f, 3800f),
        new CurvePoint(15000f, 4100f),
        new CurvePoint(21000f, 7500f),
        new CurvePoint(27000f, 7700f)
    ];

    public static bool Prefix(IIncidentTarget target, ref float __result)
    {
        __result = CombatReadinessPoints(target);
        if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        {
            Log.Message($"Recalculated incident-points to {__result}");
        }

        return false;
    }

    private static float CombatReadinessPoints(IIncidentTarget target)
    {
        ArmouryUtility.GetColonistArmouryPoints(target.PlayerPawnsForStoryteller, target, out var colonistPoints,
            out var caravanArmouryWealth);

        var caravanArmouryPoints = PointsPerWealthCurve.Evaluate(caravanArmouryWealth);
        var playerWealth = target.PlayerWealthForStoryteller;
        var playerWealthPoints = PointsPerWealthCurve.Evaluate(playerWealth / 2f) * 2f;
        var num1 = caravanArmouryPoints + playerWealthPoints;
        var num2 = Mathf.Lerp(1f, Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor,
            Find.Storyteller.difficulty.adaptationEffectFactor);
        var num3 = num1 + (colonistPoints * num2);
        if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        {
            Log.Message(
                $"Adapted caravan armoury points for {target}: {caravanArmouryPoints} from caravan wealth {caravanArmouryWealth}");
            Log.Message(
                $"Adapted player wealth points for {target}: {playerWealthPoints} from player wealth {playerWealth}");
            Log.Message(
                $"Adapted Colonist Points for {target}: {(float)(colonistPoints * (double)num2)}(adaptation factor: {Find.Storyteller.difficulty.adaptationEffectFactor})");
        }

        var x = num3 * target.IncidentPointsRandomFactorRange.RandomInRange;
        var threatScale = Find.Storyteller.difficulty.threatScale;
        var num4 = Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate(GenDate.DaysPassed);
        if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        {
            Log.Message(
                $"Random factor for {target}: {target.IncidentPointsRandomFactorRange.RandomInRange}, Threat scale: {threatScale}, Point factor from days passed: {num4}");
        }

        var num5 = PointsCurve.Evaluate(x) * threatScale * num4;
        if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
        {
            Log.Message($"Unclamped Points for {target}: {num5}");
        }

        return Mathf.Clamp(num5, GlobalMinPoints,
            Find.Storyteller.difficulty.allowBigThreats ? GlobalMaxPointsMercilesss : GlobalMaxPoints);
    }
}