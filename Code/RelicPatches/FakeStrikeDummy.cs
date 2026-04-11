
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(FakeStrikeDummy), nameof(FakeStrikeDummy.ModifyDamageAdditive))]
public static class FakeStrikeDummyPatch
{
    private static Dictionary<CardModel, Action> activeHandlers =
        new Dictionary<CardModel, Action>();
    private static int currentRound = -1;
    private static FakeStrikeDummy? _localInstance;

    static void Postfix(
        FakeStrikeDummy __instance,
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource
    )
    {
        if (!props.IsPoweredAttackRelicTracker())
        {
            return;
        }
        if (cardSource == null)
        {
            return;
        }
        if (!cardSource.Tags.Contains(CardTag.Strike))
        {
            return;
        }
        if (dealer != __instance.Owner.Creature && cardSource.Owner != __instance.Owner)
        {
            return;
        }

        if(__instance.Owner?.Creature?.CombatState == null)
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
            RegisterCardDamage(); //Call the function to register the damage increase
        };

        activeHandlers.Add(cardSource, handler);
        cardSource.Played += handler;
    }

    private static void RegisterCardDamage()
    {
        if(_localInstance == null)
        {
            RelicStatCache.RecordCustomStat(
            "FAKE_STRIKE_DUMMY",
            new List<int> { 1 }
        );
            return;
        } else
        {
            RelicStatCache.RecordCustomStat(
            _localInstance.Id.Entry,
            new List<int> { _localInstance.DynamicVars["ExtraDamage"].IntValue }
        );
        }

    }
}
