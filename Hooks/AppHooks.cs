using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using OpenQA.Selenium;
using SpecFlowBDDFramework.Drivers;
using SpecFlowBDDFramework.Utility.ReportProvider;
using TechTalk.SpecFlow;

namespace SpecFlowBDDFramework.Hooks
{
    [Binding]
    public class AppHooks: ExtentReport
    {
        private static IObjectContainer _objectContainer;
        private readonly DriverSetup driverSetup = new DriverSetup();

        public AppHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        #region Driver setup

        [BeforeScenario(Order = 1)]
        public void StartUpBrowser()
        {
            IWebDriver driver = driverSetup.GetWebDriver();
            _objectContainer.RegisterInstanceAs<IWebDriver>(driver);
        }

        [AfterScenario]
        public void TearDown()
        {
            var driver = _objectContainer.Resolve<IWebDriver>();

            if (driver != null)
            {
                driver.Quit();
            }
        }

        #endregion

        #region Extent Reporting

        [BeforeTestRun]
        public static void ReportInit()
        {
            Console.WriteLine("Report genaration...");
            DeleteAllScreenshot();
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void ReportTearDown()
        {
            Console.WriteLine("Report end../");
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void TakeFeatureName(FeatureContext featureContext)
        {
            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterF()
        {
            Console.WriteLine("After feature");
        }

        [BeforeScenario(Order = 2)]
        public static void TakeScenarioName(ScenarioContext scenarioContext)
        {
            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterStep]
        public static void AfterStep(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running after step....");
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            var driver = _objectContainer.Resolve<IWebDriver>();

            //When scenario passed
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName);
                }
                else if (stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName);
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName);
                }
                else if (stepType == "And")
                {
                    _scenario.CreateNode<And>(stepName);
                }
            }
            //When scenario fails
            if (scenarioContext.TestError != null)
            {
                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "And")
                {
                    _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
            }
        }
    

            #endregion

        
    }
}
