using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BagOfMarbles), nameof(BagOfMarbles.BeforeSideTurnStart))]
public static class BagOfMarblesPatch
{
    static void Postfix(
        BagOfMarbles __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side,
        CombatState combatState
    )
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Applied [gold]Vulnerable[/gold] to [blue]{0}[/blue] enemies.",
                new List<int> { combatState.HittableEnemies.Count }
            );
        }
    }
}
