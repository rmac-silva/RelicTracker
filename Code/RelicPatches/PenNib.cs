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

            CardModel? doubledCard = (CardModel?)CardToDoubleField.GetValue(__instance);

            int damageValue = 0;
            if (doubledCard != null && doubledCard.DynamicVars.ContainsKey("Damage"))
            {
                damageValue = doubledCard.DynamicVars.Damage.IntValue;
            } else
            {
                ModLog.Warning("\nPenNibPatch: Could not find damage value for the card being doubled.");
            }

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Triggered [blue]{0}[/blue] times.\nIncreased overall damage by [blue]{1}[/blue].",
            new List<int> { 1, damageValue }
        );
        }

        
    }
}
