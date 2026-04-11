using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RingOfTheSnake), nameof(RingOfTheSnake.ModifyHandDraw))]
public static class RingOfTheSnakePatch
{
    static void Postfix(
        RingOfTheSnake __instance,
        Player player, decimal count
    )
    {
        if (player != __instance.Owner)
		{
			return;
		}
		if (player.Creature.CombatState.RoundNumber > 1)
		{
			return;
		}
			
            RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Cards.IntValue }
        );
		
        
    }
}
