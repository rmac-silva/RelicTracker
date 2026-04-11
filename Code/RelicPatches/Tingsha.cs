using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Tingsha), nameof(Tingsha.AfterCardDiscarded))]
public static class TingshaPatch
{
    

    static void Postfix(Tingsha __instance, PlayerChoiceContext choiceContext, CardModel card)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        
        if (card.Owner == __instance.Owner && __instance.Owner.Creature.Side == __instance.Owner.Creature.CombatState.CurrentSide)
        {
            
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1, __instance.DynamicVars.Damage.IntValue }
        );
        }
    }
}
