using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(LostWisp), nameof(LostWisp.AfterCardPlayed))]
public static class LostWispPatch
{
    static void Postfix(LostWisp __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (
            cardPlay.Card.Owner == __instance.Owner
            && cardPlay.Card.Type == CardType.Power
        )
        {
            int numEnemies = __instance.Owner.Creature.CombatState.HittableEnemies.Count;

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Damage.IntValue * numEnemies }
        );
        }
    }
}
