using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(LastingCandy), nameof(LastingCandy.AfterCombatEnd))]
public static class LastingCandyPatch
{
    private static int _lastCombatID = -1;
    private static readonly System.Reflection.FieldInfo? _combatsSeenField = 
        AccessTools.Field(typeof(LastingCandy), "_combatsSeen");
        
    

    static void Prefix(LastingCandy __instance, CombatRoom room)
    {
        var _combatsSeen  = (int)_combatsSeenField.GetValue(__instance);
        _combatsSeen++;
		if (_combatsSeen % 2 == 0)
		{
			RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
		}
		
    }
}
