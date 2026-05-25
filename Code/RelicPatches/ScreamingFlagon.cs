using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ScreamingFlagon), "BeforeSideTurnEnd")]
public static class ScreamingFlagonPatch
{
    static void Prefix(
        ScreamingFlagon __instance,
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants
    )
    {
        if (side == CombatSide.Player && PileType.Hand.GetPile(__instance.Owner).IsEmpty && participants.Contains(__instance.Owner.Creature))
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
