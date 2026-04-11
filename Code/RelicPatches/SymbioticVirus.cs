using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(SymbioticVirus), nameof(SymbioticVirus.AfterSideTurnStart))]
public static class SymbioticVirusPatch
{
    static void Postfix(SymbioticVirus __instance, CombatSide side, CombatState combatState)
    {
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars["Dark"].IntValue }
        );
        }
    }
}
