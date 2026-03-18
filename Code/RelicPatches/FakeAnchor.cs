using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeAnchor), nameof(FakeAnchor.BeforeCombatStart))]
public static class FakeAnchorPatch
{
    static void Postfix(FakeAnchor __instance)
    {
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Gained [blue]{0}[/blue] [gold]Armor[/gold].",
            new List<int> { __instance.DynamicVars.Block.IntValue }
        );
    }
}
