using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(ThrowingAxe), nameof(ThrowingAxe.ModifyCardPlayCount))]
public static class ThrowingAxePatch
{
    private static readonly FieldInfo UsedThisCombatField = AccessTools.Field(
        typeof(ThrowingAxe),
        "_usedThisCombat"
    );

    static void Postfix(ThrowingAxe __instance, CardModel card, Creature? target, int playCount)
    {
        bool UsedThisCombat = (bool)ThrowingAxePatch.UsedThisCombatField.GetValue(__instance);
        if (UsedThisCombat)
        {
            return;
        }
        if (card.Owner != __instance.Owner)
        {
            return;
        }
        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Cards duplicated: [blue]{0}[/blue].",
            new List<int> { 1 }
        );
    }
}
