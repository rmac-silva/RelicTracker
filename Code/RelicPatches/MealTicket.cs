using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(MealTicket), nameof(MealTicket.AfterRoomEntered))]
public static class MealTicketPatch
{
    static void Prefix(MealTicket __instance, AbstractRoom room)
    {
        if (!LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        if (!__instance.Owner.Creature.IsDead && room is MerchantRoom)
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
