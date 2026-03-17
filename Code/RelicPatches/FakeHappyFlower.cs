using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeHappyFlower), nameof(FakeHappyFlower.AfterSideTurnStart))]
public static class FakeHappyFlowerPatch
{
    static void Postfix(FakeHappyFlower __instance, CombatSide side, CombatState combatState)
    {
        if (side == __instance.Owner.Creature.Side)
        {
            var field = AccessTools.Field(typeof(FakeHappyFlower), "TurnsSeen");
            int turnsSeen = (int)field.GetValue(__instance);

            if (turnsSeen == 0)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Generated [blue]{0}[/blue] [gold]energy[/gold].",
                    new List<int> { 1 }
                );
            }
        }
    }
}
