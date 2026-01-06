// Developed for 3rdRock by Jim Barber (January 6, 2026)

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSDToolsWPF.Core.Services;

namespace SSDToolsWPF.Tests.CoreTests;

[TestClass]
public class TrimServiceTests
{
    [TestMethod]
    public void GetFixedDrives_DoesNotThrow()
    {
        // Arrange
        var log = new LoggingService(@"C:\Temp\SSDToolsTests");
        var trim = new TrimService(log);

        // Act
        var drives = trim.GetFixedDrives();

        // Assert
        Assert.IsNotNull(drives);
    }
}