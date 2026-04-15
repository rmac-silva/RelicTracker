using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FurCoat), nameof(FurCoat.BeforeCombatStart))]
public static class FurCoatPatchCombatStart
{
    static void Prefix(FurCoat __instance)
    {
        List<MapCoord> markedCoords = __instance.GetMarkedCoords();
        if (
            markedCoords == null
            || !markedCoords.Contains(__instance.Owner.RunState.CurrentMapPoint.coord)
        )
        {
            return;
        }

        IReadOnlyList<Creature> hittableEnemies = __instance
            .Owner
            .Creature
            .CombatState
            .HittableEnemies;

        int totalHealthReduced = 0;
        foreach (Creature enemy in hittableEnemies)
        {
            totalHealthReduced += enemy.MaxHp - 1;
        }

        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { totalHealthReduced });
    }
}

[HarmonyPatch(typeof(FurCoat), nameof(FurCoat.AfterCreatureAddedToCombat))]
public static class FurCoatPatchMidCombat
{
    static void Prefix(FurCoat __instance, Creature creature)
    {
        if (creature.Side == CombatSide.Enemy)
        {
            List<MapCoord> markedCoords = __instance.GetMarkedCoords();
            if (
                markedCoords != null
                && markedCoords.Contains(__instance.Owner.RunState.CurrentMapPoint.coord)
            )
            {
                var healthReduced = creature.MaxHp - 1;
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    new List<int> { healthReduced }
                );
            }
        }
    }
}
