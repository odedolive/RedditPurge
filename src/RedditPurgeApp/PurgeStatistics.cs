using System;
using System.Collections.Generic;
using System.Text;

namespace RedditPurge
{
    /// <summary>
    /// Reddit purge statistics
    /// </summary>
    public class PurgeStatistics
    {
        /// <summary>
        /// Gets or sets the number of deleted posts.
        /// </summary>
        /// <value>
        /// The number of deleted posts.
        /// </value>
        public int DeletedPosts { get; set; }

        /// <summary>
        /// Gets or sets the number of deleted comments.
        /// </summary>
        /// <value>
        /// The number of deleted comments.
        /// </value>
        public int DeletedComments { get; set; }
    }
}
