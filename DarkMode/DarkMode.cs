using System;

public static partial class DarkMode
{
  static PlatformID pid;
  static bool initialized = false;
  static bool isWindows;
  static bool isLinux;

  static void Initialize()
  {
    pid = Environment.OSVersion.Platform;
    isWindows = false;
    isLinux = false;
    if(pid == PlatformID.Win32NT) isWindows = true;
    if(pid == PlatformID.Win32S) isWindows = true;
    if(pid == PlatformID.Win32Windows) isWindows = true;
    if(pid == PlatformID.WinCE) isWindows = true;
    if(pid == PlatformID.Unix) isLinux = true;
    if(isWindows) isLinux = false;
    initialized = true;
  }

  public static bool IsAppsDarkMode()
  {
    if(!initialized) Initialize();
    if(isWindows) return DarkModeWindows.IsAppsDarkMode();
    if(isLinux) return DarkModeLinux.IsAppsDarkMode();
    return false;
  }

  public static bool IsSystemDarkMode()
  {
    if(!initialized) Initialize();
    if(isWindows) return DarkModeWindows.IsSystemDarkMode();
    if(isLinux) return DarkModeLinux.IsSystemDarkMode();
    return false;
  }

  public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
  {
    if(!initialized) Initialize();
    if(isWindows) return DarkModeWindows.UseImmersiveDarkMode(handle, enabled);
    if(isLinux) return DarkModeLinux.UseImmersiveDarkMode(handle, enabled);
    return false;
  }
}