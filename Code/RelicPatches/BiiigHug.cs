using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BiiigHug), nameof(BiiigHug.AfterShuffle))]
public static class BiiigHugPatch
{
    static void Postfix(BiiigHug __instance, PlayerChoiceContext choiceContext, Player shuffler)
    {
        
        if (shuffler != __instance.Owner)
        {
            return;
        }

        RelicStatCache.RecordCustomStat(
            __instance.Id.Entry,
            new List<int> { 1 }
        );
    }
}
