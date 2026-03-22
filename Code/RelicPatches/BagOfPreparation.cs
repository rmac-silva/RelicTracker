using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BagOfPreparation), nameof(BagOfPreparation.ModifyHandDraw))]
public static class BagOfPreparationPatch
{
    static void Postfix(
        BagOfPreparation __instance,
        Player player, decimal count
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
                "Drew [blue]{0}[/blue] more cards.",
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
        }
    }
}
