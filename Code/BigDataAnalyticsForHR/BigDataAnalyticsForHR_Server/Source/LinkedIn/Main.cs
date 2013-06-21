using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Source.LinkedIn
{
    public class Main
    {
        public const string SearchUrl = "http://api.linkedin.com/v1/people-search";
        public const string PeopleUrl = "http://api.linkedin.com/v1/people/";
        public const string GroupUrl = "http://api.linkedin.com/v1/groups/";

        public Model.LinkedIn.PeopleRoot SearchUser(String FName, String MName, String LName, String CurCompanyName, String Skills)
        {
            //Model.LinkedIn.User user1 = GetUserDetail("SHdKZUiQ-a", Skills);

            OAuthTokens tokens = Helper.Configuration.GetLinkedInTokens();
            OAuthLinkedIn _oauth = new OAuthLinkedIn(tokens);
            Model.LinkedIn.PeopleRoot userRoot = new Model.LinkedIn.PeopleRoot();

            String FieldSelectors = ":(people:(id,first-name,last-name,headline,location:(name,country:(code),postal-code),picture-url,specialties,positions,public-profile-url))";
            String PostData = "first-name=" + FName;

            if (MName != null)
                if (MName.Trim().Length > 0)
                    PostData += "&maiden-name=" + MName;

            if (LName != null)
                if (LName.Trim().Length > 0)
                    PostData += "&last-name=" + LName;

            if (CurCompanyName != null)
                if (CurCompanyName.Trim().Length > 0)
                    PostData += "&current-company=" + CurCompanyName;

            PostData += "&format=json";

            string Url = SearchUrl + FieldSelectors + "?" + PostData;

            string result = _oauth.oAuthWebRequest(OAuthLinkedIn.Method.GET, Url, null);

            if (result == null)
                return userRoot;

            if (result.Length <= 0)
                return userRoot;

            userRoot = JsonConvert.DeserializeObject<Model.LinkedIn.PeopleRoot>(result, new Model.LinkedIn.PeopleRootConverter());

            if (userRoot != null)
            {
                if (userRoot.people != null)
                {
                    // Get people & group detail.
                    if (userRoot.people._count == 1 || userRoot.people._total == 1)
                    {
                        Model.LinkedIn.User user = GetUserDetail(userRoot.people.values[0].id, Skills);
                        user.specialties = userRoot.people.values[0].specialties;
                        user.publicProfileUrl = userRoot.people.values[0].publicProfileUrl;

                        userRoot.people.values[0] = user;
                    }
                }
            }
            return userRoot;
        }

        public Model.LinkedIn.User GetUserDetail(String UserId, String Skills)
        {
            OAuthTokens tokens = Helper.Configuration.GetLinkedInTokens();
            OAuthLinkedIn _oauth = new OAuthLinkedIn(tokens);
            Model.LinkedIn.User user = new Model.LinkedIn.User();

            String FieldSelectors = ":(id,first-name,last-name,maiden-name,picture-url,headline,location:(name,country:(code),postal-code),industry,num-recommenders,connections,summary,specialties,interests,honors,positions,associations,publications,patents,languages,skills,certifications,educations,courses,volunteer,recommendations-received,following,job-bookmarks,suggestions,date-of-birth,email-address,phone-numbers,im-accounts,twitter-accounts,group-memberships)";

            string Url = PeopleUrl + "id=" + UserId + FieldSelectors;
            string PostData = "format=json";

            Url += "?" + PostData;
            string result = _oauth.oAuthWebRequest(OAuthLinkedIn.Method.GET, Url, null);

            if (result == null)
                return user;

            if (result.Length <= 0)
                return user;

            user = JsonConvert.DeserializeObject<Model.LinkedIn.User>(result, new Model.LinkedIn.UserConverter());

            //if (user != null)
            //{
            //    if (user.groupMemberships != null)
            //    {
            //        //foreach (Model.LinkedIn.GroupMembershipsValues groupVals in user.groupMemberships.values)
            //        for (int i = 0; i < user.groupMemberships.values.Count; i++)
            //        {
            //            //System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
            //            //bw.DoWork += new System.ComponentModel.DoWorkEventHandler(GetGroupDetailWorkerThread);
            //            //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(GetGroupDetailWorkerThreadCompleted);
            //            //bw.RunWorkerAsync(user);
                        
            //            Model.LinkedIn.Group group = GetGroupDetail(UserId, user.groupMemberships.values[i]._key);

            //            group.groupPostsbyUser = GetGroupPostsAsCreator(UserId, user.groupMemberships.values[i]._key);
                        
            //            Model.LinkedIn.PostRoot postRoot = GetGroupPostsAsCommenter(UserId, user.groupMemberships.values[i]._key);

            //            if (postRoot != null)
            //            {
            //                if (group.groupPostsbyUser == null)
            //                    group.groupPostsbyUser = new Model.LinkedIn.PostRoot();

            //                foreach (Model.LinkedIn.Post grouppost in postRoot.values)
            //                {
            //                    group.groupPostsbyUser.values.Add(grouppost);
            //                }
            //            }
            //            user.groupMemberships.values[i].group = group;
            //        }
            //    }
            //}
            return user;
        }

        //static void GetGroupDetailWorkerThread(object sender, System.ComponentModel.DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        Model.LinkedIn.User user = (Model.LinkedIn.User)e.Argument;
        //    }
        //    catch (Exception Ex)
        //    {
        //    }
        //}

        //static void GetGroupDetailWorkerThreadCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        //{

        //}

        public Model.LinkedIn.GroupWithPosts GetGroupDetailWithPosts(String UserId, String GroupId)
        {
            Model.LinkedIn.GroupWithPosts groupWithPosts = new Model.LinkedIn.GroupWithPosts();

            groupWithPosts.group = GetGroupDetail(UserId, GroupId);
            groupWithPosts.groupPostsbyUser = GetGroupPostsAsCreator(UserId, GroupId);
            Model.LinkedIn.PostRoot postRoot = GetGroupPostsAsCommenter(UserId, GroupId);

            if (postRoot != null)
            {
                if (postRoot.values != null)
                {
                    if (groupWithPosts.groupPostsbyUser == null)
                    {
                        groupWithPosts.groupPostsbyUser = new Model.LinkedIn.PostRoot();
                    }
                    if (groupWithPosts.groupPostsbyUser.values == null)
                    {
                        groupWithPosts.groupPostsbyUser.values = new List<Model.LinkedIn.Post>();
                    }

                    foreach (Model.LinkedIn.Post grouppost in postRoot.values)
                    {
                        groupWithPosts.groupPostsbyUser.values.Add(grouppost);
                    }
                }
            }

            return groupWithPosts;
        }

        public Model.LinkedIn.Group GetGroupDetail(String UserId, String GroupId)
        {
            OAuthTokens tokens = Helper.Configuration.GetLinkedInTokens();
            OAuthLinkedIn _oauth = new OAuthLinkedIn(tokens);
            Model.LinkedIn.Group group = new Model.LinkedIn.Group();

            String FieldSelectors = ":(id,name,site-group-url,short-description,description,category,large-logo-url,small-logo-url)";

            string Url = GroupUrl + GroupId + FieldSelectors;
            string PostData = "format=json";

            Url += "?" + PostData;
            string result = _oauth.oAuthWebRequest(OAuthLinkedIn.Method.GET, Url, null);

            if (result == null)
                return group;

            if (result.Length <= 0)
                return group;

            group = JsonConvert.DeserializeObject<Model.LinkedIn.Group>(result, new Model.LinkedIn.GroupConverter());

            return group;
        }

        public Model.LinkedIn.PostRoot GetGroupPostsAsCreator(String UserId, String GroupId)
        {
            // http://api.linkedin.com/v1/people/id=SHdKZUiQ-a/group-memberships/2848695/posts:(creator:(first-name,last-name,picture-url),title,summary,creation-timestamp,likes,comments,attachment:(image-url,content-domain,content-url,title,summary))

            OAuthTokens tokens = Helper.Configuration.GetLinkedInTokens();
            OAuthLinkedIn _oauth = new OAuthLinkedIn(tokens);
            Model.LinkedIn.PostRoot postRoot = new Model.LinkedIn.PostRoot();

            String FieldSelectors = ":(creator:(first-name,last-name,picture-url),title,summary,creation-timestamp,likes,comments,attachment:(image-url,content-domain,content-url,title,summary))";

            string Url = PeopleUrl +
                "id=" + UserId +
                "/group-memberships/" + GroupId +
                "/posts" + FieldSelectors;

            string PostData = "category=discussion&role=creator&format=json";

            Url += "?" + PostData;
            string result = _oauth.oAuthWebRequest(OAuthLinkedIn.Method.GET, Url, null);

            if (result == null)
                return postRoot;

            if (result.Length <= 0)
                return postRoot;

            postRoot = JsonConvert.DeserializeObject<Model.LinkedIn.PostRoot>(result, new Model.LinkedIn.PostRootConverter());

            return postRoot;
        }

        public Model.LinkedIn.PostRoot GetGroupPostsAsCommenter(String UserId, String GroupId)
        {
            // http://api.linkedin.com/v1/people/id=SHdKZUiQ-a/group-memberships/2848695/posts:(creator:(first-name,last-name,picture-url),title,summary,creation-timestamp,likes,comments,attachment:(image-url,content-domain,content-url,title,summary))

            OAuthTokens tokens = Helper.Configuration.GetLinkedInTokens();
            OAuthLinkedIn _oauth = new OAuthLinkedIn(tokens);
            Model.LinkedIn.PostRoot postRoot = new Model.LinkedIn.PostRoot();

            String FieldSelectors = ":(creator:(first-name,last-name,picture-url),title,summary,creation-timestamp,likes,comments,attachment:(image-url,content-domain,content-url,title,summary))";

            string Url = PeopleUrl +
                "id=" + UserId +
                "/group-memberships/" + GroupId +
                "/posts" + FieldSelectors;

            string PostData = "category=discussion&role=commenter&format=json";

            Url += "?" + PostData;
            string result = _oauth.oAuthWebRequest(OAuthLinkedIn.Method.GET, Url, null);

            if (result == null)
                return postRoot;

            if (result.Length <= 0)
                return postRoot;

            postRoot = JsonConvert.DeserializeObject<Model.LinkedIn.PostRoot>(result, new Model.LinkedIn.PostRootConverter());

            return postRoot;
        }

    }
}
