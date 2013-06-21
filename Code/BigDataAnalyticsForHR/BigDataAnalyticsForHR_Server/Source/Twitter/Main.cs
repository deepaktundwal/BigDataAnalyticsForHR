using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitterizer;

namespace Server.Source.Twitter
{
    public class Main
    {
        /// <summary>
        /// Provides a simple, relevance-based search interface to public user accounts on Twitter. 
        /// Try querying by topical interest, full name, company name, location, or other criteria. 
        /// Exact match searches are not supported. Only the first 1,000 matching results are available.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public Model.Twitter.UsersRoot SearchUser(String userName, String keyWords)
        {
            //GetMentionList("shreyaghoshal");
            //UserTimeline("dtundwal", keyWords);

            UserSearchOptions options = new UserSearchOptions();
            options.NumberPerPage = 10;
            OAuthTokens tokens = Helper.Configuration.GetTwitterTokens();
            TwitterUserCollection results = TwitterUser.Search(tokens, userName).ResponseObject;

            Model.Twitter.UsersRoot userRoot = new Model.Twitter.UsersRoot();
            Model.Twitter.Users user;

            if (results != null)
            {
                userRoot.items = new List<Model.Twitter.Users>();

                foreach (TwitterUser tuser in results)
                {
                    user = new Model.Twitter.Users();

                    user.description = tuser.Description;
                    user.favourites_count = Convert.ToInt32(tuser.NumberOfFavorites);
                    user.followers_count = Convert.ToInt32(tuser.NumberOfFollowers);
                    user.friends_count = Convert.ToInt32(tuser.NumberOfFriends);
                    user.id = Convert.ToInt32(tuser.Id);
                    user.lang = tuser.Language;
                    user.listed_count = tuser.ListedCount;
                    user.location = tuser.Location;
                    user.name = tuser.Name;
                    user.profile_image_url = tuser.ProfileImageLocation;
                    user.screen_name = tuser.ScreenName;
                    user.time_zone = tuser.TimeZone;
                    user.url = tuser.Website;
                    user.statuses_count = tuser.NumberOfStatuses;
                    userRoot.items.Add(user);
                }

                if (userRoot.items.Count == 1)
                {
                    userRoot.items[0].statusRoot = UserTimeline(userRoot.items[0].screen_name, keyWords);
                    //userRoot.items[0].mentionList = GetMentionList(userRoot.items[0].screen_name);
                }
            }

            return userRoot;
        }

        /// <summary>
        /// Returns a collection of the most recent Tweets posted by the user indicated by the screen_name or user_id parameters.
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        public Model.Twitter.TwitterStatusRoot UserTimeline(String screenName, String keyWords)
        {
            Model.Twitter.TwitterStatusRoot tStatusRoot = new Model.Twitter.TwitterStatusRoot();

            try
            {
                OAuthTokens tokens = Helper.Configuration.GetTwitterTokens();

                UserTimelineOptions User_Options = new UserTimelineOptions();
                if (screenName != null)
                    User_Options.ScreenName = screenName;

                User_Options.IncludeRetweets = true;
                User_Options.SkipUser = true;
                User_Options.Count = 10;

                TwitterResponse<TwitterStatusCollection> timelineResponse = TwitterTimeline.UserTimeline(tokens, User_Options);

                Model.Twitter.TwitterStatus status;
                tStatusRoot.items = new List<Model.Twitter.TwitterStatus>();

                if (timelineResponse == null)
                {
                    return tStatusRoot;
                }

                foreach (TwitterStatus tstatus in timelineResponse.ResponseObject)
                {
                    status = new Model.Twitter.TwitterStatus();
                    status.created_at = tstatus.CreatedDate;
                    status.favorite_count = Convert.ToInt32(tstatus.FavoriteCount);
                    status.id = tstatus.Id;
                    status.retweet_count = Convert.ToInt32(tstatus.RetweetCount);
                    status.text = tstatus.Text;
                    status.source = tstatus.Source;
                    if (tstatus.RetweetedStatus != null)
                    {
                        status.retweeted_status = new Model.Twitter.TwitterStatus();
                        status.retweeted_status.created_at = tstatus.RetweetedStatus.CreatedDate;
                        status.retweeted_status.favorite_count = Convert.ToInt32(tstatus.RetweetedStatus.FavoriteCount);
                        status.retweeted_status.id = tstatus.RetweetedStatus.Id;
                        status.retweeted_status.retweet_count = Convert.ToInt32(tstatus.RetweetedStatus.RetweetCount);
                    }
                    tStatusRoot.items.Add(status);
                }

                //if (keyWords != null)
                //{
                //    GetRetweets(screenName, ref tStatusRoot, keyWords);
                //}
            }
            catch (Exception Ex)
            { throw Ex; }
            return tStatusRoot;
        }

