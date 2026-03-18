using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(TwistedFunnel), nameof(TwistedFunnel.BeforeSideTurnStart))]
public static class TwistedFunnelPatch
{
    static void Prefix(
        TwistedFunnel __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side,
        CombatState combatState
    )
    {
        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        )
            return;

        if (side != __instance.Owner.Creature.Side || combatState.RoundNumber > 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Applied [blue]{0}[/blue] [gold]Poison[/gold].",
            new List<int>
            {
                __instance.DynamicVars["PoisonPower"].IntValue
                    * __instance.Owner.Creature.CombatState.HittableEnemies.Count,
            }
        );
    }
}
