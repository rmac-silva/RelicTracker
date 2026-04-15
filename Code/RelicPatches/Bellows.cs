using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Bellows), nameof(Bellows.AfterPlayerTurnStart))]
public static class BellowsPatch
{
    static void Prefix(Bellows __instance, PlayerChoiceContext choiceContext, Player player)
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (player.Creature.CombatState.RoundNumber > 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { PileType.Hand.GetPile(__instance.Owner).Cards.Count }
        );
    }
}
