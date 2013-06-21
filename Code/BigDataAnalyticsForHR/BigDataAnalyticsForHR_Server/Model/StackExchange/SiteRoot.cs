using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class SiteRoot
    {
        public List<Site> items { get; set; }
        public int quota_remaining { get; set; }
        public int quota_max { get; set; }
        public bool has_more { get; set; }
    }

    public class SiteRootConverter : CustomCreationConverter<SiteRoot>
    {
        public override SiteRoot Create(Type objectType)
        {
            return new SiteRoot();
        }
    }
}
