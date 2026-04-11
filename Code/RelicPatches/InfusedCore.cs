using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(InfusedCore), nameof(InfusedCore.AfterSideTurnStart))]
public static class InfusedCorePatch
{
    static void Postfix(InfusedCore __instance, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 3 }
        );
        }
    }
}
