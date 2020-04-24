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
        /// Login to Reddit using the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public void Login(LoginCredentials credentials)
        {
            webDriver.Url = "https://www.reddit.com/login";
            var input = webDriver.FindElement(By.Id("loginUsername"));
            input.SendKeys(credentials.Username);

            input = webDriver.FindElement(By.Id("loginPassword"));
            input.SendKeys(credentials.Password);

            if (!string.IsNullOrWhiteSpace(credentials.TwoFABackupCode))
            {
                // 2-Factor-Authentication - use the provided backup code
                webDriver.WaitUntilElementExistsAndClickable(By.ClassName("AnimatedForm__submitButton")).Click();
                webDriver.WaitUntilElementExistsAndClickable(By.ClassName("switch-otp-type")).Click();
                input = webDriver.FindElement(By.Id("loginOtp"));
                input.SendKeys(credentials.TwoFABackupCode);
            }

            webDriver.ClickAndWaitForPageToLoad(By.ClassName("AnimatedForm__submitButton"));
        }

        /// <summary>
        /// Purges the account. Delete all posts and comments
        /// </summary>
        /// <param name="username">The account's username.</param>
        public PurgeStatistics PurgeAccount(string username)
        {
            var stats = new PurgeStatistics();
            stats.DeletedPosts = DeleteAll($"https://www.reddit.com/user/{username}/posts", "delete post");
            stats.DeletedComments = DeleteAll($"https://www.reddit.com/user/{username}/comments", "delete");
            stats.UnsavedPosts = SimpleActions($"https://www.reddit.com/user/{username}/saved", "unsave");
            stats.UnhiddenPosts = SimpleActions($"https://www.reddit.com/user/{username}/hidden", "unhide");
            stats.UnUpvote = UpOrDownvoteActions($"https://www.reddit.com/user/{username}/upvoted", "icon-upvote");
            stats.UnDownvote = UpOrDownvoteActions($"https://www.reddit.com/user/{username}/downvoted", "icon-downvote");
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

        private int SimpleActions(string url, string action)
        {
            webDriver.Url = url;

            int counter = 0;
            IEnumerable<IWebElement> actions = null;
            do
            {
                actions = webDriver.FindElements(By.XPath($"//span[.='{action}']"));
                foreach (var button in actions)
                {
                    try
                    {
                        webDriver.WaitUntil(_ => button.Enabled);
                        button.Click();
                        counter++;
                    }
                    catch { }
                }

                webDriver.Url = webDriver.Url; // refresh page
            } while (actions.Count() > 0);

            return counter;
        }

        private int UpOrDownvoteActions(string url, string action)
        {
            webDriver.Url = url;

            int counter = 0;
            IEnumerable<IWebElement> actions = null;
            do
            {
                actions = webDriver.FindElements(By.ClassName(action));
                foreach (var button in actions)
                {
                    try
                    {
                        webDriver.WaitUntil(_ => button.Enabled);
                        button.Click();
                        counter++;
                    }
                    catch { }
                }

                webDriver.Url = webDriver.Url; // refresh page
            } while (actions.Count() > 0);

            return counter;
        }
    }
}
