using System;
using System.Diagnostics;
using System.IO;

public static partial class DarkMode {
    private static class DarkModeLinux
    {
        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            return false;
        }

        public static bool IsAppsDarkMode()
        {
            return IsDarkMode();
        }

        public static bool IsSystemDarkMode()
        {
            return IsDarkMode();
        }

        private static bool IsDarkMode()
        {
            // First try dconf's GTK theme.
            string gtkTheme = GetDconfValue("gtk-theme");

            if (!string.IsNullOrEmpty(gtkTheme))
            {
                return EndsWithDark(gtkTheme);
            }

            // Then check the GTK configuration files.
            string theme = GetThemeFromConfigurationFile(
                Path.Combine(GetHomeDirectory(), ".config/gtk-4.0/settings.ini"));

            if (string.IsNullOrEmpty(theme))
            {
                theme = GetThemeFromConfigurationFile(
                    Path.Combine(GetHomeDirectory(), ".config/gtk-3.0/settings.ini"));
            }

            if (!string.IsNullOrEmpty(theme))
            {
                return EndsWithDark(theme);
            }

            // Finally, try the freedesktop color-scheme preference.
            string colorScheme = GetDconfValue("color-scheme");

            if (colorScheme == "prefer-dark")
            {
                return true;
            }

            return false;
        }

        private static string GetDconfValue(string key)
        {
            Process process = new Process();

            process.StartInfo.FileName = "dconf";
            process.StartInfo.Arguments =
                "read /org/gnome/desktop/interface/" + key;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            try
            {
                if (!process.Start())
                {
                    process.Dispose();
                    return null;
                }

                string value = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Dispose();

                if (value == null)
                    return null;

                value = value.Trim();

                // dconf returns strings surrounded by single quotes.
                if (value.Length >= 2 &&
                    value[0] == '\'' &&
                    value[value.Length - 1] == '\'')
                {
                    value = value.Substring(1, value.Length - 2);
                }

                return value;
            }
            catch
            {
                process.Dispose();
                return null;
            }
        }

        private static string GetThemeFromConfigurationFile(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                string[] lines = File.ReadAllLines(path);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    if (line.StartsWith("gtk-theme-name=", StringComparison.OrdinalIgnoreCase))
                    {
                        return line.Substring("gtk-theme-name=".Length).Trim();
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private static bool EndsWithDark(string theme)
        {
            return theme.EndsWith("-dark", StringComparison.Ordinal) ||
                theme.EndsWith("-Dark", StringComparison.Ordinal);
        }

        private static string GetHomeDirectory()
        {
            string home = Environment.GetEnvironmentVariable("HOME");

            if (!string.IsNullOrEmpty(home))
                return home;

            return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}