namespace Singleton;

public class SettingsManager
{
    private static SettingsManager? _instance;
    
    private readonly Dictionary<string, string> _settings;
    
    private SettingsManager()
    {
        _settings = new Dictionary<string, string>
        {
            { "Theme", "Dark" },
            { "Language", "English" },
            { "Notifications", "Enabled" }
        };
    }
    
    public static SettingsManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new SettingsManager();
        }
        return _instance;
    }
    
    public void SetSetting(string key, string value)
    {
        if (!_settings.TryAdd(key, value))
        {
            _settings[key] = value;
        }
    }

    public void PrintSettings()
    {
        foreach (var setting in _settings)
        {
            Console.WriteLine($"{setting.Key}: {setting.Value}");
        }
    }
}

internal static class Program
{
    public static void Main()
    {
        SettingsManager settingsManager = SettingsManager.GetInstance();
        
        Console.WriteLine("Начальные настройки:");
        settingsManager.PrintSettings();
        
        settingsManager.SetSetting("Theme", "Light");
        
        SettingsManager anotherSettingsManager = SettingsManager.GetInstance();

        // Проверяем, что это тот же самый экземпляр
        if (settingsManager == anotherSettingsManager)
        {
            Console.WriteLine("\nОба экземпляра одинаковы.");
        }
        
        Console.WriteLine("\nОбновленные настройки:");
        anotherSettingsManager.PrintSettings();
    }
}