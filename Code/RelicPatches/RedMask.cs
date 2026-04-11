using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RedMask), nameof(RedMask.BeforeSideTurnStart))]
public static class RedMaskPatch
{
    static void Postfix(
        RedMask __instance,
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
            new List<int> { combatState.HittableEnemies.Count() }
        );
        }
    }
}
