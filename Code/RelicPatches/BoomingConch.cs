using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(BoomingConch), nameof(BoomingConch.ModifyHandDraw))]
public static class BoomingConchPatch
{
    static void Postfix(BoomingConch __instance, Player player, decimal count)
    {
        
        if (player != __instance.Owner)
        {
            return;
        }

        AbstractRoom? currentRoom = player.RunState.CurrentRoom;

        if (player.Creature.CombatState.RoundNumber <= 1 && currentRoom.RoomType == RoomType.Elite)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Drew [blue]{0}[/blue] additional cards.",
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
        }
    }
}
