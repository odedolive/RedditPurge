using System;
using System.Collections.Generic;
using System.Linq;

namespace RedditPurge
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = GetLoginCredentials();
            var webDriver = WebDriverUtils.Create();
            try
            {
                var redditAutomation = new RedditAutomation(webDriver);
                redditAutomation.Login(credentials);
                var stats = redditAutomation.PurgeAccount(credentials.Username);
                OutputStatistics(stats);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while executing automation");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                webDriver.Quit();
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gets the login credentials.
        /// </summary>
        /// <returns></returns>
        private static LoginCredentials GetLoginCredentials()
        {
            var credentials = new LoginCredentials();
            while (credentials.IsEmpty())
            {
                Console.Write("Reddit username: ");
                credentials.Username = Console.ReadLine();
                Console.Write("Reddit password: ");
                credentials.Password = ReadPassword('*');
                Console.Write("Reddit 2-FA backup code (optional): ");
                credentials.TwoFABackupCode = ReadPassword('*');

                if (credentials.IsEmpty())
                {
                    Console.WriteLine("You must enter both username and password. Try again.");
                }
            }

            return credentials;
        }

        /// <summary>
        /// Outputs the purge statistics.
        /// </summary>
        /// <param name="stats">The stats.</param>
        private static void OutputStatistics(PurgeStatistics stats)
        { 
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(" Account Purge Statistics:");
            Console.WriteLine($" {stats.DeletedPosts} deleted posts.");
            Console.WriteLine($" {stats.DeletedComments} deleted comments.");
            Console.WriteLine();
            Console.WriteLine(" The internet never forgets...");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------");
        }

        /// <summary>
        /// Like System.Console.ReadLine(), only with a mask.
        /// </summary>
        /// <param name="mask">a <c>char</c> representing your choice of console mask</param>
        /// <returns>the string the user typed in </returns>
        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    Console.Write(mask);
                }
            }

            Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }
    }
}
