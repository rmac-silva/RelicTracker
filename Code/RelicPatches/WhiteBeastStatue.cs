using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(WhiteBeastStatue), nameof(WhiteBeastStatue.ShouldForcePotionReward))]
public static class WhiteBeastStatuePatch
{
    static void Postfix(WhiteBeastStatue __instance, Player player, RoomType roomType)
    {
        if (player != __instance.Owner)
        {
            return;
        }
        if (!roomType.IsCombatRoom())
        {
            return;
        }

        __instance.Flash();
    }
}
