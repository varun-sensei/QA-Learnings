using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Model;
using System.Reflection;
using AventStack.ExtentReports.Reporter.Config;
using OpenQA.Selenium;
using Reqnroll;

namespace SeleniumFramework.Utilities
{
    public static class ReportManager
    {
        private static ExtentReports _extentReports;
        private static ExtentTest _currentTest;
        private static string _reportPath;

        public class ExtentReport
        {
            // Global ExtentReports instance (shared across threads - safe as it's read-only after setup)
            public static ExtentReports? _extentReports;

            // Thread-safe ExtentTest instances for each thread (Feature and Scenario level)
            public static ThreadLocal<ExtentTest?> _feature = new ThreadLocal<ExtentTest?>();
            public static ThreadLocal<ExtentTest?> _scenario = new ThreadLocal<ExtentTest?>();

            public static readonly string dir = AppDomain.CurrentDomain.BaseDirectory;
            public static readonly string testResultPath = dir.Replace("bin\\Debug\\net8.0", "TestReports");

            /// <summary>
            /// Initializes ExtentReport and SparkReporter
            /// </summary>
            public static void ExtentReportInit()
            {
                string resultFilePath = Path.Combine(testResultPath, DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_ExtentSparkReporter.html");
                var sparkReporter = new ExtentSparkReporter(resultFilePath);

                sparkReporter.Config.ReportName = "CR QA Automation - Smoke Test Report";
                sparkReporter.Config.DocumentTitle = "CR QA Automation - Smoke Test Report";
                sparkReporter.Config.Theme = Theme.Dark;

                _extentReports = new ExtentReports();
                _extentReports.AttachReporter(sparkReporter);

                _extentReports.AddSystemInfo("Application", "CR QA Automation");
                _extentReports.AddSystemInfo("OS", Environment.OSVersion.ToString());
            }

            /// <summary>
            /// Finalizes and writes the report
            /// </summary>
            public static void ExtentReportTearDown()
            {
                _extentReports?.Flush();

                // Dispose thread-local variables to prevent memory leaks
                _feature.Dispose();
                _scenario.Dispose();
            }

            /// <summary>
            /// Captures a screenshot for the current scenario and saves it to the report folder
            /// </summary>
            /// <param name="driver">The WebDriver instance</param>
            /// <param name="scenarioContext">The ScenarioContext for the current test</param>
            /// <returns>Path to the screenshot file</returns>
            public static string AddScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
            {
                string scenarioTitleSafe = string.Join("_", scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
                string errorFileName = $"error_{scenarioTitleSafe}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string screenshotLocation = Path.Combine(testResultPath, errorFileName);

                if (driver is ITakesScreenshot takesScreenshot)
                {
                    Screenshot screenshot = takesScreenshot.GetScreenshot();
                    screenshot.SaveAsFile(screenshotLocation);
                }

                return screenshotLocation;
            }
        }
    

    public static void CreateTest(string testName)
        {
            _currentTest = _extentReports.CreateTest(testName);
        }

        public static void LogInfo(string message)
        {
            _currentTest?.Info(message);
            Console.WriteLine($"[INFO] {message}");
        }

        public static void LogPass(string message)
        {
            _currentTest?.Pass(message);
            Console.WriteLine($"[PASS] {message}");
        }

        public static void LogFail(string message)
        {
            _currentTest?.Fail(message);
            Console.WriteLine($"[FAIL] {message}");
        }

        public static void LogWarning(string message)
        {
            _currentTest?.Warning(message);
            Console.WriteLine($"[WARN] {message}");
        }

        public static void AddScreenshot(string screenshotPath)
        {
            if (File.Exists(screenshotPath) && _currentTest != null)
            {
                try
                {
                    _currentTest.Info("Screenshot", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                }
                catch (Exception ex)
                {
                    LogWarning($"Failed to attach screenshot: {ex.Message}");
                }
            }
        }

        public static void FlushReport()
        {
            _extentReports?.Flush();
            Console.WriteLine($"Report generated at: {Path.Combine(_reportPath, "TestReport.html")}");
        }

        public static void LogError(Exception exception)
        {
            var errorMessage = $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";
            _currentTest?.Fail(errorMessage);
            Console.WriteLine($"[ERROR] {errorMessage}");
        }

        private static string GetProjectDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}