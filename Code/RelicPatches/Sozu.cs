using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Sozu), nameof(Sozu.ModifyMaxEnergy))]
public static class SozuPatch
{
    private static int roundCounter = 0;
    static void Postfix(Sozu __instance, Player player, decimal amount)
    {
        
        if (player != __instance.Owner)
        {
            return;
        }

        if(player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Energy.IntValue, 0 }
        );
        }

    }
}

[HarmonyPatch(typeof(Sozu), nameof(Sozu.ShouldProcurePotion))]
public static class SozuPotionPatch
{
    static void Postfix(Sozu __instance, PotionModel potion, Player player)
    {
        if (player != __instance.Owner)
        {
            return;
        }

            __instance.Flash();
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 0, 1 }
        );
        

    }
}
