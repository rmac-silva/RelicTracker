using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(WhiteStar), nameof(WhiteStar.TryModifyRewards))]
public static class WhiteStarPatch
{
    static void Postfix(WhiteStar __instance, Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != __instance.Owner)
		{
			return;
		}
		if (room == null || room.RoomType != RoomType.Elite)
		{
			return;
		}
        
        __instance.Flash();
    }
}
