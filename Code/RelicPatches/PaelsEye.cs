using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(PaelsEye), nameof(PaelsEye.AfterTakingExtraTurn))]
public static class PaelsEyePatch
{
    static void Postfix(PaelsEye __instance, Player player)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress)
            return;
        if (player != __instance.Owner)
        {
            return;
        }
        RelicStatCache.RecordCustomStat(__instance.Id.Entry, new List<int> { 1, 0 });
    }
}

[HarmonyPatch(typeof(PaelsEye), nameof(PaelsEye.BeforeTurnEndEarly))]
public static class PaelsEyeExhaustedCardsPatch
{
    private static readonly System.Reflection.FieldInfo UsedThisCombatField = AccessTools.Field(
        typeof(PaelsEye),
        "_usedThisCombat"
    );
    private static readonly System.Reflection.MethodInfo CardsPlayedThisTurnField =
        AccessTools.Method(typeof(PaelsEye), "AnyCardsPlayedThisTurn");

    static void Prefix(PaelsEye __instance, PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (CombatManager.Instance == null || !CombatManager.Instance.IsInProgress)
            return;
        try
        {
            bool UsedThisCombat = (bool)UsedThisCombatField.GetValue(__instance);
            bool AnyCardsPlayedThisTurn = (bool)CardsPlayedThisTurnField.Invoke(__instance, null);

            if (UsedThisCombat || AnyCardsPlayedThisTurn || side != CombatSide.Player)
            {
                return;
            }
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                new List<int> { 0, CardPile.GetCards(__instance.Owner, PileType.Hand).Count() }
            );
        }
        catch (System.Exception e)
        {
            ModLog.Error("PaelsEyeExhaustedCardsPatch.",e);
            return;
        }
    }
}
