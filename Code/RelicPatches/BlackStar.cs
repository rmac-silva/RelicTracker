using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(BlackStar), nameof(BlackStar.TryModifyRewards))]
public static class BlackStarPatch
{
    static void Postfix(
        BlackStar __instance,
        Player player,
        List<Reward> rewards,
        AbstractRoom? room
    )
    {
        if (player != __instance.Owner)
        {
            return;
        }

        if (room != null && room.RoomType == RoomType.Elite)
        {
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
        }
    }
}
