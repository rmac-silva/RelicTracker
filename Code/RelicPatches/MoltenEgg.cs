using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

public static class EggTrackingHelper
{
    //This type of DT will allow garbage collector to clean up
    public static ConditionalWeakTable<CardCreationResult, object?> CardRewards =
        new ConditionalWeakTable<CardCreationResult, object?>();

    //This is for tracking the cards that were upgraded by the molten egg.
    public static ConditionalWeakTable<CardModel, object?> MoltenEggCards =
        new ConditionalWeakTable<CardModel, object?>();
    public static ConditionalWeakTable<CardModel, object?> ToxicEggCards =
        new ConditionalWeakTable<CardModel, object?>();
    public static ConditionalWeakTable<CardModel, object?> FrozenEggCards =
        new ConditionalWeakTable<CardModel, object?>();
    public static ConditionalWeakTable<CardModel, object?> FresnelLensCards =
        new ConditionalWeakTable<CardModel, object?>();

    public static void RegisterHook(Action<CardModel> hook) { }
}

[HarmonyPatch(
    typeof(CardCreationResult),
    nameof(CardCreationResult.ModifyCard),
    new Type[] { typeof(CardModel), typeof(RelicModel) }
)]
public static class CardRewardPatch
{
    /// <summary>
    /// Every single time a card reward gets modified by a relic, this postfix runs.
    /// We check if it was one of the eggs who modified the card, and if it was, we add it to another
    /// list of listeners for the respective egg.
    /// </summary>
    /// <param name="__instance"></param>
    /// <param name="card"></param>
    /// <param name="relic"></param>
    static void Postfix(CardCreationResult __instance, CardModel card, RelicModel? modifyingRelic)
    {
        if (EggTrackingHelper.CardRewards.TryGetValue(__instance, out _))
        {
            //This card reward was marked, meaning it came from a relic that upgrades cards.

            if (modifyingRelic.Id.Entry == "MOLTEN_EGG")
            {
                EggTrackingHelper.MoltenEggCards.Add(card, null);
            }
            else if (modifyingRelic.Id.Entry == "TOXIC_EGG")
            {
                EggTrackingHelper.ToxicEggCards.Add(card, null);
            }
            else if (modifyingRelic.Id.Entry == "FROZEN_EGG")
            {
                EggTrackingHelper.FrozenEggCards.Add(card, null);
            } else if( modifyingRelic.Id.Entry == "FRESNEL_LENS")
            {
                EggTrackingHelper.FresnelLensCards.Add(card, null);
            }
            else
            {
                //Some other relic modified this card, remove it from the tracking list as we cant be sure about the final result anymore
                EggTrackingHelper.CardRewards.Remove(__instance);
            }
        }
    }
}

