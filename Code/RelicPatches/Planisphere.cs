using HarmonyLib;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

[HarmonyPatch(typeof(Planisphere), nameof(Planisphere.AfterRoomEntered))]
public static class PlanispherePatch
{
    static void Prefix(Planisphere __instance, AbstractRoom room)
    {
        //My relic + Not dead + Boss room
        if (LocalContext.IsMe(__instance.Owner) && !__instance.Owner.Creature.IsDead)
        {
            MapPoint? currentMapPoint = __instance.Owner.RunState.CurrentMapPoint;
            if (currentMapPoint != null && currentMapPoint.PointType == MapPointType.Unknown)
            {
                Creature creature = __instance.Owner.Creature;
                int healthMissing = creature.MaxHp - creature.CurrentHp; //I have 100 max hp, I'm at 80, then I'm missing 20
                //If the health I'm missing is below the heal amount, then I'm only healing for healthMising

                RelicStatCache.RecordCustomStat(
                    __instance.Id.Entry,
                    new List<int>
                    {
                        healthMissing < __instance.DynamicVars.Heal.IntValue
                            ? healthMissing
                            : __instance.DynamicVars.Heal.IntValue,
                    }
                );
            }
        }
    }
}
