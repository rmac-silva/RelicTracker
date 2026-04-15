using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(IceCream), nameof(IceCream.ShouldPlayerResetEnergy))]
public static class IceCreamPatch
{
    static void Prefix(IceCream __instance, Player player)
    {
        if (player.Creature.CombatState.RoundNumber == 1 || player != __instance.Owner)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { player?.PlayerCombatState?.Energy ?? 0 }
        );
    }
}
