using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(SpikedGauntlets), nameof(SpikedGauntlets.ModifyMaxEnergy))]
public static class SpikedGauntletsEnergyPatch
{
    private static int roundCounter = 0;
    static void Postfix(SpikedGauntlets __instance, Player player, decimal amount)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        if(player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Added [blue]{0}[/blue] energy.\nMade [blue]{1}[/blue] [gold]Power[/gold] cards more expensive.",
                new List<int> { __instance.DynamicVars.Energy.IntValue, 0 }
            );
            
        }

    }
}

[HarmonyPatch(typeof(SpikedGauntlets), nameof(SpikedGauntlets.TryModifyEnergyCostInCombat))]
public static class SpikedGauntletsPowerPatch
{
    private static List<string> modifiedCardIdsThisTurn = new List<string>();
    private static int roundCounter = 0;

    static void Postfix(
        SpikedGauntlets __instance,
        CardModel card,
        decimal originalCost,
        ref decimal modifiedCost // Use 'ref' to ensure the change persists!
    )
    {
        // 1. Safety Checks
        if (__instance?.Owner?.Creature?.CombatState == null || card?.Owner == null) return;
        if (card.Owner.Creature != __instance.Owner.Creature) return;
        if (card.Type != CardType.Power) return;

        // 2. Round Tracking
        int currentRound = __instance.Owner.Creature.CombatState.RoundNumber;
        if (currentRound != roundCounter)
        {
            roundCounter = currentRound;
            modifiedCardIdsThisTurn.Clear();
        }

        // 3. Logic: Check if we already processed this specific card instance this turn
        // Use a unique ID or hash if possible, or the instance if it remains stable
        if (modifiedCardIdsThisTurn.Contains(card.Id.Entry)) 
        {
             // We still want the cost to be modified even if we already recorded the stat!
             modifiedCost = originalCost + 1m; 
             return;
        }

        // 4. Apply the cost increase
        modifiedCost = originalCost + 1m;
        modifiedCardIdsThisTurn.Add(card.Id.Entry);

        // 5. Record the stat
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Added [blue]{0}[/blue] energy.\nMade [blue]{1}[/blue] [gold]Power[/gold] cards more expensive.",
            new List<int> { 0, 1 }
        );
    }
}
