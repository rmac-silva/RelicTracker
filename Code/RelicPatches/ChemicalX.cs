using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ChemicalX), nameof(ChemicalX.ModifyXValue))]
public static class ChemicalXPatch
{
    static void Postfix(
        ChemicalX __instance,
        CardModel card, int originalValue
    )
    {
        if (card.Owner == __instance.Owner)
        {

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars["Increase"].IntValue }
        );
        }
    }
}
