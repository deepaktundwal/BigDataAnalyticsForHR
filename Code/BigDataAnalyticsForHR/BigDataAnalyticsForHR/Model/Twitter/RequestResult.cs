using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// Describes the result status of a request
    /// </summary>
    public enum RequestResult
    {
        /// <summary>
        /// The request was completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// The URI requested is invalid or the resource requested, such as a user, does not exists.
        /// </summary>
        FileNotFound,

        /// <summary>
        /// The request was invalid.  An accompanying error message will explain why.
        /// </summary>
        BadRequest,

        /// <summary>
        /// Authentication credentials were missing or incorrect.
        /// </summary>
        Unauthorized,

        /// <summary>
        /// Returned by the Search API when an invalid format is specified in the request.
        /// </summary>
        NotAcceptable,

        /// <summary>
        /// The authorized user, or client IP address, is being rate limited.
        /// </summary>
        RateLimited,

        /// <summary>
        /// Twitter is currently down.
        /// </summary>
        TwitterIsDown,

        /// <summary>
        /// Twitter is online, but is overloaded. Try again later.
        /// </summary>
        TwitterIsOverloaded,

        /// <summary>
        /// The request failed due to a connection issue or timeout.
        /// </summary>
        ConnectionFailure,

        /// <summary>
        /// Something unexpected happened. See the error message for additional information.
        /// </summary>
        Unknown,

        /// <summary>
        /// Failed to authenticate with the proxy.
        /// </summary>
        ProxyAuthenticationRequired
    }

}
