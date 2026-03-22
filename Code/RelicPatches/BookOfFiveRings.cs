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
                "Triggered [blue]{0}[/blue] times.\nHealed [blue]{1}[/blue] [gold]HP[/gold].",
                new List<int> { 1, __instance.DynamicVars.Heal.IntValue }
            );
        
    }
}
