using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MummifiedHand), nameof(MummifiedHand.AfterCardPlayed))]
public static class MummifiedHandPatch
{
    static void Postfix(MummifiedHand __instance, PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress)
        {
            return;
        } 
            
        if (cardPlay.Card.Owner != __instance.Owner) {
            return;
        }

        if (cardPlay.Card.Type != CardType.Power)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );

    }
}
