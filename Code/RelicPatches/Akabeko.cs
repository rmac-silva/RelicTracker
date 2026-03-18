using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Akabeko), nameof(Akabeko.AfterSideTurnStart))]
public static class AkabekoPatch
{
    static void Postfix(Akabeko __instance, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]Vigor[/gold].",
                new List<int> { 8 }
            );
        }
    }
}
