using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;

[HarmonyPatch(typeof(WingCharm), nameof(WingCharm.TryModifyCardRewardOptionsLate))]
public static class WingCharmPatch
{
    private static List<CardCreationResult> _currentCardRewards = new List<CardCreationResult>();

    static void Postfix(
        WingCharm __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
        //Not the relic owner
        if (player != __instance.Owner)
        {
            return;
        }

        //If we're looking at the same list of cards, do nothing
        if (_currentCardRewards.SequenceEqual(cardRewards))
        {
            return;
        }


         //If there are no card rewards, do nothing

        //No enchantable cards, do nothing
        Swift canonicalSwift = ModelDb.Enchantment<Swift>();
        List<CardCreationResult> list = cardRewards
            .Where((CardCreationResult r) => canonicalSwift.CanEnchant(r.Card))
            .ToList();
        if (list.Count == 0)
        {
            return;
        }

        _currentCardRewards.Clear();
        _currentCardRewards.AddRange(list);

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}
