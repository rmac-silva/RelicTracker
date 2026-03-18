using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(CrackedCore), nameof(CrackedCore.BeforeSideTurnStart))]
public static class CrackedCorePatch
{
    static void Postfix(CrackedCore __instance, PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
		{
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Channeled [blue]{0}[/blue] [gold]Lightning[/gold].",
                new List<int> { __instance.DynamicVars["Lightning"].IntValue }
            );
		}
    }
}
