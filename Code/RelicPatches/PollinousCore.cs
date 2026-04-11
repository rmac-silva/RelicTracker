using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PollinousCore), nameof(PollinousCore.ModifyHandDraw))]


public static class PollinousCorePatch
{

    private static readonly FieldInfo CardsPlayedLastTurnField = AccessTools.Field(
        typeof(PollinousCore),
        "_turnsSeen"
    );

    static void Postfix(PollinousCore __instance, Player player, decimal count)
    {
        if (player != __instance.Owner)
		{
			return;
		}

        int TurnsSeen = (int)CardsPlayedLastTurnField.GetValue(__instance);

		if (TurnsSeen == __instance.DynamicVars["Turns"].IntValue)
		{
			RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Cards.IntValue }
        );
		}
			
            
		

    }
}
