using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BowlerHat), nameof(BowlerHat.AfterGoldGained))]
public static class BowlerHatPatch
{
    static void Prefix(BowlerHat __instance, Player player)
    {

        if (player != __instance.Owner)
        {
            return;
        }

        //Fetch the variable _pendingBonusGold
        var field = AccessTools.Field(typeof(BowlerHat), "_pendingBonusGold");
        decimal bonusGold = (decimal)field.GetValue(__instance);

        if (bonusGold >= 0)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] additional [gold]Gold[/gold].",
                new List<int> { (int)Math.Floor(bonusGold) }
            );
        }
    }
}
