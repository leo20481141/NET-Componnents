using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public static partial class DarkMode {
    private static class DarkModeWindows
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int attr,
            ref int attrValue,
            int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private const string PERSONALIZE_KEY =
            @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string APPS_USE_LIGHT_THEME = "AppsUseLightTheme";
        private const string SYSTEM_USES_LIGHT_THEME = "SystemUsesLightTheme";


        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            int useDarkMode = enabled ? 1 : 0;

            int result = DwmSetWindowAttribute(
                handle,
                DWMWA_USE_IMMERSIVE_DARK_MODE,
                ref useDarkMode,
                sizeof(int));

            if (result != 0)
            {
                result = DwmSetWindowAttribute(
                    handle,
                    DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1,
                    ref useDarkMode,
                    sizeof(int));
            }

            return result == 0;
        }


        public static bool IsAppsDarkMode()
        {
            return IsDarkModeRegistryValue(APPS_USE_LIGHT_THEME);
        }


        public static bool IsSystemDarkMode()
        {
            return IsDarkModeRegistryValue(SYSTEM_USES_LIGHT_THEME);
        }


        private static bool IsDarkModeRegistryValue(string valueName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(PERSONALIZE_KEY);

            if (key == null)
                return false;

            object value = key.GetValue(valueName);

            key.Close();

            if (value == null)
                return false;

            if (!(value is int))
                return false;

            int useLightTheme = (int)value;

            return useLightTheme == 0;
        }
    }
}