using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Brimstone), nameof(Brimstone.AfterSideTurnStart))]
public static class BrimstonePatch
{
    static void Postfix(Brimstone __instance, CombatSide side, CombatState combatState)
    {
        if (side == __instance.Owner.Creature.Side && LocalContext.IsMe(__instance.Owner))
        {
            IEnumerable<Creature> targets =
                from c in combatState.GetOpponentsOf(__instance.Owner.Creature)
                where c.IsAlive
                select c;

            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                new List<int> { 2, targets.Count() }
            );
        }
    }
}
