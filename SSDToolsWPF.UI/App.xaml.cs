// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.IO;
using System.Windows;

namespace SSDToolsWPF.UI;

public partial class App : Application
{
    private static readonly string LogFilePath = @"C:\SSDTools_Startup.log";

    public App()
    {
        // Log startup
        LogStartup("Application constructor started");

        // Hook into unhandled exceptions
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

        LogStartup("Exception handlers registered");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            LogStartup("OnStartup called");
            LogStartup($"Command line args: {string.Join(", ", e.Args)}");
            LogStartup($"Current directory: {Environment.CurrentDirectory}");
            LogStartup($"Is elevated: {IsElevated()}");

            base.OnStartup(e);
            LogStartup("OnStartup completed successfully");
        }
        catch (Exception ex)
        {
            LogException("OnStartup", ex);
            MessageBox.Show($"Startup error: {ex.Message}\n\nSee log file: {LogFilePath}", 
                "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            throw;
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        LogStartup($"OnExit called with exit code: {e.ApplicationExitCode}");
        base.OnExit(e);
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        LogException("DispatcherUnhandledException", e.Exception);
        MessageBox.Show($"Unhandled UI exception: {e.Exception.Message}\n\nSee log file: {LogFilePath}", 
            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true; // Prevent app from crashing
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        LogException("UnhandledException", ex ?? new Exception("Unknown exception"));
        if (e.IsTerminating)
        {
            LogStartup("Application is terminating due to unhandled exception");
        }
    }

    private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        LogException("UnobservedTaskException", e.Exception);
        e.SetObserved(); // Prevent app from crashing
    }

    private static void LogStartup(string message)
    {
        try
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [INFO] {message}";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // If we can't write to C:\, try temp folder
            try
            {
                string tempLog = Path.Combine(Path.GetTempPath(), "SSDTools_Startup.log");
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [INFO] {message}";
                File.AppendAllText(tempLog, logEntry + Environment.NewLine);
            }
            catch
            {
                // Ignore logging errors
            }
        }
    }

    private static void LogException(string context, Exception ex)
    {
        try
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [ERROR] {context}: {ex.GetType().Name}: {ex.Message}\n" +
                          $"StackTrace: {ex.StackTrace}\n" +
                          $"InnerException: {ex.InnerException?.Message}\n";
            File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // If we can't write to C:\, try temp folder
            try
            {
                string tempLog = Path.Combine(Path.GetTempPath(), "SSDTools_Startup.log");
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [ERROR] {context}: {ex.GetType().Name}: {ex.Message}\n" +
                              $"StackTrace: {ex.StackTrace}\n" +
                              $"InnerException: {ex.InnerException?.Message}\n";
                File.AppendAllText(tempLog, logEntry + Environment.NewLine);
            }
            catch
            {
                // Ignore logging errors
            }
        }
    }

    private static bool IsElevated()
    {
        try
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }
}