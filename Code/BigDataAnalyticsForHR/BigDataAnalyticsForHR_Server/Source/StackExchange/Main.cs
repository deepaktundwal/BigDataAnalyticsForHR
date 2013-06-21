using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Model.StackExchange;

namespace Server.Source.StackExchange
{
    public class Main
    {

        public UserRoot GetUserDetail(UserToSearch ObjUserToSearch)
        {
            string UserName = ObjUserToSearch.f_name;
            if (ObjUserToSearch.m_name != null)
                if(ObjUserToSearch.m_name.Length > 0)
                    UserName += " " + ObjUserToSearch.m_name;

            UserName += " " + ObjUserToSearch.l_name;

            //JObject UserDetail = new JObject();
            //UserScoreDetailRoot ObjUserScoreDetailRoot = new UserScoreDetailRoot();
            UserRoot stExUserRoot = new UserRoot();
            stExUserRoot.items = new List<Model.StackExchange.User>();
            //JArray ArrUserScoreDetail = new JArray();
            JObject JsonObject = new JObject();

            Constants.api_site_parameter = "stackoverflow";
            JsonObject = GetUserData(UserName, "");
            String strJson = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            UserRoot user = JsonConvert.DeserializeObject<UserRoot>(strJson, new UserRootConverter());

            //UserScoreDetail userScrDetail;
            UserScore ObjUserScore;
            NetworkUserRoot networkUser = null;

            if (user != null)
            {
                //ObjUserScoreDetailRoot.lstUser = new List<User>();
                //ObjUserScoreDetailRoot.lstUserScoreDetail = new List<UserScoreDetail>();
                //userScrDetail = new UserScoreDetail();
                //userScrDetail.lstUserScore = new List<UserScore>();

                if (!Constants.IsNetworkSiteLoaded)
                {
                    LoadStackExchangeNetworkSites();
                }
                if (user.items.Count > 1)
                {
                    foreach (Server.Model.StackExchange.User useritem in user.items)
                    {
                        //ObjUserScore = new UserScore();
                        //userScrDetail = new UserScoreDetail();
                        //userScrDetail.lstUserScore = new List<UserScore>();

                        //ObjUserScore.reputation = useritem.reputation;
                        //ObjUserScore.badge_count = useritem.badge_counts;
                        //userScrDetail.display_name = useritem.display_name;
                        //userScrDetail.profile_image = useritem.profile_image;
                        //userScrDetail.user_id = useritem.user_id;
                        //userScrDetail.account_id = useritem.account_id;
                        //userScrDetail.location = useritem.location;
                        
                        //userScrDetail.lstUserScore.Add(ObjUserScore);
                        ////String strSerialized = JsonConvert.SerializeObject(userScrDetail);
                        //ObjUserScoreDetailRoot.lstUser.Add(useritem);
                        //ObjUserScoreDetailRoot.UserScoreDetail.Add(userScrDetail);
                        stExUserRoot.items.Add(useritem);
                    }
                }
                if (user.items.Count == 1)
                {
                    int AccountId = user.items[0].account_id;
                    int UserId = user.items[0].user_id;
                    //userScrDetail.display_name = user.items[0].display_name;
                    //userScrDetail.profile_image = user.items[0].profile_image;
                    //userScrDetail.location = user.items[0].location;

                    JsonObject = GetUserAssociatedAccounts(AccountId);
                    String strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
                    networkUser = JsonConvert.DeserializeObject<NetworkUserRoot>(strUserData, new NetworkUserRootConverter());

                    Model.StackExchange.User stExUser = user.items[0];

                    if (networkUser != null)
                    {
                        UserRoot SubSiteUser;
                        AnswerRoot AnswerRootData;
                        Tag_ScoreRoot Tag_ScoreRootData;
                        stExUser.userScore = new UserScore();

                        foreach (Network_Users NUser in networkUser.items)
                        {
                            if (Constants.ApiParameterDict.ContainsKey(NUser.site_name) && !NUser.site_name.StartsWith("Meta "))
                            {
                                Constants.api_site_parameter = (String)Constants.ApiParameterDict[NUser.site_name];
                            }
                            else
                                continue;

                            // Get user details from sister sites.
                            JsonObject = GetUserDataById(UserId);
                            strJson = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
                            SubSiteUser = JsonConvert.DeserializeObject<UserRoot>(strJson, new UserRootConverter());

                            if (SubSiteUser.items.Count == 0)
                                continue;

                            //ObjUserScore = new UserScore();
                            //stExUser.userScore = new UserScore();
                            stExUser.userScore.reputation += SubSiteUser.items[0].reputation;
                            if (SubSiteUser.items[0].badge_counts != null)
                            {
                                if (stExUser.userScore.badge_count == null)
                                    stExUser.userScore.badge_count = SubSiteUser.items[0].badge_counts;

                                stExUser.userScore.badge_count.gold += SubSiteUser.items[0].badge_counts.gold;
                                stExUser.userScore.badge_count.silver += SubSiteUser.items[0].badge_counts.silver;
                                stExUser.userScore.badge_count.bronze += SubSiteUser.items[0].badge_counts.bronze;
                            }

                            //if (ObjUserToSearch.skills == null)
                            //{
                                JsonObject = GetUserTagList(UserId);
                                strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
                                TagRoot tagData = JsonConvert.DeserializeObject<TagRoot>(strUserData, new TagRootConverter());

                                AnswerRootData = new AnswerRoot();
                                Tag_ScoreRootData = new Tag_ScoreRoot();
                                Dictionary<int, string> DictAnswerID = new Dictionary<int, string>();
                                if (ObjUserToSearch.skills.Length > 0)
                                {
                                    //ObjUserToSearch.skills = ObjUserToSearch.skills.ToUpper();
                                    string[] ArrUserSkills = ObjUserToSearch.skills.ToUpper().Split(new char[] { ',' });

                                    foreach (Tag tag in tagData.items)
                                    {
                                        // Get answer details only for given user skills.
                                        if (ArrUserSkills.Contains(tag.name.Trim()))
                                        {
                                            GetTagTopAnswer_By_User(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData, ref DictAnswerID);
                                            GetTagTopAnswer_OverAll(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData);

                                            //ObjUserScoreDetailRoot.lstAnswer = AnswerRootData.items;
                                            //ObjUserScoreDetailRoot.lstTag_Score = Tag_ScoreRootData.items;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (Tag tag in tagData.items)
                                    {
                                        GetTagTopAnswer_By_User(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData, ref DictAnswerID);
                                        GetTagTopAnswer_OverAll(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData);

                                            //ObjUserScoreDetailRoot.lstAnswer = AnswerRootData.items;
                                            //ObjUserScoreDetailRoot.lstTag_Score = Tag_ScoreRootData.items;
                                    }
                                }
                            //}
                            //else
                            //{
                            //    AnswerRootData = GetTagTopAnswer_By_User(ObjUserToSearch.skills, ref JsonObject, ObjUserScore, UserId, ref strUserData);
                            //    Tag_ScoreRootData = GetTagTopAnswer_OverAll(ObjUserToSearch.skills, ref JsonObject, ObjUserScore, UserId, ref strUserData);
                               
                            //    ObjUserScoreDetailRoot.lstAnswer = AnswerRootData.items;
                            //    ObjUserScoreDetailRoot.lstTag_Score = Tag_ScoreRootData.items;
                            //}

                            //ObjUserScoreDetailRoot.lstNetwork_Users.Add(NUser);
                            //userScrDetail.lstUserScore.Add(ObjUserScore);
                        }
                    }
                    //String strSerialized = JsonConvert.SerializeObject(userScrDetail);
                    //ObjUserScoreDetailRoot.UserScoreDetail = userScrDetail;
                    stExUserRoot.items.Add(stExUser);

                }
            }
            //UserDetail.Add("UserDetail", ArrUserScoreDetail);
            //return ObjUserScoreDetailRoot;
            return stExUserRoot;
        }

        public Model.StackExchange.User GetUserDetailById(UserToSearch ObjUserToSearch)
        {
            //UserScoreDetailRoot ObjUserScoreDetailRoot = new UserScoreDetailRoot();
            //UserScoreDetail userScrDetail = new UserScoreDetail();
            //userScrDetail.lstUserScore = new List<UserScore>();
            JObject JsonObject = new JObject();

            //UsersRoot stExUserRoot = new UsersRoot();
            //stExUserRoot.items = new List<Model.StackExchange.User>();

            int AccountId = ObjUserToSearch.stackEx_account_id;
            int UserId = ObjUserToSearch.stackEx_user_id;
            //userScrDetail.display_name = ObjUser.display_name;
            //userScrDetail.profile_image = ObjUser.profile_image;
            //userScrDetail.location = ObjUser.location;

            Constants.api_site_parameter = "stackoverflow";
            JsonObject = GetUserDataById(AccountId);
            String strJson = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            UserRoot user = JsonConvert.DeserializeObject<UserRoot>(strJson, new UserRootConverter());


            JsonObject = GetUserAssociatedAccounts(AccountId);
            String strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            NetworkUserRoot networkUser = JsonConvert.DeserializeObject<NetworkUserRoot>(strUserData, new NetworkUserRootConverter());

            Model.StackExchange.User stExUser = new Model.StackExchange.User();
            stExUser.userScore = new UserScore();

            //ObjUserScoreDetailRoot.lstUser.Add(ObjUser);
            if (user != null)
            {
                if(user.items.Count > 0)
                    stExUser = user.items[0];
            }
            if (networkUser != null)
            {
                UserRoot SubSiteUser;
                AnswerRoot AnswerRootData;
                Tag_ScoreRoot Tag_ScoreRootData;

                foreach (Network_Users NUser in networkUser.items)
                {
                    if (Constants.ApiParameterDict.ContainsKey(NUser.site_name) && !NUser.site_name.StartsWith("Meta "))
                    {
                        Constants.api_site_parameter = (String)Constants.ApiParameterDict[NUser.site_name];
                    }
                    else
                        continue;

                    // Get user details from sister sites.
                    JsonObject = GetUserDataById(UserId);
                    strJson = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
                    SubSiteUser = JsonConvert.DeserializeObject<UserRoot>(strJson, new UserRootConverter());

                    if (SubSiteUser.items.Count == 0)
                        continue;

                    //UserScore ObjUserScore = new UserScore();
                    stExUser.userScore.reputation += SubSiteUser.items[0].reputation;
                    if (SubSiteUser.items[0].badge_counts != null)
                    {
                        if (stExUser.userScore.badge_count == null)
                            stExUser.userScore.badge_count = SubSiteUser.items[0].badge_counts;

                        stExUser.userScore.badge_count.gold += SubSiteUser.items[0].badge_counts.gold;
                        stExUser.userScore.badge_count.silver += SubSiteUser.items[0].badge_counts.silver;
                        stExUser.userScore.badge_count.bronze += SubSiteUser.items[0].badge_counts.bronze;
                    }

                    JsonObject = GetUserTagList(UserId);
                    strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
                    TagRoot tagData = JsonConvert.DeserializeObject<TagRoot>(strUserData, new TagRootConverter());

                    AnswerRootData = new AnswerRoot();
                    Tag_ScoreRootData = new Tag_ScoreRoot();

                    Dictionary<int, string> DictAnswerID = new Dictionary<int, string>();
                    if (ObjUserToSearch.skills.Length > 0)
                    {
                        ObjUserToSearch.skills = ObjUserToSearch.skills.ToUpper();
                        string[] ArrUserSkills = ObjUserToSearch.skills.Split(new char[] { ',' });

                        foreach (Tag tag in tagData.items)
                        {
                            // Get answer details only for given user skills.
                            if (ArrUserSkills.Contains(tag.name.Trim()))
                            {
                                GetTagTopAnswer_By_User(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData, ref DictAnswerID);
                                GetTagTopAnswer_OverAll(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData);

                                //ObjUserScoreDetailRoot.lstAnswer = AnswerRootData.items;
                                //ObjUserScoreDetailRoot.lstTag_Score = Tag_ScoreRootData.items;
                            }
                        }
                    }
                    else
                    {
                        foreach (Tag tag in tagData.items)
                        {
                            GetTagTopAnswer_By_User(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData, ref DictAnswerID);
                            GetTagTopAnswer_OverAll(tag.name, ref stExUser, ref JsonObject, stExUser.userScore, UserId, ref strUserData);

                            //ObjUserScoreDetailRoot.lstAnswer = AnswerRootData.items;
                            //ObjUserScoreDetailRoot.lstTag_Score = Tag_ScoreRootData.items;
                        }
                    }

                    //userScrDetail.lstUserScore.Add(ObjUserScore);
                    //ObjUserScoreDetailRoot.lstNetwork_Users.Add(NUser);
                }
            }
            //ObjUserScoreDetailRoot.UserScoreDetail = userScrDetail;
            //stExUserRoot.items.Add(stExUser);

            return stExUser;
        }

        public QuestionRoot GetUserQuestions(int UserId)
        {
            UserQuestions UserQuestion = new UserQuestions();
            JObject ObjQuestion = UserQuestion.GetUserQuestions(UserId);

            String strQuestionData = JsonConvert.SerializeObject(ObjQuestion, Formatting.Indented);
            QuestionRoot Question = JsonConvert.DeserializeObject<QuestionRoot>(strQuestionData, new QuestionRootConverter());
            return Question;
        }

        private Tag_ScoreRoot GetTagTopAnswer_OverAll(string Skills, ref Model.StackExchange.User ObjUser, ref JObject JsonObject, UserScore ObjUserScore, int UserId, ref String strUserData)
        {
            Tag_ScoreRoot Tag_ScoreRootData;
            // Get user tags top answers from sister sites.
            JsonObject = GetTopAnswersOnTag(Skills);
            strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            Tag_ScoreRootData = JsonConvert.DeserializeObject<Tag_ScoreRoot>(strUserData, new Tag_ScoreRootConverter());

            if (Tag_ScoreRootData.items != null)
            {
                foreach (Tag_Score tagScr in Tag_ScoreRootData.items)
                {
                    ObjUser.lstTag_Score.Add(tagScr);
                    if (tagScr.user.user_id == UserId)
                        ObjUserScore.score_TopAnsOnTag += tagScr.score;
                }
            }

            return Tag_ScoreRootData;
        }

        private AnswerRoot GetTagTopAnswer_By_User(string Skills, ref Model.StackExchange.User ObjUser, ref JObject JsonObject, UserScore ObjUserScore, int UserId, ref String strUserData, ref Dictionary<int, string> DictAnswerID)
        {
            AnswerRoot AnswerRootData;
            // Get user tags top answers from sister sites.
            JsonObject = GetUserTagTopAnswers(UserId, Skills);
            strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            AnswerRootData = JsonConvert.DeserializeObject<AnswerRoot>(strUserData, new AnswerRootConverter());

            if (AnswerRootData.items != null)
            {

                foreach (Answer Ans in AnswerRootData.items)
                {
                    if (DictAnswerID.ContainsKey(Ans.answer_id))
                        continue;
                    else
                        DictAnswerID.Add(Ans.answer_id, Ans.title);

                    ObjUserScore.count_user_tags ++;
                    ObjUser.lstAnswer.Add(Ans);
                    ObjUserScore.score_User_Tag_Top_Ans += Ans.score;
                    if (Ans.is_accepted)
                    {
                        ObjUserScore.is_acceptedCount++;
                        ObjUser.lstAcceptedAnswer.Add(Ans);
                    }
                }
            }
            return AnswerRootData;
        }

        //public List<UserScoreDetail> GetUserDetailById(int UserId, int AccountId, string Skills)
        //{
        //    List<UserScoreDetail> UserDetail = new List<UserScoreDetail>();
        //    JObject JsonObject = new JObject();
        //    UserScoreDetail userScrDetail;
        //    UserScore ObjUserScore;
        //    Model.NetworkUserRoot networkUser = null;
        //    String strJson;

        //    userScrDetail = new UserScoreDetail();
        //    userScrDetail.lstUserScore = new List<UserScore>();

        //    if (!Constants.IsNetworkSiteLoaded)
        //    {
        //        LoadStackExchangeNetworkSites();
        //    }

        //    JsonObject = GetUserAssociatedAccounts(AccountId);
        //    String strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
        //    networkUser = JsonConvert.DeserializeObject<Model.NetworkUserRoot>(strUserData, new NetworkUserRootConverter());

        //    if (networkUser != null)
        //    {
        //        Model.UserRoot SubSiteUser;
        //        Model.AnswerRoot AnswerRootData;
        //        Model.Tag_ScoreRoot Tag_ScoreRootData;

        //        foreach (Network_Users NUser in networkUser.items)
        //        {
        //            if (Constants.ApiParameterDict.ContainsKey(NUser.site_name))
        //            {
        //                Constants.api_site_parameter = (String)Constants.ApiParameterDict[NUser.site_name];
        //            }
        //            else
        //                continue;

        //            // Get user details from sister sites.
        //            JsonObject = GetUserDataById(UserId);
        //            strJson = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
        //            SubSiteUser = JsonConvert.DeserializeObject<Model.UserRoot>(strJson, new UserConverter());

        //            ObjUserScore = new UserScore();
        //            userScrDetail.display_name = SubSiteUser.items[0].display_name;
        //            userScrDetail.profile_image = SubSiteUser.items[0].profile_image;
        //            userScrDetail.user_id = UserId;
        //            userScrDetail.account_id = AccountId;
        //            ObjUserScore.reputation = SubSiteUser.items[0].reputation;
        //            ObjUserScore.badge_count = SubSiteUser.items[0].badge_counts;

        //            if (Skills.Length == 0)
        //            {
        //                JsonObject = GetUserTagList(UserId);
        //                strUserData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
        //                Model.TagRoot tagData = JsonConvert.DeserializeObject<Model.TagRoot>(strUserData, new TagRootConverter());

        //                foreach (Tag tag in tagData.items)
        //                {
        //                    AnswerRootData = GetTagTopAnswer_By_User(tag.name, ref JsonObject, ObjUserScore, UserId, ref strUserData);
        //                    Tag_ScoreRootData = GetTagTopAnswer_OverAll(tag.name, ref JsonObject, ObjUserScore, UserId, ref strUserData);
        //                }
        //            }
        //            else
        //            {
        //                AnswerRootData = GetTagTopAnswer_By_User(Skills, ref JsonObject, ObjUserScore, UserId, ref strUserData);
        //                Tag_ScoreRootData = GetTagTopAnswer_OverAll(Skills, ref JsonObject, ObjUserScore, UserId, ref strUserData);
        //            }

        //            userScrDetail.lstUserScore.Add(ObjUserScore);
        //        }
        //    }
        //    UserDetail.Add(userScrDetail);

        //    return UserDetail;
        //}


        private void LoadStackExchangeNetworkSites()
        {
            Constants.ApiParameterDict = new JObject();
            JObject JsonObject = GetStackExchangeSites();
            Constants.IsNetworkSiteLoaded = true;

            String strSiteData = JsonConvert.SerializeObject(JsonObject, Formatting.Indented);
            SiteRoot siteData = JsonConvert.DeserializeObject<SiteRoot>(strSiteData, new SiteRootConverter());

            foreach (Site networkSite in siteData.items)
            {
                Constants.ApiParameterDict.Add(networkSite.name, networkSite.api_site_parameter);
            }
        }

        public JObject GetUserData(String UserName, String EmailId)
        {
            User User = new User();
            return User.GetUserData(UserName);
        }

        public JObject GetUserDataById(int UserId)
        {
            User_By_Ids User_By_Ids = new User_By_Ids();
            return User_By_Ids.GetUserData(UserId);
        }

        public JObject GetUserAssociatedAccounts(int AccountId)
        {
            UserAssociated User = new UserAssociated();
            return User.GetUserAssociatedAccounts(AccountId);
        }

        public JObject GetUserAnswers(int UserId)
        {
            UserAnswers UserAnswer = new UserAnswers();
            return UserAnswer.GetUserAnswers(UserId);
        }

        public JObject GetUserTagList(int UserId)
        {
            UserTags userTags = new UserTags();
            return userTags.GetUserTagList(UserId);
        }

        public JObject GetUserTagTopAnswers(int UserId, String Tags)
        {
            User_Tags_TopAnswers userTagsTopAnswers = new User_Tags_TopAnswers();
            return userTagsTopAnswers.GetUserTagsTopAnswers(UserId, Tags);
        }

        public JObject GetStackExchangeSites()
        {
            Sites sites = new Sites();
            return sites.GetStackExchangeSites();
        }

        public JObject GetTopAnswersOnTag(String Tags)
        {
            TopAnswerersOnTag TopAnsOnTag = new TopAnswerersOnTag();
            return TopAnsOnTag.GetTopAnswersOnTag(Tags);
        }
    }
}

        //private void Connect()
        //{
        //    try
        //    {
        //        String Url = PrepareUserUrl();
        //        //WebClient webClient = new WebClient();
        //        //webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
        //        //webClient.DownloadDataCompleted += webClient_DownloadDataCompleted;
        //        //webClient.DownloadStringAsync(new System.Uri(Url));

        //        var request = (HttpWebRequest)WebRequest.Create(Url);
        //        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        //        using (var response = request.GetResponse())
        //        using (var stream = response.GetResponseStream())
        //        using (var reader = new StreamReader(stream))
        //        {
        //            var json = reader.ReadToEnd();
        //            UpdateUserData(json);
        //        }

        //    }
        //    catch (Exception e)
        //    { }
        //}

        //void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {

        //    }
        //    else
        //    {
        //        UpdateUserData(e.Result);
        //    }
        //}

        //void webClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
               
        //    }
        //    else
        //    { 
        //        string result = System.Text.Encoding.UTF8.GetString(e.Result);
        //        UpdateUserData(result);
        //    }
        //}
