using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// Describes the access level of the OAuth Token
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        /// The request may not be authenticated or the Access Level header was missing from the response.
        /// </summary>
        Unknown,

        /// <summary>
        /// The OAuth token has read access levels only.
        /// </summary>
        Read,

        /// <summary>
        /// The OAuth token has read write access only.
        /// </summary>
        ReadWrite,

        /// <summary>
        /// The OAuth token has read write and direct messages access.
        /// </summary>
        ReadWriteDirectMessage,

        /// <summary>
        /// There was no OAuth token access level available for reading in the response headers.
        /// </summary>
        Unavailable
    }
}
