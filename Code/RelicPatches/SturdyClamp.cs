using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(SturdyClamp), nameof(SturdyClamp.AfterPreventingBlockClear))]
public static class SturdyClampPatch
{
    static void Postfix(SturdyClamp __instance, AbstractModel preventer, Creature creature)
    {
        if (__instance != preventer || creature != __instance.Owner.Creature)
        {
            return;
        }
        int block = creature.Block;
        if (block != 0)
        {
            if (block > 10)
            {
                RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 10 }
        );
            }
            else
            {
                RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { block }
        );
            }
        }
    }
}
