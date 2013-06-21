using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// The TwitterUserCollection class.
    /// </summary>
    [DataContract]
    public class TwitterUserCollection : TwitterCollection<TwitterUser>, ITwitterObject
    {
        /// <summary>
        /// Gets or sets the next cursor.
        /// </summary>
        /// <value>The next cursor.</value>
        [DataMember]
        public long NextCursor { get; internal set; }

        /// <summary>
        /// Gets or sets the previous cursor.
        /// </summary>
        /// <value>The previous cursor.</value>
        [DataMember]
        public long PreviousCursor { get; internal set; }

        /// <summary>
        /// Gets or sets information about the user's rate usage.
        /// </summary>
        /// <value>The rate limiting object.</value>
        [DataMember]
        public RateLimiting RateLimiting { get; internal set; }

        /// <summary>
        /// Deserializes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static TwitterUserCollection DeserializeWrapper(JObject value)
        {
            if (value == null || value.SelectToken("users") == null)
            {
                return null;
            }

            TwitterUserCollection result = JsonConvert.DeserializeObject<TwitterUserCollection>(value.SelectToken("users").ToString());
            result.NextCursor = value.SelectToken("next_cursor").Value<long>();
            result.PreviousCursor = value.SelectToken("previous_cursor").Value<long>();

            return result;
        }
    }
}
