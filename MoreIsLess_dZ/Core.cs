using System;
using System.Collections;
using System.Reflection;
using Harmony;
using Newtonsoft.Json;
using static MoreIsLess_dZ.Logger;
using BattleTech;
using BattleTech.UI;
using UnityEngine;

namespace MoreIsLess_dZ
{
    public static class Core
    {
        #region Init

        public static void Init(string modDir, string settings)
        {
            var harmony = HarmonyInstance.Create("com.donZappo.MoreIsLess");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            // read settings
            try
            {
                Settings = JsonConvert.DeserializeObject<ModSettings>(settings);
                Settings.modDirectory = modDir;
            }
            catch (Exception)
            {
                Settings = new ModSettings();
            }

            // blank the logfile
            Clear();
            // PrintObjectFields(Settings, "Settings");
        }
        // logs out all the settings and their values at runtime
        internal static void PrintObjectFields(object obj, string name)
        {
            LogDebug($"[START {name}]");

            var settingsFields = typeof(ModSettings)
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var field in settingsFields)
            {
                if (field.GetValue(obj) is IEnumerable &&
                    !(field.GetValue(obj) is string))
                {
                    LogDebug(field.Name);
                    foreach (var item in (IEnumerable)field.GetValue(obj))
                    {
                        LogDebug("\t" + item);
                    }
                }
                else
                {
                    LogDebug($"{field.Name,-30}: {field.GetValue(obj)}");
                }
            }

            LogDebug($"[END {name}]");
        }

        #endregion

        internal static ModSettings Settings;
    }

    [HarmonyPatch(typeof(AAR_UnitStatusWidget), "FillInPilotData", null)]
    public static class AAR_UnitStatusWidget_FillInPilotData
    {
        private static void Prefix(AAR_UnitStatusWidget __instance, ref int xpEarned, SimGameState ___simState, UnitResult ___UnitData)
        {
            if (___simState.Constants.Story.MaximumDebt == 42 || Core.Settings.AlwaysOn)
            {
                var totalXP = ___UnitData.pilot.TotalXP;
                var mechsKilled = ___UnitData.pilot.MechsKilled;
                var vehiclesKilled = ___UnitData.pilot.OthersKilled;

                // get the total ejections - Dependent on Panic System. Thanks gnivler! 
                int? mechEjections = 0;
                int? vehicleEjections = 0;

                if (Core.Settings.PanicEjections)
                {
                    mechEjections = ___UnitData.pilot.StatCollection.GetStatistic("MechsEjected")?.Value<int>();
                    vehicleEjections = ___UnitData.pilot.StatCollection.GetStatistic("VehiclesEjected")?.Value<int>();
                }

                totalXP = (int)(mechsKilled * Core.Settings.XP_MechKill + vehiclesKilled * Core.Settings.XP_VehicleKill +
                    mechEjections * Core.Settings.XP_MechEjection + vehicleEjections * Core.Settings.XP_VehicleEjection);

                int totalXPChunks = totalXP / Core.Settings.intPerXP;
                float NewXP = (float)xpEarned;

                float XPMulti = Core.Settings.floatXPMulti;
                float CorrectedXPFactor = Mathf.Pow(XPMulti, (float)totalXPChunks);

                NewXP = (float)((int)(NewXP * CorrectedXPFactor));

                if ((float)totalXP + NewXP >= (float)Core.Settings.XPMax && Core.Settings.XPCap)
                {
                    NewXP = (float)(Core.Settings.XPMax - totalXP);
                }
                else
                {
                    NewXP = (float)((int)NewXP);
                }

                int unspentXP = ___UnitData.pilot.UnspentXP;
                int XPCorrection = xpEarned - (int)NewXP;
                ___UnitData.pilot.StatCollection.Set<int>("ExperienceUnspent", unspentXP - XPCorrection);
                xpEarned = (int)NewXP;
            }
        }
    }
}