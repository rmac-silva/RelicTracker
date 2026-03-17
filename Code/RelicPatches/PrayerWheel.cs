using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(PrayerWheel), nameof(PrayerWheel.TryModifyRewards))]


public static class PrayerWheelPatch
{

    

    static void Postfix(PrayerWheel __instance, Player player, List<Reward> rewards, AbstractRoom? room)
    {
        if (player != __instance.Owner)
		{
			return;
		}
		if (room == null || room.RoomType != RoomType.Monster)
		{
			return;
		}
        
			RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Added [blue]{0}[/blue] card rewards.",
                new List<int> { 1 }
            );
		
			
            
		

    }
}
