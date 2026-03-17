using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PaelsBlood), nameof(PaelsBlood.ModifyHandDraw))]
public static class PaelsBloodPatch
{
    static void Postfix(PaelsBlood __instance, Player player, decimal count)
    {
        if (player != __instance.Owner)
        {
            return;
        }
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Drew [blue]{0}[/blue] additional cards.",
            new List<int> { __instance.DynamicVars.Cards.IntValue }
        );
    }
}
