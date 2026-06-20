using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BowlerHat), "ModifyGoldGained")]
public static class BowlerHatPatch
{
    static void Prefix(BowlerHat __instance, Player player, decimal amount)
    {

        if (player != __instance.Owner)
        {
            return;
        }
        
        var bonusGold =  amount * ( __instance.DynamicVars["GoldIncrease"].BaseValue - 1); //We were getting 100 gold, we have a bonus of 25% increase
        //Variable is 1.25, so we do 100 * (1.25 - 1) = 100 * 0.25 = 25 bonus gold
        
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { (int)Math.Floor(bonusGold) }
        );
        
    }
}
