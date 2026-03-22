using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;



[HarmonyPatch(typeof(ConfusedPower), nameof(ConfusedPower.AfterCardDrawn))]
public static class ConfusedPowerPatchForFakeSnecko
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
                "FAKE_SNECKO_EYE",
                "Reduced card costs by [blue]{0}[/blue].\nIncreased card costs by [blue]{1}[/blue].",
                new List<int> { Math.Abs(energyDifference), 0 }
            );
        }
        else
        {
            RelicStatCache.RecordCustomStat(
                "FAKE_SNECKO_EYE",
                "Reduced card costs by [blue]{0}[/blue].\nIncreased card costs by [blue]{1}[/blue].",
                new List<int> { 0, Math.Abs(energyDifference) }
            );
        }
    }
}
