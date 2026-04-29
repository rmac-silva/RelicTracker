using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

public static class BrilliantScarfHelper
{
    private static bool _hasTriggeredThisRound = false;
}

[HarmonyPatch(typeof(BrilliantScarf), nameof(BrilliantScarf.TryModifyEnergyCostInCombatLate))]
public static class BrilliantScarfPatch
{
    private static readonly System.Reflection.FieldInfo? _numCardsPlayedField = AccessTools.Field(typeof(BrilliantScarf), "_cardsPlayedThisTurn");
    private static bool _triggeredThisRound = false;
    static void Prefix(
        BrilliantScarf __instance,
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost
    )
    {
        modifiedCost = originalCost;

        var numCardsPlayed = _numCardsPlayedField.GetValue(__instance) as int? ?? 0;
        bool shouldModify = numCardsPlayed == 5;

        ModLog.Info($"Brilliant Scarf: Should modify cost for card {card.Id.Entry}: {shouldModify}");

        if(numCardsPlayed == 0 || numCardsPlayed == 1)
        {
            _triggeredThisRound = false;
        }
        
        if (shouldModify && !_triggeredThisRound)
        {
            _triggeredThisRound = true;
            RelicStatCache.RecordTriggerStat(__instance.Id.Entry);
        }

    }
}
