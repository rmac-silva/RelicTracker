using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PumpkinCandle), nameof(PumpkinCandle.ModifyMaxEnergy))]
public static class PumpkinCandleEnergyPatch
{
    private static int roundCounter = -1;

    static void Postfix(PumpkinCandle __instance, Player player, decimal amount)
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (__instance.ActiveAct != __instance.Owner.RunState.CurrentActIndex)
        {
            return;
        }

        if (roundCounter == player.Creature.CombatState.RoundNumber)
        {
            return;
        }

        roundCounter = player.Creature.CombatState.RoundNumber;

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Added [blue]{0}[/blue] [gold]Energy[/gold].",
            new List<int> { __instance.DynamicVars.Energy.IntValue }
        );
    }
}
