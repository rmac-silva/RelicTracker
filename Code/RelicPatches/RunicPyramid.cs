using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RunicPyramid), nameof(RunicPyramid.ShouldFlush))]
public static class RunicPyramidPatch
{
    static void Postfix(RunicPyramid __instance, Player player)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { CardPile.GetCards(__instance.Owner, PileType.Hand).Count() }
        );
    }
}
