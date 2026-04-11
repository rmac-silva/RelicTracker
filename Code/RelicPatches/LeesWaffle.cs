using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(LeesWaffle), nameof(LeesWaffle.AfterObtained))]
public static class LeesWafflePatch
{
    static void Prefix(LeesWaffle __instance)
    {
        
            Creature creature = __instance.Owner.Creature;
            int amountHealed = creature.MaxHp - creature.CurrentHp;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { amountHealed }
        );
        
    }
}
