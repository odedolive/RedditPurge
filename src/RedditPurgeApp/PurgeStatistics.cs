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
        public int DeletedPosts { get; set; }

        /// <summary>
        /// Gets or sets the number of deleted comments.
        /// </summary>
        public int DeletedComments { get; set; }

        /// <summary>
        /// Gets or sets the number of unsaved posts
        /// </summary>
        public int UnsavedPosts { get; set; }

        /// <summary>
        /// Gets or sets the number of "unhidden" posts
        /// </summary>
        public int UnhiddenPosts { get; set; }

        /// <summary>
        /// GEts or sets the number of posts that thier upvote was undone
        /// </summary>
        public int UnUpvote { get; set; }

        /// <summary>
        /// GEts or sets the number of posts that thier downvote was undone
        /// </summary>
        public int UnDownvote { get; set; }
    }
}
