// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using SSDToolsWPF.Core.Services;
using SSDToolsWPF.UI.ViewModels;

namespace SSDToolsWPF.UI;

public partial class MainWindow : Window
{
    private readonly LoggingService _logging;
    private readonly DefragService _defrag;
    private readonly TrimService _trim;
    private readonly TaskSchedulerService _tasks;
    private readonly MainViewModel _vm;
    private static readonly Regex PercentageRegex = new(@"(\d+)\s*%", RegexOptions.Compiled);
    private static readonly Regex VerboseRetrimRegex = new(@"VERBOSE:\s*Retrim:\s*(\d+)%\s*complete", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex VerboseRetrimOfRegex = new(@"VERBOSE:\s*Retrim\s+of\s+([A-Z]:?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    
    private DateTime _lastProgressUpdate = DateTime.MinValue;
    private readonly TimeSpan _progressThrottle = TimeSpan.FromMilliseconds(500); // 0.5 seconds
    private int _lastPercentage = -1;

    public MainWindow()
    {
        InitializeComponent();

        // Set window icon from PNG resource
        try
        {
            var iconUri = new Uri("pack://application:,,,/Resource/ssd_drive.png", UriKind.Absolute);
            var iconStream = Application.GetResourceStream(iconUri);
            if (iconStream != null)
            {
                var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = iconStream.Stream;
                bitmapImage.EndInit();
                Icon = bitmapImage;
            }
        }
        catch { /* Icon file not found, continue without icon */ }

        // Set taskbar icon from ICO resource
        try
        {
            var icoUri = new Uri("pack://application:,,,/Resource/favicon.ico", UriKind.Absolute);
            var icoStream = Application.GetResourceStream(icoUri);
            if (icoStream != null)
            {
                var iconBitmapDecoder = new System.Windows.Media.Imaging.IconBitmapDecoder(
                    icoStream.Stream,
                    System.Windows.Media.Imaging.BitmapCreateOptions.None,
                    System.Windows.Media.Imaging.BitmapCacheOption.Default);
                Icon = iconBitmapDecoder.Frames[0];
            }
        }
        catch { /* ICO file not found or invalid, use PNG */ }

        string baseFolder = @"D:\PCTools";

        _logging = new LoggingService(baseFolder, AppendLogLine);
        _defrag = new DefragService(_logging);
        _trim = new TrimService(_logging);
        _tasks = new TaskSchedulerService(_logging, baseFolder);

        _vm = new MainViewModel(_logging, _defrag, _trim, _tasks);
        DataContext = _vm;

        _logging.Log("SSDToolsWPF.UI started.");
    }

    private void AppendLogLine(string line)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => AppendLogLine(line));
            return;
        }

        // Append to log first so we can see what we're receiving
        LogTextBox.AppendText(line + Environment.NewLine);
        LogTextBox.CaretIndex = LogTextBox.Text.Length;
        LogTextBox.ScrollToEnd();

        // Check for VERBOSE Retrim messages first (highest priority)
        var verboseRetrimMatch = VerboseRetrimRegex.Match(line);
        if (verboseRetrimMatch.Success)
        {
            int percentage = int.Parse(verboseRetrimMatch.Groups[1].Value);
            
            // Throttle updates to 0.5 seconds minimum between percentage changes
            var now = DateTime.Now;
            if (percentage != _lastPercentage && (now - _lastProgressUpdate) >= _progressThrottle)
            {
                _vm.ProgressPercentage = percentage + "%";
                _lastPercentage = percentage;
                _lastProgressUpdate = now;
            }
            else if (percentage != _lastPercentage)
            {
                // Queue the update after throttle delay
                var delay = _progressThrottle - (now - _lastProgressUpdate);
                if (delay > TimeSpan.Zero)
                {
                    Task.Delay(delay).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            if (percentage != _lastPercentage) // Check again in case it changed
                            {
                                _vm.ProgressPercentage = percentage + "%";
                                _lastPercentage = percentage;
                                _lastProgressUpdate = DateTime.Now;
                            }
                        });
                    });
                }
            }
        }
        
        // Check for "VERBOSE: Retrim of X:" to update busy message
        var verboseRetrimOfMatch = VerboseRetrimOfRegex.Match(line);
        if (verboseRetrimOfMatch.Success && _vm.IsBusy)
        {
            string driveLetter = verboseRetrimOfMatch.Groups[1].Value;
            _vm.BusyMessage = $"TRIM of drive {driveLetter}";
        }

        // Fallback: Extract any percentage if VERBOSE pattern didn't match
        if (!verboseRetrimMatch.Success)
        {
            var match = PercentageRegex.Match(line);
            if (match.Success)
            {
                _vm.ProgressPercentage = match.Groups[1].Value + "%";
            }
        }
    }

    private void BtnRepairService_Click(object sender, RoutedEventArgs e) => _vm.RepairService();

    private void BtnCheckTrim_Click(object sender, RoutedEventArgs e) => _vm.CheckTrimStatus();

    private void BtnTestAll_Click(object sender, RoutedEventArgs e) => _vm.TestTrimAll();

    private void BtnSetupTasks_Click(object sender, RoutedEventArgs e) => _vm.SetupMonthlyTasks();

    private void BtnManualTrim_Click(object sender, RoutedEventArgs e) => _vm.ManualTrim();

    private void BtnStatus_Click(object sender, RoutedEventArgs e) => _vm.ShowStatusSummary();

    private void BtnRefreshDrives_Click(object sender, RoutedEventArgs e) => _vm.RefreshDrives();

    private void BtnExit_Click(object sender, RoutedEventArgs e) => Close();

    private void BtnAbout_Click(object sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow
        {
            Owner = this
        };
        aboutWindow.ShowDialog();
    }
}