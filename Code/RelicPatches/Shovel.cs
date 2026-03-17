using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.RestSite;

[HarmonyPatch(typeof(DigRestSiteOption), nameof(DigRestSiteOption.OnSelect))]
public static class DigRestSiteOptionPatch
{
    static void Postfix(DigRestSiteOption __instance)
    {
        RelicStatCache.RecordCustomStat(
            "SHOVEL",
            "Dug up [blue]{0}[/blue] relics.",
            new List<int> { 1 }
        );
    }
}
