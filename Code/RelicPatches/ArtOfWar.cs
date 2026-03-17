using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ArtOfWar), nameof(ArtOfWar.AfterEnergyReset))]
public static class ArtOfWarPatch
{
    static void Prefix(ArtOfWar __instance, Player player)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        var field = AccessTools.Field(typeof(ArtOfWar), "_anyAttacksPlayedLastTurn");
        bool anyAttacksPlayedLastTurn = (bool)field.GetValue(__instance);
        if (player.Creature.CombatState.RoundNumber > 1 && !anyAttacksPlayedLastTurn)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]Energy[/gold].",
                new List<int> { 1 }
            );
        }
    }
}
