using Reqnroll;
using SeleniumFramework.Support;
using SeleniumFramework.Utilities;
using SeleniumFramework.Config;

namespace SeleniumFramework.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Initialize driver
            DriverSupport.InitializeDriver();

            // Navigate to base URL
            DriverSupport.Driver.Navigate().GoToUrl(ConfigManager.Config.BaseUrl);

            // Initialize test in report
            ReportManager.CreateTest(_scenarioContext.ScenarioInfo.Title);
            ReportManager.LogInfo($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Handle test result
            if (_scenarioContext.TestError != null)
            {
                var screenshotPath = TakeScreenshot();
                ReportManager.LogFail($"Test failed: {_scenarioContext.TestError.Message}");
                ReportManager.LogInfo($"Screenshot: {screenshotPath}");
            }
            else
            {
                ReportManager.LogPass("Test passed successfully");
            }

            // Close driver
            DriverSupport.CloseDriver();
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            ReportManager.ExtentReport.ExtentReportInit();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            ReportManager.FlushReport();
        }

        private string TakeScreenshot()
        {
            var fileName = $"Screenshots/{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMddHHmmss}.png";
            DriverSupport.TakeScreenshot(fileName);
            return fileName;
        }
    }
}