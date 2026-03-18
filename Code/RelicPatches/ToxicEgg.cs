using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ToxicEgg), nameof(ToxicEgg.TryModifyCardBeingAddedToDeck))]
public static class ToxicEggPatch
{
    static void Prefix(ToxicEgg __instance, CardModel card)
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        if (card.Type != CardType.Skill)
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