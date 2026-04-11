using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;


[HarmonyPatch(typeof(LocManager), "SetLanguageInternal")]
public static class LanguageChangePatch
{
    static void Postfix(string language, Dictionary<string, LocTable> tables, bool overridesActive, List<LocValidationError> validationErrors)
    {
        ModLog.Info("Language changed to " + language);
        LocalizationHelper.SetLanguage(language);
    }
}