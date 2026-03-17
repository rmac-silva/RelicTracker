using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RuinedHelmet), nameof(RuinedHelmet.TryModifyPowerAmountReceived))]
public static class RuinedHelmetPatch
{
    private static readonly System.Reflection.FieldInfo UsedThisCombatField = AccessTools.Field(
        typeof(RuinedHelmet),
        "_usedThisCombat"
    );
    static void Postfix(
        RuinedHelmet __instance,
        PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount
    )
    {
        
        modifiedAmount = amount;

		if (!(canonicalPower is StrengthPower))
		{
			return;
		}
		if (target != __instance.Owner.Creature)
		{
            return;
		}
		if (amount <= 0m)
		{
			return;
		}

        bool UsedThisCombat = (bool)UsedThisCombatField.GetValue(__instance);
		if (UsedThisCombat)
		{
			return;
		}

        modifiedAmount = amount * 2m;
		

            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Added [blue]{0}[/blue] [gold]Strength[/gold].",
                new List<int> { (int)amount }
            );
        
			
		
        
    }
}
