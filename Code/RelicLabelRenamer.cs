using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

public static class RelicLabelRenamer
{
    private static Dictionary<string, RelicLabelInfo> relicLabelMappings = new Dictionary<string, RelicLabelInfo>()
    {
        { "BLACK_BLOOD", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]HP[/gold].", 12) },
        { "BOOKMARK", new RelicLabelInfo("Reduced cost of [blue]{0}[/blue] cards.", 1) },
        { "BURNING_BLOOD", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]HP[/gold].", 6) },
        { "CAPTAINS_WHEEL", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 18) },
        { "CHANDELIER", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 3) },
        { "CHOICES_PARADOX", new RelicLabelInfo("Generated [blue]{0}[/blue] cards.", 1) },
        { "CROSSBOW", new RelicLabelInfo("Generated [blue]{0}[/blue] attacks.", 1) },
        {
            "DARKSTONE_PERIAPT",
            new RelicLabelInfo("Maximum [gold]HP[/gold] increased by [blue]{0}[/blue].", 1)
        },
        {
            "DAUGHTER_OF_THE_WIND",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 1)
        },
        {
            "DRAGON_FRUIT",
            new RelicLabelInfo("Maximum [gold]HP[/gold] increased by [blue]{0}[/blue].", 1)
        },
        { "DREAM_CATCHER", new RelicLabelInfo("Added [blue]{0}[/blue] cards.", 1) },
        { "EMBER_TEA", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].", 2) },
        {
            "FORGOTTEN_SOUL",
            new RelicLabelInfo("Dealt [blue]{0}[/blue] [orange]damage[/orange].", 1)
        },
        { "FUNERARY_MASK", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Souls[/gold].", 3) },
        { "GAME_PIECE", new RelicLabelInfo("Drew [blue]{0}[/blue] cards.", 1) },
        { "GORGET", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Plating[/gold].", 4) },
        {
            "GREMLIN_HORN",
            new RelicLabelInfo(
                "Gained [blue]{0}[/blue] [gold]Energy[/gold].\nDrew [blue]{0}[/blue] cards.",
                1
            )
        },
        {
            "HAND_DRILL",
            new RelicLabelInfo("Applied [blue]{0}[/blue] [gold]Vulnerable[/gold].", 2)
        },
        {
            "HELICAL_DART",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Dexterity[/gold].", 1)
        },
        { "HISTORY_COURSE", new RelicLabelInfo("Played [blue]{0}[/blue] copies.", 1) },
        { "HORN_CLEAT", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 14) },
        { "IRON_CLUB", new RelicLabelInfo("Drew [blue]{0}[/blue] additional cards.", 1) },
        { "IVORY_TILE", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 1) },
        { "JOSS_PAPER", new RelicLabelInfo("Drew [blue]{0}[/blue] additional cards.", 1) },
        { "KUNAI", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Dexterity[/gold].", 1) },
        { "LANTERN", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 1) },
        {
            "LASTING_CANDY",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Power[/gold] card rewards.", 1)
        },
        { "MUSIC_BOX", new RelicLabelInfo("Created [blue]{0}[/blue] [gold]Ethereal[/gold] copies.", 1) },
        { "NINJA_SCROLL", new RelicLabelInfo("Created [blue]{0}[/blue] [gold]Shivs[/gold].", 3) },
        { "NUNCHAKU", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 1) },
        {
            "ODDLY_SMOOTH_STONE",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Dexterity[/gold].", 1)
        },
        { "ORNAMENTAL_FAN", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 4) },
        { "PAELS_FLESH", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 1) },
        { "PANTOGRAPH", new RelicLabelInfo("Healed [blue]{0}[/blue] [gold]HP[/gold].", 25) },
        { "PENDULUM", new RelicLabelInfo("Drew [blue]{0}[/blue] additional cards.", 1) },
        { "PERMAFROST", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 7) },
        {
            "PETRIFIED_TOAD",
            new RelicLabelInfo("Procured [blue]{0}[/blue] [gold]Potion-Shaped Rocks[/gold].", 1)
        },
        { "PLANISPHERE", new RelicLabelInfo("Healed [blue]{0}[/blue] [gold]HP[/gold].", 5) },
        {
            "RAINBOW_RING",
            new RelicLabelInfo(
                "Gained [blue]{0}[/blue] [gold]Strength[/gold].\nGained [blue]{0}[/blue] [gold]Dexterity[/gold].",
                1
            )
        },
        {
            "REPTILE_TRINKET",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].", 3)
        },
        { "RIPPLE_BASIN", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 4) },
        {
            "RUNIC_CAPACITOR",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Orb[/gold] slots.", 3)
        },
        { "SAI", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Block[/gold].", 7) },
        { "SHURIKEN", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].", 1) },
        {
            "SWORD_OF_JADE",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].", 3)
        },
        {
            "TUNGSTEN_ROD",
            new RelicLabelInfo(
                "Reduced [orange]damage[/orange] by [blue]{0}[/blue] [gold]HP[/gold].",
                1
            )
        },
        {
            "VENERABLE_TEA_SET",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 2)
        },
        { "VEXING_PUZZLEBOX", new RelicLabelInfo("Created [blue]{0}[/blue] colorless cards.", 1) },
        {
            "FAKE_VENERABLE_TEA_SET",
            new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Energy[/gold].", 1)
        },
        {"SPARKLING_ROUGE", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].\nGained [blue]{0}[/blue] [gold]Dexterity[/gold].", 1) },
        {"CENTENNIAL_PUZZLE", new RelicLabelInfo("Drew [blue]{0}[/blue] cards.", 3) },
        {"STONE_CRACKER", new RelicLabelInfo("Upgraded [blue]{0}[/blue] cards.", 2) },
        {"VAJRA", new RelicLabelInfo("Gained [blue]{0}[/blue] [gold]Strength[/gold].", 1) },
        {"UNCEASING_TOP", new RelicLabelInfo("Drew [blue]{0}[/blue] cards.", 1) },
    };

    public static string GetAlternateLabel(string relicId, int value)
    {
        if (relicLabelMappings.TryGetValue(relicId, out RelicLabelInfo labelInfo))
        {
            int displayValue = DefaultValueFormatter(value, labelInfo.multiplier);
            return string.Format(labelInfo.label, displayValue);
        }

        return "";
    }

    private static int DefaultValueFormatter(int rawValue, int factor)
    {
        return rawValue * factor;
    }

    private struct RelicLabelInfo
    {
        public string label; //The new label to show: "Healing Gained:[blue]{0}[/blue]"
        public int multiplier; //A simple value to multiply the raw stat by before showing it to the player.

        public RelicLabelInfo(string label, int multiplier)
        {
            this.label = label;
            this.multiplier = multiplier;
        }
    }
}
