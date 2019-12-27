using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace RedditPurge
{
    public class WebDriverUtils
    {
        /// <summary>
        /// Creates a WebDriver instance.
        /// </summary>
        /// <returns>IWebDriver instance</returns>
        public static IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");

            IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory, options);
            return driver;
        }
    }
}
