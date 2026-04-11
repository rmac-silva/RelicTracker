using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RegalPillow), nameof(RegalPillow.AfterRestSiteHeal))]
public static class RegalPillowPatch
{
    static void Postfix(
        RegalPillow __instance,
        Player player, bool isMimicked
    )
    {
        if (player != __instance.Owner)
		{
			return;
		}
			
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Heal.IntValue }
        );
		
        
    }
}
