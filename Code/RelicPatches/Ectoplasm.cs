using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Ectoplasm), nameof(Ectoplasm.ModifyMaxEnergy))]
public static class EctoplasmPatch
{
private static int roundCounter = 0;
    static void Postfix(Ectoplasm __instance, Player player, decimal amount)
    {
        
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] energy.\nLost [blue]{1}[/blue] gold.",
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
                "Gained [blue]{0}[/blue] energy.\nLost [blue]{1}[/blue] [gold]Gold[/gold].",
                new List<int> { 0, (int)amount }
            );
        }
    }
}
