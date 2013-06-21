using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    //public class UserScoreDetailRoot
    //{
    //    public UserScoreDetailRoot()
    //    {
    //        UserScoreDetail = new UserScoreDetail();
    //        lstUser = new List<User>();
    //        lstNetwork_Users = new List<Network_Users>();
    //        lstAnswer = new List<Answer>();
    //        lstAcceptedAnswer = new List<Answer>();
    //        lstTag_Score = new List<Tag_Score>();
    //    }
    //    public UserScoreDetail UserScoreDetail { get; set; }
    //    public List<User> lstUser { get; set; }
    //    //public List<Network_Users> lstNetwork_Users { get; set; }
    //    //public List<Answer> lstAnswer { get; set; }
    //    //public List<Answer> lstAcceptedAnswer { get; set; }
    //    //public List<Tag_Score> lstTag_Score { get; set; }

    //}
    //public class UserScoreDetailConverter : CustomCreationConverter<UserScoreDetailRoot>
    //{
    //    public override UserScoreDetailRoot Create(Type objectType)
    //    {
    //        return new UserScoreDetailRoot();
    //    }
    //}
    //public class UserScoreDetail
    //{
    //    public UserScore userScore { get; set; }
    //    public string display_name { get; set; }
    //    public string profile_image { get; set; }
    //    public int user_id { get; set; }
    //    public int account_id { get; set; }
    //    public string location { get; set; }
    //}

    public class UserScore
    {
        public int score_TopAnsOnTag { get; set; }
        public int reputation { get; set; }
        public int score_User_Tag_Top_Ans { get; set; }
        public int is_acceptedCount { get; set; }
        public BadgeCounts badge_count { get; set; }
        public int count_user_tags { get; set; }
    }
}
