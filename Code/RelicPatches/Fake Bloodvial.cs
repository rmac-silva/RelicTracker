using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeBloodVial), nameof(FakeBloodVial.AfterPlayerTurnStartLate))]
public static class FakeBloodVialPatch
{
    static void Prefix(BloodVial __instance, PlayerChoiceContext choiceContext, Player player)
    {
        
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber <= 1)
        {
            Creature creature = __instance.Owner.Creature;
            int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
            //If the health I'm missing is below the heal amount, then I'm only healing for healthMising

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { healthMissing < __instance.DynamicVars.Heal.IntValue ? healthMissing : __instance.DynamicVars.Heal.IntValue }
        );
        }
    }
}
