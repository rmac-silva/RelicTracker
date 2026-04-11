using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(TuningFork), "DoActivateVisuals")]
public static class TuningForkPatch
{
    static void Postfix(TuningFork __instance)
    {
    
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Block.IntValue }
        );
        
    }
}