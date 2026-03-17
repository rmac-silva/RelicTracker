using HarmonyLib;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(MawBank), nameof(MawBank.AfterRoomEntered))]
public static class MawBankPatch
{
    static void Postfix(MawBank __instance, AbstractRoom room)
    {
        
        

        if (__instance.Owner.RunState.BaseRoom == room && !__instance.HasItemBeenBought)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]gold[/gold].",
                new List<int> { __instance.DynamicVars.Gold.IntValue }
            );
        }
    }
}
