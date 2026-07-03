using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MummifiedHand), nameof(MummifiedHand.AfterCardPlayed))]
public static class MummifiedHandPatch
{
    public static bool mummifiedHandRunning = false;

    static void Prefix(
        MummifiedHand __instance,
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay
    )
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress)
        {
            return;
        }

        if (cardPlay.Card.Owner != __instance.Owner)
        {
            return;
        }

        if (cardPlay.Card.Type != CardType.Power)
        {
            return;
        }
        mummifiedHandRunning = true;
        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 1, 0 });
    }

    static void Postfix(
        MummifiedHand __instance,
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay
    )
    {
        mummifiedHandRunning = false;
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.SetToFreeThisTurn))]
public static class MummifiedHandFreeCardPatch
{
    static void Prefix(CardModel __instance)
    {
        if (MummifiedHandPatch.mummifiedHandRunning)
        {
            RelicStatCache.RecordCustomStat(
                "MUMMIFIED_HAND",
                new List<int> { 0, __instance.EnergyCost.Canonical }
            );
        }
    }
}
