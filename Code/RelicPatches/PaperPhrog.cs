// using BaseLib.Extensions;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Relics;
// using MegaCrit.Sts2.Core.ValueProps;

// [HarmonyPatch(typeof(PaperPhrog), nameof(PaperPhrog.ModifyVulnerableMultiplier))]
// public static class PaperPhrogPatch
// {
//     private static int roundCounter = -1;
//     private static List<CardModel> CardsAppliedThisRound = new List<CardModel>();

//     static void Postfix(
//         PaperPhrog __instance,
//         Creature target,
//         decimal amount,
//         ValueProp props,
//         Creature? dealer,
//         CardModel? cardSource
//     )
//     {
//         // 1. Safety & filtering
//         if (__instance?.Owner?.Creature?.CombatState == null) return;
//         if (target == __instance.Owner.Creature) return; // Paper Phrog helps attacks vs ENEMIES
//         if (cardSource == null || !props.IsPoweredAttack_()) return; // Skip previews/math-only hits

//         // 2. Round Tracking (Safely use the Relic's owner state)
//         int currentRound = __instance.Owner.Creature.CombatState.RoundNumber;
//         if (currentRound != roundCounter)
//         {
//             roundCounter = currentRound;
//             CardsAppliedThisRound.Clear();
//         }

//         // 3. Prevent multi-counting if the same card hits multiple enemies or multi-hits
//         if (!CardsAppliedThisRound.Contains(cardSource))
//         {
//             CardsAppliedThisRound.Add(cardSource);

//             // Paper Phrog increases Vulnerable from 1.5x (+50%) to 1.75x (+75%).
//             // The "bonus" damage specifically from Phrog is (BaseDamage * 0.25).
//             int baseDamage = cardSource.DynamicVars.Damage.IntValue;
//             int phrogBonus = (int)Math.Floor(baseDamage * 0.25m);

//             if (phrogBonus > 0)
//             {
//                 RelicStatCache.RecordCustomStat(
//                     __instance.Id.Entry,
//                     "Increased vulnerable damage by [blue]{0}[/blue].",
//                     new List<int> { phrogBonus }
//                 );
//             }
//         }
//     }
// }
