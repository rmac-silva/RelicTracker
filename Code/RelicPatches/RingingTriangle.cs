using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RingingTriangle), nameof(RingingTriangle.ShouldFlush))]
public static class RingingTrianglePatch
{
    static void Postfix(
        RingingTriangle __instance,
        Player player
    )
    {
        if (player != __instance.Owner)
		{
			return;
		}
			
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Retained [blue]{0}[/blue] cards.",
                new List<int> { CardPile.GetCards(__instance.Owner, PileType.Hand).Count() }
            );
		
        
    }
}