        /// <summary>
        /// Returns up to 100 of the first retweets of a given tweet.
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="tStatusRoot"></param>
        /// <param name="keyWords"></param>
        public static void GetRetweets(String screenName, ref Model.Twitter.TwitterStatusRoot tStatusRoot, String keyWords)
        {
            if (keyWords.Length <= 0)
                return;

            // Split keywords.
            string[] arrKeyWords = keyWords.Split(new char[] { ',' });
            bool tweetContainsKeyword;
            OAuthTokens tokens = Helper.Configuration.GetTwitterTokens();

            Model.Twitter.TwitterStatus status;
            for (int i = 0; i < tStatusRoot.items.Count; i++)
            {
                Model.Twitter.TwitterStatus tstatus = tStatusRoot.items[i];
                tweetContainsKeyword = false;

                if (tstatus.retweet_count > 0)
                {
                    // Get retweets for only those tweets which contains keywords in there body.
                    foreach (String strKeyWord in arrKeyWords)
                    {
                        if (tstatus.text.ToUpper().Contains(strKeyWord.ToUpper()))
                        {
                            // keyword found in tweet text, break the loop.
                            tweetContainsKeyword = true;
                            break;
                        }
                    }

                    // Tweet text doesn't contain keywords, jump to new tweet.
                    if (!tweetContainsKeyword)
                        continue;

                    TwitterResponse<TwitterStatusCollection> timelineResponse = TwitterStatus.Retweets(tokens, tstatus.id);

                    if (timelineResponse.ResponseObject == null)
                        continue;

                    tstatus.retweets = new List<Model.Twitter.TwitterStatus>();

                    foreach (TwitterStatus tInnerstatus in timelineResponse.ResponseObject)
                    {
                        // Ignore retweets by user himself.
                        if (tInnerstatus.User.ScreenName.ToUpper() == screenName.ToUpper())
                            continue;

                        status = new Model.Twitter.TwitterStatus();
                        status.created_at = tInnerstatus.CreatedDate;
                        status.favorite_count = Convert.ToInt32(tInnerstatus.FavoriteCount);
                        status.id = tInnerstatus.Id;
                        status.retweet_count = Convert.ToInt32(tInnerstatus.RetweetCount);
                        status.text = tInnerstatus.Text;

                        status.user = new Model.Twitter.Users();
                        status.user.description = tInnerstatus.User.Description;
                        status.user.favourites_count = Convert.ToInt32(tInnerstatus.User.NumberOfFavorites);
                        status.user.followers_count = Convert.ToInt32(tInnerstatus.User.NumberOfFollowers);
                        status.user.friends_count = Convert.ToInt32(tInnerstatus.User.NumberOfFriends);
                        status.user.id = Convert.ToInt32(tInnerstatus.User.Id);
                        status.user.lang = tInnerstatus.User.Language;
                        status.user.listed_count = tInnerstatus.User.ListedCount;
                        status.user.location = tInnerstatus.User.Location;
                        status.user.name = tInnerstatus.User.Name;
                        status.user.profile_image_url = tInnerstatus.User.ProfileImageLocation;
                        status.user.screen_name = tInnerstatus.User.ScreenName;
                        status.user.time_zone = tInnerstatus.User.TimeZone;
                        status.user.url = tInnerstatus.User.Website;

                        tstatus.retweets.Add(status);
                    }
                    tStatusRoot.items[i] = tstatus;
                }
            }
        }

