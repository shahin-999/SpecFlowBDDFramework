using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V85.HeadlessExperimental;
using SpecFlowBDDFramework.Utility.PropertyReader;

namespace SpecFlowBDDFramework.Utility.ReportProvider
{
    public class ExtentReport
    {
        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultPath = dir.Replace("bin\\Debug\\net6.0", "TestResult");
        public static string screenshotPath = testResultPath + "Screenshot";

        #region Extent report

        public static void ExtentReportInit()
        {
            var htmlReporter = new ExtentHtmlReporter(testResultPath);
            htmlReporter.Config.ReportName = "Automation Status Report";
            htmlReporter.Config.DocumentTitle = "Automation Status Report";
            htmlReporter.Config.Theme = Theme.Dark;
            htmlReporter.Start();

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(htmlReporter);
            _extentReports.AddSystemInfo("Application", EnvReader.GetInstance().GetAppName());
            _extentReports.AddSystemInfo("Enviroment", EnvReader.GetInstance().GetEnvType().ToUpper());
            _extentReports.AddSystemInfo("Browser", ConfigReader.GetInstance().GetDriverType().ToString().ToUpper());
            _extentReports.AddSystemInfo("OS", EnvReader.GetInstance().GetOS().ToUpper());
        }

        public static void ExtentReportTearDown()
        {
            _extentReports.Flush();
        }

        #endregion

        #region Screenshot
        public static string AddScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            var timestamp = DateTime.Now.ToFileTime();
            DateTime now = DateTime.Now;
            string dateTime = now.ToString("yyyy-MM-dd-HH-mm-ss");
            string screenShotLocation = Path.Combine(screenshotPath, "Screenshot-" + dateTime + ".png");
            screenshot.SaveAsFile(screenShotLocation);

            return screenShotLocation;
        }

        public static void DeleteAllScreenshot()
        {
            // Check if the folder exists
            if (Directory.Exists(screenshotPath))
            {
                // Get all files in the folder
                string[] files = Directory.GetFiles(screenshotPath);

                // Iterate through each file and delete it
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file: {file}");
                }

                Console.WriteLine("All files deleted successfully.");
            }
            else
            {
                Console.WriteLine("Folder does not exist.");
            }
        }

        #endregion
    }
}
