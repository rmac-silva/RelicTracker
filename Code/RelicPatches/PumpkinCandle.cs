using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PumpkinCandle), nameof(PumpkinCandle.ModifyMaxEnergy))]
public static class PumpkinCandleEnergyPatch
{
    private static FieldInfo _kindleCountField = AccessTools.Field(
        typeof(PumpkinCandle),
        "_kindleCount"
    );
    private static int roundCounter = -1;

    static void Postfix(PumpkinCandle __instance, Player player, decimal amount)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        int _kindleCount = (int)_kindleCountField.GetValue(__instance);
        if (_kindleCount <= 0)
        {
            return;
        }

        roundCounter = player.Creature.CombatState.RoundNumber;

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Energy.IntValue }
        );
    }
}
