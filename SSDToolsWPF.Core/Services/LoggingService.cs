// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Text;

namespace SSDToolsWPF.Core.Services;

public class LoggingService
{
    private readonly string _logFilePath;
    private readonly Action<string>? _uiCallback;
    private readonly object _lockObject = new();

    public LoggingService(string baseFolder, Action<string>? uiCallback = null)
    {
        Directory.CreateDirectory(baseFolder);
        _logFilePath = Path.Combine(baseFolder, "SSD_Trim_Log.txt");
        _uiCallback = uiCallback;
    }

    public void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var line = $"{timestamp}\t{message}";

        lock (_lockObject)
        {
            try
            {
                // Use FileShare.ReadWrite to allow other processes to access the file
                using var stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                using var writer = new StreamWriter(stream, Encoding.UTF8);
                writer.WriteLine(line);
            }
            catch (IOException)
            {
                // If file is still locked, retry once after a short delay
                Thread.Sleep(50);
                try
                {
                    using var stream = new FileStream(_logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    using var writer = new StreamWriter(stream, Encoding.UTF8);
                    writer.WriteLine(line);
                }
                catch
                {
                    // If still fails, silently continue - don't crash the app
                }
            }
        }

        _uiCallback?.Invoke(line);
    }

    public string LogFilePath => _logFilePath;
}