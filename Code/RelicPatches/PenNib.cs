using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PenNib), nameof(PenNib.BeforeCardPlayed))]
public static class PenNibPatch
{

    private static readonly System.Reflection.FieldInfo AttacksPlayedField = AccessTools.Field(
        typeof(PenNib),
        "_attacksPlayed"
    );

    private static readonly System.Reflection.FieldInfo CardToDoubleField = AccessTools.Field(
        typeof(PenNib),
        "_attackToDouble"
    );
    static void Postfix(PenNib __instance, CardPlay cardPlay)
    {
        
        int AttacksPlayed = (int)AttacksPlayedField.GetValue(__instance);

        if (AttacksPlayed == null)
        {
            return;
        }
        if (AttacksPlayed == 0)
        {

            CardModel? DoubledCard = (CardModel?)CardToDoubleField.GetValue(__instance);

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Triggered [blue]{0}[/blue] times.\nIncreased overall damage by [blue]{1}[/blue].",
            new List<int> { 1, DoubledCard?.DynamicVars.Damage.IntValue ?? 0 }
        );
        }

        
    }
}
