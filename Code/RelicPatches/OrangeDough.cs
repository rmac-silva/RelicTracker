using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(OrangeDough), nameof(OrangeDough.AfterSideTurnStart))]
public static class OrangeDoughPatch
{
    static void Postfix(OrangeDough __instance, CombatSide side, CombatState combatState)
    {
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Added [blue]{0}[/blue] colorless cards.",
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
        }
    }
}
