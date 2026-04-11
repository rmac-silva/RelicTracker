// RelicTracker.ModLog - Mod Logger borrowed from BetterSpire2
using Godot;

public static class ModLog
{
    private static string? _logPath;

    private static readonly object _lock = new object();

    private static string LogPath =>
        _logPath
        ?? (_logPath = Path.Combine(OS.GetUserDataDir(), "RelicTracker", "relicStats.log"));

    public static void Init()
    {
        try
        {
            File.WriteAllText(
                LogPath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] RelicTracker log started\n  OS: {OS.GetName()} / {OS.GetDistributionName()}\n  Godot: {Engine.GetVersionInfo()["string"]}\n"
            );
        }
        catch { }
    }

    public static void Info(string message)
    {
        try
        {
            lock (_lock)
            {
                File.AppendAllText(LogPath, $"[{DateTime.Now:HH:mm:ss}] [INFO] {message}\n");
            }
        }
        catch { }
    }

    public static void Error(string context, Exception ex)
    {
        try
        {
            lock (_lock)
            {
                File.AppendAllText(
                    LogPath,
                    $"[{DateTime.Now:HH:mm:ss}] [ERROR] in {context}: {ex}\n"
                );
            }
        }
        catch { }
    }

    public static void Warning(string message)
    {
        try
        {
            lock (_lock)
            {
                File.AppendAllText(LogPath, $"[{DateTime.Now:HH:mm:ss}] [WARNING] {message}\n");
            }
        }
        catch { }
    }
}
