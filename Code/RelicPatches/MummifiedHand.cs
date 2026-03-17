using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MummifiedHand), nameof(MummifiedHand.AfterCardPlayed))]
public static class MummifiedHandPatch
{
    static void Postfix(MummifiedHand __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != __instance.Owner)
        {
            return;
        }
        if (!CombatManager.Instance.IsInProgress)
        {
            return;
        }
        if (cardPlay.Card.Type != CardType.Power)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Triggered [blue]{0}[/blue] times.",
            new List<int> { 1 }
        );

    }
}
