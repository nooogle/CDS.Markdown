using System.Runtime.InteropServices;

namespace Demo;

/// <summary>
/// Provides system and application information as a formatted string.
/// </summary>
public static class SystemInfoHelper
{
    /// <summary>
    /// Gets a string containing information about the system and application.
    /// </summary>
    public static string GetSystemInfo()
    {
        var appName = Application.ProductName;
        var appVersion = Application.ProductVersion.Split('+')[0];
        var appBitDepth = Environment.Is64BitProcess ? "64-bit" : "32-bit";
        var appArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
        var appFramework = RuntimeInformation.FrameworkDescription;
        var osVersion = Environment.OSVersion.VersionString;
        var osBitDepth = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
        var osArchitecture = RuntimeInformation.OSArchitecture.ToString();

        return $"Application: {appName} [{appVersion}] " +
               $"running as {appBitDepth} {appArchitecture} " +
               $"using {appFramework} " +
               $"on {osVersion} {osBitDepth} and {osArchitecture} processor";
    }
}
