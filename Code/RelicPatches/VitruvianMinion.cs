using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(VitruvianMinion), nameof(VitruvianMinion.ModifyDamageMultiplicative))]
public static class VitruvianMinionDamagePatch
{
    private static Dictionary<CardModel, Action> activeHandlers =
        new Dictionary<CardModel, Action>();
    private static int currentRound = -1;
    private static VitruvianMinion? _localInstance;

    static void Postfix(
        VitruvianMinion __instance,
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        )
            return;

        if (cardSource == null)
        {
            return;
        }
        if (cardSource.Owner != __instance.Owner)
        {
            return;
        }
        if (!cardSource.Tags.Contains(CardTag.Minion))
        {
            return;
        }

        if (__instance.Owner?.Creature?.CombatState == null)
        {
            return;
        }

        if (_localInstance == null)
        {
            _localInstance = __instance;
        }

        if (currentRound != __instance.Owner.Creature.CombatState.RoundNumber)
        {
            foreach (var kvp in activeHandlers)
            {
                kvp.Key.Played -= kvp.Value;
            }
            activeHandlers.Clear();
            currentRound = __instance.Owner.Creature.CombatState.RoundNumber;
        }

        //If already handled, ignore it
        if (activeHandlers.ContainsKey(cardSource))
            return;

        Action handler = null;
        handler = () =>
        {
            cardSource.Played -= handler; // Unsubscribe to itself
            activeHandlers.Remove(cardSource); // Self-remove from active handlers
            RegisterCardDamage(amount); //Call the function to register the damage increase
        };

        activeHandlers.Add(cardSource, handler);
        cardSource.Played += handler;
    }

    private static void RegisterCardDamage(decimal amount)
    {
        if (_localInstance == null)
        {
            RelicStatCache.RecordCustomStat(
            "VITRUVIAN_MINION",
            new List<int> { 1, 0, (int)amount }
        );
            return;
        }
        else
        {
            RelicStatCache.RecordCustomStat(
            _localInstance.Id.Entry,
            new List<int> { 1, 0, (int)amount }
        );
        }
    }
}

[HarmonyPatch(typeof(VitruvianMinion), nameof(VitruvianMinion.ModifyBlockMultiplicative))]
public static class VitruvianMinionBlockPatch
{
    private static Dictionary<CardModel, Action> activeHandlers =
        new Dictionary<CardModel, Action>();
    private static int currentRound = -1;
    private static VitruvianMinion? _localInstance;

    static void Postfix(
        VitruvianMinion __instance,
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay
    )
    {
        if (
            CombatManager.Instance == null
            || !CombatManager.Instance.IsInProgress
            || __instance.Owner?.Creature?.CombatState == null
        )
            return;

        if (cardSource == null)
        {
            return;
        }
        if (cardSource.Owner != __instance.Owner)
        {
            return;
        }
        if (!cardSource.Tags.Contains(CardTag.Minion))
        {
            return;
        }

        if (__instance.Owner?.Creature?.CombatState == null)
        {
            return;
        }

        if (_localInstance == null)
        {
            _localInstance = __instance;
        }

        if (currentRound != __instance.Owner.Creature.CombatState.RoundNumber)
        {
            foreach (var kvp in activeHandlers)
            {
                kvp.Key.Played -= kvp.Value;
            }
            activeHandlers.Clear();
            currentRound = __instance.Owner.Creature.CombatState.RoundNumber;
        }

        //If already handled, ignore it
        if (activeHandlers.ContainsKey(cardSource))
            return;

        Action handler = null;
        handler = () =>
        {
            cardSource.Played -= handler; // Unsubscribe to itself
            activeHandlers.Remove(cardSource); // Self-remove from active handlers
            RegisterCardBlock(block); //Call the function to register the damage increase
        };

        activeHandlers.Add(cardSource, handler);
        cardSource.Played += handler;
    }

    private static void RegisterCardBlock(decimal amount)
    {
        if (_localInstance == null)
        {
            RelicStatCache.RecordCustomStat(
            "VITRUVIAN_MINION",
            new List<int> { 1, (int)amount, 0 }
        );
            return;
        }
        else
        {
            RelicStatCache.RecordCustomStat(
            _localInstance.Id.Entry,
            new List<int> { 1, (int)amount, 0 }
        );
        }
    }
}
