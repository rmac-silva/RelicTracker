using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(MiniatureCannon), nameof(MiniatureCannon.ModifyDamageAdditive))]
public static class CannonPatch
{
    private static int roundCounter = -1;
    private static List<string> upgradedCardsThisRound = new List<string>();
    static void Postfix(
        MiniatureCannon __instance,
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
        if (!cardSource.IsUpgraded)
        {
            return;
        }
        if (dealer != __instance.Owner.Creature && cardSource.Owner != __instance.Owner)
        {
            return;
        }

        if(__instance.Owner.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = __instance.Owner.Creature.CombatState.RoundNumber;
            upgradedCardsThisRound.Clear();
        }

        if(__instance.Owner.Creature.CombatState.RoundNumber == roundCounter && upgradedCardsThisRound.Contains(cardSource.Id.Entry))
        {
            return;
        }

        roundCounter = __instance.Owner.Creature.CombatState.RoundNumber;
        upgradedCardsThisRound.Add(cardSource.Id.Entry);

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Added [blue]{0}[/blue] damage to attacks (including not played).",
            new List<int> { __instance.DynamicVars["ExtraDamage"].IntValue }
        );
    }
}
