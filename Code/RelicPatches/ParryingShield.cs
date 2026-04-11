using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ParryingShield), nameof(ParryingShield.AfterTurnEnd))]
public static class ParryingShieldPatch
{
    static void Postfix(
        ParryingShield __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side
    )
    {
        if (
            side == CombatSide.Player
            && !((decimal)__instance.Owner.Creature.Block < __instance.DynamicVars.Block.BaseValue)
        )
        {
            int numCreatures = __instance.Owner.RunState.Rng.CombatTargets.Counter;
            if (numCreatures != 0)
            {
                RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Damage.IntValue }
        );
            }
        }
    }
}