        public static Model.Twitter.TwitterStatusRoot GetRetweets(String screenName, decimal tweetId)
        {
            OAuthTokens tokens = Helper.Configuration.GetTwitterTokens();
            Model.Twitter.TwitterStatusRoot tStatusRoot = new Model.Twitter.TwitterStatusRoot();

            TwitterResponse<TwitterStatusCollection> timelineResponse = TwitterStatus.Retweets(tokens, tweetId);

            if (timelineResponse == null)
                return tStatusRoot;

            if (timelineResponse.ResponseObject == null)
                return tStatusRoot;

            tStatusRoot.items = new List<Model.Twitter.TwitterStatus>();
            Model.Twitter.TwitterStatus status;

            foreach (TwitterStatus tInnerstatus in timelineResponse.ResponseObject)
            {
                // Ignore retweets by user himself.
                if (tInnerstatus.User.ScreenName.ToUpper() == screenName.ToUpper())
                    continue;

                status = new Model.Twitter.TwitterStatus();
                status.created_at = tInnerstatus.CreatedDate;
                status.favorite_count = Convert.ToInt32(tInnerstatus.FavoriteCount);
                status.id = tInnerstatus.Id;
                status.retweet_count = Convert.ToInt32(tInnerstatus.RetweetCount);
                status.text = tInnerstatus.Text;

                status.user = new Model.Twitter.Users();
                status.user.description = tInnerstatus.User.Description;
                status.user.favourites_count = Convert.ToInt32(tInnerstatus.User.NumberOfFavorites);
                status.user.followers_count = Convert.ToInt32(tInnerstatus.User.NumberOfFollowers);
                status.user.friends_count = Convert.ToInt32(tInnerstatus.User.NumberOfFriends);
                status.user.id = Convert.ToInt32(tInnerstatus.User.Id);
                status.user.lang = tInnerstatus.User.Language;
                status.user.listed_count = tInnerstatus.User.ListedCount;
                status.user.location = tInnerstatus.User.Location;
                status.user.name = tInnerstatus.User.Name;
                status.user.profile_image_url = tInnerstatus.User.ProfileImageLocation;
                status.user.screen_name = tInnerstatus.User.ScreenName;
                status.user.time_zone = tInnerstatus.User.TimeZone;
                status.user.url = tInnerstatus.User.Website;

                tStatusRoot.items.Add(status);
            }
            return tStatusRoot;
        }

        /// <summary>
        /// Returns most recent mentions (status containing @username) for the authenticating user.
        /// </summary>
        /// <param name="ScreenName"></param>
        /// <returns></returns>
        public static Model.Twitter.MentionRoot GetMentionList(String ScreenName)
        {
            OAuthTokens tokens = Helper.Configuration.GetTwitterTokens();
            SearchOptions options = new SearchOptions();
            options.NumberPerPage = 5;
            options.ResultType = SearchOptionsResultType.Recent;
            options.IncludeEntities = false;

            if (!ScreenName.StartsWith("@"))
                ScreenName = "@" + ScreenName;

            TwitterResponse<TwitterSearchResultCollection> searchResponse = TwitterSearch.Search(tokens, ScreenName, options);

            Model.Twitter.MentionRoot mentinoRoot = new Model.Twitter.MentionRoot();
            Model.Twitter.Mention mention;

            mentinoRoot.results = new List<Model.Twitter.Mention>();

            if (searchResponse == null)
            {
                return mentinoRoot;
            }
            if (searchResponse.ResponseObject == null)
            {
                mentinoRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.MentionRoot>(searchResponse.Content, new Server.Model.Twitter.MentionRootConverter());
            }
            else
            {
                foreach (TwitterSearchResult searchResult in searchResponse.ResponseObject)
                {
                    mention = new Model.Twitter.Mention();
                    mention.from_user = searchResult.FromUserDisplayName;
                    mention.from_user_id = Convert.ToDecimal(searchResult.FromUserId);
                    mention.from_user_name = searchResult.FromUserScreenName;
                    mention.id = searchResult.Id;
                    mention.profile_image_url = searchResult.ProfileImageLocation;
                    mention.text = searchResult.Text;

                    mentinoRoot.results.Add(mention);
                }
            }
            return mentinoRoot;
        }
    }
}
