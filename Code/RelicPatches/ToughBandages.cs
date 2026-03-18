using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ToughBandages), nameof(ToughBandages.AfterCardDiscarded))]
public static class ToughBandagesPatch
{
    

    static void Postfix(Tingsha __instance, PlayerChoiceContext choiceContext, CardModel card)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        
        if (card.Owner == __instance.Owner && __instance.Owner.Creature.Side == __instance.Owner.Creature.CombatState.CurrentSide)
        {
            
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Triggered [blue]{0}[/blue] times.\nAdded a total of [blue]{1}[/blue] [gold]Block[/gold].",
                new List<int> { 1, __instance.DynamicVars.Block.IntValue }
            );
        }
    }
}
