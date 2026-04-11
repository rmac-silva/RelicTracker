using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Pocketwatch), nameof(Pocketwatch.ModifyHandDraw))]


public static class PocketwatchPatch
{

    private static readonly FieldInfo CardsPlayedLastTurnField = AccessTools.Field(
        typeof(Pocketwatch),
        "_cardsPlayedLastTurn"
    );

    static void Postfix(Pocketwatch __instance, Player player, decimal count)
    {
        if (player.Creature.CombatState.RoundNumber == 1)
		{
			return;
		}
		if (player != __instance.Owner)
		{
			return;
		}

        int _cardsPlayedLastTurn = (int)CardsPlayedLastTurnField.GetValue(__instance);
		
        if ((decimal)_cardsPlayedLastTurn <= __instance.DynamicVars["CardThreshold"].BaseValue)
		{
			
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Cards.IntValue }
        );
		}

    }
}
