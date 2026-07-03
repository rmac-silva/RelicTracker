using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(DemonTongue), nameof(DemonTongue.AfterDamageReceived))]
public static class DemonTonguePatch
{

    private static readonly System.Reflection.FieldInfo WasTriggeredField = AccessTools.Field(
        typeof(DemonTongue),
        "_triggeredThisTurn"
    );

    static void Prefix(
        DemonTongue __instance,
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        
        bool triggeredThisTurn = (bool)WasTriggeredField.GetValue(__instance);

        if (
            __instance.Owner.Creature.CombatState != null
            && __instance.Owner.Creature.CombatState.CurrentSide == __instance.Owner.Creature.Side
            && target == __instance.Owner.Creature
            && result.UnblockedDamage > 0
            && !triggeredThisTurn
        )
        {

            Creature creature = __instance.Owner.Creature;
            int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
            //If the health I'm missing is below the heal amount, then I'm only healing for healthMising

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { healthMissing < result.UnblockedDamage ? healthMissing : result.UnblockedDamage });
        }
            
    }
}
