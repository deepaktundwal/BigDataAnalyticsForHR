using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//using Server.Source.StackExchange;
//using Server.Model.StackExchange;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;

namespace WCFServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BigDataAnalyser" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BigDataAnalyser.svc or BigDataAnalyser.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BigDataAnalyser : IBigDataAnalyser
    {
        public void DoWork()
        {
        }

        #region Stack Exchange

        public String GetUserDetail(Stream userToSearch)
        {
            byte[] resultBytes = Encoding.UTF8.GetBytes("0");
            //JObject ResponseJson = new JObject();
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());
                Server.Source.StackExchange.Main stackExchange = new Server.Source.StackExchange.Main();
                Server.Model.StackExchange.UserRoot ObjUserScoreDetailRoot = stackExchange.GetUserDetail(user);
                strSerialized = JsonConvert.SerializeObject(ObjUserScoreDetailRoot);
                strSerialized = Helper.GetUTF8String(strSerialized);

                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String GetUserDetailById(Stream userToSearch)
        {
            byte[] resultBytes = Encoding.UTF8.GetBytes("0");
            //JObject ResponseJson = new JObject();
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());
                Server.Source.StackExchange.Main stackExchange = new Server.Source.StackExchange.Main();
                Server.Model.StackExchange.User ObjUser = stackExchange.GetUserDetailById(user);
                strSerialized = JsonConvert.SerializeObject(ObjUser);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String GetUserQuestion(Stream userToSearch)
        {
            //JObject ResponseJson = new JObject();
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());
                Server.Source.StackExchange.Main stackExchange = new Server.Source.StackExchange.Main();
                Server.Model.StackExchange.QuestionRoot ObjQuestionRoot = stackExchange.GetUserQuestions(user.stackEx_user_id);
                strSerialized = JsonConvert.SerializeObject(ObjQuestionRoot);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        //private void GetUserDetail()
        //{
        //    UserToSearch userToSearch = new UserToSearch();
        //    //userToSearch.skills = "c#, windows-phone-7";
        //    userToSearch.f_name = "johngb";
        //    //userToSearch.l_name = "Tundwal";
        //    Main stackExchange = new Main();
        //    //List<UserScoreDetail> UserDetail = stackExchange.GetUserDetail(userToSearch);
        //    //List<UserScoreDetail> UserDetail = stackExchange.GetUserDetailById(1852224, 2080032, "c#, windows-phone-7");
        //}

        //public void GetAssociatedAccounts(String AccountId)
        //{
        //    //String strUserData = JsonConvert.SerializeObject(ObjuserData, Formatting.Indented);
        //    //BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot user = JsonConvert.DeserializeObject<BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot>(strUserData, new UserConverter());
        //    //String AccountId = Convert.ToString(user.items[0].account_id);

        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetUserAssociatedAccounts(Convert.ToInt32(AccountId));

        //    //String strUserData = JsonConvert.SerializeObject(ObjuserData, Formatting.Indented);
        //    //BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot user = JsonConvert.DeserializeObject<BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot>(strUserData, new UserConverter());
        //    //String AccountId = Convert.ToString(user.UserDetail[0].items[0].account_id);
        //    ////String UserId = (String)((JObject)((JArray)((JObject)((JArray)(JArray)ObjuserData["UserDetail"])[0])["items"])[0])["user_id"];
        //    //StackExchangeMain stackExchange = new StackExchangeMain();
        //    //Newtonsoft.Json.Linq.JObject userData = stackExchange.GetUserAssociatedAccounts(AccountId);
        //}

        //public void GetUserAnswers(String UserId)
        //{
        //    //String strUserData = JsonConvert.SerializeObject(ObjuserData, Formatting.Indented);
        //    //BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot user = JsonConvert.DeserializeObject<BigDataAnalyticsForHR_WinForm_Server.Model.UserRoot>(strUserData, new UserConverter());
        //    //String UserId = Convert.ToString(user.items[0].user_id);
        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetUserAnswers(Convert.ToInt32(UserId));
        //}

        //public void GetUserTagList(String UserId)
        //{
        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetUserTagList(Convert.ToInt32(UserId));
        //}

        //public void GetStackExchangeSites()
        //{
        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetStackExchangeSites();
        //}

        //public void GetUserTagTopAnswers(String UserId, String Tags)
        //{
        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetUserTagTopAnswers(Convert.ToInt32(UserId), Tags);
        //}

        //public void GetTopAnswersOnTag(String Tags)
        //{
        //    Main stackExchange = new Main();
        //    Newtonsoft.Json.Linq.JObject userData = stackExchange.GetTopAnswersOnTag(Tags);
        //}

        #endregion Stack Exchange

        #region Twitter

        public String Twitter_SearchUser(Stream userToSearch)
        {
            //byte[] resultBytes = Encoding.UTF8.GetBytes("0");
            //JObject ResponseJson = new JObject();
            String strSerialized = "";
            //Stream stream = null;
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());

                string UserName = user.f_name;
                if (user.m_name != null)
                    if (user.m_name.Length > 0)
                        UserName += " " + user.m_name;

                UserName += " " + user.l_name;

                Server.Source.Twitter.Main twtMain = new Server.Source.Twitter.Main();
                Server.Model.Twitter.UsersRoot userRoot = twtMain.SearchUser(UserName, user.skills);

                strSerialized = JsonConvert.SerializeObject(userRoot);
                //stream = Helper.GenerateSampleStream(strSerialized);
                strSerialized = Helper.GetUTF8String(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String Twitter_UserTimeline(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());

                Server.Source.Twitter.Main twtMain = new Server.Source.Twitter.Main();
                Server.Model.Twitter.TwitterStatusRoot result = twtMain.UserTimeline(user.twitter_screenname, user.skills);
                strSerialized = JsonConvert.SerializeObject(result);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String Twitter_GetRetweets(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.Twitter.Timeline timeline = JsonConvert.DeserializeObject<Server.Model.Twitter.Timeline>(struserToSearch, new Server.Model.Twitter.TimelineConverter());

                Server.Model.Twitter.TwitterStatusRoot results = Server.Source.Twitter.Main.GetRetweets(timeline.screen_name, timeline.id);

                if (results != null)
                    timeline.tStatusRoot = results;

                strSerialized = JsonConvert.SerializeObject(timeline);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String Twitter_GetMentionList(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());

                Server.Model.Twitter.MentionRoot result = Server.Source.Twitter.Main.GetMentionList(user.twitter_screenname);
                strSerialized = JsonConvert.SerializeObject(result);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        #endregion Twitter

        #region LinkedIn

        public String LinkedIn_SearchUser(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());

                Server.Source.LinkedIn.Main lMain = new Server.Source.LinkedIn.Main();
                Server.Model.LinkedIn.PeopleRoot lPeople = lMain.SearchUser(user.f_name, user.m_name, user.l_name, user.curCompany_name, user.skills);

                strSerialized = JsonConvert.SerializeObject(lPeople);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String LinkedIn_UserDetail(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.StackExchange.UserToSearch user = JsonConvert.DeserializeObject<Server.Model.StackExchange.UserToSearch>(struserToSearch, new Server.Model.StackExchange.UserToSearchConverter());

                Server.Source.LinkedIn.Main lMain = new Server.Source.LinkedIn.Main();
                Server.Model.LinkedIn.User lUser = lMain.GetUserDetail(user.LinkedIn_user_id, user.skills);

                strSerialized = JsonConvert.SerializeObject(lUser);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        public String LinkedIn_GetGroupDetail(Stream userToSearch)
        {
            //Stream stream = null;
            String strSerialized = "";
            try
            {
                // convert Stream Data to StreamReader
                StreamReader reader = new StreamReader(userToSearch);
                String struserToSearch = reader.ReadToEnd();

                Server.Model.LinkedIn.GroupWithPosts groupwithPosts = JsonConvert.DeserializeObject<Server.Model.LinkedIn.GroupWithPosts>(struserToSearch, new Server.Model.LinkedIn.GroupWithPostsConverter()); 

                Server.Source.LinkedIn.Main lMain = new Server.Source.LinkedIn.Main();
                Server.Model.LinkedIn.GroupWithPosts result = lMain.GetGroupDetailWithPosts(groupwithPosts.userId, groupwithPosts.groupId);

                if (result != null)
                {
                    groupwithPosts.group = result.group;
                    groupwithPosts.groupPostsbyUser = result.groupPostsbyUser;
                }
                strSerialized = JsonConvert.SerializeObject(groupwithPosts);
                strSerialized = Helper.GetUTF8String(strSerialized);
                //stream = Helper.GenerateSampleStream(strSerialized);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            //return stream;
            return strSerialized;
        }

        #endregion
    }
}
