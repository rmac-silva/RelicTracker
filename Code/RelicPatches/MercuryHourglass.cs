using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MercuryHourglass), nameof(MercuryHourglass.AfterPlayerTurnStart))]
public static class MercuryHourglassPatch
{
    static void Postfix(
        MercuryHourglass __instance,
        PlayerChoiceContext choiceContext, Player player
    )
    
    {
        if (player == __instance.Owner)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Dealt [blue]{0}[/blue] damage.",
                new List<int>
                {
                    __instance.DynamicVars.Damage.IntValue
                        * player.Creature.CombatState.HittableEnemies.Count,
                }
            );
        }
    }
}
