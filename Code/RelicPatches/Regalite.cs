using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Regalite), nameof(Regalite.AfterCardEnteredCombat))]
public static class RegalitePatch
{
    static void Postfix(
        Regalite __instance,
        CardModel card
    )
    {
        if (card.Owner == __instance.Owner && card.VisualCardPool.IsColorless)
		{
			
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] block.",
                new List<int> { __instance.DynamicVars.Block.IntValue }
            );
		}
        
    }
}
