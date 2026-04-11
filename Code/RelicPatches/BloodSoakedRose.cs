using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BloodSoakedRose), nameof(BloodSoakedRose.ModifyMaxEnergy))]
public static class BloodSoakedRoseEnergyPatch
{
    private static int roundCounter = 0;
    private static int _lastCombatId = -1;

    static void Postfix(BloodSoakedRose __instance, Player player, decimal amount)
    {

        if (player != __instance.Owner)
        {
            return;
        }

        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
            _lastCombatId = CombatStartManager._currentCombatId;

        }

        if (player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Energy.IntValue, 0 }
        );
        }
    }
}

[HarmonyPatch(typeof(Enthralled), nameof(Enthralled.ShouldPlay))]
public static class EnthralledPatch
{
    // Static variable to remember the last round we processed
    private static int _lastProcessedRound = -1;

    static void Postfix(Enthralled __instance, CardModel card, AutoPlayType autoPlayType)
    {
        int currentRound = __instance.Owner.Creature.CombatState.RoundNumber;

        if (card.Owner != __instance.Owner)
        {
            return;
        }
        CardPile? pile = __instance.Pile;
        if (pile == null || pile.Type != PileType.Hand)
        {
            return;
        }
        if (card is Enthralled)
        {
            return;
        }
        if (autoPlayType != AutoPlayType.None)
        {
            return;
        }

        if (currentRound != _lastProcessedRound)
        {
            _lastProcessedRound = currentRound;

            RelicStatCache.RecordCustomStat(
            "BLOOD_SOAKED_ROSE",
            new List<int> { 0, 1 }
        );
        }
    }
}
