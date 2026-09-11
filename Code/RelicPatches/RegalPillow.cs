using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RegalPillow), nameof(RegalPillow.AfterRestSiteHeal))]
public static class RegalPillowPatch
{
    static void Prefix(
        RegalPillow __instance,
        Player player, bool isMimicked
    )
    {
        if (player != __instance.Owner)
		{
			return;
		}
			
            var amountToHeal = player.Creature.MaxHp - player.Creature.CurrentHp;

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { Math.Min(__instance.DynamicVars.Heal.IntValue,amountToHeal) }
        );
		
        
    }
}
