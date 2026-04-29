using HarmonyLib;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;

[HarmonyPatch(typeof(RunSaveManager),nameof(RunSaveManager.LoadRunSave))]
public static class LoadSingleplayerPatch
{
    static void Postfix(ReadSaveResult<SerializableRun> __result)
    {
        ModLog.Info("Loading singleplayer run save, loading relic stat cache...");
        RelicStatCache.LoadCacheFromSingleplayerSave();
    }
}

[HarmonyPatch(typeof(RunSaveManager),nameof(RunSaveManager.LoadMultiplayerRunSave))]
public static class LoadMultiplayerPatch
{
    static void Postfix(ReadSaveResult<SerializableRun> __result)
    {
        ModLog.Info("Loading multiplayer run save, loading relic stat cache...");
        RelicStatCache.LoadCacheFromMultiplayerSave();
    }
}

[HarmonyPatch(typeof(RunSaveManager), nameof(RunSaveManager.SaveRun), new Type[] { typeof(SerializableRun), typeof(bool) })]
public static class SaveRunPatch
{
    static void Prefix(Task __result, SerializableRun save, bool isMultiplayer)
    {
        ModLog.Info("Saving run, saving relic stat cache...");
        RelicStatCache.SaveCache(isMultiplayer);
    }
}
