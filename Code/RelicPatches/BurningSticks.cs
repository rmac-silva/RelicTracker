using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BurningSticks), nameof(BurningSticks.AfterCardExhausted))]
public static class BurningSticksPatch
{


    private static readonly System.Reflection.FieldInfo WasUsedField = AccessTools.Field(
        typeof(BurningSticks),
        "_wasUsedThisCombat"
    );

    static void Prefix(
        BurningSticks __instance,
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal
    )
    {
        if (card.Owner != __instance.Owner)
        {
            return;
        }

        if (WasUsedField == null)
        {
            return;
        }

        bool usedThisCombat = (bool)WasUsedField.GetValue(__instance);
        

        if (!usedThisCombat && card.Type == CardType.Skill)
        {
           
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
        }
    }
}
