using RimWorld;
using UnityEngine;
using Verse;

namespace CRC_Reintegrated
{
    public static class MarvsStoryTellerUtility
    {
        private const float GlobalMaxPoints = 10000f;
        private const float GlobalMaxPointsMercilesss = 20000f;
        private const float GlobalMinPoints = 35f;
        public static readonly SimpleCurve PointsPerWealthCurve = new SimpleCurve()
    {
      {
        new CurvePoint(0.0f, 0.0f),
        true
      },
      {
        new CurvePoint(100f, 0.0f),
        true
      },
      {
        new CurvePoint(1000f, 90f),
        true
      },
      {
        new CurvePoint(14000f, 350f),
        true
      },
      {
        new CurvePoint(30000f, 550f),
        true
      },
      {
        new CurvePoint(100000f, 850f),
        true
      },
      {
        new CurvePoint(200000f, 950f),
        true
      },
      {
        new CurvePoint(300000f, 2200f),
        true
      },
      {
        new CurvePoint(400000f, 2550f),
        true
      },
      {
        new CurvePoint(500000f, 2800f),
        true
      },
      {
        new CurvePoint(600000f, 2950f),
        true
      },
      {
        new CurvePoint(700000f, 3600f),
        true
      },
      {
        new CurvePoint(800000f, 3800f),
        true
      },
      {
        new CurvePoint(900000f, 3900f),
        true
      }
    };
        private static readonly SimpleCurve PointsCurve = new SimpleCurve()
    {
      {
        new CurvePoint(0.0f, 35f),
        true
      },
      {
        new CurvePoint(100f, 35f),
        true
      },
      {
        new CurvePoint(200f, 80f),
        true
      },
      {
        new CurvePoint(500f, 220f),
        true
      },
      {
        new CurvePoint(1000f, 380f),
        true
      },
      {
        new CurvePoint(1500f, 400f),
        true
      },
      {
        new CurvePoint(2000f, 750f),
        true
      },
      {
        new CurvePoint(3000f, 1200f),
        true
      },
      {
        new CurvePoint(4000f, 1550f),
        true
      },
      {
        new CurvePoint(5000f, 1600f),
        true
      },
      {
        new CurvePoint(6000f, 1710f),
        true
      },
      {
        new CurvePoint(9000f, 3800f),
        true
      },
      {
        new CurvePoint(15000f, 4100f),
        true
      },
      {
        new CurvePoint(21000f, 7500f),
        true
      },
      {
        new CurvePoint(27000f, 7700f),
        true
      }
    };

        public static bool Prefix(IIncidentTarget target, ref float __result)
        {
            __result = CombatReadinessPoints(target);
            return false;
        }

        private static float CombatReadinessPoints(IIncidentTarget target)
        {
            ArmouryUtility.GetColonistArmouryPoints(target.PlayerPawnsForStoryteller, target, out var colonistPoints, out var caravanArmouryWealth);
            var num1 = PointsPerWealthCurve.Evaluate(caravanArmouryWealth) + target.PlayerWealthForStoryteller;
            var num2 = Mathf.Lerp(1f, Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor, Find.Storyteller.difficulty.adaptationEffectFactor);
            var num3 = num1 + (colonistPoints * num2);
            if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
            {
                Log.Message("Adapted Colonist Points for " + target.ToString() + ": " + (float)(colonistPoints * (double)num2) + "(adaptation factor: " + Find.Storyteller.difficulty.adaptationEffectFactor + ")");
            }

            var x = num3 * target.IncidentPointsRandomFactorRange.RandomInRange;
            var threatScale = Find.Storyteller.difficulty.threatScale;
            var num4 = Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate(GenDate.DaysPassed);
            if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
            {
                Log.Message("Random factor for " + target.ToString() + ": " + target.IncidentPointsRandomFactorRange.RandomInRange + ", Threat scale: " + threatScale + ", Point factor from days passed: " + num4);
            }

            var num5 = PointsCurve.Evaluate(x) * threatScale * num4;
            if (CRC_Loader.settings.debugLog && GenTicks.TicksGame % 500 == 0)
            {
                Log.Message("Unclamped Points for " + target.ToString() + ": " + num5);
            }

            return Mathf.Clamp(num5, GlobalMinPoints, Find.Storyteller.difficulty.isExtreme ? GlobalMaxPointsMercilesss : GlobalMaxPoints);
        }
    }
}
