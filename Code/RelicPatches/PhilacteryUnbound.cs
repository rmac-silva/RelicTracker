using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PhylacteryUnbound), nameof(PhylacteryUnbound.BeforeCombatStart))]
public static class PhylacteryUnboundInitialSummonPatch
{
    private static int _lastCombatId = 0;
    static void Postfix(PhylacteryUnbound __instance)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress || !CombatStartManager.IsNewCombat(ref _lastCombatId)) return;

        //Check for ownership
        if(!LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        _lastCombatId = CombatStartManager._currentCombatId;
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
    static void Postfix(PhylacteryUnbound __instance, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        
        if(!LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        if(side != CombatSide.Player)
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
