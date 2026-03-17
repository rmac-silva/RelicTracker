using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(StrikeDummy), nameof(StrikeDummy.ModifyDamageAdditive))]
public static class StrikeDummyPatch
{

    private static List<CardModel> upgradedStrikesThisTurn = new List<CardModel>();
    private static int currentRound = -1;

    static void Postfix(
        StrikeDummy __instance,
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (!props.IsPoweredAttack_())
        {
            return;
        }
        if (cardSource == null)
        {
            return;
        }
        if (!cardSource.Tags.Contains(CardTag.Strike))
        {
            return;
        }
        if (dealer != __instance.Owner.Creature && cardSource.Owner != __instance.Owner)
        {
            return;
        }

        currentRound = __instance.Owner.Creature.CombatState.RoundNumber;

        if(upgradedStrikesThisTurn.Contains(cardSource))
        {
            return;
        } else
        {
            upgradedStrikesThisTurn.Add(cardSource);
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Increased [gold]Strike[/gold] damage by [blue]{0}[/blue] (including unplayed cards).",
            new List<int> { __instance.DynamicVars["ExtraDamage"].IntValue }
        );
    }
}
