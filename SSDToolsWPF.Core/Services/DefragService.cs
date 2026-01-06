// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Diagnostics;
using System.ServiceProcess;

namespace SSDToolsWPF.Core.Services;

public class DefragService
{
    private readonly LoggingService _log;

    public DefragService(LoggingService log)
    {
        _log = log;
    }

    public void Repair()
    {
        _log.Log("0% - Checking Optimize Drives service (defragsvc)...");
        Thread.Sleep(500); // 0.5 second delay

        ServiceController? svc;
        try
        {
            svc = new ServiceController("defragsvc");
        }
        catch (Exception ex)
        {
            _log.Log($"defragsvc not found: {ex.Message}");
            return;
        }

        _log.Log($"20% - Current defragsvc status: {svc.Status}");
        Thread.Sleep(500);

        try
        {
            _log.Log("40% - Configuring service start type...");
            Thread.Sleep(500);
            
            var psi = new ProcessStartInfo
            {
                FileName = "sc.exe",
                Arguments = "config defragsvc start= demand",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process.Start(psi)?.WaitForExit();
            
            _log.Log("60% - Set defragsvc start type to 'demand' (Manual).");
            Thread.Sleep(500);
        }
        catch (Exception ex)
        {
            _log.Log($"Failed to set defragsvc start type: {ex.Message}");
        }

        _log.Log("70% - Refreshing service status...");
        Thread.Sleep(500);
        
        svc.Refresh();
        
        if (svc.Status != ServiceControllerStatus.Running)
        {
            try
            {
                _log.Log("80% - Starting defragsvc...");
                Thread.Sleep(500);
                
                svc.Start();
                svc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                _log.Log($"Failed to start defragsvc: {ex.Message}");
            }
        }

        _log.Log("95% - Final status check...");
        Thread.Sleep(500);
        
        svc.Refresh();
        
        _log.Log($"100% - defragsvc repair complete. Status: {svc.Status}");
    }
}