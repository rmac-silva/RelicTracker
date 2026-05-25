using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeOrichalcum), "BeforeSideTurnEndVeryEarly")]
public static class FakeOrichalcumPatch
{
    static void Postfix(FakeOrichalcum __instance, PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != __instance.Owner.Creature.Side)
		{
			return;
		}
		if (__instance.Owner.Creature.Block > 0)
		{
			return;
		}

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { __instance.DynamicVars.Block.IntValue }
        );
    }
}
