using System.Collections.Generic;
using Newtonsoft.Json;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// Represents the callback signature for asynchronous methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result">The result.</param>
    /// <remarks></remarks>
    public delegate void TwitterAsyncCallback<T>(T result)
            where T : ITwitterObject;

    /// <summary>
    /// The base object class
    /// </summary>
    public class TwitterObject : ITwitterObject
    {
        /// <summary>
        /// The format that all twitter dates are in.
        /// </summary>
        protected const string DateFormat = "ddd MMM dd HH:mm:ss zz00 yyyy";

        /// <summary>
        /// The format that all twitter search api dates are in.
        /// </summary>
        protected const string SearchDateFormat = "ddd, dd MMM yyyy HH:mm:ss +zz00";

        /// <summary>
        /// Annotations are additional pieces of data, supplied by Twitter clients, in a non-structured dictionary.
        /// </summary>
        /// <value>The annotations.</value>
        [JsonProperty(PropertyName = "annotations")]
        public Dictionary<string, string> Annotations { get; set; }
    }
}
