using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

[HarmonyPatch(typeof(CombatRoom), "EnterInternal", new Type[] { typeof(IRunState), typeof(bool) })]
public static class CombatStartPatch
{
    static void Postfix()
    {
        CombatStartManager.NotifyCombatStarted();
    }
}

public static class CombatStartManager
{
    public static int _currentCombatId = 0;

    public static void NotifyCombatStarted()
    {
        // Every time a room is entered, we generate a new ID
        _currentCombatId++;
    }

    /// <summary>
    /// Checks if the provided ID matches the current combat.
    /// Used by patches to detect if they need to reset their local variables.
    /// </summary>
    public static bool IsNewCombat(ref int localCombatId)
    {
        if (localCombatId != _currentCombatId)
        {
            localCombatId = _currentCombatId;
            return true;
        }
        return false;
    }
}
