using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BookRepairKnife), nameof(BookRepairKnife.AfterDiedToDoom))]
public static class BookRepairKnifePatch
{
    static void Postfix(
        BookRepairKnife __instance,
        PlayerChoiceContext choiceContext,
        IReadOnlyList<Creature> creatures
    )
    {
        int num = creatures.Count(
            (Creature c) => c != __instance.Owner.Creature && c.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal())
        );
        if (num >= 0)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 3 }
        );
        }
    }
}
