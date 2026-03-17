using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(SealOfGold), nameof(SealOfGold.AfterSideTurnStart))]
public static class SealOfGoldPatch
{
    static void Prefix(
        SealOfGold __instance,
        CombatSide side, CombatState combatState
    )
    {
        if (side == __instance.Owner.Creature.Side && __instance.Owner.Gold >= __instance.DynamicVars.Gold.IntValue)
		{

            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Lost [blue]{0}[/blue] [gold]Gold[/gold].\nGained [blue]{1}[/blue] energy.",
                new List<int>
                {
                    __instance.DynamicVars.Gold.IntValue, __instance.DynamicVars.Energy.IntValue
                }
            );
        }
        
        
    }
}
