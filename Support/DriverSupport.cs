using OpenQA.Selenium;
using SeleniumFramework.Config;

namespace SeleniumFramework.Support
{
    public static class DriverSupport
    {
        [ThreadStatic] // Enables parallel execution
        private static IWebDriver _driver;

        public static IWebDriver Driver
        {
            get => _driver ?? throw new InvalidOperationException("Driver not initialized");
            set => _driver = value;
        }

        public static void InitializeDriver()
        {
            var browserType = Enum.Parse<BrowserType>(ConfigManager.Config.Browser);
            Driver = Browser.CreateDriver(browserType);

            // Set timeouts from config
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigManager.Config.Timeout);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigManager.Config.Timeout);
        }

        public static void CloseDriver()
        {
            Driver?.Quit();
            Driver = null;
        }

        public static void TakeScreenshot(string fileName)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                screenshot.SaveAsFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}