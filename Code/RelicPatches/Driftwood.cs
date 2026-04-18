using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;

[HarmonyPatch(typeof(CardReward), nameof(CardReward.Reroll))]
public static class DriftwoodRerollPatch
{
    static void Prefix(
        CardReward __instance
    )
    {
        
            RelicStatCache.RecordCustomStat(
            "DRIFTWOOD",
            new List<int> { 1 }
        );
        
    }
}
