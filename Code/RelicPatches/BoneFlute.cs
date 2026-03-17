using HarmonyLib;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Relics;

[HarmonyPatch(typeof(BoneFlute), nameof(BoneFlute.AfterAttack))]
public static class BoneFlutePatch
{
    static void Postfix(BoneFlute __instance, AttackCommand command)
    {
        if (command.Attacker.PetOwner?.Creature != __instance.Owner.Creature)
        {
            return;
        }

        if (command.Attacker?.Monster is Osty)
        {
            RelicStatCache.RecordCustomStat(
                __instance.Id.Entry,
                "Gained [blue]{0}[/blue] [gold]Block[/gold].",
                new List<int> { 2 }
            );
        }
    }
}
