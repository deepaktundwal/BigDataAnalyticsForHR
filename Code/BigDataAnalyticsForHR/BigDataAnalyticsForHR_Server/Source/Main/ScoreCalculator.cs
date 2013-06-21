using Server.Model.Output;
using Server.Model.StackExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Source.Main
{
    public class ScoreCalculator
    {
        public Model.Output.Score GetScore(Model.StackExchange.User stExUser, Model.Twitter.Users tUser, Model.LinkedIn.User liUser, UserToSearch userToSearch)
        {
            Model.Output.Score Scr = new Model.Output.Score();

            TechnicalScore tScore = new TechnicalScore();

            if (liUser.recommendationsReceived != null)
                tScore.RecommendationsReceived_Count = liUser.recommendationsReceived._total;


            // Split keywords.
            string[] arrKeyWords = userToSearch.skills.Split(new char[] { ',' });
            for (int i = 0; i < tUser.statusRoot.items.Count; i++)
            {
                foreach (String strKeyWord in arrKeyWords)
                {
                    if (tUser.statusRoot.items[i].text.ToUpper().Contains(strKeyWord.ToUpper()))
                    {
                        tScore.TweetCount++;
                    }
                }
            }

            // Gold=2, silver=1, bronze=1/2, total (more gold) +5
            if (stExUser.badge_counts != null)
            {
                tScore.BadgeCount = stExUser.badge_counts.gold * 2;
                tScore.BadgeCount += stExUser.badge_counts.silver;
                tScore.BadgeCount += stExUser.badge_counts.bronze * 1/2;
            }
            
            return Scr;
        }
    }
}
