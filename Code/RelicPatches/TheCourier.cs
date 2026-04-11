using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(TheCourier), nameof(TheCourier.ShouldRefillMerchantEntry))]
public static class TheCourierPatch
{
    static void Postfix(TheCourier __instance, MerchantEntry entry, Player player)
    {
        if (player == __instance.Owner)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
        }
    }
}
