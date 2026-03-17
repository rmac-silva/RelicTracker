// using BaseLib.Extensions;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Context;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Powers;
// using MegaCrit.Sts2.Core.ValueProps;

// [HarmonyPatch(typeof(PowerModel), nameof(PowerModel.AfterDamageReceived))]
// public static class DiamondDiademPatch
// {
//     static void Postfix(
//         PowerModel __instance,
//         PlayerChoiceContext choiceContext,
//         Creature target,
//         DamageResult result,
//         ValueProp props,
//         Creature? dealer,
//         CardModel? cardSource
//     )
//     {
//         // Check if the power is the actual Diadem buff, and it actually affected this hit
//         if (__instance is not DiamondDiademPower || target != __instance.Owner)
//             return;
            
//         // Only track for local player
//         if (target == null || !LocalContext.IsMe(target))
//             return;

//         if (props.IsPoweredAttack_() && result.UnblockedDamage > 0)
//         {
//             // By the time it has hit "AfterDamageReceived", we know exactly how much damage got through!
//             // According to DiamondDiademPower, it halves the incoming damage, which means the amount
//             // it saved is equal to the final unblocked damage it permitted (Math trick for x / 2).
//             // (If unblocked hit was 5, original was 10. Diadem saved 5.)
//             int savedDamage = (int)(result.UnblockedDamage); 
            
//             RelicStatCache.RecordCustomStat(
//                 "DIAMOND_DIADEM",
//                 "Reduced damage by [blue]{0}[/blue].",
//                 new List<int> { savedDamage }
//             );
//         }
//     }
// }