using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(LetterOpener), nameof(LetterOpener.AfterCardPlayed))]
public static class LetterOpenerPatch
{
    private static readonly System.Reflection.FieldInfo? SkillsPlayedField = 
        AccessTools.Field(typeof(LetterOpener), "_skillsPlayedThisTurn") ?? 
        AccessTools.Field(typeof(LetterOpener), "SkillsPlayedThisTurn");
    static void Postfix(LetterOpener __instance, PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Owner == __instance.Owner
            && CombatManager.Instance.IsInProgress
            && cardPlay.Card.Type == CardType.Skill
        )
        {
            int numEnemies = __instance.Owner.Creature.CombatState.HittableEnemies.Count;

            int skillsPlayedThisTurn;
            
            skillsPlayedThisTurn = (int)SkillsPlayedField.GetValue(__instance);
            
            int intValue = __instance.DynamicVars.Cards.IntValue;
            if (skillsPlayedThisTurn % intValue == 0)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Dealt [blue]{0}[/blue] damage.",
                    new List<int> { __instance.DynamicVars.Damage.IntValue * numEnemies }
                );
            }
        }
    }
}
