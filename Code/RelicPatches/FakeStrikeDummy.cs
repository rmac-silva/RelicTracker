using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(FakeStrikeDummy), nameof(FakeStrikeDummy.ModifyDamageAdditive))]
public static class FakeStrikeDummyPatch
{
    static void Postfix(
        FakeStrikeDummy __instance,
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

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Added [blue]{0}[/blue] damage.",
            new List<int> { 1 }
        );
    }
}
