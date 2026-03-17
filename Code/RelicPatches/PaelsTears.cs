using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PaelsTears), nameof(PaelsTears.AfterSideTurnStart))]
public static class PaelsTearsPatch
{
    static void Postfix(PaelsTears __instance, CombatSide side, CombatState combatState)
    {
        var field = AccessTools.Field(typeof(PaelsTears), "_hadLeftoverEnergy");
        bool HadLeftoverEnergy = (bool)field.GetValue(__instance);

        if (side == __instance.Owner.Creature.Side && HadLeftoverEnergy)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] additional energy.",
                new List<int> { __instance.DynamicVars.Energy.IntValue }
            );
        }
    }
}
