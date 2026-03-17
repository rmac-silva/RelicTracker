using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(CloakClasp), nameof(CloakClasp.BeforeTurnEnd))]
public static class CloakClaspPatch
{
    static void Postfix(
        CloakClasp __instance,
        PlayerChoiceContext choiceContext, CombatSide side
    )
    {
        if (side == __instance.Owner.Creature.Side)
        {
            IReadOnlyList<CardModel> cards = PileType.Hand.GetPile(__instance.Owner).Cards;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]block[/gold].",
                new List<int> { 1 * cards.Count }
            );
        }
    }
}
