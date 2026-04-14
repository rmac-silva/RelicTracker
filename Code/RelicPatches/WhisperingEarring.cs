using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(WhisperingEarring), nameof(WhisperingEarring.ModifyMaxEnergy))]
public static class WhisperingEarringEnergyPatch
{
    private static int roundCounter = -1;

    static void Postfix(WhisperingEarring __instance, Player player, decimal amount)
    {
        if (player == __instance.Owner && player.Creature.CombatState.RoundNumber != roundCounter)
        {
            roundCounter = player.Creature.CombatState.RoundNumber;
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 0, 1, 0 }
        );
        }
    }
}

[HarmonyPatch(typeof(WhisperingEarring), nameof(WhisperingEarring.BeforePlayPhaseStartLate))]
public static class WhisperingEarringCardsPlayedPatch
{
    private static Dictionary<CardModel, Action> activeHandlers = new Dictionary<CardModel, Action>();
    private static bool hasExecutedThisCombat = false;

    private static int _lastCombatId = -1;
    static void Prefix(WhisperingEarring __instance, Player player)
    {
        if (player != __instance.Owner) return;

        if(CombatStartManager.IsNewCombat(ref _lastCombatId))
        {
            hasExecutedThisCombat = false; // Reset for new combat
        }

        if (hasExecutedThisCombat) return; // Only execute once per combat
       

        //Reset how many cards have been played

        Action handler = null;

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1, 0, 0 }
        );

        // Record how many cards are in your hand
        foreach (CardModel c in player.PlayerCombatState.Hand.Cards)
        {

            if(activeHandlers.ContainsKey(c))
            {
                continue; // Don't add multiple handlers for the same card
            }

            handler = () =>
            {
                c.Played -= handler; // Unsubscribe to itself
                activeHandlers.Remove(c); // Self-remove from active handlers
                RegisterCardPlayed(); //Call the function to register the damage increase
            };

            c.Played += handler;
            
            activeHandlers.Add(c, handler);
        }
    }

    private static void RegisterCardPlayed()
    {
        RelicStatCache.RecordCustomStat(
            "WHISPERING_EARRING",
            new List<int> { 0, 0, 1 }
        );
    }

    // By taking 'Task __result' as a parameter and returning 'Task', 
    // you can wait for the original method to finish.
    static async Task Postfix(Task __result)
    {
        await __result; // Wait for the game's actual logic to finish

        // --- CLEANUP ---
        // Once the Earring is done playing cards, remove all remaining handlers
        foreach (var kvp in activeHandlers)
        {
            kvp.Key.Played -= kvp.Value;
        }

        activeHandlers.Clear();
        hasExecutedThisCombat = true;
    }
}
