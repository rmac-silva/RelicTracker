using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(MysticLighter), nameof(MysticLighter.ModifyDamageAdditive))]
public static class MysticLighterPatch
{
    private static List<string> upgradedCardsThisRound = new List<string>();
    private static int roundCounter = -1;

    static void Postfix(
        MysticLighter __instance,
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource

    )
    {



        

        if (!props.IsPoweredAttackRelicTracker())
        {
            return;
        }
        if (cardSource?.Enchantment == null)
        {
            return;
        }
        if (cardSource.Owner != __instance.Owner)
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
            new List<int> { __instance.DynamicVars.Damage.IntValue }
        );
    }
}
