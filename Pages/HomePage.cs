using OpenQA.Selenium;
using SeleniumFramework.Pages;

public class HomePage : BasePage
{
    // Good: Descriptive names and proper encapsulation
    private By UserAvatar => By.CssSelector(".user-profile .avatar");
    private By NavigationMenu => By.Id("main-nav");

    // Avoid: Hard-coded strings and unclear names
    // private By btn1 => By.XPath("//div[1]/button[2]");
}

// Alternative: Use PageFactory pattern (optional)
//public class HomePage
//{
//    [FindsBy(How = How.Id, Using = "username")]
//    private IWebElement UsernameField;
//}

public class SafeActions
{
    public static bool TryClick(By locator, IWebDriver driver, int maxAttempts = 3)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            try
            {
                driver.FindElement(locator).Click();
                return true;
            }
            catch (StaleElementReferenceException)
            {
                System.Threading.Thread.Sleep(1000);
            }
            catch (ElementClickInterceptedException)
            {
                // Scroll element into view and retry
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);",
                    driver.FindElement(locator));
                System.Threading.Thread.Sleep(500);
            }
        }
        return false;
    }
}