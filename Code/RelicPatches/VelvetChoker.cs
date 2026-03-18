using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;


[HarmonyPatch(typeof(VelvetChoker), nameof(VelvetChoker.ModifyMaxEnergy))]
public static class VelvetChokerEnergyPatch
{
    private static int _lastCombatId = -1;
    private static int roundCounter = -1;

    static void Postfix(VelvetChoker __instance, Player player, decimal amount)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        ) {return;}
        
        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
            _lastCombatId = CombatStartManager._currentCombatId;

        }
        
        if(player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained an additional [blue]{0}[/blue] [gold]energy[/gold].\n[red]Blocked[/red] [blue]{1}[/blue] times.",
                new List<int> { __instance.DynamicVars.Energy.IntValue, 0 }
            );
        }

    }
}

[HarmonyPatch(typeof(VelvetChoker), nameof(VelvetChoker.ShouldPlay))]
public static class VelvetChokerRegretPatch
{
    private static int roundCounter = -1;
    private static int _lastCombatId = -1;

    private static readonly FieldInfo CardsPlayedThisTurnField = AccessTools.Field(typeof(VelvetChoker), "_cardsPlayedThisTurn");
    static void Postfix(VelvetChoker __instance, CardModel card, AutoPlayType _)
    {
        if (card.Owner != __instance.Owner)
		{
			return;
		}

        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        ){return;}
        
        if (CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            roundCounter = -1; // Reset for the new fight
            _lastCombatId = CombatStartManager._currentCombatId;
        }

        int CardsPlayedThisTurn = (int)CardsPlayedThisTurnField.GetValue(__instance);
        
        if(CardsPlayedThisTurn >= __instance.DynamicVars.Cards.IntValue && roundCounter != __instance.Owner.Creature.CombatState.RoundNumber)
        {
            roundCounter = __instance.Owner.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained an additional [blue]{0}[/blue] [gold]energy[/gold].\n[red]Blocked[/red] [blue]{1}[/blue] times.",
                new List<int> { 0, 1 }
            );
        }
        

    }
}
