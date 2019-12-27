using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace RedditPurge
{
    /// <summary>
    /// WebDriver extension methods
    /// </summary>
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Clicks on a web element using it's locator and wait for the page to reload.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="elementLocator">The element locator.</param>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="NoSuchElementException">No elements " + elementLocator + " ClickAndWaitForPageToLoad</exception>
        public static void ClickAndWaitForPageToLoad(this IWebDriver driver, By elementLocator, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                var elements = driver.FindElements(elementLocator);
                if (elements.Count == 0)
                {
                    throw new NoSuchElementException("No elements " + elementLocator + " ClickAndWaitForPageToLoad");
                }
                var element = elements.FirstOrDefault(e => e.Displayed);
                element.Click();
                wait.Until(ExpectedConditions.StalenessOf(element));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                throw;
            }
        }

        /// <summary>
        /// Waits until element exists and clickable.
        /// </summary>
        /// <param name="driver">The driver.</param>
        /// <param name="elementLocator">The element locator.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static IWebElement WaitUntilElementExistsAndClickable(this IWebDriver driver, By elementLocator, int timeout = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementExists(elementLocator));
                return wait.Until(ExpectedConditions.ElementToBeClickable(elementLocator));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Element with locator: '" + elementLocator + "' was not found.");
                throw;
            }
        }

        public static void WaitUntil(this IWebDriver driver, Func<IWebDriver, bool> predicate, int timeout = 30)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
                wait.Until(d =>
                {
                    try
                    {
                        return predicate(d);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Predicate invocation failed.");
                        return false;
                    }
                }
                );
            }
            catch (NoSuchElementException)
            {
                throw;
            }
        }
    }
}
