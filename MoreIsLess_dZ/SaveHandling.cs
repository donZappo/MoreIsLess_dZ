using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using BattleTech;
using BattleTech.Save;

namespace MoreIsLess_dZ
{
    public static class SaveHandling
    {
        //This patch is useful for setting up the Sim after the career or campaign is initially started. 
        [HarmonyPatch(typeof(SimGameState), "_OnAttachUXComplete")]
        public static class SimGameState__OnAttachUXComplete_Patch
        {
            public static void Postfix()
            {
                //var sim = UnityGameInstance.BattleTechGame.Simulation;
                //if ()
                //{
                //}
                //else
                //{
                //    LogDebug("_OnAttachUXComplete Postfix");
                //}
            }
        }

        //On game load.
        [HarmonyPatch(typeof(SimGameState), "Rehydrate")]
        public static class SimGameState_Rehydrate_Patch
        {
            static void Postfix(SimGameState __instance, GameInstanceSave gameInstanceSave)
            {
                DeserializeXXX();
            }
        }

        internal static void DeserializeXXX()
        {
            //var sim = UnityGameInstance.BattleTechGame.Simulation;

            //Core.WarStatus = JsonConvert.DeserializeObject<WarStatus>(sim.CompanyTags.First(x => x.StartsWith("GalaxyAtWarSave{")).Substring(15));
            //LogDebug(">>> Deserialization complete");
            //LogDebug($"Size after load: {JsonConvert.SerializeObject(Core.WarStatus).Length / 1024}kb");
        }

        //What happens on game save.
        [HarmonyPatch(typeof(SimGameState), "Dehydrate")]
        public static class SimGameState_Dehydrate_Patch
        {
            public static void Prefix()
            {
                SerializeXXX();
            }
        }
        internal static void SerializeXXX()
        {
            //var sim = UnityGameInstance.BattleTechGame.Simulation;
            //sim.CompanyTags.Where(tag => tag.StartsWith("GalaxyAtWar")).Do(x => sim.CompanyTags.Remove(x));
            //sim.CompanyTags.Add("GalaxyAtWarSave" + JsonConvert.SerializeObject(Core.WarStatus));
            //LogDebug($"Serializing object size: {JsonConvert.SerializeObject(Core.WarStatus).Length / 1024}kb");
            //LogDebug(">>> Serialization complete");
        }
    }
}
