using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(SelfFormingClay), nameof(SelfFormingClay.AfterDamageReceived))]
public static class SelfFormingClayPatch
{
    static void Prefix(
        SelfFormingClay __instance,
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (target == __instance.Owner.Creature && result.UnblockedDamage > 0)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] Block.",
                new List<int> { __instance.DynamicVars["BlockNextTurn"].IntValue }
            );
        }
    }
}
