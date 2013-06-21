using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace BigDataAnalyticsForHR.Model.Twitter
{
    [DataContract]
    public abstract class TwitterCollection<T> : Collection<T>
        where T : class, ITwitterObject
    {
        /// <summary>
        /// Gets or sets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        [DataMember]
        public System.Collections.Generic.Dictionary<string, string> Annotations { get; set; }
    }
}
