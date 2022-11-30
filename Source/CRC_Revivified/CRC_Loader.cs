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


    public static CRC_Settings settings;
    private static Harmony harmony;

    private static string currentVersion;

    public CRC_Loader(ModContentPack content) : base(content)
    {
        harmony = new Harmony("net.marvinkosh.rimworld.mod.combatreadinesscheck");
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.CombatReadinessCheck"));
        settings = GetSettings<CRC_Settings>();
        PatchMap();
        PatchCaravan();
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
            settings.numPointsPerColonist = 45;
            settings.percentOfValueForBuildings = 25f;
            settings.percentOfValueForArmour = 100f;
            settings.percentOfValueForIndustrialWeapons = 100f;
            settings.percentOfValueForPreIndustrialWeapons = 25f;
            settings.percentOfCombatPowerForReleasableAnimals = 9f;
            settings.percentOfCombatPowerForColonyMechs = 27f;
            RefreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingThisIsFine".Translate()))
        {
            settings.numPointsPerColonist = 85;
            settings.percentOfValueForBuildings = 50f;
            settings.percentOfValueForArmour = 200f;
            settings.percentOfValueForIndustrialWeapons = 200f;
            settings.percentOfValueForPreIndustrialWeapons = 50f;
            settings.percentOfCombatPowerForReleasableAnimals = 18f;
            settings.percentOfCombatPowerForColonyMechs = 54f;
            RefreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingFeelsBadMan".Translate()))
        {
            settings.numPointsPerColonist = 125;
            settings.percentOfValueForBuildings = 75f;
            settings.percentOfValueForArmour = 250f;
            settings.percentOfValueForIndustrialWeapons = 250f;
            settings.percentOfValueForPreIndustrialWeapons = 65f;
            settings.percentOfCombatPowerForReleasableAnimals = 27f;
            settings.percentOfCombatPowerForColonyMechs = 81f;
            RefreshBuffer();
        }

        if (listingStandard1.ButtonText("CRC.SettingPainTrain".Translate()))
        {
            settings.numPointsPerColonist = 165;
            settings.percentOfValueForBuildings = 100f;
            settings.percentOfValueForArmour = 300f;
            settings.percentOfValueForIndustrialWeapons = 300f;
            settings.percentOfValueForPreIndustrialWeapons = 75f;
            settings.percentOfCombatPowerForReleasableAnimals = 36f;
            settings.percentOfCombatPowerForColonyMechs = 108f;
            RefreshBuffer();
        }

        listingStandard1.Gap();
        listingStandard1.CheckboxLabeled("CRC.SettingPreIndustrialArmor".Translate(), ref settings.preIndustrialArmor,
            null, 50f);
        listingStandard1.Gap();
        listingStandard1.CheckboxLabeled("CRC.SettingDebugLogging".Translate(), ref settings.debugLog);
        if (currentVersion != null)
        {
            listingStandard1.Gap();
            GUI.contentColor = Color.gray;
            listingStandard1.Label("CRC.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard1.NewColumn();
        RefreshBuffer();
        listingStandard1.Label("CRC.SettingPointsPerColonist".Translate());
        string numInputBuffer1;
        listingStandard1.TextFieldNumeric(ref settings.numPointsPerColonist, ref numInputBuffer1, 1f, 1000f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentBuildingWealth".Translate());
        string numInputBuffer2;
        listingStandard1.TextFieldNumeric(ref settings.percentOfValueForBuildings, ref numInputBuffer2, max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentArmour".Translate());
        string numInputBuffer6;
        listingStandard1.TextFieldNumeric(ref settings.percentOfValueForArmour, ref numInputBuffer6, max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentIndustrialWeapons".Translate());
        string numInputBuffer3;
        listingStandard1.TextFieldNumeric(ref settings.percentOfValueForIndustrialWeapons, ref numInputBuffer3,
            max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentPreIndustrialWeapons".Translate());
        string numInputBuffer4;
        listingStandard1.TextFieldNumeric(ref settings.percentOfValueForPreIndustrialWeapons, ref numInputBuffer4,
            max: 800f);
        listingStandard1.Gap();
        listingStandard1.Label("CRC.SettingPercentReleaseableAnimalPower".Translate());
        string numInputBuffer5;
        listingStandard1.TextFieldNumeric(ref settings.percentOfCombatPowerForReleasableAnimals,
            ref numInputBuffer5, max: 800f);
        string numInputBuffer7;
        if (ModLister.BiotechInstalled)
        {
            listingStandard1.Gap();
            listingStandard1.Label("CRC.SettingPercentColonyMechsPower".Translate());
            listingStandard1.TextFieldNumeric(ref settings.percentOfCombatPowerForColonyMechs,
                ref numInputBuffer7, max: 800f);
        }

        listingStandard1.End();

        void RefreshBuffer()
        {
            numInputBuffer1 = settings.numPointsPerColonist.ToString();
            numInputBuffer2 = settings.percentOfValueForBuildings.ToString();
            numInputBuffer6 = settings.percentOfValueForArmour.ToString();
            numInputBuffer3 = settings.percentOfValueForIndustrialWeapons.ToString();
            numInputBuffer4 = settings.percentOfValueForPreIndustrialWeapons.ToString();
            numInputBuffer5 = settings.percentOfCombatPowerForReleasableAnimals.ToString();
            numInputBuffer7 = settings.percentOfCombatPowerForColonyMechs.ToString();
        }
    }

    private static void PatchMap()
    {
        //CRC_Loader.harmony = new Harmony("net.marvinkosh.rimworld.mod.combatreadinesscheck");
        Log.Message("Combat Readiness Check: Trying to patch Map.PlayerWealthForStoryteller");
        var getMethod = AccessTools.Method(typeof(Map), "get_PlayerWealthForStoryteller");
        if (getMethod == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original Map.PlayerWealthForStoryteller.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsMapWealthForStoryTeller), "Prefix");
            if (method == null)
            {
                Log.Warning("Got null method when attempting to load prefix.");
            }
            else
            {
                harmony.Patch(getMethod, new HarmonyMethod(method));
                Log.Message("Combat Readiness Check: Patched Map.PlayerWealthForStoryteller.");
            }
        }
    }

    private static void PatchCaravan()
    {
        //CRC_Loader.harmony = new Harmony("net.marvinkosh.rimworld.mod.combatreadinesscheck");
        Log.Message("Combat Readiness Check: Trying to patch Caravan.PlayerWealthForStoryteller");
        var getMethod = AccessTools.Method(typeof(Caravan), "get_PlayerWealthForStoryteller");
        if (getMethod == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original Caravan.PlayerWealthForStoryteller.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsCaravanWealthForStoryTeller), "Prefix");
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
        //CRC_Loader.harmony = new Harmony("net.marvinkosh.rimworld.mod.combatreadinesscheck");
        Log.Message("Combat Readiness Check: Trying to patch StorytellerUtility.DefaultThreatPointsNow");
        var methodInfo = AccessTools.Method(typeof(StorytellerUtility), "DefaultThreatPointsNow");
        if (methodInfo == null)
        {
            Log.Warning(
                "Got null original method when attempting to find original StorytellerUtility.DefaultThreatPointsNow.");
        }
        else
        {
            var method = AccessTools.Method(typeof(MarvsStoryTellerUtility), "Prefix");
            if (method == null)
            {
                Log.Warning("Got null method when attempting to load prefix.");
            }
            else
            {
                harmony.Patch(methodInfo, new HarmonyMethod(method));
                Log.Message("Combat Readiness Check: Patched StorytellerUtility.DefaultThreatPointsNow.");
            }
        }
    }

    public class CRC_Settings : ModSettings
    {
        public bool debugLog;
        public int numPointsPerColonist = 45;
        public float percentOfCombatPowerForColonyMechs = 27f;
        public float percentOfCombatPowerForReleasableAnimals = 9f;
        public float percentOfValueForArmour = 100f;
        public float percentOfValueForBuildings = 25f;
        public float percentOfValueForIndustrialWeapons = 100f;
        public float percentOfValueForPreIndustrialWeapons = 25f;
        public bool preIndustrialArmor;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref numPointsPerColonist, "num_Points_Per_Colonist", 34, true);
            Scribe_Values.Look(ref percentOfValueForBuildings, "percent_Value_For_Buildings", 25f, true);
            Scribe_Values.Look(ref percentOfValueForArmour, "percent_Value_For_Armour", 100f, true);
            Scribe_Values.Look(ref percentOfValueForIndustrialWeapons, "percent_Value_For_IndustrialWeapons", 100f,
                true);
            Scribe_Values.Look(ref percentOfValueForPreIndustrialWeapons, "percent_Value_For_PreIndustrialWeapons",
                25f, true);
            Scribe_Values.Look(ref percentOfCombatPowerForReleasableAnimals, "percent_Value_For_ReleasableAnimals",
                9f, true);
            Scribe_Values.Look(ref percentOfCombatPowerForColonyMechs, "percent_Value_For_ColonyMechs",
                27f, true);
            Scribe_Values.Look(ref debugLog, "shouldLogDebugInfo", forceSave: true);
            Scribe_Values.Look(ref preIndustrialArmor, "preIndustrialArmor");
        }
    }
}