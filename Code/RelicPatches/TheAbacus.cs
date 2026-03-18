using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(TheAbacus), nameof(TheAbacus.AfterShuffle))]
public static class TheAbacusPatch
{
    static void Postfix(TheAbacus __instance, PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler == __instance.Owner)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]Block[/gold].",
                new List<int> { __instance.DynamicVars.Block.IntValue }
            );
        }
    }
}
