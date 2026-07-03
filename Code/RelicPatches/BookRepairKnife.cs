using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BookRepairKnife), nameof(BookRepairKnife.AfterDiedToDoom))]
public static class BookRepairKnifePatch
{
    static void Prefix(
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
            Creature creature = __instance.Owner.Creature;
            int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
            //If the health I'm missing is below the heal amount, then I'm only healing for healthMising

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { healthMissing < __instance.DynamicVars.Heal.IntValue ? healthMissing : __instance.DynamicVars.Heal.IntValue });
        }
        
    }
}
