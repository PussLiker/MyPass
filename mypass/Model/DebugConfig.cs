using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public static class DebugConfig
{
    public static bool IsDebugEnabled { get; private set; } = true; // По умолчанию false
    public static string LogFileName { get; private set; } = "Log.txt"; // Файл по умолчанию

    private static string debugFolderPath; // Путь к папке Debug

    // Метод для загрузки настроек из файла
    public static void LoadConfig()
    {
        try
        {
            // Путь для создания папки Debug
            debugFolderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\", "Debug"));

            // Проверяем и создаем папку, если её нет
            if (!Directory.Exists(debugFolderPath))
            {
                Directory.CreateDirectory(debugFolderPath);
            }

            // Финальный путь для файла DebugLoggingSetting.txt
            string configFilePath = Path.Combine(debugFolderPath, "DebugLoggingSetting.txt");

            // Проверяем и создаем файл DebugLoggingSetting.txt, если его нет
            if (!File.Exists(configFilePath))
            {
                string defaultConfigContent = "IsDebugEnabled=true\nLogFileName=Log.txt";
                File.WriteAllText(configFilePath, defaultConfigContent);
            }

            // Чтение настроек из DebugLoggingSetting.txt
            var settings = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(configFilePath))
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains('='))
                {
                    var keyValue = line.Split(new[] { '=' }, 2);
                    settings[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            // Устанавливаем значения из файла настроек
            if (settings.ContainsKey("IsDebugEnabled"))
            {
                IsDebugEnabled = settings["IsDebugEnabled"].ToLower() == "true";
            }

            if (settings.ContainsKey("LogFileName"))
            {
                LogFileName = Path.Combine(debugFolderPath, settings["LogFileName"]);
            }

        }
        catch (Exception)
        {
            // хз
        }
    }
}
