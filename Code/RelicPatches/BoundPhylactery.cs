using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BoundPhylactery), nameof(BoundPhylactery.AfterEnergyResetLate))]
public static class BoundPhylacteryPatch
{
    private static int _lastCombatId = 0;

    static void Postfix(BoundPhylactery __instance, Player player)
    {
        if (
            player == __instance.Owner
            && player?.Creature?.CombatState?.RoundNumber != 1
            && CombatStartManager.IsNewCombat(ref _lastCombatId)
        )
        {
            _lastCombatId = CombatStartManager._currentCombatId;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Summon.IntValue }
        );
        }
    }
}
