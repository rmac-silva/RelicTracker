using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(BeatingRemnant), "ModifyHpLostAfterOsty")]
public static class BeatingRemnantPatch
{
    private static readonly System.Reflection.FieldInfo? _numCardsPlayedField = AccessTools.Field(
        typeof(BeatingRemnant),
        "_damageReceivedThisTurn"
    );

    static void Prefix(
        BeatingRemnant __instance,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return;
        }
        if (target != __instance.Owner.Creature)
        {
            return;
        }

        var damage_received_this_turn = _numCardsPlayedField.GetValue(__instance) as decimal? ?? 0;

        if (damage_received_this_turn >= 20) //We have taken more than 20 damage this turn, so any damage we're going to take is going to be completely cancelled
        {
            RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { (int)amount });
        }
        else
        {
            //Compute how much damage we actually prevented
            //Let's imagine I take 14 damage, and have damageReceivedThisTurn = 15
            //We're going to do (20 - 15) and that's the maximum amount of damage I can take (5)
            var damage_we_can_still_take =
                __instance.DynamicVars["MaxHpLoss"].BaseValue - damage_received_this_turn;

            //So now we need to simply subtract that amount from the damage incoming, and that's how much I prevented
            //In this case 14 damage - 5 that we can still take, results in 9 damage prevented
            var damage_prevented = Math.Max(0, amount - damage_we_can_still_take);

            if (damage_prevented > 0)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    new List<int> { (int)damage_prevented }
                );
            }
        }
    }
}
