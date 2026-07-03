using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(EternalFeather), nameof(EternalFeather.AfterRoomEntered))]
public static class EternalFeatherPatch
{
    static void Prefix(EternalFeather __instance, AbstractRoom room)
    {
        if (room is RestSiteRoom && LocalContext.IsMe(__instance.Owner))
        {
            int numCards = PileType.Deck.GetPile(__instance.Owner).Cards.Count / __instance.DynamicVars.Cards.IntValue;

            Creature creature = __instance.Owner.Creature;
            int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
            //If the health I'm missing is below the heal amount, then I'm only healing for healthMising

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { healthMissing < __instance.DynamicVars.Heal.IntValue * numCards ? healthMissing : __instance.DynamicVars.Heal.IntValue * numCards });
        }

    }
}
