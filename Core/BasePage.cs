using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumFramework.Config;
using SeleniumFramework.Support;
using SeleniumFramework.Utilities;

namespace SeleniumFramework.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => DriverSupport.Driver;
        protected WebDriverWait Wait => new WebDriverWait(Driver, TimeSpan.FromSeconds(30));

        // Common methods
        protected void Click(By locator) => CustomWaits.WaitAndClick(locator);
        protected void Type(By locator, string text) => CustomWaits.WaitAndSendKeys(locator, text);
        protected string GetText(By locator) => CustomWaits.WaitAndGetText(locator);
        protected bool IsVisible(By locator) => CustomWaits.IsElementVisible(locator);

        public void NavigateTo(string relativeUrl)
        {
            Driver.Navigate().GoToUrl($"{ConfigManager.Config.BaseUrl}{relativeUrl}");
            CustomWaits.WaitForPageLoad();
        }
    }
}