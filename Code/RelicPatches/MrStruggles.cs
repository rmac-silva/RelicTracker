using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MrStruggles), nameof(MrStruggles.AfterPlayerTurnStart))]
public static class MrStrugglesPatch
{
    static void Postfix(MrStruggles __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player == __instance.Owner)
        {
            ICombatState combatState = player.Creature.CombatState;

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { combatState.HittableEnemies.Count * combatState.RoundNumber }
        );
        }
    }
}
