using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PaelsWing), nameof(PaelsWing.OnSacrifice))]
public static class PaelsWingPatch
{
    static void Postfix(PaelsWing __instance)
    {
        var field = AccessTools.Field(typeof(PaelsWing), "_rewardsSacrificed");
        int RewardsSacrificed = (int)field.GetValue(__instance);

        if (RewardsSacrificed % __instance.DynamicVars["Sacrifices"].IntValue == 0)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] relics.",
                new List<int> { 1 }
            );
        } else
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] relics.",
                new List<int> { 0 }
            );
        }
    }
}
