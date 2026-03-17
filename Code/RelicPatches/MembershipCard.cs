// using HarmonyLib;
// using MegaCrit.Sts2.Core.Context;
// using MegaCrit.Sts2.Core.Entities.Merchant;
// using MegaCrit.Sts2.Core.Entities.Players;
// using MegaCrit.Sts2.Core.Models.Relics;

// [HarmonyPatch(typeof(MembershipCard), nameof(MembershipCard.ModifyMerchantPrice))]
// public static class MembershipCardPatch
// {
//     static void Postfix(MembershipCard __instance, Player player, MerchantEntry entry, decimal originalPrice)
//     {

//         if (player != __instance.Owner)
//         {
//             return;
//         }
//         if (!LocalContext.IsMe(__instance.Owner))
//         {
//             return;
//         }


//         var discountedValue = originalPrice - originalPrice * (__instance.DynamicVars["Discount"].BaseValue / 100m);
//         ModLog.Warning($"\nTriggered membership card.\nOriginal price: {originalPrice}. Discounted price: {discountedValue}.\n");

//         RelicStatCache.RecordCustomStat(
//             __instance.Id.Entry,
//             "Discounted [blue]{0}[/blue] [gold]Gold[/gold].",
//             new List<int> { (int)discountedValue }
//         );

//     }
// }
