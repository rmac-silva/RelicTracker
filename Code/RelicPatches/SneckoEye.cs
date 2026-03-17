using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(SneckoEye), nameof(SneckoEye.ModifyHandDraw))]
public static class SneckoEyePatch
{
    static void Postfix(SneckoEye __instance, Player player, decimal count)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Drew [blue]{0}[/blue] additional cards.\nReduced card costs by [blue]{1}[/blue].\nIncreased card costs by [blue]{2}[/blue].",
            new List<int> { __instance.DynamicVars.Cards.IntValue, 0, 0 }
        );
    }
}

[HarmonyPatch(typeof(ConfusedPower), nameof(ConfusedPower.AfterCardDrawn))]
public static class ConfusedPowerPatch
{
    static void Postfix(
        ConfusedPower __instance,
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (card.Owner != __instance.Owner.Player)
        {
            return;
        }
        if (card.EnergyCost.Canonical < 0)
        {
            return;
        }

        //Modified Cost : 0 - Original cost (1) == negative if cost was reduced, positive if cost was increased
        int energyDifference = card.EnergyCost.GetResolved() - card.EnergyCost.Canonical;

        if (energyDifference < 0)
        {
            RelicStatCache.RecordCustomStat(
                "SNECKO_EYE",
                "Drew [blue]{0}[/blue] additional cards.\nReduced card costs by [blue]{1}[/blue].\nIncreased card costs by [blue]{2}[/blue].",
                new List<int> { 0, Math.Abs(energyDifference), 0 }
            );
        }
        else
        {
            RelicStatCache.RecordCustomStat(
                "SNECKO_EYE",
                "Drew [blue]{0}[/blue] additional cards.\nReduced card costs by [blue]{1}[/blue].\nIncreased card costs by [blue]{2}[/blue].",
                new List<int> { 0, 0, Math.Abs(energyDifference) }
            );
        }
    }
}
