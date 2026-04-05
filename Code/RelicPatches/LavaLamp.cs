using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

[HarmonyPatch(typeof(LavaLamp), nameof(LavaLamp.TryModifyCardRewardOptionsLate))]
public static class LavaLampPatch
{
    private static readonly System.Reflection.FieldInfo? TookDamageField = 
        AccessTools.Field(typeof(LavaLamp), "_tookDamageThisCombat") ?? // Try common naming convention
        AccessTools.Field(typeof(LavaLamp), "TookDamageThisCombat");   // Fallback

    static void Postfix(
        LavaLamp __instance,
        Player player,
        List<CardCreationResult> cardRewards,
        CardCreationOptions options
    )
    {
        
        if (__instance == null || __instance.Owner?.RunState == null || player == null)
        {
            return;
        }

        if (!(__instance.Owner.RunState.CurrentRoom is CombatRoom))
        {
            return;
        }

        if (player != __instance.Owner)
        {
            return;
        }

        
        bool tookDamageThisCombat;
        if (TookDamageField != null)
        {
            tookDamageThisCombat = (bool)TookDamageField.GetValue(__instance);
        }
        else
        {
            // If field not found, fallback to Traverse which handles missing members gracefully
            tookDamageThisCombat = Traverse.Create(__instance).Field("_tookDamageThisCombat").GetValue<bool>();
        }

        if (tookDamageThisCombat)
        {
            return;
        } else
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Upgraded [blue]{0}[/blue] card rewards.",
                new List<int> { cardRewards.Count }
            );
        }

    }
}
