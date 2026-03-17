using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FestivePopper), nameof(FestivePopper.AfterPlayerTurnStart))]
public static class FestivePopperPatch
{
    static void Postfix(FestivePopper __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player == __instance.Owner)
        {
            CombatState combatState = player.Creature.CombatState;
            if (combatState.RoundNumber == 1)
            {
                int numEnemies = combatState.HittableEnemies.Count;

                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Dealt [blue]{0}[/blue] [orange]damage[/orange].",
                    new List<int> { 9 * numEnemies }
                );
            }
        }
    }
}
