using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BlessedAntler), nameof(BlessedAntler.ModifyMaxEnergy))]
public static class BlessedAntlerEnergyPatch
{
    private static int roundCounter = 0;
    static void Postfix(BlessedAntler __instance, Player player, decimal amount)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (player != __instance.Owner)
        {
            return;
        }

        if(player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.
            RoundNumber;
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Generated [blue]{0}[/blue] [gold]energy[/gold].\nShuffled [blue]{1}[/blue] [gold]dazed[/gold] cards.",
                new List<int> { 1, 0 }
            );
        }

    }
}

[HarmonyPatch(typeof(BlessedAntler), nameof(BlessedAntler.BeforeHandDraw))]
public static class BlessedAntlerDazedPatch
{
    static void Postfix(
        BlessedAntler __instance,
        Player player,
        PlayerChoiceContext choiceContext,
        CombatState combatState
    )
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress) return;
        if (player != __instance.Owner)
        {
            return;
        }

        if (combatState.RoundNumber == 1)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Generated [blue]{0}[/blue] [gold]energy[/gold].\nShuffled [blue]{1}[/blue] [gold]dazed[/gold] cards.",
                new List<int> { 0, 3 }
            );
        }
    }
}
