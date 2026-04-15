using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BrilliantScarf), nameof(BrilliantScarf.TryModifyEnergyCostInCombat))]
public static class BrilliantScarfPatch
{
    static void Prefix(
        BrilliantScarf __instance,
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost
    )
    {
        modifiedCost = originalCost;

        var shouldModify = Traverse
            .Create(__instance)
            .Method("ShouldModifyCost", card)
            .GetValue<bool>();
        if (shouldModify)
        {
            RelicStatCache.RecordTriggerStat(__instance.Id.Entry);
        }
    }
}
