using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(PhilosophersStone), nameof(PhilosophersStone.ModifyMaxEnergy))]
public static class PhilosophersStoneEnergyPatch
{
    private static int _roundNumber = -1;
    private static int _combatID = -1;

    static void Postfix(PhilosophersStone __instance, Player player, decimal amount)
    {
        // Only trigger for this relic's owner
        if (player != __instance.Owner)
        {
            return;
        }

        //If the combats are different, reset the round tracking
        if (CombatStartManager.IsNewCombat(ref _combatID))
        {
            _roundNumber = -1;
        }
        
        // Only trigger once per round
        if (player.Creature.CombatState.RoundNumber == _roundNumber)
        {
            return;
        }


        _roundNumber = player.Creature.CombatState.RoundNumber;

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { (int)__instance.DynamicVars.Energy.IntValue, 0 }
        );
    }
}

[HarmonyPatch(typeof(PhilosophersStone), nameof(PhilosophersStone.AfterRoomEntered))]
public static class PhilosophersStoneSTREnterPatch
{
    static void Postfix(PhilosophersStone __instance, AbstractRoom room)
    {
        //Not the player, same combat round, or it's not
        if (room is not CombatRoom)
        {
            return;
        }

        IEnumerable<Creature> targets =
            from c in __instance.Owner.Creature.CombatState.GetOpponentsOf(
                __instance.Owner.Creature
            )
            where c.IsAlive
            select c;

        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 0, targets.Count() });
    }
}

[HarmonyPatch(typeof(PhilosophersStone), nameof(PhilosophersStone.AfterCreatureAddedToCombat))]
public static class PhilosophersStoneSTRMidCombatPatch
{
    static void Postfix(PhilosophersStone __instance, Creature creature)
    {
        if (creature.Side == __instance.Owner.Creature.Side)
		{
			return;
		}

        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 0, 1 });
    }
}
