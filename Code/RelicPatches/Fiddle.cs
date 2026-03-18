using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Fiddle), nameof(Fiddle.ModifyHandDrawLate))]
public static class FiddlePatch
{
    static void Postfix(Fiddle __instance, Player player, decimal count)
    {
        
        if (player != __instance.Owner)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Drew [blue]{0}[/blue] additional cards.",
            new List<int> { 2 }
        );
    }
}
