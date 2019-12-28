using System;
using System.Collections.Generic;
using System.Text;

namespace RedditPurge
{
    /// <summary>
    /// Reddit's login credentials
    /// </summary>
    public class LoginCredentials
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the 2-FA backup code.
        /// </summary>
        /// <value>
        /// The 2-FA backup code.
        /// </value>
        public string TwoFABackupCode { get; set; }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password);
        }
    }
}
