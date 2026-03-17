using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(MiniRegent), nameof(MiniRegent.AfterStarsSpent))]
public static class MiniRegentPatch
{
private static readonly System.Reflection.FieldInfo UsedThisTurnField = AccessTools.Field(
        typeof(MiniRegent),
        "_usedThisTurn"
    );
    static void Prefix(MiniRegent __instance, int amount, Player spender)
    {
        
        bool UsedThisTurn = (bool)UsedThisTurnField.GetValue(__instance);

        if (spender == __instance.Owner && !UsedThisTurn)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]Strength[/gold].",
                new List<int> { __instance.DynamicVars.Strength.IntValue }
            );
        }
    }
}
