using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class User
    {
        public int user_id { get; set; }
        public string user_type { get; set; }
        public int creation_date { get; set; }
        public string display_name { get; set; }
        public string profile_image { get; set; }
        public int reputation { get; set; }
        public int reputation_change_day { get; set; }
        public int reputation_change_week { get; set; }
        public int reputation_change_month { get; set; }
        public int reputation_change_quarter { get; set; }
        public int reputation_change_year { get; set; }
        public int age { get; set; }
        public int last_access_date { get; set; }
        public int last_modified_date { get; set; }
        public bool is_employee { get; set; }
        public string link { get; set; }
        public string website_url { get; set; }
        public string location { get; set; }
        public int account_id { get; set; }
        public BadgeCounts badge_counts { get; set; }
        public int answer_count { get; set; }
        public int accept_rate { get; set; }
        public string about_me { get; set; }
        public int up_vote_count { get; set; }
        public int down_vote_count { get; set; }
        public int question_count { get; set; }

        public List<Question> lstQuestion { get; set; }
        public List<Answer> lstAnswer { get; set; }
        public List<Answer> lstAcceptedAnswer { get; set; }
        public List<Tag_Score> lstTag_Score { get; set; }
        public UserScore userScore { get; set; }
    }

    //public class UsersRoot
    //{
    //    public List<User> items { get; set; }
    //}

    //public class UsersRootConverter : CustomCreationConverter<UsersRoot>
    //{
    //    public override UsersRoot Create(Type objectType)
    //    {
    //        return new UsersRoot();
    //    }
    //}
}
