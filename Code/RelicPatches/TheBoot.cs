using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

/// <summary>
/// Registers the damage gained from The Boot. Takes the original damage value and calculates the difference between that and the maximum The Boot value.
/// </summary>
[HarmonyPatch(typeof(TheBoot), nameof(TheBoot.ModifyHpLostBeforeOsty))]
public static class TheBootPatch
{
    static void Postfix(
        TheBoot __instance,
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (dealer != __instance.Owner.Creature)
        {
            return;
        }
        if (!props.IsPoweredAttackRelicTracker())
        {
            return;
        }
        if (amount < 1m)
        {
            return;
        }
        if (amount >= __instance.DynamicVars["DamageMinimum"].BaseValue)
        {
            return;
        }

        //Was going to deal 3 damage, but the boot increased it to 5. minimum - original = damageIncrease.
        int damageIncreased = __instance.DynamicVars["DamageMinimum"].IntValue - (int)amount;

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 0, damageIncreased }
        );
    }
}

/// <summary>
/// Increases the generic trigger counter
/// </summary>
[HarmonyPatch(typeof(TheBoot), nameof(TheBoot.AfterModifyingHpLostBeforeOsty))]
public static class TheBootTriggerCountPatch
{
    static void Postfix(TheBoot __instance)
    {
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1, 0 }
        );
    }
}
