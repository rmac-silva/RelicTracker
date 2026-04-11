using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Kusarigama), "DoActivateVisuals")]
public static class KusarigamaPatch
{
    static void Postfix(Kusarigama __instance)
    {


        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 6 }
        );

    }
}
