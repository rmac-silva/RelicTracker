using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
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
    private static int _lastCombatId = -1;

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

        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
        }

        if (player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Energy.IntValue, 0 }
        );
        }

        
    }
}

[HarmonyPatch(typeof(PumpkinCandle), nameof(PumpkinCandle.AfterObtained))]
public static class PumpkinCandleObtainedPatch
{
    static void Postfix(PumpkinCandle __instance)
    {
        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 0, 0 });
    }
}

[HarmonyPatch(typeof(PumpkinCandle), nameof(PumpkinCandle.Rekindle))]
public static class PumpkinCandleRekindlePatch
{

    static void Postfix(PumpkinCandle __instance)
    {
        if (!LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        

        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 0, 1 });
    }
}
