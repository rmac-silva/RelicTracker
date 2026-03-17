using System.Text.Json;
using Godot;

public static class RelicStatCache
{
    private static int _currentRunId = 0; // You can increment this at the start of each run to keep separate files
    private static readonly string SavePath = Path.Combine(OS.GetUserDataDir(), "RelicTracker");
    private static Dictionary<string, string> _cache;
    private static readonly object _lock = new();

    // Ensures we don't crash even if someone forgets to call Initialize()
    private static void EnsureInitialized()
    {
        if (_cache != null)
            return;
        else
        {
            _cache = new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// Increments the trigger count for a given relic
    /// </summary>
    /// <param name="id"></param>
    /// <param name="statName"></param>
    public static void RecordTriggerStat(string id)
    {
        ModLog.Info($"[Trigger] \nRecording trigger stat for {id}, key: {id}_UsageCount\n");
        EnsureInitialized();
        string key = $"{id}_UsageCount";

        lock (_lock)
        {
            if (_cache.ContainsKey(key))
                _cache[key] = (_cache[key].ToInt() + 1).ToString();
            else
                _cache[key] = "1";
        }

        SaveToDebugCache();
    }

    public static void RecordCustomStat(string id, string stringFormat, List<int> values)
    {
        ModLog.Info($"[Custom Stat] \nRecording custom stat for {id}: {stringFormat} with values {string.Join(", ", values)}\n");
        EnsureInitialized();
        string key = $"{id}_CustomStat";

        lock (_lock)
        {
            if (_cache.ContainsKey(key))
            {
                //Just update the values
                for (int i = 0; i < values.Count; i++)
                {
                    _cache[key + $"_Value{i}"] = (
                        _cache[key + $"_Value{i}"].ToInt() + values[i]
                    ).ToString();
                }
            }
            else
            {
                //Create an entry with the string format
                _cache[key] = stringFormat;

                //Register the values, that will later replace the string
                //Replace {0} and {1}... with the values in the list
                for (int i = 0; i < values.Count; i++)
                {
                    _cache[key + $"_Value{i}"] = values[i].ToString();
                }
            }
        }
        SaveToDebugCache();
    }

    public static bool HasStatsForRelic(string id)
    {
        EnsureInitialized();
        string keyOne = $"{id}_UsageCount";
        string keyTwo = $"{id}_CustomStat";
        lock (_lock)
        {
            return _cache.ContainsKey(keyOne) || _cache.ContainsKey(keyTwo);
        }
    }

    public static int GetTriggeredCount(string id)
    {

        EnsureInitialized();
        string key = $"{id}_UsageCount";
        lock (_lock)
        {
            return _cache.TryGetValue(key, out string value) ? value.ToInt() : 0;
        }
    }

    public static string GetDetailedStats(string id)
    {
        EnsureInitialized();
        string key = $"{id}_CustomStat";
        lock (_lock)
        {
            string stringFormat = _cache.TryGetValue(key, out string value) ? value : "";

            //Search for {0} and {1} replacing them with key_Value0 and key_Value1
            for (int i = 0; i < 10; i++)
            {
                //Search for the value in the _cache
                if (!_cache.TryGetValue(key + $"_Value{i}", out string v))
                {
                    break;
                }

                stringFormat = stringFormat.Replace($"{{{i}}}", v);
            }

            return stringFormat;
        }
    }

    private static void SaveToDebugCache()
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(Path.Combine(SavePath, $"run_{_currentRunId}")) ?? SavePath
        );
        string json = JsonSerializer.Serialize(_cache);
        File.WriteAllText(Path.Combine(SavePath, $"run_{_currentRunId}.json"), json);
    }

    public static int SetRunId(int newRunId)
    {
        _currentRunId = newRunId;
        return _currentRunId;
    }

    public static void InitializeForNewRun()
    {
        ModLog.Info($"Initializing cache for new run. Current run id: {_currentRunId}\n");
        WipeOldCache();
        
        _currentRunId++;


        _cache = new Dictionary<string, string>();
        SaveToDebugCache();
    }

    public static void WipeOldCache()
    {
        string filePath = Path.Combine(SavePath, $"run_{_currentRunId}.json");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    #region - Saving & Loading

    public static void LoadCacheFromSingleplayerSave()
    {
        string filePath = Path.Combine(SavePath, $"singleplayer_save.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _cache = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        else
        {
            ModLog.Error($"Singleplayer save file not found at {filePath}. Starting with an empty cache.",new FileNotFoundException("Save file not foudn"));
            _cache = new Dictionary<string, string>();
        }
    }
    public static void LoadCacheFromMultiplayerSave()
    {
        string filePath = Path.Combine(SavePath, $"multiplayer_save.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _cache = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        else
        {
            ModLog.Error($"Multiplayer save file not found at {filePath}. Starting with an empty cache.",new FileNotFoundException("Save file not foudn"));
            _cache = new Dictionary<string, string>();
        }
    }

    private static void LoadCacheFromDebugFile()
    {
        string filePath = Path.Combine(SavePath, $"run_{_currentRunId}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _cache = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        else
        {
            ModLog.Error($"Debug cache file not found at {filePath}. Starting with an empty cache.",new FileNotFoundException("Debug cache file not found"));
            _cache = new Dictionary<string, string>();
        }
    }

    public static void SaveCache(bool multiplayerSave)
    {
        LoadCacheFromDebugFile();
        
        string fileName = multiplayerSave ? "multiplayer_save.json" : "singleplayer_save.json";
        Directory.CreateDirectory(SavePath);
        string json = JsonSerializer.Serialize(_cache);
        File.WriteAllText(Path.Combine(SavePath, fileName), json);
    }

    #endregion
}