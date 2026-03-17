/// <summary>
/// This is an example of a postfix patch for a relic that records a custom stat when the relic activates.
/// You can copy this structure for other relics by changing the class and method names,
/// as well as the logic inside the Postfix method to match the specific effect of each relic.
///
/// This can be used for example in the bag of marbles, where it doesn't matter if we log the usage before or after.
/// We can just log the number of enemies affected after the relic is processed, since that number won't change.
/// </summary>
// [HarmonyPatch(typeof(BagOfMarbles), nameof(BagOfMarbles.BeforeSideTurnStart))]
// public static class BagOfMarblesPatch
// {
//     static void Postfix(
//         BagOfMarbles __instance,
//         PlayerChoiceContext choiceContext,
//         CombatSide side,
//         CombatState combatState
//     )
//     {
//         if (side == __instance.Owner.Creature.Side && combatState.RoundNumber <= 1)
//         {
//            
//             RelicStatCache.RecordCustomStat(
//                 __instance.Id.Entry,
//                 "Applied [gold]Vulnerable[/gold] to [blue]{0}[/blue] enemies.",
//                 new List<int> { combatState.HittableEnemies.Count }
//             );
//         }
//     }
// }
