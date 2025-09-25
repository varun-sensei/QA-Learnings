using Newtonsoft.Json;

namespace SeleniumFramework.Config
{
    public class Configuration
    {
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public bool Headless { get; set; }
        public int Timeout { get; set; } = 30;
    }

    public static class ConfigManager
    {
        private static Configuration _config;

        public static Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    // Support different environments
                    var environment = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") ?? "development";
                    var configFile = $"appsettings.{environment}.json";

                    if (!File.Exists(configFile)) configFile = "appsettings.json";

                    var json = File.ReadAllText(configFile);
                    _config = JsonConvert.DeserializeObject<Configuration>(json);
                }
                return _config;
            }
        }
    }
}