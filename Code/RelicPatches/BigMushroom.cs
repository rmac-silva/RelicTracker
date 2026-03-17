using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BigMushroom), nameof(BigMushroom.ModifyHandDraw))]
public static class BigMushroomPatch
{
    static void Postfix(
        BigMushroom __instance,
        Player player, decimal cardsToDraw
    )
    {
        if (player != __instance.Owner)
        {
            return;
        }

        if (player.Creature.CombatState.RoundNumber == 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Drew [blue]{0}[/blue] less cards.",
                new List<int> { 2 }
            );
        }
    }
}
