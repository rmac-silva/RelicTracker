using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(UndyingSigil), nameof(UndyingSigil.ModifyDamageMultiplicative))]
public static class UndyingSigilPatch
{
    private static int roundCounter = -1;
    

    static void Postfix(
        UndyingSigil __instance,
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        

        if (dealer == null)
        {
            return;
        }
        if (!props.IsPoweredAttack_())
        {
            return;
        }
        if (target != __instance.Owner.Creature)
        {
            return;
        }
        if (dealer == __instance.Owner.Creature)
        {
            return;
        }
        if (dealer.CurrentHp > dealer.GetPowerAmount<DoomPower>())
        {
            return;
        }

        if (__instance.Owner?.Creature?.CombatState == null)
        {
            return;
        }

        if (roundCounter != __instance.Owner.Creature.CombatState.RoundNumber)
        {
            roundCounter = __instance.Owner.Creature.CombatState.RoundNumber;
            __instance.Flash();
        }

    }

    
}
