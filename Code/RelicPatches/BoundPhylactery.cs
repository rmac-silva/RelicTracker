using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BoundPhylactery), "SummonPet")]
public static class BoundPhylacteryPatch
{
    static void Postfix(
        BoundPhylactery __instance
    )
    {
        
            

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Summoned [orange]Osty[/orange] [blue]{0}[/blue] times.",
            new List<int> { __instance.DynamicVars.Summon.IntValue }
        );
        
    }
}
