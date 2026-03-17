using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(RingOfTheDrake), nameof(RingOfTheDrake.ModifyHandDraw))]
public static class RingOfTheDrakePatch
{
    static void Postfix(
        RingOfTheDrake __instance,
        Player player, decimal count
    )
    {
        if (player != __instance.Owner)
		{
			return;
		}
		if ((decimal)player.Creature.CombatState.RoundNumber > __instance.DynamicVars["Turns"].BaseValue)
		{
			return;
		}
			
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Drew [blue]{0}[/blue] additional cards.",
                new List<int> { __instance.DynamicVars.Cards.IntValue }
            );
		
        
    }
}
