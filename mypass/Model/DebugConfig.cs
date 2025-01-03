﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// Зачем нужен: этот класс нужен для управлением настройками логирования
// Наследование: напрямую из этого класса ничего не наследуется
// Методы: 'LoadConfig' - нужен для загрузки значений с файла настроек (DebugLoggingSetting.txt), а также считываение уже имеющихся настроек. Имеет настройки по умолчанию

// Пример использования: DebugConfig.LoadConfig() - после вызова создаётся файл, настроек, если его нет, а также файл дебагинга.                    


public static class DebugConfig
{
    public static bool IsDebugEnabled { get; private set; } = false; // По умолчанию false
    public static string LogFileName { get; private set; } = "Log.txt"; // Файл по умолчанию

    private static string _debugFolderPath; // Путь к папке Debug

    // Метод для загрузки настроек из файла
    public static void LoadConfig()
    {
        // Путь для создания папки Debug
        _debugFolderPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Debug"));

        // Проверяем и создаем папку, если её нет
        if (!Directory.Exists(_debugFolderPath))
        {
            Directory.CreateDirectory(_debugFolderPath);
        }

        // Финальный путь для файла DebugLoggingSetting.txt
        string configFilePath = Path.Combine(_debugFolderPath, "DebugLoggingSetting.txt");

        // Проверяем и создаем файл DebugLoggingSetting.txt, если его нет
        if (!File.Exists(configFilePath))
        {
            string defaultConfigContent = $"IsDebugEnabled={IsDebugEnabled}\nLogFileName={LogFileName}";
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
            LogFileName = Path.Combine(_debugFolderPath, settings["LogFileName"]);
        }
    }
}
