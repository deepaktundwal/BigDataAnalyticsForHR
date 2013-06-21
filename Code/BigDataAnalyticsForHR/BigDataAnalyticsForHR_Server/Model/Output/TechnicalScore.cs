using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Output
{
    public class Score
    {
        // technical score - 60%
        public int TechnicalScore = 0;
        // socioTechnical score - 20%
        public int SocioTechnicalScore = 0;
        // social score - 20%
        public int SocialScore = 0;
    }

    public class TechnicalScore
    {
        public int FinalScore = 0;
        // 2 marks per recommendation
        public int RecommendationsReceived_Count = 0;
        // search skill text tweets - Tweet on skill/keyword: Tweet-1, IsRetween-1/2, ReTweetCounts-2
        public int TweetCount = 0;
        // Consistency: No of yrs of experience & organization duration.
        // exp (yrs) / companies = score. eg. 6/6->1, 6/1->6
        //public int Consistency = 0;
        // Gold=2, silver=1, bronze=1/2, total (more gold) +5
        public int BadgeCount = 0;
        // Good reputation is less time - Extra point
        public int Reputation = 0;
        // Whom he is following: Preparelist of best fields, compre with this person.
        public int FollowingTopUsersCount = 0;
        // Location data: top 5 technical groups
        public int TopTechnicalGroupCount = 0;
    }

    public class SocioTechnical
    {
        public int FinalScore = 0;
        public int TweeterFollowersCount = 0;
        public int HoursSpendOnSocialSites = 0;
        public int CompaniesCount_IT = 0;
        public int GroupsCount_IT = 0;
        // Type of friends: More than 30% from same industry
        public int FriendsCount_SameIndustry = 0;
    }

    public class Social
    {
        public int FinalScore = 0;
        public int FriendsCount = 0;
        public int FollowersCount = 0;
        public int GroupCount = 0;
        public int CompainesCount_NonIT = 0;
        // Type of friends: More than 30% classmates
        public int FriendsCount_ClassMates;
        // Location data: top 5 social group
        public int TopSocialGroupCount = 0;
    }

    public class Behaviour
    {
        // Type
    }
}
