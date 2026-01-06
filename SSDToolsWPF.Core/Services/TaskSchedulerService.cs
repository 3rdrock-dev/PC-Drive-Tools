// Developed for 3rdRock by Jim Barber (January 6, 2026)

using Microsoft.Win32.TaskScheduler;

namespace SSDToolsWPF.Core.Services;

public class TaskSchedulerService
{
    private readonly LoggingService _log;
    private readonly string _baseFolder;
    private readonly string _wrapperFolder;

    public TaskSchedulerService(LoggingService log, string baseFolder)
    {
        _log = log;
        _baseFolder = baseFolder;
        _wrapperFolder = Path.Combine(baseFolder, "TrimTasks");
        Directory.CreateDirectory(_wrapperFolder);
    }

    public void CreateMonthlyTrimTask(string driveLetter, int currentIndex, int totalCount)
    {
        driveLetter = driveLetter.Trim().TrimEnd(':', '\\').ToUpperInvariant();
        
        int baseProgress = (currentIndex - 1) * 100 / totalCount;
        int step = 100 / totalCount;
        
        _log.Log($"{baseProgress}% - Setting up task for drive {driveLetter}...");
        Thread.Sleep(300);
        
        string wrapperPath = Path.Combine(_wrapperFolder, $"Trim_{driveLetter}.ps1");
        string logFile = Path.Combine(_baseFolder, "SSD_Trim_Log.txt");

        _log.Log($"{baseProgress + step / 4}% - Creating PowerShell script...");
        Thread.Sleep(300);

        string content =
            $@"$timestamp = Get-Date -Format ""yyyy-MM-dd HH:mm:ss""
            $line = ""$timestamp`tTRIM run on drive {driveLetter}""
            Add-Content -Path ""{logFile}"" -Value $line
            Optimize-Volume -DriveLetter {driveLetter} -ReTrim -Verbose | Out-File -Append ""{logFile}""";

        File.WriteAllText(wrapperPath, content);

        _log.Log($"{baseProgress + step / 2}% - Configuring scheduled task...");
        Thread.Sleep(300);

        using var ts = new TaskService();
        var td = ts.NewTask();
        td.RegistrationInfo.Description = $"Monthly SSD TRIM on drive {driveLetter}";

        var trigger = new MonthlyTrigger { DaysOfMonth = new[] { 1 } };
        td.Triggers.Add(trigger);

        string args = $"-NoProfile -ExecutionPolicy Bypass -File \"{wrapperPath}\"";
        td.Actions.Add(new ExecAction("powershell.exe", args, null));

        _log.Log($"{baseProgress + (step * 3) / 4}% - Registering task...");
        Thread.Sleep(300);

        string taskName = $"MonthlySSDTrim_{driveLetter}";
        ts.RootFolder.RegisterTaskDefinition(
            taskName,
            td,
            TaskCreation.CreateOrUpdate,
            "SYSTEM",
            null,
            TaskLogonType.ServiceAccount);

        int endProgress = currentIndex * 100 / totalCount;
        _log.Log($"{endProgress}% - Task {taskName} created for drive {driveLetter}.");
        Thread.Sleep(300);
    }
}