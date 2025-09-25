using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SeleniumFramework.Config;

namespace SeleniumFramework.Support
{
    public enum BrowserType { Chrome, Firefox, Edge }

    public static class Browser
    {
        public static IWebDriver CreateDriver(BrowserType browserType)
        {
            return browserType switch
            {
                BrowserType.Chrome => new ChromeDriver(GetChromeOptions()),
                BrowserType.Firefox => new FirefoxDriver(GetFirefoxOptions()),
                BrowserType.Edge => new EdgeDriver(GetEdgeOptions()),
                _ => throw new NotImplementedException($"Browser {browserType} not implemented")
            };
        }

        private static ChromeOptions GetChromeOptions()
        {
            var options = new ChromeOptions();
            options.AddArguments("--start-maximized", "--disable-notifications");
            if (ConfigManager.Config.Headless) options.AddArgument("--headless");
            return options;
        }

        private static FirefoxOptions GetFirefoxOptions()
        {
            return new FirefoxOptions();
        }

        private static EdgeOptions GetEdgeOptions()
        {
            return new EdgeOptions();
        }
    }
}