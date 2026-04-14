using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(LastingCandy), nameof(LastingCandy.AfterCombatEnd))]
public static class LastingCandyPatch
{
    private static int _lastCombatID = -1;

    private static bool WillTrigger(LastingCandy __instance)
    {
        if (__instance.CombatsSeen > 0)
        {
            return __instance.CombatsSeen % 2 == 0;
        }
        return false;
    }

    static void Postfix(LastingCandy __instance, CombatRoom room)
    {
        if (WillTrigger(__instance) && CombatStartManager.IsNewCombat(ref _lastCombatID))
        {
            RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 1 });
        }
    }
}
