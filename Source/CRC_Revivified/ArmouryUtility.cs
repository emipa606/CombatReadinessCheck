using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace CRC_Reintegrated;

public static class ArmouryUtility
{
    private const int ArmouryPointsUpdateInterval = 2400;
    private static readonly Dictionary<Map, float> cachedStorytellerArmouryPoints = new Dictionary<Map, float>();
    private static readonly Dictionary<Map, int> tickOfLastCacheUpdate = new Dictionary<Map, int>();

    public static void GetStorytellerArmouryPoints(IIncidentTarget target, out float armouryPoints)
    {
        if (target is not Map map)
        {
            armouryPoints = 0.0f;
            return;
        }

        if (!tickOfLastCacheUpdate.ContainsKey(map))
        {
            tickOfLastCacheUpdate.Add(map, GenTicks.TicksGame);
        }
        else if (cachedStorytellerArmouryPoints.ContainsKey(map) &&
                 GenTicks.TicksGame < tickOfLastCacheUpdate[map] + ArmouryPointsUpdateInterval)
        {
            armouryPoints = cachedStorytellerArmouryPoints[map];
            return;
        }

        var num = 0.0f;
        var outThings1 = new List<Thing>();
        ThingOwnerUtility.GetAllThingsRecursively(map, ThingRequest.ForGroup(ThingRequestGroup.Apparel),
            outThings1, false,
            x => x is not PassingShip &&
                 (x is not Pawn pawn || pawn.Faction == Faction.OfPlayer));
        foreach (var thing in outThings1)
        {
            if (thing.SpawnedOrAnyParentSpawned && !thing.Position.Fogged(map) &&
                thing.GetStatValue(StatDefOf.ArmorRating_Sharp) >
                (ModLister.HasActiveModWithName("Combat Extended") ? 4.0 : 0.389999985694885))
            {
                num += (float)(thing.MarketValue * (double)CRC_Loader.settings.percentOfValueForArmour /
                               100.0);
            }
        }

        var outThings2 = new List<Thing>();
        ThingOwnerUtility.GetAllThingsRecursively(map, ThingRequest.ForGroup(ThingRequestGroup.Weapon),
            outThings2, false,
            x => x is not PassingShip &&
                 (x is not Pawn pawn || pawn.Faction == Faction.OfPlayer));
        foreach (var thing in outThings2)
        {
            if (!thing.SpawnedOrAnyParentSpawned || thing.Position.Fogged(map))
            {
                continue;
            }

            if (thing.def.techLevel >= TechLevel.Industrial)
            {
                num += (float)(thing.MarketValue *
                    (double)CRC_Loader.settings.percentOfValueForIndustrialWeapons / 100.0);
            }
            else
            {
                num += (float)(thing.MarketValue *
                    (double)CRC_Loader.settings.percentOfValueForPreIndustrialWeapons / 100.0);
            }
        }

        armouryPoints = num;
        if (tickOfLastCacheUpdate.ContainsKey(map))
        {
            tickOfLastCacheUpdate[map] = GenTicks.TicksGame;
        }
        else
        {
            Log.Warning($"tickOfLastCacheUpdate did not contain {map}");
        }

        if (cachedStorytellerArmouryPoints.ContainsKey(map))
        {
            cachedStorytellerArmouryPoints[map] = armouryPoints;
        }
        else
        {
            cachedStorytellerArmouryPoints.Add(map, armouryPoints);
        }

        foreach (var key in cachedStorytellerArmouryPoints.Keys)
        {
            if (key == null)
            {
                cachedStorytellerArmouryPoints.Remove(key);
            }
        }
    }

