using NUnit.Framework;
using OpenQA.Selenium;
using SpecFlowBDDFramework.Utility;

namespace SpecFlowBDDFramework.StepDefinitions
{
    [Binding]
    public class TestStepDefs1
    {
        private IWebDriver driver;

        public TestStepDefs1(IWebDriver driver)
        {
            this.driver = driver;
        }


        [Given(@"Navigate to the home page")]
        public void GivenNavigateToTheHomePage()
        {
            driver.Url = ConfigReader.GetInstance().GetBaseUrl();
        }

        [When(@"Click on the login url")]
        public void WhenClickOnTheLoginUrl()
        {
            IWebElement btn = driver.FindElement(By.XPath("//a[@class='ico-loginq333']"));
            btn.Click();
        }

        [Then(@"Verify that the login page is visible")]
        public void ThenVerifyThatTheLoginPageIsVisible()
        {
            Console.WriteLine("Login page");
            IWebElement btn = driver.FindElement(By.XPath("//a[@class='ico-login']"));
            btn.Click();
            Thread.Sleep(5000);
            
        }
        
    }
}
