using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeHappyFlower), nameof(FakeHappyFlower.AfterSideTurnStart))]
public static class FakeHappyFlowerPatch
{
    private static readonly System.Reflection.FieldInfo TurnsSeenField = AccessTools.Field(
        typeof(FakeHappyFlower),
        "_turnsSeen"
    );
    static void Postfix(FakeHappyFlower __instance, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side)
        {
           
            int turnsSeen = (int)TurnsSeenField.GetValue(__instance);

            if (turnsSeen == 0)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Generated [blue]{0}[/blue] [gold]energy[/gold].",
                    new List<int> { __instance.DynamicVars.Energy.IntValue }
                );
            }
        }
    }
}
