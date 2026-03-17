using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PrismaticGem), nameof(PrismaticGem.ModifyMaxEnergy))]
public static class PrismaticGemEnergyPatch
{
    private static int roundCounter = -1;

    static void Postfix(PrismaticGem __instance, Player player, decimal amount)
    {
        if (player != __instance.Owner)
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
