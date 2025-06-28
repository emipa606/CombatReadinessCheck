using HarmonyLib;
using Mlie;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace CRC_Reintegrated;

internal class CRC_Loader : Mod
{
    private const string ModName = "Combat Readiness Check";


    public static CRC_Settings Settings;
    private static Harmony harmony;

    private static string currentVersion;

    public CRC_Loader(ModContentPack content) : base(content)
    {
        harmony = new Harmony("net.marvinkosh.rimworld.mod.combatreadinesscheck");
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
        Settings = GetSettings<CRC_Settings>();
        patchMap();
        patchCaravan();
        PatchDefaultThreatPointsNow();
    }

    public override string SettingsCategory()
    {
        return ModName;
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard1 = new Listing_Standard
        {
            ColumnWidth = inRect.width / 3f
        };
        listingStandard1.Begin(inRect);
        listingStandard1.ColumnWidth = inRect.width / 2.1f;
        if (listingStandard1.ButtonText("CRC.SettingFair".Translate()))
        {
            Settings.NumPointsPerColonist = 45;
            Settings.PercentOfValueForBuildings = 25f;
            Settings.PercentOfValueForArmour = 100f;
            Settings.PercentOfValueForIndustrialWeapons = 100f;
            Settings.PercentOfValueForPreIndustrialWeapons = 25f;
            Settings.PercentOfCombatPowerForReleasableAnimals = 9f;
            Settings.PercentOfCombatPowerForColonyMechs = 27f;
            refreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingThisIsFine".Translate()))
        {
            Settings.NumPointsPerColonist = 85;
            Settings.PercentOfValueForBuildings = 50f;
            Settings.PercentOfValueForArmour = 200f;
            Settings.PercentOfValueForIndustrialWeapons = 200f;
            Settings.PercentOfValueForPreIndustrialWeapons = 50f;
            Settings.PercentOfCombatPowerForReleasableAnimals = 18f;
            Settings.PercentOfCombatPowerForColonyMechs = 54f;
            refreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingFeelsBadMan".Translate()))
        {
            Settings.NumPointsPerColonist = 125;
            Settings.PercentOfValueForBuildings = 75f;
            Settings.PercentOfValueForArmour = 250f;
            Settings.PercentOfValueForIndustrialWeapons = 250f;
            Settings.PercentOfValueForPreIndustrialWeapons = 65f;
            Settings.PercentOfCombatPowerForReleasableAnimals = 27f;
            Settings.PercentOfCombatPowerForColonyMechs = 81f;
            refreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingPainTrain".Translate()))
        {
            Settings.NumPointsPerColonist = 165;
            Settings.PercentOfValueForBuildings = 100f;
            Settings.PercentOfValueForArmour = 300f;
            Settings.PercentOfValueForIndustrialWeapons = 300f;
            Settings.PercentOfValueForPreIndustrialWeapons = 75f;
            Settings.PercentOfCombatPowerForReleasableAnimals = 36f;
            Settings.PercentOfCombatPowerForColonyMechs = 108f;
            refreshBuffer();
        }

        listingStandard1.Gap();
        listingStandard1.CheckboxLabeled("CRC.SettingPreIndustrialArmor".Translate(), ref Settings.PreIndustrialArmor,
            null, 50f);
        listingStandard1.Gap();
        listingStandard1.CheckboxLabeled("CRC.SettingDebugLogging".Translate(), ref Settings.DebugLog);
        if (currentVersion != null)
        {
            listingStandard1.Gap();
            GUI.contentColor = Color.gray;
            listingStandard1.Label("CRC.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard1.NewColumn();
        refreshBuffer();
        listingStandard1.Label("CRC.SettingPointsPerColonist".Translate());
        string numInputBuffer1;
        listingStandard1.TextFieldNumeric(ref Settings.NumPointsPerColonist, ref numInputBuffer1, 1f, 1000f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentBuildingWealth".Translate());
        string numInputBuffer2;
        listingStandard1.TextFieldNumeric(ref Settings.PercentOfValueForBuildings, ref numInputBuffer2, max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentArmour".Translate());
        string numInputBuffer6;
        listingStandard1.TextFieldNumeric(ref Settings.PercentOfValueForArmour, ref numInputBuffer6, max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentIndustrialWeapons".Translate());
        string numInputBuffer3;
        listingStandard1.TextFieldNumeric(ref Settings.PercentOfValueForIndustrialWeapons, ref numInputBuffer3,
            max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentPreIndustrialWeapons".Translate());
        string numInputBuffer4;
        listingStandard1.TextFieldNumeric(ref Settings.PercentOfValueForPreIndustrialWeapons, ref numInputBuffer4,
            max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentReleaseableAnimalPower".Translate());
        string numInputBuffer5;
        listingStandard1.TextFieldNumeric(ref Settings.PercentOfCombatPowerForReleasableAnimals,
            ref numInputBuffer5, max: 800f);
        string numInputBuffer7;
        if (ModLister.BiotechInstalled)
        {
            listingStandard1.Gap();
            listingStandard1.Label("CRC.SettingPercentColonyMechsPower".Translate());
            listingStandard1.TextFieldNumeric(ref Settings.PercentOfCombatPowerForColonyMechs,
                ref numInputBuffer7, max: 800f);
        }

        listingStandard1.End();
        return;

        void refreshBuffer()
        {
            numInputBuffer1 = Settings.NumPointsPerColonist.ToString();
            numInputBuffer2 = Settings.PercentOfValueForBuildings.ToString();
            numInputBuffer6 = Settings.PercentOfValueForArmour.ToString();
            numInputBuffer3 = Settings.PercentOfValueForIndustrialWeapons.ToString();
            numInputBuffer4 = Settings.PercentOfValueForPreIndustrialWeapons.ToString();
            numInputBuffer5 = Settings.PercentOfCombatPowerForReleasableAnimals.ToString();
            numInputBuffer7 = Settings.PercentOfCombatPowerForColonyMechs.ToString();
        }
    }

    private static void patchMap()
    {
        var getMethod = AccessTools.Method(typeof(Map), "get_PlayerWealthForStoryteller");
        if (getMethod == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original Map.PlayerWealthForStoryteller.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsMapWealthForStoryTeller),
                nameof(MarvsMapWealthForStoryTeller.Prefix));
            if (method == null)
            {
                Log.Warning("Got null method when attempting to load prefix.");
            }
            else
            {
                harmony.Patch(getMethod, new HarmonyMethod(method));
            }
        }
    }

    private static void patchCaravan()
    {
        var getMethod = AccessTools.Method(typeof(Caravan), "get_PlayerWealthForStoryteller");
        if (getMethod == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original Caravan.PlayerWealthForStoryteller.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsCaravanWealthForStoryTeller),
                nameof(MarvsCaravanWealthForStoryTeller.Prefix));
            if (method == null)
            {
                Log.Warning("Got null method when attempting to load prefix.");
            }
            else
            {
                harmony.Patch(getMethod, new HarmonyMethod(method));
                Log.Message("Combat Readiness Check: Patched Caravan.PlayerWealthForStoryteller.");
            }
        }
    }

    private static void PatchDefaultThreatPointsNow()
    {
        var methodInfo = AccessTools.Method(typeof(StorytellerUtility), "DefaultThreatPointsNow");
        if (methodInfo == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original StorytellerUtility.DefaultThreatPointsNow.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsStoryTellerUtility), nameof(MarvsStoryTellerUtility.Prefix));
            if (method == null)
            {
                Log.Warning("Got null method when attempting to load prefix.");
            }
            else
            {
                harmony.Patch(methodInfo, new HarmonyMethod(method));
            }
        }
    }

    public class CRC_Settings : ModSettings
    {
        public bool DebugLog;
        public int NumPointsPerColonist = 45;
        public float PercentOfCombatPowerForColonyMechs = 27f;
        public float PercentOfCombatPowerForReleasableAnimals = 9f;
        public float PercentOfValueForArmour = 100f;
        public float PercentOfValueForBuildings = 25f;
        public float PercentOfValueForIndustrialWeapons = 100f;
        public float PercentOfValueForPreIndustrialWeapons = 25f;
        public bool PreIndustrialArmor;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref NumPointsPerColonist, "num_Points_Per_Colonist", 34, true);
            Scribe_Values.Look(ref PercentOfValueForBuildings, "percent_Value_For_Buildings", 25f, true);
            Scribe_Values.Look(ref PercentOfValueForArmour, "percent_Value_For_Armour", 100f, true);
            Scribe_Values.Look(ref PercentOfValueForIndustrialWeapons, "percent_Value_For_IndustrialWeapons", 100f,
                true);
            Scribe_Values.Look(ref PercentOfValueForPreIndustrialWeapons, "percent_Value_For_PreIndustrialWeapons",
                25f, true);
            Scribe_Values.Look(ref PercentOfCombatPowerForReleasableAnimals, "percent_Value_For_ReleasableAnimals",
                9f, true);
            Scribe_Values.Look(ref PercentOfCombatPowerForColonyMechs, "percent_Value_For_ColonyMechs",
                27f, true);
            Scribe_Values.Look(ref DebugLog, "shouldLogDebugInfo", forceSave: true);
            Scribe_Values.Look(ref PreIndustrialArmor, "preIndustrialArmor");
        }
    }
}