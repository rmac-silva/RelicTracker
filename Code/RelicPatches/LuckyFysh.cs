using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(LuckyFysh), nameof(LuckyFysh.AfterCardChangedPiles))]
public static class LuckyFyshPatch
{
    static void Postfix(
        LuckyFysh __instance,
        CardModel card,
        PileType oldPileType,
        AbstractModel? source
    )
    {
        CardPile? pile = card.Pile;
        if (pile != null && pile.Type == PileType.Deck && card.Owner == __instance.Owner)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Gold.IntValue }
        );
        }
    }
}
