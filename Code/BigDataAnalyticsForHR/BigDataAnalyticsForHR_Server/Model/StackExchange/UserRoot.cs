using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Server.Model.StackExchange
{
    public class UserRoot
    {
        //public List<UserOuter> UserDetail { get; set; }
        public string type { get; set; }
        public List<User> items { get; set; }
        public int quota_remaining { get; set; }
        public int quota_max { get; set; }
        public bool has_more { get; set; }
    }

    public class UserRootConverter : CustomCreationConverter<UserRoot>
    {
        public override UserRoot Create(Type objectType)
        {
            return new UserRoot();
        }
    }
}
