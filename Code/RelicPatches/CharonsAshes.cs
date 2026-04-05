using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(CharonsAshes), nameof(CharonsAshes.AfterCardExhausted))]
public static class CharonsAshesPatch
{
    static void Postfix(
        CharonsAshes __instance,
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _
    )
    {
        if (card.Owner == __instance.Owner)
        {
            int numEnemies = __instance.Owner.Creature.CombatState.HittableEnemies.Count;

            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Dealt [blue]{0}[/blue] [orange]damage[/orange] to enemies.",
                new List<int> { 3 * numEnemies }
            );
        }
    }
}
