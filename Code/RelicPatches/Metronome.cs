using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(Metronome), nameof(Metronome.AfterOrbChanneled))]
public static class MetronomePatch
{
    private static readonly System.Reflection.FieldInfo OrbsChanneledField = AccessTools.Field(
        typeof(Metronome),
        "_orbsChanneled"
    );

    static void Prefix(
        Metronome __instance,
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel orb
    )
    {
        if (player == __instance.Owner)
        {
            int OrbsChanneled = (int)OrbsChanneledField.GetValue(__instance);

            if (OrbsChanneled+1 == __instance.DynamicVars["OrbCount"].IntValue)
            {
                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    "Dealt [blue]{0}[/blue] damage.",
                    new List<int>
                    {
                        __instance.DynamicVars.Damage.IntValue
                            * player.Creature.CombatState.HittableEnemies.Count,
                    }
                );
            }
        }
    }
}
