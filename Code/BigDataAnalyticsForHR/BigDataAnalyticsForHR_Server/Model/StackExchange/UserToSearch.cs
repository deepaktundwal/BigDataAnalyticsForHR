using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class UserToSearch
    {
        public string f_name { get; set; }
        public string m_name { get; set; }
        public string l_name { get; set; }
        public string emailId { get; set; }
        public string jobRole { get; set; }
        public string skills { get; set; }
        public int stackEx_user_id { get; set; }
        public int stackEx_account_id { get; set; }
        public int twitter_user_id { get; set; }
        public string twitter_screenname { get; set; }
        public string LinkedIn_user_id { get; set; }
        public string curCompany_name { get; set; }
    }

    public class UserToSearchConverter : CustomCreationConverter<UserToSearch>
    {
        public override UserToSearch Create(Type objectType)
        {
            return new UserToSearch();
        }
    }
}
