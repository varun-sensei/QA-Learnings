using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumFramework.Support;

namespace SeleniumFramework.Utilities
{
    public static class CustomWaits
    {
        private static IWebDriver Driver => DriverSupport.Driver;

        public static void WaitAndClick(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator)).Click();
        }

        public static void WaitAndSendKeys(By locator, string text, int timeoutSeconds = 30)
        {
            var element = WaitForElementVisible(locator, timeoutSeconds);
            element.Clear();
            element.SendKeys(text);
        }

        public static string WaitAndGetText(By locator, int timeoutSeconds = 30)
        {
            return WaitForElementVisible(locator, timeoutSeconds).Text;
        }

        public static bool IsElementVisible(By locator, int timeoutSeconds = 10)
        {
            try
            {
                return WaitForElementVisible(locator, timeoutSeconds).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public static IWebElement WaitForElementVisible(By locator, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public static void WaitForPageLoad(int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver =>
                ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}