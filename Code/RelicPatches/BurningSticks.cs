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

        ModLog.Warning($"Burning Sticks exhausted card. Card: {card.Id.Entry}");
        if (card.Owner != __instance.Owner)
        {
            return;
        }

        if (WasUsedField == null)
        {
            ModLog.Error("Could not find field _wasUsedThisCombat on BurningSticks", new System.Exception("Field not found"));
            return;
        }

        bool usedThisCombat = (bool)WasUsedField.GetValue(__instance);
        ModLog.Warning($"Burning Sticks exhausted card. Used this combat: {usedThisCombat}. Card: {card.Id.Entry}");
        

        if (!usedThisCombat && card.Type == CardType.Skill)
        {
           
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Duplicated [blue]{0}[/blue] cards.",
                new List<int> { 1 }
            );
        }
    }
}
