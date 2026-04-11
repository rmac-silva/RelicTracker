using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Relics;

public static class GamblingChipHelper
{
    public static ConditionalWeakTable<CardModel, object?> activeHandlers =
        new ConditionalWeakTable<CardModel, object?>();

    public static void RecordGamblingChipStat()
    {
        RelicStatCache.RecordCustomStat("GAMBLING_CHIP", new List<int>() { 1 });
    }
}

[HarmonyPatch(typeof(GamblingChip), nameof(GamblingChip.AfterPlayerTurnStart))]
public static class GamblingChipPatch
{
    static void Prefix(GamblingChip __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player == __instance.Owner)
        {
            GamblingChipHelper.activeHandlers.Clear();

            if (__instance.Owner.Creature.CombatState.RoundNumber <= 1)
            {
                //Tags all the cards in hand of the player. When they are discarded we can check if they were tagged
                //If they are in the list, we increase the count of the gambling chip
                foreach (CardModel c in player.PlayerCombatState.Hand.Cards)
                {
                    GamblingChipHelper.activeHandlers.Add(c, null);
                }
            }
        }
    }
}

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.DiscardAndDraw))]
public static class GamblingChipDiscardPatch
{
    static void Prefix(
        PlayerChoiceContext choiceContext,
        IEnumerable<CardModel> cardsToDiscard,
        int cardsToDraw
    )
    {
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        List<CardModel> discardCards = cardsToDiscard.ToList();
        if (discardCards.Count == 0)
        {
            return;
        }

        foreach (CardModel card in discardCards)
        {
            if (GamblingChipHelper.activeHandlers.TryGetValue(card, out _))
            {
                //If the card is in the list, we increase the count of the gambling chip
                GamblingChipHelper.RecordGamblingChipStat();
            }
        }
    }
}
