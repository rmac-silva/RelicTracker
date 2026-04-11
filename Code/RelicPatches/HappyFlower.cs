using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(HappyFlower), nameof(HappyFlower.AfterSideTurnStart))]
public static class HappyFlowerPatch
{
    private static readonly System.Reflection.FieldInfo TurnsSeenField = AccessTools.Field(
        typeof(HappyFlower),
        "_turnsSeen"
    );
    static void Postfix(HappyFlower __instance, CombatSide side, CombatState combatState)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (side == __instance.Owner.Creature.Side)
        {
            
            int turnsSeen = (int)TurnsSeenField.GetValue(__instance);

            if (turnsSeen == 0)
            {
                RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
            }
        }
    }
}
