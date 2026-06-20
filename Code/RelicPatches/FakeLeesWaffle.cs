using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeLeesWaffle), nameof(FakeLeesWaffle.AfterObtained))]
public static class FakeLeesWafflePatch
{
    static void Prefix(FakeLeesWaffle __instance)
    {
        
            Creature creature = __instance.Owner.Creature;
            int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
            int amountHealed = creature.MaxHp * (__instance.DynamicVars.Heal.IntValue/100);
            
            if(amountHealed > healthMissing)
            {
                amountHealed = healthMissing;
            }

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { amountHealed }
        );
        
    }
}
