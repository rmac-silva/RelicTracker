using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(DivineRight), nameof(DivineRight.AfterRoomEntered))]
public static class DivineRightPatch
{
    static void Postfix(DivineRight __instance, AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]stars[/gold].",
                new List<int> { __instance.DynamicVars.Stars.IntValue }
            );
        }
    }
}
[HarmonyPatch(typeof(DivineDestiny), nameof(DivineDestiny.AfterSideTurnStart))]
public static class DivineDestinyPatch
{
    static void Postfix(DivineDestiny __instance, CombatSide side, CombatState combatState)
    {
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1) {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]stars[/gold].",
                new List<int> { __instance.DynamicVars.Stars.IntValue }
            );
        }
    }
}
