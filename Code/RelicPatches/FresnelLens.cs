using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics; // Ensure this matches FresnelLens's namespace

namespace RelicTracker.Patches
{
    [HarmonyPatch(typeof(FresnelLens), "EnchantCard")]
    public static class FresnelLensEnchantPatch
    {

        [HarmonyPostfix]
        public static void Postfix(FresnelLens __instance)
        {
            // Verify we actually have a valid instance and result before recording
            if (__instance != null)
            {
                // Using your specific RecordCustomStat format
                RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );

            }
        }
    }
}