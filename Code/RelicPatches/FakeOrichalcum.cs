using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(FakeOrichalcum), nameof(FakeOrichalcum.BeforeTurnEndVeryEarly))]
public static class FakeOrichalcumPatch
{
    static void Postfix(FakeOrichalcum __instance, PlayerChoiceContext choiceContext, CombatSide side)
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
