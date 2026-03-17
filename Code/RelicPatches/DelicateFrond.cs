using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(DelicateFrond), nameof(DelicateFrond.BeforeCombatStart))]
public static class DelicateFrondPatch
{
    static void Prefix(DelicateFrond __instance)
    {
        int freePotionSlots = __instance.Owner.Potions.Count() - __instance.Owner.MaxPotionCount;
        ModLog.Info($"Delicate Frond: Free Potion Slots = {freePotionSlots}");
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Generated [blue]{0}[/blue] potions.",
            new List<int> { -1 * freePotionSlots }
        );
    }
}
