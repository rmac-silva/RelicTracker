using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(StoneCalendar), nameof(StoneCalendar.BeforeTurnEnd))]
public static class StoneCalendarPatch
{
    static void Prefix(
        StoneCalendar __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side
    )
    {
        if (side == __instance.Owner.Creature.Side)
        {
            int intValue = __instance.DynamicVars["DamageTurn"].IntValue;
            int roundNumber = __instance.Owner.Creature.CombatState.RoundNumber;

            if (roundNumber == intValue)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Dealt [blue]{0}[/blue] damage.",
                    new List<int>
                    {
                        __instance.Owner.Creature.CombatState.HittableEnemies.Count
                            * __instance.DynamicVars.Damage.IntValue,
                    }
                );
            }
        }
    }
}
