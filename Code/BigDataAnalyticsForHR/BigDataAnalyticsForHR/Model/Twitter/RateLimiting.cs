using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// Provides data about the user's current rate limiting.
    /// </summary>
    public class RateLimiting
    {
        /// <summary>
        /// Gets the remaining number of requests until requests are denied.
        /// </summary>
        /// <value>The remaining requests.</value>
        public int Remaining { get; internal set; }

        /// <summary>
        /// Gets the total number of requests allowed before requests are denied.
        /// </summary>
        /// <value>The total number of requests.</value>
        public int Total { get; internal set; }

        /// <summary>
        /// Gets the date the remaining number of requests will be reset.
        /// </summary>
        /// <value>The reset date.</value>
        public DateTime ResetDate { get; internal set; }
    }
}
