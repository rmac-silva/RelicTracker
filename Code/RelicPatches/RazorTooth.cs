using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RazorTooth), nameof(RazorTooth.AfterCardPlayed))]
public static class RazorToothPatch
{

    static void Prefix(RazorTooth __instance,PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != __instance.Owner)
		{
			return;
		}
		CardType type = cardPlay.Card.Type;
		if ((uint)(type - 1) > 1u)
		{
			return;
		}
		if (!cardPlay.Card.IsUpgradable)
		{
			return;
		}
            
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Upgraded [blue]{0}[/blue] cards.",
            new List<int> { 1 }
        );
        

    }
}
