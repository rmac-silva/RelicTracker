using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Saves;

[HarmonyPatch(typeof(WelcomeToWongos), "CheckObtainWongoBadge")]
public static class WongoPatch
{
    static void Prefix(WelcomeToWongos __instance, int pointsEarned)
    {
        int wongoPoints = SaveManager.Instance.Progress.WongoPoints;
        int num = wongoPoints % 2000;
        int num2 = num + pointsEarned;
        int num3 = wongoPoints + pointsEarned;
        ModLog.Info($"Wongo badge points earned: {pointsEarned}. Total: {num3}.");

        if (num2 >= 2000)
        {
            // await RelicCmd.Obtain<WongoCustomerAppreciationBadge>(base.Owner);`
            RelicStatCache.RecordCustomStat(
                "WONGO_CUSTOMER_APPRECIATION_BADGE",
                new List<int> { num3 }
            );
        }
    }
}
