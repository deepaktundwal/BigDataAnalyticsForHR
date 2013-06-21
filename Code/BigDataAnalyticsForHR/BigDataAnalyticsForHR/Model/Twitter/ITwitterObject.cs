using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    /// <summary>
    /// The ITwitterObject interface.
    /// </summary>
    /// <tocexclude />
    public interface ITwitterObject
    {
        /// <summary>
        /// Annotations are additional pieces of data, supplied by Twitter clients, in a non-structured dictionary.
        /// </summary>
        /// <value>The annotations.</value>
        System.Collections.Generic.Dictionary<string, string> Annotations { get; set; }
    }
}
