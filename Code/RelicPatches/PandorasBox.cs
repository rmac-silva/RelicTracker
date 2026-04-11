using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PandorasBox), nameof(PandorasBox.AfterObtained))]
public static class PandorasBoxPatch
{
    static void Prefix(PandorasBox __instance)
    {
        
            List<CardModel> source = PileType.Deck.GetPile(__instance.Owner).Cards.Where((CardModel c) => c != null && c.IsBasicStrikeOrDefend && c.IsRemovable).ToList();
            
        
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { source.Count }
        );
        
    }
}
