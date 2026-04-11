using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

public static class RelicLabelRenamer
{
    private static Dictionary<string, int> relicMultipliers = new Dictionary<string, int>()
    {
        { "BLACK_BLOOD", 12 },
        { "BOOKMARK", 1 },
        { "BURNING_BLOOD", 6 },
        { "CAPTAINS_WHEEL", 18 },
        { "CHANDELIER", 3 },
        { "CHOICES_PARADOX", 1 },
        { "CROSSBOW", 1 },
        { "DARKSTONE_PERIAPT", 1 },
        { "DAUGHTER_OF_THE_WIND", 1 },
        { "DRAGON_FRUIT", 1 },
        { "DREAM_CATCHER", 1 },
        { "EMBER_TEA", 2 },
        { "FORGOTTEN_SOUL", 1 },
        { "FUNERARY_MASK", 3 },
        { "GAME_PIECE", 1 },
        { "GORGET", 4 },
        { "GREMLIN_HORN", 1 },
        { "HAND_DRILL", 2 },
        { "HELICAL_DART", 1 },
        { "HISTORY_COURSE", 1 },
        { "HORN_CLEAT", 14 },
        { "IRON_CLUB", 1 },
        { "IVORY_TILE", 1 },
        { "JOSS_PAPER", 1 },
        { "KUNAI", 1 },
        { "LANTERN", 1 },
        { "LASTING_CANDY", 1 },
        { "MUSIC_BOX", 1 },
        { "NINJA_SCROLL", 3 },
        { "NUNCHAKU", 1 },
        { "ODDLY_SMOOTH_STONE", 1 },
        { "ORNAMENTAL_FAN", 4 },
        { "PAELS_FLESH", 1 },
        { "PANTOGRAPH", 25 },
        { "PENDULUM", 1 },
        { "PERMAFROST", 7 },
        { "PETRIFIED_TOAD", 1 },
        { "PLANISPHERE", 5 },
        { "RAINBOW_RING", 1 },
        { "REPTILE_TRINKET", 3 },
        { "RIPPLE_BASIN", 4 },
        { "RUNIC_CAPACITOR", 3 },
        { "SAI", 7 },
        { "SHURIKEN", 1 },
        { "SWORD_OF_JADE", 3 },
        { "TUNGSTEN_ROD", 1 },
        { "VENERABLE_TEA_SET", 2 },
        { "VEXING_PUZZLEBOX", 1 },
        { "FAKE_VENERABLE_TEA_SET", 1 },
        { "SPARKLING_ROUGE", 1 },
        { "CENTENNIAL_PUZZLE", 3 },
        { "STONE_CRACKER", 2 },
        { "VAJRA", 1 },
        { "UNCEASING_TOP", 1 },
    };

    public static string GetAlternateLabel(string relicId, int value)
    {
        string locText = LocalizationHelper.GetLocalizedString(relicId);

        if (string.IsNullOrWhiteSpace(locText))
        {
            return ""; // Hide tooltip strings that are intentionally left blank or missing
        }

        int multiplier = relicMultipliers.TryGetValue(relicId, out int storedMultiplier) ? storedMultiplier : 1;
        int displayValue = DefaultValueFormatter(value, multiplier);

        try 
        {
            return string.Format(locText, displayValue);
        }
        catch (System.FormatException)
        {
            return ""; 
        }
    }

    

    private static int DefaultValueFormatter(int rawValue, int factor)
    {
        return rawValue * factor;
    }
}
