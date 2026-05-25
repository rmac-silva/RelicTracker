using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(MealTicket), nameof(MealTicket.AfterRoomEntered))]
public static class MealTicketPatch
{
    static void Postfix(MealTicket __instance, AbstractRoom room)
    {
        if (!LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        if (!__instance.Owner.Creature.IsDead && room is MerchantRoom)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                new List<int> { __instance.DynamicVars.Heal.IntValue }
            );
        }
    }
}
