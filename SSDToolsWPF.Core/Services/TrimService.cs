// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Diagnostics;
using SSDToolsWPF.Core.Models;

namespace SSDToolsWPF.Core.Services;

public class TrimService
{
    private readonly LoggingService _log;

    public TrimService(LoggingService log)
    {
        _log = log;
    }

    public void CheckTrimStatus()
    {
        _log.Log("0% - Starting TRIM status check...");
        Thread.Sleep(500);

        try
        {
            _log.Log("25% - Preparing fsutil command...");
            Thread.Sleep(500);

            var psi = new ProcessStartInfo
            {
                FileName = "fsutil.exe",
                Arguments = "behavior query DisableDeleteNotify",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            _log.Log("50% - Executing fsutil query...");
            Thread.Sleep(500);

            using var proc = Process.Start(psi);
            if (proc == null)
            {
                _log.Log("Failed to start fsutil.");
                return;
            }

            _log.Log("75% - Reading results...");
            Thread.Sleep(500);

            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            if (!string.IsNullOrWhiteSpace(output))
            {
                foreach (var line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    _log.Log(line);
            }

            if (!string.IsNullOrWhiteSpace(error))
                _log.Log("fsutil error: " + error);

            _log.Log("100% - TRIM status check complete.");
        }
        catch (Exception ex)
        {
            _log.Log($"TRIM status query failed: {ex.Message}");
        }
    }

    public List<DriveInfoModel> GetFixedDrives()
    {
        var list = new List<DriveInfoModel>();
        foreach (var di in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.IsReady))
        {
            list.Add(new DriveInfoModel
            {
                Letter = di.Name.TrimEnd('\\').TrimEnd(':'),
                FileSystem = "Unknown",
                Label = di.VolumeLabel,
                Health = "Unknown"
            });
        }
        return list;
    }

    public void RunTrimOnDrive(string letter)
    {
        letter = letter.Trim().TrimEnd(':', '\\').ToUpperInvariant();
        _log.Log($"TRIM on drive {letter} started.");

        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -Command \"Optimize-Volume -DriveLetter {letter} -ReTrim -Verbose\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi);
            if (proc == null)
            {
                _log.Log("Failed to start powershell.exe for TRIM.");
                return;
            }

            // Read output streams line-by-line in real-time
            var outputTask = Task.Run(() =>
            {
                while (!proc.StandardOutput.EndOfStream)
                {
                    string? line = proc.StandardOutput.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        _log.Log(line);
                }
            });

            var errorTask = Task.Run(() =>
            {
                while (!proc.StandardError.EndOfStream)
                {
                    string? line = proc.StandardError.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        _log.Log(line); // PowerShell verbose output goes to stderr
                }
            });

            proc.WaitForExit();
            Task.WaitAll(outputTask, errorTask);

            _log.Log($"TRIM on drive {letter} completed with exit code {proc.ExitCode}.");
        }
        catch (Exception ex)
        {
            _log.Log($"TRIM on drive {letter} failed: {ex.Message}");
        }
    }

    public void RunTrimOnAllDrives()
    {
        var drives = GetFixedDrives();
        if (!drives.Any())
        {
            _log.Log("No fixed drives found for TRIM.");
            return;
        }

        int driveIndex = 0;
        foreach (var d in drives)
        {
            driveIndex++;
            _log.Log($"Processing drive {driveIndex} of {drives.Count}...");
            RunTrimOnDrive(d.Letter);
        }
    }
}