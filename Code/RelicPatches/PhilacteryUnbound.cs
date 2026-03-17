using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PhylacteryUnbound), nameof(PhylacteryUnbound.BeforeCombatStart))]
public static class PhylacteryUnboundInitialSummonPatch
{
    static void Postfix(PhylacteryUnbound __instance)
    {
        //Check if I'm the owner for multiplayer sessions
        if (!LocalContext.IsMe(__instance.Owner.Creature))
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Summoned [orange]Osty[/orange] [blue]{0}[/blue] times.",
            new List<int> { __instance.DynamicVars["StartOfCombat"].IntValue }
        );
    }
}

[HarmonyPatch(typeof(PhylacteryUnbound), nameof(PhylacteryUnbound.AfterSideTurnStart))]
public static class PhylacteryUnboundPerTurnPatch
{
    static void Postfix(PhylacteryUnbound __instance,CombatSide side, CombatState combatState)
    {
        //Check if I'm the owner for multiplayer sessions
        if(side != CombatSide.Player || !LocalContext.IsMe(__instance.Owner.Creature))
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Summoned [orange]Osty[/orange] [blue]{0}[/blue] times.",
            new List<int> { __instance.DynamicVars["StartOfTurn"].IntValue }
        );
    }
}