#region - Card Adding
[HarmonyPatch(
    typeof(CardPileCmd),
    nameof(CardPileCmd.Add),
    new Type[]
    {
        typeof(CardModel),
        typeof(PileType),
        typeof(CardPilePosition),
        typeof(AbstractModel),
        typeof(bool),
    }
)]
public static class CardPileAddPatch
{
    static void Prefix(
        CardModel card,
        PileType newPileType,
        CardPilePosition position,
        AbstractModel source,
        bool skipVisuals
    )
    {
        if (newPileType != PileType.Deck)
        {
            return;
        }

        if (card is null)
        {
            return;
        }

        if (EggTrackingHelper.MoltenEggCards.TryGetValue(card, out _))
        {
            RelicStatCache.RecordCustomStat(
            "MOLTEN_EGG",
            new List<int> { 1 }
        );

            EggTrackingHelper.MoltenEggCards.Remove(card);
        }
        else if (EggTrackingHelper.ToxicEggCards.TryGetValue(card, out _))
        {
            RelicStatCache.RecordCustomStat(
            "TOXIC_EGG",
            new List<int> { 1 }
        );

            EggTrackingHelper.ToxicEggCards.Remove(card);
        }
        else if (EggTrackingHelper.FrozenEggCards.TryGetValue(card, out _))
        {
            RelicStatCache.RecordCustomStat(
            "FROZEN_EGG",
            new List<int> { 1 }
        );

            EggTrackingHelper.FrozenEggCards.Remove(card);
        } else if (EggTrackingHelper.FresnelLensCards.TryGetValue(card, out _))
        {
            RelicStatCache.RecordCustomStat(
            "FRESNEL_LENS",
            new List<int> { 1 }
        );

            EggTrackingHelper.FresnelLensCards.Remove(card);
        }
    }
}
#endregion
//Seems to work, just need to add exlusions in the flash method. As its giving false information
//Make a helper for that, that holds a list of exclusions (Girya) too
#region - Molten Egg
[HarmonyPatch(typeof(MoltenEgg), nameof(MoltenEgg.TryModifyCardBeingAddedToDeck))]
public static class MoltenEggAddToDeckPatch
{
    static void Prefix(MoltenEgg __instance, CardModel card)
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        if (card.Type != CardType.Attack)
        {
            return;
        }
        if (!card.IsUpgradable)
        {
            return;
        }
        if (card.CurrentUpgradeLevel >= 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}

[HarmonyPatch(typeof(MoltenEgg), nameof(MoltenEgg.TryModifyCardRewardOptionsLate))]
public static class MoltenEggCardRewardPatch
{
    static void Prefix(
        MoltenEgg __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (options.Flags.HasFlag(CardCreationFlags.NoHookUpgrades))
        {
            return;
        }

        foreach (CardCreationResult c in cardRewards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Attack && card.IsUpgradable)
            {
                //Molten egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}

[HarmonyPatch(typeof(MoltenEgg), nameof(MoltenEgg.ModifyMerchantCardCreationResults))]
public static class MoltenEggMerchantPatch
{
    static void Prefix(MoltenEgg __instance, Player player, List<CardCreationResult> cards)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        foreach (CardCreationResult c in cards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Attack && card.IsUpgradable)
            {
                //Toxic egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}
#endregion

#region - Toxic Egg
[HarmonyPatch(typeof(ToxicEgg), nameof(ToxicEgg.TryModifyCardBeingAddedToDeck))]
public static class ToxicEggAddToDeckPatch
{
    static void Prefix(ToxicEgg __instance, CardModel card)
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        if (card.Type != CardType.Skill)
        {
            return;
        }
        if (!card.IsUpgradable)
        {
            return;
        }
        if (card.CurrentUpgradeLevel >= 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}

[HarmonyPatch(typeof(ToxicEgg), nameof(ToxicEgg.TryModifyCardRewardOptionsLate))]
public static class ToxicEggCardRewardPatch
{
    static void Prefix(
        ToxicEgg __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (options.Flags.HasFlag(CardCreationFlags.NoHookUpgrades))
        {
            return;
        }

        foreach (CardCreationResult c in cardRewards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Skill && card.IsUpgradable)
            {
                //Toxic egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}

[HarmonyPatch(typeof(ToxicEgg), nameof(ToxicEgg.ModifyMerchantCardCreationResults))]
public static class ToxicEggMerchantPatch
{
    static void Prefix(ToxicEgg __instance, Player player, List<CardCreationResult> cards)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        foreach (CardCreationResult c in cards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Skill && card.IsUpgradable)
            {
                //Toxic egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}
#endregion

#region - Frozen Egg
[HarmonyPatch(typeof(FrozenEgg), nameof(FrozenEgg.TryModifyCardBeingAddedToDeck))]
public static class FrozenEggAddToDeckPatch
{
    static void Prefix(FrozenEgg __instance, CardModel card)
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        if (card.Type != CardType.Power)
        {
            return;
        }
        if (!card.IsUpgradable)
        {
            return;
        }
        if (card.CurrentUpgradeLevel >= 1)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}

[HarmonyPatch(typeof(FrozenEgg), nameof(FrozenEgg.TryModifyCardRewardOptionsLate))]
public static class FrozenEggCardRewardPatch
{
    static void Prefix(
        FrozenEgg __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (options.Flags.HasFlag(CardCreationFlags.NoHookUpgrades))
        {
            return;
        }

        foreach (CardCreationResult c in cardRewards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Power && card.IsUpgradable)
            {
                //Frozen egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}

[HarmonyPatch(typeof(FrozenEgg), nameof(FrozenEgg.ModifyMerchantCardCreationResults))]
public static class FrozenEggMerchantPatch
{
    static void Prefix(FrozenEgg __instance, Player player, List<CardCreationResult> cards)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        foreach (CardCreationResult c in cards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Power && card.IsUpgradable)
            {
                //Frozen egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}
#endregion

#region - Fresnel Lens
[HarmonyPatch(typeof(FresnelLens), nameof(FresnelLens.TryModifyCardBeingAddedToDeck))]
public static class FresnelLensAddToDeckPatch
{
    static void Prefix(FresnelLens __instance, CardModel card, out CardModel? newCard)
    {
        newCard = null;
		if (card.Owner != __instance.Owner)
		{
			return;
		}
		if (!ModelDb.Enchantment<Nimble>().CanEnchant(card))
		{
			return;
		}

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}

[HarmonyPatch(typeof(FresnelLens), nameof(FresnelLens.TryModifyCardRewardOptionsLate))]
public static class FresnelLensCardRewardPatch
{
    static void Prefix(
        FresnelLens __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
       if (player != __instance.Owner)
		{
			return;
		}

        foreach (CardCreationResult c in cardRewards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (card.Type == CardType.Power && card.IsUpgradable)
            {
                //Frozen egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
        }
    }
}

[HarmonyPatch(typeof(FresnelLens), nameof(FresnelLens.ModifyMerchantCardCreationResults))]
public static class FresnelLensMerchantPatch
{
    static void Prefix(FresnelLens __instance, Player player, List<CardCreationResult> cards)
    {
        if (player != __instance.Owner)
        {
            return;
        }

        Nimble nimble = ModelDb.Enchantment<Nimble>();
        foreach (CardCreationResult c in cards)
        {
            if (EggTrackingHelper.CardRewards.TryGetValue(c, out _))
            {
                continue;
            }

            CardModel card = c.Card;

            if (nimble.CanEnchant(card))
			{
                //Frozen egg will upgrade this card. Mark it
                EggTrackingHelper.CardRewards.Add(c, null);
            }
            
        }
    }
}
#endregion