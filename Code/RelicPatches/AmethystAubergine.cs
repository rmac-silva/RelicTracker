using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(AmethystAubergine), nameof(AmethystAubergine.TryModifyRewards))]
public static class AmethystAuberginePatch
{
    static void Postfix(
        AmethystAubergine __instance,
        Player player,
        List<Reward> rewards,
        AbstractRoom? room
    )
    {

        if (player != __instance.Owner)
        {
            return;
        }

        if (room == null)
        {
            return;
        }
        if (!room.RoomType.IsCombatRoom())
        {
            return;
        }
        if (
            room.RoomType == RoomType.Boss
            && player.RunState.CurrentActIndex >= player.RunState.Acts.Count - 1
        )
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            "Gained [blue]{0}[/blue] [gold]Gold[/gold].",
            new List<int> { 10 }
        );
    }
}
