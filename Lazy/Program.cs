namespace Lazy;

public class Configuration
{
    private Dictionary<string, string> PrimarySettings { get; set; }
    private readonly Envelope _optionalSettings = new();
    public Configuration()
    {
        PrimarySettings = new Dictionary<string, string>
        {
            { "Theme", "Dark" },
            { "Language", "English" },
            { "Notifications", "Enabled" }
        };
    }

    public string? GetPrimarySetting(string key)
    {
        return PrimarySettings.GetValueOrDefault(key);
    }

    public void SetPrimarySetting(Dictionary<string, string> settings)
    {
        foreach (var s in settings)
            PrimarySettings.Add(s.Key, s.Value);
    }
}

public class Envelope
{
    
    private SecondarySettings? _secondarySettings = null;
    
    public string? GetSecondarySetting(string key)
    {
        IsExist();
        return _secondarySettings!.GetSecondarySetting(key);
    }
    
    public void SetSecondarySettings(Dictionary<string, string> settings)
    {
        IsExist();
        _secondarySettings!.SetSecondarySettings(settings);
    }

    private void IsExist() => 
        _secondarySettings ??= new SecondarySettings();
    private class SecondarySettings
    {
        private Dictionary<string, string> SecondarySettingsDictionary { get; set; }
        
        public SecondarySettings()
        {
            Thread.Sleep(5000);
            SecondarySettingsDictionary = new Dictionary<string, string>
            {
                { "UI first option", "Compact mode" },
                { "UI second option", "Use project colors in main toolbar" }
            };
        }
        
        public string? GetSecondarySetting(string key)
        {
            return SecondarySettingsDictionary.GetValueOrDefault(key);
        }
        
        public void SetSecondarySettings(Dictionary<string, string> settings)
        {
            foreach (var s in settings)
                SecondarySettingsDictionary.Add(s.Key, s.Value);
        }
    }
}

public class Application
{
    private readonly Configuration _config  = new();

    public void Run()
    {
        Console.WriteLine("Приложение запущено");
        Console.WriteLine("Использование конфигурации:");
        Console.WriteLine($"Theme: {_config.GetPrimarySetting("Theme")}");
        Console.WriteLine($"Language: {_config.GetPrimarySetting("Language")}");
    }
}

internal static class Program
{
    private static void Main()
    {
        var app = new Application();
        app.Run();
    }
}