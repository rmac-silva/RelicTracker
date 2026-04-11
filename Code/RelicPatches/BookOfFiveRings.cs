using HarmonyLib;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BookOfFiveRings), "DoActivateVisuals")]
public static class BookOfFiveRingsPatch
{
    static void Postfix(BookOfFiveRings __instance)
    {
        
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1, __instance.DynamicVars.Heal.IntValue }
        );
        
    }
}
