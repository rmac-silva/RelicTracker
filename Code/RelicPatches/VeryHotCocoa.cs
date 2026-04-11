using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(VeryHotCocoa), nameof(VeryHotCocoa.AfterSideTurnStart))]
public static class VeryHotCocoaEnergyPatch
{

    static void Postfix(VeryHotCocoa __instance, CombatSide side, CombatState combatState)
    {
        

        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        )
            return;

        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Energy.IntValue }
        );
        }

    }
}
