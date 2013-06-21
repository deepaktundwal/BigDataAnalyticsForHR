using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model.StackExchange;
using Server.Model.StackExchange;

namespace BigDataAnalyticsForHR
{
    public class Constants
    {
        //public static string ServiceUrl = "http://localhost:1688/BigDataAnalyser.svc/";
        public static string ServiceUrl = "http://accafe2f68d5437d85b88fc8a628b6f7.cloudapp.net/BigDataAnalyser.svc/";
        //public static UserScoreDetailRoot ObjUserScoreDetailRoot = new UserScoreDetailRoot();
        public static UserToSearch ObjUserToSearch;

        public static string ServiceMethod_GetUserDetail = "GetUserDetail";
        public static string ServiceMethod_GetUserDetailById = "GetUserDetailById";
        public static string ServiceMethod_GetUserQuestions = "GetUserQuestion";

        public static string ServiceMethod_Twitter_SearchUser = "Twitter_SearchUser";
        public static string ServiceMethod_Twitter_UserTimeline = "Twitter_UserTimeline";
        public static string ServiceMethod_Twitter_GetRetweets = "Twitter_GetRetweets";
        public static string ServiceMethod_Twitter_GetMentionList = "Twitter_GetMentionList";

        public static string ServiceMethod_LinkedIn_SearchUser = "LinkedIn_SearchUser";
        public static string ServiceMethod_LinkedIn_UserDetail = "LinkedIn_UserDetail";
        public static string ServiceMethod_LinkedIn_GetGroupDetail = "LinkedIn_GetGroupDetail";

    }

    public enum LinkedInAPIType
    {
        UserSearch,
        UserDetail,
        GroupDetailWithComments
    }

    public enum TwitterAPIType
    {
        UserSearch,
        UserTimeLine,
        Retweets,
        Mention
    }

    public enum StackExchangeAPIType
    {
        UserSearch,
        UserDetailById,
        UserQuestion
    }

    public enum SocialNetworkSite
    {
        Twitter,
        LinkedIn,
        StackExchange
    }
}
