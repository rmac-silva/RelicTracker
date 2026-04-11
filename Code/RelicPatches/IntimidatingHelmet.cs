using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(IntimidatingHelmet), nameof(IntimidatingHelmet.BeforeCardPlayed))]
public static class IntimidatingHelmetPatch
{
    static void Postfix(IntimidatingHelmet __instance, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Owner == __instance.Owner
            && cardPlay.Resources.EnergyValue >= __instance.DynamicVars.Energy.IntValue
        )
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Block.IntValue }
        );
        }
    }
}
