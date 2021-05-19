using RimWorld.Planet;

namespace CRC_Reintegrated
{
    public static class MarvsCaravanWealthForStoryTeller
    {
        public static bool Prefix(Caravan __instance, ref float __result)
        {
            __result = 0.0f;
            return false;
        }
    }
}