using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Bread), nameof(Bread.ModifyMaxEnergy))]
public static class BreadMaxEnergyPatch
{
    private static int roundCounter = 0;
    private static int _lastCombatId = -1;

    static void Postfix(Bread __instance, Player player, decimal amount)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress)
            return;

        if (player != __instance.Owner)
        {
            return;
        }

        CombatState? combatState = player.Creature.CombatState;
        int currentRound = combatState.RoundNumber;

        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
        }

        if (combatState != null && combatState.RoundNumber > 1 && currentRound != roundCounter)
        {
            roundCounter = currentRound;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 0, 1 }
        );
        }
    }
}

[HarmonyPatch(typeof(Bread), nameof(Bread.AfterSideTurnStart))]
public static class BreadEnergyLossPatch
{
    static void Postfix(Bread __instance, CombatSide side, CombatState combatState)
    {
        if (side != __instance.Owner.Creature.Side)
        {
            return;
        }

        if (combatState != null && combatState.RoundNumber == 1)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 2, 0 }
        );
        }
    }
}
