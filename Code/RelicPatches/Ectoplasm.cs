using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Ectoplasm), nameof(Ectoplasm.ModifyMaxEnergy))]
public static class EctoplasmPatch
{
private static int roundCounter = 0;
    private static int _lastCombatId = -1;
    static void Postfix(Ectoplasm __instance, Player player, decimal amount)
    {

        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
            _lastCombatId = CombatStartManager._currentCombatId;
        }
        
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1, 0 }
        );
        }
    }
}

[HarmonyPatch(typeof(Ectoplasm), nameof(Ectoplasm.ShouldGainGold))]
public static class EctoplasmMoneyPatch
{

    static void Postfix(Ectoplasm __instance, decimal amount, Player player)
    {
        
        if (player == __instance.Owner)
        {
            
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 0, (int)amount }
        );
        }
    }
}
