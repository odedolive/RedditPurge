using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RedditPurge
{
    public class RedditAutomation
    {
        /// <summary>
        /// The web driver
        /// </summary>
        private readonly IWebDriver webDriver;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedditAutomation"/> class.
        /// </summary>
        /// <param name="webDriver">The web driver.</param>
        public RedditAutomation(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        /// <summary>
        /// Loginto Reddit using the provided credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="twoFactorAuthenticationSeed">The two factor authentication seed.</param>
        public void Login(string username, string password, string twoFactorAuthenticationSeed = null)
        {
            webDriver.Url = "https://www.reddit.com/login/?dest=https%3A%2F%2Fwww.reddit.com%2F";
            var input = webDriver.FindElement(By.Id("loginUsername"));
            input.SendKeys(username);

            input = webDriver.FindElement(By.Id("loginPassword"));
            input.SendKeys(password);

            webDriver.ClickAndWaitForPageToLoad(By.ClassName("AnimatedForm__submitButton"));
        }

        /// <summary>
        /// Purges the account. Delete all posts and comments
        /// </summary>
        /// <param name="username">The account's username.</param>
        public PurgeStatistics PurgeAccount(string username)
        {
            var stats = new PurgeStatistics();
            stats.DeletedPosts = DeleteAll($"https://www.reddit.com/user/{username}/posts", "Delete Post");
            stats.DeletedComments = DeleteAll($"https://www.reddit.com/user/{username}/comments", "Delete");
            return stats;
        }

        /// <summary>
        /// Deletes all the conent on a given url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="confirmButton">The confirm button.</param>
        private int DeleteAll(string url, string confirmButton)
        {
            webDriver.Url = url;

            int counter = 0;
            IEnumerable<IWebElement> moreOptionsButtons = null;
            do
            {
                try
                {
                    moreOptionsButtons = webDriver.FindElements(By.ClassName("icon-menu"));
                    foreach (var button in moreOptionsButtons.Skip(1))
                    {
                        webDriver.WaitUntil(_ => button.Enabled);
                        button.Click();
                        By deleteButtonSelector = By.XPath("//span[.='delete']");
                        webDriver.WaitUntilElementExistsAndClickable(deleteButtonSelector).Click();

                        var confirmButtonSelector = By.XPath($"//button[.='{confirmButton}']");
                        webDriver.WaitUntilElementExistsAndClickable(confirmButtonSelector).Click();

                        counter++;
                    }
                }
                catch
                {
                    // If anything goes wrong here we'll simply move on, refresh the page
                    // and try to see if there are anything else we need to delete.
                    // Some errors might happen if you have a lot of comments/posts, and 
                    // scrolling down the page is dynamically loading more comments causing
                    // the execution flow to break as the site is handling callbacks while
                    // the delete confirmation modal is open.
                }
                
                webDriver.Url = webDriver.Url; // refresh page
            } while (moreOptionsButtons.Count() > 1);

            return counter;
        }
    }
}
