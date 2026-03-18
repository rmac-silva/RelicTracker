using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MoltenEgg), nameof(MoltenEgg.TryModifyCardBeingAddedToDeck))]
public static class MoltenEggPatch
{
    static void Prefix(MoltenEgg __instance, CardModel card)
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        if (card.Type != CardType.Attack)
        {
            return;
        }
        if (!card.IsUpgradable)
        {
            return;
        }
        if (card.CurrentUpgradeLevel >= 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Upgraded [blue]{0}[/blue] cards throughout the run.",
            new List<int> { 1 }
        );
    }
}