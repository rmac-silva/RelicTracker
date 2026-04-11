using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(SlingOfCourage), nameof(SlingOfCourage.AfterRoomEntered))]
public static class SlingOfCouragePatch
{
    static void Postfix(SlingOfCourage __instance, AbstractRoom room)
    {
        if (room.RoomType == RoomType.Elite)
		{

            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Strength.IntValue }
        );
        }
    }
}
