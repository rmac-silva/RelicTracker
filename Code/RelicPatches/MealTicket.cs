using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(MealTicket), nameof(MealTicket.AfterRoomEntered))]
public static class MealTicketPatch
{
    static void Postfix(MealTicket __instance, AbstractRoom room)
    {

        if (!__instance.Owner.Creature.IsDead && room is MerchantRoom)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Healed [blue]{0}[/blue] [gold]HP[/gold].",
                new List<int> { __instance.DynamicVars.Heal.IntValue }
            );
        }
    }
}
