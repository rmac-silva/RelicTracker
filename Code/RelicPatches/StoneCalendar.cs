using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(StoneCalendar), "BeforeSideTurnEnd")]
public static class StoneCalendarPatch
{
    static void Prefix(
        StoneCalendar __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants
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
