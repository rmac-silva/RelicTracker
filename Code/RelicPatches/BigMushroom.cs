using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BigMushroom), nameof(BigMushroom.ModifyHandDraw))]
public static class BigMushroomPatch
{
    private static int _lastCombatID = 0;

    static void Postfix(BigMushroom __instance, Player player, decimal cardsToDraw)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        if (
            player.Creature.CombatState.RoundNumber == 1
            && CombatStartManager.IsNewCombat(ref _lastCombatID)
        )
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
        }
    }
}
