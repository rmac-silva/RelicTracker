using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(EternalFeather), nameof(EternalFeather.AfterRoomEntered))]
public static class EternalFeatherPatch
{
    static void Postfix(EternalFeather __instance, AbstractRoom room)
    {
        if (room is RestSiteRoom)
        {
            int numCards = PileType.Deck.GetPile(__instance.Owner).Cards.Count / __instance.DynamicVars.Cards.IntValue;

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Heal.IntValue * numCards }
        );
        }

    }
}
