using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

[HarmonyPatch(typeof(CloneRestSiteOption), nameof(CloneRestSiteOption.OnSelect))]
public static class PaelsGrowthPatch
{
    static void Prefix(CloneRestSiteOption __instance)
    {
        var owner = Traverse.Create(__instance).Property("Owner").GetValue<Player>();
        if(owner != null)
        {
            
            IEnumerable<CardModel> enumerable = owner.Deck.Cards.Where((CardModel c) => c.Enchantment is Clone).ToList();
            RelicStatCache.RecordCustomStat(
                "PAELS_GROWTH",
                new List<int> { enumerable.Count()}
            );
        }
    }
}