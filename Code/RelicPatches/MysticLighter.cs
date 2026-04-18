using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(MysticLighter), nameof(MysticLighter.ModifyDamageAdditive))]
public static class MysticLighterPatch
{
    private static Dictionary<CardModel, Action> activeHandlers =
        new Dictionary<CardModel, Action>();
    private static MysticLighter? _localInstance;
    private static int currentRound = -1;

    static void Postfix(
        MysticLighter __instance,
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
        if (cardSource?.Enchantment == null)
        {
            return;
        }
        if (cardSource.Owner != __instance.Owner)
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

        if (__instance.Owner.Creature.CombatState.RoundNumber != currentRound)
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

        ModLog.Info($"Registering Mystic Lighter handler for card: {cardSource.Id.Entry}");

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
        ModLog.Info("Registering Mystic Lighter damage for card play");
        if (_localInstance == null)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            _localInstance.Id.Entry,
            new List<int> { _localInstance.DynamicVars.Damage.IntValue }
        );
    }
}
