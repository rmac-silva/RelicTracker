using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(MeatOnTheBone), nameof(MeatOnTheBone.AfterCombatVictoryEarly))]
public static class MeatOnTheBonePatch
{
    static void Postfix(MeatOnTheBone __instance, CombatRoom _)
    {

        if (!__instance.Owner.Creature.IsDead)
        {

            Creature creature = __instance.Owner.Creature;
            int num = (int)((decimal)creature.MaxHp * (__instance.DynamicVars["HpThreshold"].BaseValue / 100m));
            bool willHeal = creature.CurrentHp <= num;

            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Healed [blue]{0}[/blue] [gold]HP[/gold].",
                new List<int> { __instance.DynamicVars.Heal.IntValue }
            );
        }
    }
}
