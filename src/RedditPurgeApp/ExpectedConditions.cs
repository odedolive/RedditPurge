using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedditPurge
{
    /// <summary>
    /// Supplies a set of common conditions that can be waited for using <see cref="WebDriverWait"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// IWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3))
    /// IWebElement element = wait.Until(ExpectedConditions.ElementExists(By.Id("foo")));
    /// </code>
    /// </example>
    public sealed class ExpectedConditions
    {
        /// <summary>
        /// An expectation for checking that an element is present on the DOM of a
        /// page. This does not necessarily mean that the element is visible.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located.</returns>
        public static Func<IWebDriver, IWebElement> ElementExists(By locator)
        {
            return (driver) => { return driver.FindElement(locator); };
        }

        /// <summary>
        /// An expectation for checking an element is visible and enabled such that you
        /// can click it.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located and clickable (visible and enabled).</returns>
        public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
        {
            return (driver) =>
            {
                var element = ElementIfVisible(driver.FindElement(locator));
                try
                {
                    if (element != null && element.Enabled)
                    {
                        return element;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        /// Wait until an element is no longer attached to the DOM.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns><see langword="false"/> is the element is still attached to the DOM; otherwise, <see langword="true"/>.</returns>
        public static Func<IWebDriver, bool> StalenessOf(IWebElement element)
        {
            return (driver) =>
            {
                try
                {
                    // Calling any method forces a staleness check
                    return element == null || !element.Enabled;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            };
        }

        /// <summary>
        /// Checks if an element is visible.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static IWebElement ElementIfVisible(IWebElement element)
        {
            return element.Displayed ? element : null;
        }
    }
}
