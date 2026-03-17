using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BloodVial), nameof(BloodVial.AfterPlayerTurnStartLate))]
public static class BloodVialPatch
{
    static void Postfix(BloodVial __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Healed [blue]{0}[/blue] [gold]HP[/gold].",
                new List<int> { __instance.DynamicVars.Heal.IntValue }
            );
        }
    }
}
