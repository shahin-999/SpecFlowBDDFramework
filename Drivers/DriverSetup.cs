using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using SpecFlowBDDFramework.Utility;

namespace SpecFlowBDDFramework.Drivers
{
    public class DriverSetup
    {
        private IWebDriver driver = null;
        private readonly DriverType driverType = ConfigReader.GetInstance().GetDriverType();
        private readonly bool isHeadless = ConfigReader.GetInstance().IsHeadless();
        private readonly bool isFullScreen = ConfigReader.GetInstance().IsFullScreen();
        private readonly int waitTime = ConfigReader.GetInstance().ImplicitWait();

        public IWebDriver GetWebDriver()
        {
            if (driver == null)
            {
                driver = CreateDriver();
            }
			return driver;
        }

        private IWebDriver CreateDriver()
        {
			switch (driverType)
			{
				case DriverType.CHROME:
					ChromeOptions chromeOptions = new ChromeOptions();
					if (isHeadless)
					{
						chromeOptions.AddArgument("--headless");
					}
					chromeOptions.AddArgument("--disable-popup-blocking");
					chromeOptions.AddArgument("--disable-notifications");
					chromeOptions.AddArgument("--disable-extensions");
					chromeOptions.AddArgument("--disable-web-security");
					chromeOptions.AddArgument("--ignore-certificate-errors");
					chromeOptions.AddArgument("--disable-cache");
					driver = new ChromeDriver(chromeOptions);
					break;

				case DriverType.FIREFOX:
					FirefoxOptions firefoxOptions = new FirefoxOptions();
					if (isHeadless)
					{
						firefoxOptions.AddArgument("--headless");
					}
					firefoxOptions.AddArgument("--disable-popup-blocking");
					firefoxOptions.AddArgument("--disable-notifications");
					firefoxOptions.AddArgument("--disable-extensions");
					firefoxOptions.AddArgument("--disable-web-security");
					firefoxOptions.AcceptInsecureCertificates = true;
					driver = new FirefoxDriver(firefoxOptions);
					break;

				case DriverType.EDGE:
					EdgeOptions edgeOptions = new EdgeOptions();
					if (isHeadless)
					{
						edgeOptions.AddArgument("headless");
					}
					edgeOptions.AddArgument("disable-popup-blocking");
					edgeOptions.AddArgument("disable-notifications");
					edgeOptions.AddArgument("disable-extensions");
					edgeOptions.AddArgument("disable-web-security");
					edgeOptions.AddArgument("ignore-certificate-errors");
					edgeOptions.AddArgument("disable-cache");
					driver = new EdgeDriver(edgeOptions);
					break;

				case DriverType.SAFARI:
					SafariOptions safariOptions = new SafariOptions();
					/*
					 * Safari does not support headless mode or as many arguments
					 * Will add options in future if avaiable
					 */
					driver = new SafariDriver(safariOptions);
					break;

				default:
					throw new ArgumentException("Unable to create browser driver");					
			}
			if (isFullScreen)
			{
				driver.Manage().Window.Maximize();
			}

			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(waitTime);

			return driver;
		}
    }

}
