using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ScreamingFlagon), nameof(ScreamingFlagon.BeforeTurnEnd))]
public static class ScreamingFlagonPatch
{
    static void Prefix(
        ScreamingFlagon __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side
    )
    {
        if (side == CombatSide.Player && PileType.Hand.GetPile(__instance.Owner).IsEmpty)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int>
                {
                    (int)(
                        __instance.Owner.Creature.CombatState.HittableEnemies.Count
                        * __instance.DynamicVars.Damage.IntValue
                    ),
                }
        );
        }
    }
}
