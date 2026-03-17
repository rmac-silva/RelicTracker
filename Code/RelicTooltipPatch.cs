using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using MegaCrit.Sts2.Core.Context;

[HarmonyPatch(typeof(RelicModel), nameof(RelicModel.Flash), new Type[0])]
public static class RelicFlashPatch
{
    // This "Prefix" runs BEFORE the actual Flash occurs
    static void Prefix(RelicModel __instance)
    {

        // Additional hail mary check
        if (!__instance.IsMutable) 
        {
            return;
        }

        //Checks if we own the relic
        if (__instance.Owner != null && !LocalContext.IsMe(__instance.Owner))
        {
            return;
        }

        // __instance is the specific relic that just activated (e.g., Burning Blood, Anchor)
        string relicId = __instance.Id.Entry;

        // Save to your JSON or Dictionary here
        RelicStatCache.RecordTriggerStat(relicId);
    }
}

[HarmonyPatch(
    typeof(Player),
    "CreateForNewRun",
    new Type[] { typeof(CharacterModel), typeof(UnlockState), typeof(ulong) }
)]
public static class PlayerCreationPatch
{
    static void Postfix(Player __result)
    {
        RelicStatCache.InitializeForNewRun();
    }
}

[HarmonyPatch(typeof(RelicModel), "get_HoverTip")] // Swapped to the singular getter!
public static class RelicTooltipPatch
{
    public static void Postfix(RelicModel __instance, ref HoverTip __result)
    {
        string newDescription;
        //Fetch the amount of times it was triggered
        ;

        if (RelicStatCache.HasStatsForRelic(__instance.Id.Entry))
        {
            string detailedStats = RelicStatCache.GetDetailedStats(__instance.Id.Entry);

            if (detailedStats != null && detailedStats != "")
            {
                newDescription =
                    __result.Description + "\n\n[red][Relic Tracker][/red]\n" + detailedStats;
            }
            else
            {
                int triggerCount = RelicStatCache.GetTriggeredCount(__instance.Id.Entry);
                string extraText =
                    $"\n\n[red][Relic Tracker][/red]\n[gold]Times Triggered:[/gold] {triggerCount}";
                newDescription = __result.Description + extraText;
            }
        }
        else
        {
            //No data yet. Just add a note about that.
            newDescription =
                __result.Description
                + "\n\n[red][Relic Tracker][/red]\n[gold]No data yet...[/gold]";
        }

        //Box the struct into an object so Reflection can modify it
        object customHoverTip = __result;

        //Grab the property using Harmony's AccessTools to bypass 'private set'
        PropertyInfo descriptionProp = AccessTools.Property(typeof(HoverTip), "Description");

        //Set the value directly on the property
        descriptionProp.SetValue(customHoverTip, newDescription);

        //Unbox it back into the HoverTip object
        __result = (HoverTip)customHoverTip;
    }
}
