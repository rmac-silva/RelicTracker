// using BaseLib.Extensions;
// using HarmonyLib;
// using MegaCrit.Sts2.Core.Entities.Creatures;
// using MegaCrit.Sts2.Core.Models;
// using MegaCrit.Sts2.Core.Models.Relics;
// using MegaCrit.Sts2.Core.ValueProps;

// [HarmonyPatch(typeof(PaperKrane), nameof(PaperKrane.ModifyWeakMultiplier))]
// public static class PaperKranePatch
// {
//     static void Postfix(
//         PaperKrane __instance,
//         Creature target,
//         decimal amount,
//         ValueProp props,
//         Creature? dealer,
//         CardModel? cardSource
//     )
//     {
//         if (target != __instance.Owner.Creature)
//         {
//             return;
//         }
//         if (!props.IsPoweredAttack_())
//         {
//             return;
//         }

//         RelicStatCache.RecordCustomStat(
//             __instance.Id.Entry,
//             "Modified weak values [blue]{0}[/blue] times.",
//             new List<int> { 1 }
//         );
//     }
// }
