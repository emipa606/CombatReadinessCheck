using HarmonyLib;
using Mlie;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace CRC_Reintegrated
{
    internal class CRC_Loader : Mod
    {
        private const string ModName = "Combat Readiness Check";
        private const string SettingPointsPerColonist = "Base threat points per colonist";
        private const string SettingPercentBuildingWealth = "Percentage of building wealth used";
        private const string SettingPercentArmour = "Percentage of value used for armour";

        private const string SettingPercentIndustrialWeapons =
            "Percentage of value used for industrial weapons or better";

        private const string SettingPercentPreIndustrialWeapons = "Percentage of value used for pre-industrial weapons";
        private const string SettingPercentReleaseableAnimalPower = "Percentage of combat animal strength to use";
        private const string SettingDebugLogging = "Show debug output in log";

        private const string SettingPreIndustrialArmor =
            "Use pre-industrial weapon percent also for pre-industrial armors";

        private const string SettingFair = "Fair";
        private const string SettingThisIsFine = "This is fine";
        private const string SettingFeelsBadMan = "Feels bad man";
        private const string SettingPainTrain = "Pain train";
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
            var listingStandard2 = listingStandard1;
            listingStandard2.Begin(inRect);
            listingStandard2.ColumnWidth = inRect.width / 2;
            if (listingStandard2.ButtonText(SettingFair))
            {
                settings.numPointsPerColonist = 45;
                settings.percentOfValueForBuildings = 25f;
                settings.percentOfValueForArmour = 100f;
                settings.percentOfValueForIndustrialWeapons = 100f;
                settings.percentOfValueForPreIndustrialWeapons = 25f;
                settings.percentOfCombatPowerForReleasableAnimals = 9f;
                RefreshBuffer();
            }

            if (listingStandard2.ButtonText(SettingThisIsFine))
            {
                settings.numPointsPerColonist = 85;
                settings.percentOfValueForBuildings = 50f;
                settings.percentOfValueForArmour = 200f;
                settings.percentOfValueForIndustrialWeapons = 200f;
                settings.percentOfValueForPreIndustrialWeapons = 50f;
                settings.percentOfCombatPowerForReleasableAnimals = 18f;
                RefreshBuffer();
            }

            if (listingStandard2.ButtonText(SettingFeelsBadMan))
            {
                settings.numPointsPerColonist = 125;
                settings.percentOfValueForBuildings = 75f;
                settings.percentOfValueForArmour = 250f;
                settings.percentOfValueForIndustrialWeapons = 250f;
                settings.percentOfValueForPreIndustrialWeapons = 65f;
                settings.percentOfCombatPowerForReleasableAnimals = 27f;
                RefreshBuffer();
            }

            if (listingStandard2.ButtonText(SettingPainTrain))
            {
                settings.numPointsPerColonist = 165;
                settings.percentOfValueForBuildings = 100f;
                settings.percentOfValueForArmour = 300f;
                settings.percentOfValueForIndustrialWeapons = 300f;
                settings.percentOfValueForPreIndustrialWeapons = 75f;
                settings.percentOfCombatPowerForReleasableAnimals = 36f;
                RefreshBuffer();
            }

            listingStandard2.Gap();
            listingStandard2.CheckboxLabeled(SettingPreIndustrialArmor, ref settings.preIndustrialArmor);
            listingStandard2.Gap();
            listingStandard2.CheckboxLabeled(SettingDebugLogging, ref settings.debugLog);
            if (currentVersion != null)
            {
                listingStandard2.Gap();
                GUI.contentColor = Color.gray;
                listingStandard2.Label($"Installed mod-version: {currentVersion}");
                GUI.contentColor = Color.white;
            }

            listingStandard2.NewColumn();
            RefreshBuffer();
            listingStandard2.Label(SettingPointsPerColonist);
            string numInputBuffer1;
            listingStandard2.TextFieldNumeric(ref settings.numPointsPerColonist, ref numInputBuffer1, 1f, 1000f);
            listingStandard2.Gap();
            listingStandard2.Label(SettingPercentBuildingWealth);
            string numInputBuffer2;
            listingStandard2.TextFieldNumeric(ref settings.percentOfValueForBuildings, ref numInputBuffer2, max: 800f);
            listingStandard2.Gap();
            listingStandard2.Label(SettingPercentArmour);
            string numInputBuffer6;
            listingStandard2.TextFieldNumeric(ref settings.percentOfValueForArmour, ref numInputBuffer6, max: 800f);
            listingStandard2.Gap();
            listingStandard2.Label(SettingPercentIndustrialWeapons);
            string numInputBuffer3;
            listingStandard2.TextFieldNumeric(ref settings.percentOfValueForIndustrialWeapons, ref numInputBuffer3,
                max: 800f);
            listingStandard2.Gap();
            listingStandard2.Label(SettingPercentPreIndustrialWeapons);
            string numInputBuffer4;
            listingStandard2.TextFieldNumeric(ref settings.percentOfValueForPreIndustrialWeapons, ref numInputBuffer4,
                max: 800f);
            listingStandard2.Gap();
            listingStandard2.Label(SettingPercentReleaseableAnimalPower);
            string numInputBuffer5;
            listingStandard2.TextFieldNumeric(ref settings.percentOfCombatPowerForReleasableAnimals,
                ref numInputBuffer5, max: 800f);
            listingStandard2.End();

            void RefreshBuffer()
            {
                numInputBuffer1 = settings.numPointsPerColonist.ToString();
                numInputBuffer2 = settings.percentOfValueForBuildings.ToString();
                numInputBuffer6 = settings.percentOfValueForArmour.ToString();
                numInputBuffer3 = settings.percentOfValueForIndustrialWeapons.ToString();
                numInputBuffer4 = settings.percentOfValueForPreIndustrialWeapons.ToString();
                numInputBuffer5 = settings.percentOfCombatPowerForReleasableAnimals.ToString();
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
                Scribe_Values.Look(ref debugLog, "shouldLogDebugInfo", forceSave: true);
                Scribe_Values.Look(ref preIndustrialArmor, "preIndustrialArmor");
            }
        }
    }
}