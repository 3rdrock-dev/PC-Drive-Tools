// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SSDToolsWPF.Core.Models;
using SSDToolsWPF.Core.Services;

namespace SSDToolsWPF.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly LoggingService _log;
    private readonly DefragService _defrag;
    private readonly TrimService _trim;
    private readonly TaskSchedulerService _tasks;

    public ObservableCollection<DriveInfoModel> Drives { get; } = new();

    private DriveInfoModel? _selectedDrive;

    public DriveInfoModel? SelectedDrive
    {
        get => _selectedDrive;
        set { _selectedDrive = value; OnPropertyChanged(); }
    }

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private string _busyMessage = string.Empty;

    public string BusyMessage
    {
        get => _busyMessage;
        set { _busyMessage = value; OnPropertyChanged(); }
    }

    private string _progressPercentage = string.Empty;

    public string ProgressPercentage
    {
        get => _progressPercentage;
        set { _progressPercentage = value; OnPropertyChanged(); }
    }

    public MainViewModel(
        LoggingService log,
        DefragService defrag,
        TrimService trim,
        TaskSchedulerService tasks)
    {
        _log = log;
        _defrag = defrag;
        _trim = trim;
        _tasks = tasks;

        RefreshDrives();
    }

    public async Task RefreshDrivesAsync()
    {
        IsBusy = true;
        BusyMessage = "Refreshing drives...";
        ProgressPercentage = string.Empty;

        try
        {
            // Get drives on background thread
            var drives = await Task.Run(() => _trim.GetFixedDrives());

            // Update collection on UI thread
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Drives.Clear();
                foreach (var d in drives)
                    Drives.Add(d);
            });

            _log.Log("Drive list refreshed.");
        }
        finally
        {
            IsBusy = false;
            BusyMessage = string.Empty;
            ProgressPercentage = string.Empty;
        }
    }

    public void RefreshDrives() => _ = RefreshDrivesAsync();

    public async Task RepairServiceAsync()
    {
        await RunWithBusyIndicatorAsync("Repairing defrag service...", () =>
        {
            _defrag.Repair();
        });
    }

    public void RepairService() => _ = RepairServiceAsync();

    public async Task CheckTrimStatusAsync()
    {
        await RunWithBusyIndicatorAsync("Checking TRIM status...", () =>
        {
            _trim.CheckTrimStatus();
        });
    }

    public void CheckTrimStatus() => _ = CheckTrimStatusAsync();

    public async Task TestTrimAllAsync()
    {
        await RunWithBusyIndicatorAsync("Running TRIM on all drives...", () =>
        {
            _trim.RunTrimOnAllDrives();
        });
    }

    public void TestTrimAll() => _ = TestTrimAllAsync();

    public async Task SetupMonthlyTasksAsync()
    {
        await RunWithBusyIndicatorAsync("Setting up monthly tasks...", () =>
        {
            var driveList = Drives.ToList();
            int totalCount = driveList.Count;
            int currentIndex = 0;

            foreach (var d in driveList)
            {
                currentIndex++;
                _tasks.CreateMonthlyTrimTask(d.Letter, currentIndex, totalCount);
            }
        });
    }

    public void SetupMonthlyTasks() => _ = SetupMonthlyTasksAsync();

    public async Task ManualTrimAsync()
    {
        if (SelectedDrive == null)
        {
            _log.Log("Manual TRIM: no drive selected.");
            return;
        }

        await RunWithBusyIndicatorAsync($"Running TRIM on drive {SelectedDrive.Letter}...", () =>
        {
            _trim.RunTrimOnDrive(SelectedDrive.Letter);
        });
    }

    public void ManualTrim() => _ = ManualTrimAsync();

    public async Task ShowStatusSummaryAsync()
    {
        await RunWithBusyIndicatorAsync("Checking status...", () =>
        {
            _log.Log("Status summary requested.");
            _defrag.Repair();
            _trim.CheckTrimStatus();
        });
    }

    public void ShowStatusSummary() => _ = ShowStatusSummaryAsync();

    private async Task RunWithBusyIndicatorAsync(string message, Action action)
    {
        IsBusy = true;
        BusyMessage = message;
        ProgressPercentage = string.Empty;

        try
        {
            await Task.Run(action);
        }
        finally
        {
            IsBusy = false;
            BusyMessage = string.Empty;
            ProgressPercentage = string.Empty;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}