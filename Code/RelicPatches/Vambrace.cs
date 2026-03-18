using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Vambrace), nameof(Vambrace.AfterModifyingBlockAmount))]
public static class VambracePatch
{
    static void Postfix(
        Vambrace __instance,
        decimal modifiedAmount, CardModel? cardSource, CardPlay? cardPlay
    )
    {
        if (modifiedAmount <= 0m)
		{
			return;
		}
		if (cardSource == null)
		{
			return;
		}

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Gained an additional [blue]{0}[/blue] [gold]Block[/gold].",
            new List<int> { (int)(modifiedAmount/2) }
        );
    }
}
