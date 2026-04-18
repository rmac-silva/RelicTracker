using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

[HarmonyPatch(typeof(PaelsLegion), nameof(PaelsLegion.ModifyBlockMultiplicative))]
public static class PaelsLegionPatch
{
    private static FieldInfo CooldownField = AccessTools.Field(typeof(PaelsLegion), "_cooldown");
    private static int currentRound = -1;
    private static int combatID = -1;
    private static Dictionary<CardModel, Action> activeHandlers =
        new Dictionary<CardModel, Action>();

    static void Prefix(
        PaelsLegion __instance,
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay
    )
    {
        if (!props.IsCardOrMonsterMoveRelicTracker())
        {
            return;
        }
        if (cardSource == null)
        {
            return;
        }
        if (target != __instance.Owner.Creature)
        {
            return;
        }
        int cooldown = (int)CooldownField.GetValue(__instance);
        if (cooldown != null && cooldown > 0)
        {
            return;
        }

        if (
            currentRound != __instance.Owner.Creature.CombatState.RoundNumber
            || CombatStartManager.IsNewCombat(ref combatID)
        )
        {
            foreach (var kvp in activeHandlers)
            {
                kvp.Key.Played -= kvp.Value;
            }
            activeHandlers.Clear();
            currentRound = __instance.Owner.Creature.CombatState.RoundNumber;
        }

        if (activeHandlers.ContainsKey(cardSource))
            return;

        Action handler = null;
        handler = () =>
        {
            cardSource.Played -= handler; // Unsubscribe to itself
            activeHandlers.Remove(cardSource); // Self-remove from active handlers
            RegisterBlockGain(block); //Call the function to register the damage increase
        };

        activeHandlers.Add(cardSource, handler);
        cardSource.Played += handler;
    }

    private static void RegisterBlockGain(decimal block)
    {
        RelicStatCache.RecordCustomStat("PAELS_LEGION", new List<int> { (int)(block) });
    }
}
