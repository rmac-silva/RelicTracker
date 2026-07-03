using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Random;

[HarmonyPatch(typeof(PandorasBox), nameof(PandorasBox.AfterObtained))]
public static class PandorasBoxPatch
{
    public static bool IsPandoraRunning = false;

    static void Prefix(PandorasBox __instance)
    {
        IsPandoraRunning = true;

        List<CardModel> source = PileType
            .Deck.GetPile(__instance.Owner)
            .Cards.Where((CardModel c) => c != null && c.IsBasicStrikeOrDefend && c.IsRemovable)
            .ToList();

        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { source.Count });
    }

    static void Postfix()
    {
        IsPandoraRunning = false;
    }
}

[HarmonyPatch(
    typeof(CardCmd),
    nameof(CardCmd.Transform),
    new Type[] { typeof(IEnumerable<CardTransformation>), typeof(Rng), typeof(CardPreviewStyle) }
)]
public static class PandorasBoxTransformPatch
{
    static async Task<IEnumerable<CardPileAddResult>> Postfix(
        Task<IEnumerable<CardPileAddResult>> __result
    )
    {
        IEnumerable<CardPileAddResult> results = await __result;

        if (PandorasBoxPatch.IsPandoraRunning && results != null)
        {
            List<string> res = new List<string>();
            
            foreach (CardPileAddResult result in results)
            {
                if(result.cardAdded.Type == CardType.Attack)
                {
                    res.Add($"[red]{result.cardAdded.Title}[/red]");
                }
                if(result.cardAdded.Type == CardType.Power)
                {
                    res.Add($"[gold]{result.cardAdded.Title}[/gold]");
                }
                if(result.cardAdded.Type == CardType.Skill)
                {
                    res.Add($"[blue]{result.cardAdded.Title}[/blue]");
                }
            }
            

            RelicStatCache.RecordAdditionalStat("PANDORAS_BOX", res);
        }

        return results;
    }
}
