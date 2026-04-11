using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RoyalPoison), nameof(RoyalPoison.AfterPlayerTurnStart))]
public static class RoyalPoisonPatch
{
    static void Postfix(RoyalPoison __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Damage.IntValue }
        );
        }
    }
}
