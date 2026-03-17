using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Anchor), nameof(Anchor.BeforeCombatStart))]
public static class AnchorPatch
{
    static void Postfix(Anchor __instance)
    {
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Gained [blue]{0}[/blue] [gold]Armor[/gold].",
            new List<int> { 10 }
        );
    }
}
