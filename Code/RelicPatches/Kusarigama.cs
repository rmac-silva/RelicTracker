using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Kusarigama), "DoActivateVisuals")]
public static class KusarigamaPatch
{
    static void Postfix(Kusarigama __instance)
    {


        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Dealt [blue]{0}[/blue] damage.",
            new List<int> { 6 }
        );

    }
}
