using Verse;

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
            (double)CRC_Loader.Settings.PercentOfValueForBuildings / 100.0);


        __result = armouryPoints + buildings;
        return false;
    }
}