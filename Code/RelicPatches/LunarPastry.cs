using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(LunarPastry), nameof(LunarPastry.AfterTurnEnd))]
public static class LunarPastryPatch
{
    static void Postfix(LunarPastry __instance, PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == __instance.Owner.Creature.Side)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]stars[/gold].",
                new List<int> { __instance.DynamicVars.Stars.IntValue }
            );
        }
    }
}
