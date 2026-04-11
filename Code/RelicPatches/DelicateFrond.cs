using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(DelicateFrond), nameof(DelicateFrond.BeforeCombatStart))]
public static class DelicateFrondPatch
{
    static void Prefix(DelicateFrond __instance)
    {
        int freePotionSlots = __instance.Owner.Potions.Count() - __instance.Owner.MaxPotionCount;
        
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { -1 * freePotionSlots }
        );
    }
}
