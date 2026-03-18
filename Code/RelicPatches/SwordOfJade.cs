using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(SwordOfJade), nameof(SwordOfJade.AfterRoomEntered))]
public static class SwordOfJadePatch
{
    static void Postfix(SwordOfJade __instance, AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "[gold]Times Triggered:[/gold] [blue]{0}[/blue]",
                new List<int> { 1 }
            );
        }
    }
}