    private static float GetArmouryWealth(Pawn dude)
    {
        var num = 0.0f;
        if (dude.equipment != null)
        {
            foreach (var thing in dude.equipment.AllEquipmentListForReading)
            {
                if (!thing.def.IsRangedWeapon && !thing.def.IsMeleeWeapon)
                {
                    continue;
                }

                if (thing.def.techLevel >= TechLevel.Industrial)
                {
                    num += (float)(thing.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForIndustrialWeapons / 100.0);
                }
                else
                {
                    num += (float)(thing.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForPreIndustrialWeapons / 100.0);
                }
            }
        }

        if (dude.apparel != null)
        {
            var wornApparel = dude.apparel.WornApparel;
            foreach (var apparel in wornApparel)
            {
                if (!(apparel.GetStatValue(StatDefOf.ArmorRating_Sharp) >
                      (ModLister.HasActiveModWithName("Combat Extended") ? 4.0 : 0.389999985694885)))
                {
                    continue;
                }

                if (CRC_Loader.settings.preIndustrialArmor && apparel.def.techLevel < TechLevel.Industrial)
                {
                    num += (float)(apparel.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForPreIndustrialWeapons / 100.0);
                }
                else
                {
                    num += (float)(apparel.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForArmour / 100.0);
                }
            }
        }

        if (dude.inventory == null)
        {
            return num;
        }

        foreach (var thing in dude.inventory.innerContainer)
        {
            if (thing.def.IsMeleeWeapon || thing.def.IsRangedWeapon)
            {
                if (thing.def.techLevel >= TechLevel.Industrial)
                {
                    num += (float)(thing.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForIndustrialWeapons / 100.0);
                }
                else
                {
                    num += (float)(thing.MarketValue *
                        (double)CRC_Loader.settings.percentOfValueForPreIndustrialWeapons / 100.0);
                }
            }

            if (!thing.def.IsApparel || !(thing.GetStatValue(StatDefOf.ArmorRating_Sharp) >
                                          (ModLister.HasActiveModWithName("Combat Extended")
                                              ? 4.0
                                              : 0.389999985694885)))
            {
                continue;
            }

            if (CRC_Loader.settings.preIndustrialArmor && thing.def.techLevel < TechLevel.Industrial)
            {
                num += (float)(thing.MarketValue *
                    (double)CRC_Loader.settings.percentOfValueForPreIndustrialWeapons / 100.0);
            }
            else
            {
                num += (float)(thing.MarketValue *
                    (double)CRC_Loader.settings.percentOfValueForArmour / 100.0);
            }
        }

        return num;
    }

    private static float GetBattleScore(Pawn dude)
    {
        var num = ((0.5f * dude.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness)) +
                   (0.5f * dude.health.capacities.GetLevel(PawnCapacityDefOf.Sight))) *
                  dude.health.capacities.GetLevel(PawnCapacityDefOf.Manipulation);
        if (num * (double)dude.BodySize < 0.200000002980232)
        {
            return 0.0f;
        }

        foreach (var hediff in dude.health.hediffSet.hediffs)
        {
            if (hediff.def.lethalSeverity != -1.0 && hediff.def.PossibleToDevelopImmunityNaturally() &&
                !hediff.FullyImmune())
            {
                return 0.0f;
            }
        }

        if (num > 1.20000004768372)
        {
            num = 1.2f;
        }

        return num * dude.BodySize * CRC_Loader.settings.numPointsPerColonist;
    }

    public static void GetColonistArmouryPoints(
        IEnumerable<Pawn> colonists,
        IIncidentTarget target,
        out float colonistPoints,
        out float caravanArmouryWealth)
    {
        var num1 = 0.0f;
        var num2 = 0.0f;
        var num3 = 1f;
        var num4 = 1f;
        if (target is Caravan)
        {
            num3 = 0.7f;
            num4 = 0.5f;
        }

        foreach (var colonist in colonists)
        {
            if (colonist.ParentHolder is Building_CryptosleepCasket)
            {
                continue;
            }

            if (target is Caravan)
            {
                num2 += GetArmouryWealth(colonist) * num4;
            }

            if (colonist.RaceProps.Animal)
            {
                if (colonist.Faction == Faction.OfPlayer && !colonist.Downed &&
                    colonist.training.HasLearned(TrainableDefOf.Release) &&
                    colonist.health.capacities.GetLevel(PawnCapacityDefOf.Moving) >= 0.15)
                {
                    num1 += (float)(CRC_Loader.settings.percentOfCombatPowerForReleasableAnimals *
                        (double)colonist.kindDef.combatPower / 100.0) * num3;
                }
            }
            else if (!colonist.WorkTagIsDisabled(WorkTags.Violent) &&
                     colonist.health.capacities.GetLevel(PawnCapacityDefOf.Moving) >= 0.15)
            {
                num1 += GetBattleScore(colonist);
            }
        }

        if (target is Caravan)
        {
            num2 /= 2f;
        }

        caravanArmouryWealth = num2;
        colonistPoints = num1;
    }
}