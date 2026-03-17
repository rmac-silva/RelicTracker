using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RadiantPearl), nameof(RadiantPearl.BeforeHandDraw))]
public static class RadiantPearlEnergyPatch
{

    static void Postfix(RadiantPearl __instance, Player player, PlayerChoiceContext choiceContext, CombatState combatState)
    {
        if (player == __instance.Owner && combatState.RoundNumber == 1)
        {
            
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Generated [blue]{0}[/blue] [gold]Luminesce[/gold].",
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
        }

    }
}
