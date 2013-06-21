using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Twitter
{
    public class UsersRoot
    {
        public List<Users> items { get; set; }
    }

    public class UsersRootConverter : CustomCreationConverter<UsersRoot>
    {
        public override UsersRoot Create(Type objectType)
        {
            return new UsersRoot();
        }
    }
}
