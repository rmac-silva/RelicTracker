/// This is an example of a relic patch that tracks stats for a relic, before it is processed.
/// This is useful in the ArtOfWar, since the variables anyAttacksPlayedLastTurn get reset after the relic is processed.
// [HarmonyPatch(typeof(ArtOfWar), nameof(ArtOfWar.AfterEnergyReset))]
// public static class RelicPrefixTemplate
// {
//     static void Prefix(ArtOfWar __instance, Player player)
//     {
//         var field = AccessTools.Field(typeof(ArtOfWar), "_anyAttacksPlayedLastTurn");
//         bool anyAttacksPlayedLastTurn = (bool)field.GetValue(__instance);
//         
//         if (player.Creature.CombatState.RoundNumber > 1 && !anyAttacksPlayedLastTurn)
//         {
//             RelicStatCache.RecordCustomStat(
//                 __instance.Id.Entry,
//                 "Gained [blue]{0}[/blue] [gold]Energy[/gold].",
//                 new List<int> { 1 }
//             );
//         }
//     }
// }
