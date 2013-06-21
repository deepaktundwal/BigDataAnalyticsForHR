using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Server.Model.StackExchange;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BigDataAnalyticsForHR
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class UserDetail : BigDataAnalyticsForHR.Common.LayoutAwarePage
    {
        HttpClient httpClient;
        Server.Model.LinkedIn.PeopleRoot lPeoples;
        Server.Model.Twitter.UsersRoot twitterUserRoot;
        Server.Model.StackExchange.UserRoot stExRoot;

        //static int StackExSelectedIndex = -1;
        //static int SearchLeavel = -1; // 0: Twitter, 1: StackEx, 2: LinkedIn
        //static bool IsMultiUserResult = false;

        public UserDetail()
        {
            this.InitializeComponent();

            txtUserName.Text = Constants.ObjUserToSearch.f_name + " " + Constants.ObjUserToSearch.m_name + " " + Constants.ObjUserToSearch.l_name;
            txtTitle.Text = Constants.ObjUserToSearch.jobRole;
            lPeoples = null;
            twitterUserRoot = null;
            //IsMultiUserResult = false;
            //SearchLeavel = -1;
            //StackExSelectedIndex = -1;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ProggressBarVisible(true);
                lstOptions.IsEnabled = false;

                // Add this code.
                if (this.Frame != null)
                {
                    if (lstOptions.SelectedIndex == 0)
                    {
                        //this.Frame.Navigate(typeof(StackExchangeDetails));
                        await LoadTwitterData(TwitterAPIType.UserSearch, Constants.ServiceMethod_Twitter_SearchUser, Constants.ObjUserToSearch);

                        if (twitterUserRoot != null)
                        {
                            if (twitterUserRoot.items != null)
                            {
                                if (twitterUserRoot.items.Count == 1)
                                {
                                    await GetTwitterUserReTweets(Constants.ObjUserToSearch.skills);
                                    await GetTwitterMentionList();
                                }
                            }
                        }
                        BuildMultiUserResultList(SocialNetworkSite.Twitter);
                    }
                    if (lstOptions.SelectedIndex == 1)
                    {
                        //this.Frame.Navigate(typeof(StackExchangeDetails));
                        await LoadStackExData(StackExchangeAPIType.UserSearch, Constants.ServiceMethod_GetUserDetail, Constants.ObjUserToSearch);
                        BuildMultiUserResultList(SocialNetworkSite.StackExchange);
                    }
                    if (lstOptions.SelectedIndex == 2)
                    {
                        //this.Frame.Navigate(typeof(StackExchangeDetails));
                        await LoadLinkedInData(LinkedInAPIType.UserSearch, Constants.ServiceMethod_LinkedIn_SearchUser, Constants.ObjUserToSearch);
                        if (lPeoples != null)
                        {
                            await GetLinkedInGroupDetailWithComment(lPeoples);
                            BuildMultiUserResultList(SocialNetworkSite.LinkedIn);
                        }
                    }
                }
            }
            catch (Exception Ex)
            { ProggressBarVisible(false); }
            finally
            {
                ProggressBarVisible(false);
                lstOptions.IsEnabled = true;
            }
        }

        #region LinkedIn

        private async Task LoadLinkedInData(LinkedInAPIType APIType, string linkedInAPIUrl, object postData)
        {
            MessageDialog dlg = null;
            try
            {
                //ProggressBarVisible(true);
                //lstOptions.IsEnabled = false;
                Helpers.CreateHttpClient(ref httpClient);

                String strSerialized = JsonConvert.SerializeObject(postData);
                Stream stream = GenerateSampleStream(strSerialized);
                StreamContent streamContent = new StreamContent(stream);

                string resourceAddress = Constants.ServiceUrl + linkedInAPIUrl;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

                request.Content = streamContent;
                request.Headers.TransferEncodingChunked = true;
                //httpClient.Timeout = new TimeSpan(10000000);
                HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                await LinkedIn_Response(APIType, response);
            }
            catch (Exception Ex)
            {
                dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
            }
            finally
            {
                //ProggressBarVisible(false);
                //lstOptions.IsEnabled = true;
            }

            if (dlg != null)
                await dlg.ShowAsync();
        }

        private async Task LoadLinkedInUserDetails()
        {
            Server.Model.LinkedIn.User selectedUser = ((Server.Model.LinkedIn.User)((ListBoxItem)lstUserList.SelectedItem).Tag);
            Constants.ObjUserToSearch.LinkedIn_user_id = selectedUser.id;

            await LoadLinkedInData(LinkedInAPIType.UserDetail, Constants.ServiceMethod_LinkedIn_UserDetail, Constants.ObjUserToSearch);

            if (lPeoples != null)
            {
                await GetLinkedInGroupDetailWithComment(lPeoples);
            }

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(StackExchangeDetails));
            }
        }

        private async Task GetLinkedInGroupDetailWithComment(Server.Model.LinkedIn.PeopleRoot lPeoples)
        {
            if (lPeoples != null)
            {
                if (lPeoples.people != null)
                {
                    // Get people & group detail.
                    if (lPeoples.people._total == 1)
                    {
                        Server.Model.LinkedIn.User user = lPeoples.people.values[0];

                        if (user.groupMemberships != null)
                        {
                            for (int i = 0; i < user.groupMemberships.values.Count; i++)
                            {
                                //System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();
                                //bw.DoWork += new System.ComponentModel.DoWorkEventHandler(GetGroupDetailWorkerThread);
                                //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(GetGroupDetailWorkerThreadCompleted);
                                //bw.RunWorkerAsync(user);
                                Server.Model.LinkedIn.GroupWithPosts groupwithPosts = new Server.Model.LinkedIn.GroupWithPosts();
                                groupwithPosts.userId = user.id;
                                groupwithPosts.groupId = user.groupMemberships.values[i]._key;
                                groupwithPosts.groupIndex = i;
                                await LoadLinkedInData(LinkedInAPIType.GroupDetailWithComments, Constants.ServiceMethod_LinkedIn_GetGroupDetail, groupwithPosts);
                            }
                        }
                    }
                }
            }
        }

        async Task LinkedIn_Response(LinkedInAPIType APIType, HttpResponseMessage response)
        {
            String ResponseString = await response.Content.ReadAsStringAsync();
            ResponseString = Helpers.GetUTF8String(ResponseString);

            //Stream responseStream = await response.Content.ReadAsStreamAsync();
            //byte[] resbuffer = new byte[responseStream.Length];
            //responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
            //String ResponseString = Helpers.GetString(resbuffer);

            if (APIType == LinkedInAPIType.UserSearch)
            {
                lPeoples = JsonConvert.DeserializeObject<Server.Model.LinkedIn.PeopleRoot>(ResponseString, new Server.Model.LinkedIn.PeopleRootConverter());
            }
            else if (APIType == LinkedInAPIType.UserDetail)
            {
                Server.Model.LinkedIn.User user = JsonConvert.DeserializeObject<Server.Model.LinkedIn.User>(ResponseString, new Server.Model.LinkedIn.UserConverter());
                
                // Clear old records.
                lPeoples.people.values.Clear();
                lPeoples.people.values = null;
                lPeoples.people = null;
                lPeoples.people = new Server.Model.LinkedIn.People();
                lPeoples.people.values = new List<Server.Model.LinkedIn.User>();
                lPeoples.people.values.Add(user);
            }
            else if (APIType == LinkedInAPIType.GroupDetailWithComments)
            {
                Server.Model.LinkedIn.GroupWithPosts groupWithPosts =
                    JsonConvert.DeserializeObject<Server.Model.LinkedIn.GroupWithPosts>(ResponseString, new Server.Model.LinkedIn.GroupWithPostsConverter());

                if (groupWithPosts != null)
                {
                    lPeoples.people.values[0].groupMemberships.values[groupWithPosts.groupIndex].group = groupWithPosts.group;
                    lPeoples.people.values[0].groupMemberships.values[groupWithPosts.groupIndex].group.groupPostsbyUser = groupWithPosts.groupPostsbyUser;
                }
            }
        }

        #endregion LinkedIn

        #region Twitter

        //private async Task LoadTwitterData()
        //{
        //    MessageDialog dlg = null;
        //    try
        //    {
        //        ProggressBarVisible(true);
        //        lstOptions.IsEnabled = false;
        //        Helpers.CreateHttpClient(ref httpClient);

        //        String strSerialized = JsonConvert.SerializeObject(Constants.ObjUserToSearch);
        //        Stream stream = GenerateSampleStream(strSerialized);
        //        StreamContent streamContent = new StreamContent(stream);

        //        string resourceAddress = Constants.ServiceUrl + Constants.ServiceMethod_Twitter_SearchUser;
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

        //        request.Content = streamContent;
        //        request.Headers.TransferEncodingChunked = true;
        //        HttpResponseMessage response = await httpClient.SendAsync(request);
        //        await Helpers.Twitter_SerchUserResult(response);


        //        BuildMultiUserResultList(SocialNetworkSite.Twitter);
        //    }
        //    catch (Exception Ex)
        //    {
        //        dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
        //    }
        //    finally
        //    {
        //        ProggressBarVisible(false);
        //        lstOptions.IsEnabled = true;
        //    }

        //    if (dlg != null)
        //        await dlg.ShowAsync();
        //}

        private async Task LoadTwitterData(TwitterAPIType APIType, string TwitterAPIUrl, object postData)
        {
            MessageDialog dlg = null;
            try
            {
                //ProggressBarVisible(true);
                //lstOptions.IsEnabled = false;
                Helpers.CreateHttpClient(ref httpClient);

                String strSerialized = JsonConvert.SerializeObject(postData);
                Stream stream = GenerateSampleStream(strSerialized);
                StreamContent streamContent = new StreamContent(stream);

                string resourceAddress = Constants.ServiceUrl + TwitterAPIUrl;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

                request.Content = streamContent;
                request.Headers.TransferEncodingChunked = true;
                //httpClient.Timeout = new TimeSpan(10000000);
                HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                await Twitter_Response(APIType, response);
            }
            catch (Exception Ex)
            {
                dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
            }
            finally
            {
                //ProggressBarVisible(false);
                //lstOptions.IsEnabled = true;
            }

            if (dlg != null)
                await dlg.ShowAsync();
        }

        private async Task GetTwitterUserTimeLine()
        {
            if (twitterUserRoot != null)
            {
                if (twitterUserRoot.items != null)
                {
                    // Get user timeline.
                    Constants.ObjUserToSearch.twitter_screenname = twitterUserRoot.items[0].screen_name;
                    await LoadTwitterData(TwitterAPIType.UserTimeLine, Constants.ServiceMethod_Twitter_UserTimeline, Constants.ObjUserToSearch);
                }
            }
        }

        private async Task GetTwitterUserReTweets(String keyWords)
        {
            if (twitterUserRoot != null)
            {
                if (twitterUserRoot.items != null)
                {
                    if (keyWords.Length <= 0)
                        return;

                    // Split keywords.
                    string[] arrKeyWords = keyWords.Split(new char[] { ',' });
                    bool tweetContainsKeyword;

                    if (twitterUserRoot.items[0].statusRoot == null)
                        return;

                    for (int i = 0; i < twitterUserRoot.items[0].statusRoot.items.Count; i++)
                    {
                        Server.Model.Twitter.TwitterStatus tstatus = twitterUserRoot.items[0].statusRoot.items[i];
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

                            Server.Model.Twitter.Timeline timeline = new Server.Model.Twitter.Timeline();
                            timeline.id = tstatus.id;
                            timeline.screen_name = twitterUserRoot.items[0].screen_name;
                            timeline.tweetIndex = i;
                            await LoadTwitterData(TwitterAPIType.Retweets, Constants.ServiceMethod_Twitter_GetRetweets, timeline);
                        }
                    }
                }
            }
        }

        private async Task GetTwitterMentionList()
        {
            if (twitterUserRoot != null)
            {
                if (twitterUserRoot.items != null)
                {
                    Constants.ObjUserToSearch.twitter_screenname = twitterUserRoot.items[0].screen_name;
                    await LoadTwitterData(TwitterAPIType.Mention, Constants.ServiceMethod_Twitter_GetMentionList, Constants.ObjUserToSearch);
                }
            }
        }

        private async Task LoadTwitterUserDetails()
        {
            await GetTwitterUserTimeLine();
            await GetTwitterUserReTweets(Constants.ObjUserToSearch.skills);
            await GetTwitterMentionList();
        }

        async Task Twitter_Response(TwitterAPIType APIType, HttpResponseMessage response)
        {
            String ResponseString = await response.Content.ReadAsStringAsync();
            ResponseString = Helpers.GetUTF8String(ResponseString);
            //Stream responseStream = await response.Content.ReadAsStreamAsync();
            //byte[] resbuffer = new byte[responseStream.Length];
            //responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
            //String ResponseString = Helpers.GetString(resbuffer);

            if (APIType == TwitterAPIType.UserSearch)
            {
                twitterUserRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.UsersRoot>(ResponseString, new Server.Model.Twitter.UsersRootConverter());
            }
            else if (APIType == TwitterAPIType.Mention)
            {
                Server.Model.Twitter.MentionRoot tSearhResultRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.MentionRoot>(ResponseString, new Server.Model.Twitter.MentionRootConverter());
                
                if(tSearhResultRoot != null)
                    twitterUserRoot.items[lstUserList.SelectedIndex].mentionList = tSearhResultRoot;
            }
            else if (APIType == TwitterAPIType.Retweets)
            {
                Server.Model.Twitter.Timeline timeline = JsonConvert.DeserializeObject<Server.Model.Twitter.Timeline>(ResponseString, new Server.Model.Twitter.TimelineConverter());

                //Server.Model.Twitter.TwitterStatusRoot tstatusRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.TwitterStatusRoot>(ResponseString, new Server.Model.Twitter.TwitterStatusRootConverter());

                if (timeline == null)
                    return;

                if (timeline.tStatusRoot == null)
                    return;
                if (timeline.tStatusRoot.items == null)
                    return;

                Server.Model.Twitter.TwitterStatus status;

                twitterUserRoot.items[lstUserList.SelectedIndex].statusRoot.items[timeline.tweetIndex].retweets = new List<Server.Model.Twitter.TwitterStatus>();

                foreach (Server.Model.Twitter.TwitterStatus tInnerstatus in timeline.tStatusRoot.items)
                {
                    // Ignore retweets by user himself.
                    if (tInnerstatus.user.screen_name.ToUpper() == twitterUserRoot.items[lstUserList.SelectedIndex].screen_name.ToUpper())
                        continue;

                    status = new Server.Model.Twitter.TwitterStatus();
                    status.created_at = tInnerstatus.created_at;
                    status.favorite_count = tInnerstatus.favorite_count;
                    status.id = tInnerstatus.id;
                    status.retweet_count = Convert.ToInt32(tInnerstatus.retweet_count);
                    status.text = tInnerstatus.text;

                    status.user = new Server.Model.Twitter.Users();
                    status.user.description = tInnerstatus.user.description;
                    status.user.favourites_count = tInnerstatus.user.favourites_count;
                    status.user.followers_count = tInnerstatus.user.followers_count;
                    status.user.friends_count = tInnerstatus.user.friends_count;
                    status.user.id = tInnerstatus.user.id;
                    status.user.lang = tInnerstatus.user.lang;
                    status.user.listed_count = tInnerstatus.user.listed_count;
                    status.user.location = tInnerstatus.user.location;
                    status.user.name = tInnerstatus.user.name;
                    status.user.profile_image_url = tInnerstatus.user.profile_image_url;
                    status.user.screen_name = tInnerstatus.user.screen_name;
                    status.user.time_zone = tInnerstatus.user.time_zone;
                    status.user.url = tInnerstatus.user.url;

                    twitterUserRoot.items[0].statusRoot.items[timeline.tweetIndex].retweets.Add(status);
                }
            }
            else if (APIType == TwitterAPIType.UserTimeLine)
            {
                Server.Model.Twitter.TwitterStatusRoot tStatusRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.TwitterStatusRoot>(ResponseString, new Server.Model.Twitter.TwitterStatusRootConverter());

                if(tStatusRoot != null)
                    twitterUserRoot.items[lstUserList.SelectedIndex].statusRoot = tStatusRoot;
            }
        }

        #endregion Twitter

        #region Stack Exchange

        private async Task LoadStackExData(StackExchangeAPIType APIType, string StackExInAPIUrl, object postData)
        {
            MessageDialog dlg = null;
            try
            {
                Helpers.CreateHttpClient(ref httpClient);

                String strSerialized = JsonConvert.SerializeObject(postData);
                Stream stream = GenerateSampleStream(strSerialized);
                StreamContent streamContent = new StreamContent(stream);

                string resourceAddress = Constants.ServiceUrl + StackExInAPIUrl;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

                request.Content = streamContent;
                request.Headers.TransferEncodingChunked = true;
                //httpClient.Timeout = new TimeSpan(10000000);
                HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                await StackExchange_Response(APIType, response);
            }
            catch (Exception Ex)
            {
                dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
            }
            finally
            {
                //ProggressBarVisible(false);
                //lstOptions.IsEnabled = true;
            }

            if (dlg != null)
                await dlg.ShowAsync();
        }

        private async Task LoadStackExDetails()
        {
                User selectedUser = ((User)((ListBoxItem)lstUserList.SelectedItem).Tag);

                Constants.ObjUserToSearch.stackEx_user_id = selectedUser.user_id;
                Constants.ObjUserToSearch.stackEx_account_id = selectedUser.account_id;
                //StackExSelectedIndex = lstUserList.SelectedIndex;
                await LoadStackExData(StackExchangeAPIType.UserDetailById, Constants.ServiceMethod_GetUserDetailById, Constants.ObjUserToSearch);
        }

        async Task StackExchange_Response(StackExchangeAPIType APIType, HttpResponseMessage response)
        {
            String ResponseString = await response.Content.ReadAsStringAsync();
            ResponseString = Helpers.GetUTF8String(ResponseString);

            //Stream responseStream = await response.Content.ReadAsStreamAsync();
            //byte[] resbuffer = new byte[responseStream.Length];
            //responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
            //String ResponseString = Helpers.GetString(resbuffer);

            if (APIType == StackExchangeAPIType.UserSearch)
            {
                stExRoot = JsonConvert.DeserializeObject<UserRoot>(ResponseString, new UserRootConverter());
                //Constants.ObjUserScoreDetailRoot = new UserScoreDetailRoot();
                //Constants.ObjUserScoreDetailRoot.UserScoreDetail = userRoot.UserScoreDetail;
                //Constants.ObjUserScoreDetailRoot.lstUser = userRoot.lstUser;
                //Constants.ObjUserScoreDetailRoot.lstNetwork_Users = userRoot.lstNetwork_Users;
                //Constants.ObjUserScoreDetailRoot.lstAnswer = userRoot.lstAnswer;
                //Constants.ObjUserScoreDetailRoot.lstTag_Score = userRoot.lstTag_Score;
            }
            else if (APIType == StackExchangeAPIType.UserDetailById)
            {
                stExRoot = JsonConvert.DeserializeObject<UserRoot>(ResponseString, new UserRootConverter());
                //Constants.ObjUserScoreDetailRoot = new UserScoreDetailRoot();
                //Constants.ObjUserScoreDetailRoot.UserScoreDetail = user.UserScoreDetail;
                //Constants.ObjUserScoreDetailRoot.lstUser = user.lstUser;
                //Constants.ObjUserScoreDetailRoot.lstNetwork_Users = user.lstNetwork_Users;
                //Constants.ObjUserScoreDetailRoot.lstAnswer = user.lstAnswer;
                //Constants.ObjUserScoreDetailRoot.lstTag_Score = user.lstTag_Score;

                User selectedUser = ((User)((ListBoxItem)lstUserList.SelectedItem).Tag);
                //Constants.ObjUserScoreDetailRoot.lstUser.Add(selectedUser);
                //Constants.ObjUserScoreDetailRoot.UserScoreDetail.display_name = selectedUser.display_name;
                //Constants.ObjUserScoreDetailRoot.UserScoreDetail.profile_image = selectedUser.profile_image;
                //Constants.ObjUserScoreDetailRoot.UserScoreDetail.location = selectedUser.location;

                //StackExSelectedIndex = lstUserList.SelectedIndex;
            }
            else if (APIType == StackExchangeAPIType.UserQuestion)
            {

            }
        }

        #endregion StackExchange

        private void ProggressBarVisible(bool visible)
        {
            ProgressRingLoad.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingLoad.IsActive = visible;
        }

        private bool BuildMultiUserResultList(SocialNetworkSite siteName)
        {
            bool IsMultiUserResult = false;
            lstUserList.Items.Clear();
            lstUserList.Tag = null;

            if (siteName == SocialNetworkSite.Twitter)
            {
                IsMultiUserResult = MultiUserResultList_Twitter();
            }
            else if (siteName == SocialNetworkSite.StackExchange)
            {
                IsMultiUserResult = MultiUserResultList_StackEx();
            }
            else if (siteName == SocialNetworkSite.LinkedIn)
            {
                IsMultiUserResult = MultiUserResultList_LinkedIn();
            }
            return IsMultiUserResult;
        }

        #region MultiUserResult List

        private bool MultiUserResultList_Twitter()
        {
            if (twitterUserRoot.items.Count > 1)
            {
                foreach (Server.Model.Twitter.Users user in twitterUserRoot.items)
                {
                    ListBoxItem item = new ListBoxItem();
                    Grid grd = new Grid();

                    ColumnDefinition def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);
                    def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);

                    Image img = new Image();
                    BitmapImage myBitmap = new BitmapImage(new Uri(user.profile_image_url));
                    img.Source = myBitmap;
                    img.Height = 100;
                    img.Width = 100;
                    img.Stretch = Stretch.Fill;
                    grd.Children.Add(img);
                    Grid.SetColumn(img, 0);

                    StackPanel sp = new StackPanel();
                    TextBlock txtblk = new TextBlock();
                    txtblk.FontSize = 25;
                    txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    txtblk.Text = user.name;

                    sp.Children.Add(txtblk);


                    txtblk = new TextBlock();
                    txtblk.FontSize = 20;
                    txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    txtblk.Text = user.screen_name;

                    sp.Children.Add(txtblk);
                    

                    if (user.location != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                        txtblk.Text = user.location;

                        sp.Children.Add(txtblk);
                    }
                    grd.Children.Add(sp);
                    Grid.SetColumn(sp, 1);

                    item.Content = grd;
                    item.Tag = user;
                    lstUserList.Items.Add(item);
                    lstUserList.Tag = lPeoples;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool MultiUserResultList_LinkedIn()
        {
            if (lPeoples.people.values.Count > 1)
            {
                foreach (Server.Model.LinkedIn.User user in lPeoples.people.values)
                {
                    ListBoxItem item = new ListBoxItem();
                    Grid grd = new Grid();

                    ColumnDefinition def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);
                    def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);

                    Image img = new Image();
                    BitmapImage myBitmap = new BitmapImage(new Uri(user.pictureUrl));
                    img.Source = myBitmap;
                    img.Height = 100;
                    img.Width = 100;
                    img.Stretch = Stretch.Fill;
                    grd.Children.Add(img);
                    Grid.SetColumn(img, 0);

                    String name = user.firstName;
                    if (user.maidenname != null)
                        name += " " + user.maidenname;
                    if (user.lastName != null)
                        name += " " + user.lastName;

                    StackPanel sp = new StackPanel();
                    TextBlock txtblk = new TextBlock();
                    txtblk.FontSize = 25;
                    txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    txtblk.Text = name;

                    sp.Children.Add(txtblk);

                    if (user.headline != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                        txtblk.Text = user.headline;

                        sp.Children.Add(txtblk);
                    }

                    if (user.positions != null)
                    {
                        if (user.positions.values != null)
                        {
                            foreach(Server.Model.LinkedIn.PositionsValues position in user.positions.values)
                            {
                                if (position.isCurrent == true)
                                {
                                    txtblk = new TextBlock();
                                    txtblk.FontSize = 20;
                                    txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                                    txtblk.Text = position.company.name;

                                    sp.Children.Add(txtblk);
                                    break;
                                }
                            }
                        }
                    }

                    if (user.location != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                        txtblk.Text = user.location.name;

                        sp.Children.Add(txtblk);
                    }
                    grd.Children.Add(sp);
                    Grid.SetColumn(sp, 1);

                    item.Content = grd;
                    item.Tag = user;
                    lstUserList.Items.Add(item);
                    lstUserList.Tag = lPeoples;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool MultiUserResultList_StackEx()
        {
            if (stExRoot == null)
                return false;
            if (stExRoot.items == null)
                return false;
            if (stExRoot.items.Count > 0)
            {
                foreach (User user in stExRoot.items)
                {
                    ListBoxItem item = new ListBoxItem();
                    Grid grd = new Grid();

                    ColumnDefinition def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);
                    def = new ColumnDefinition();
                    def.Width = GridLength.Auto;
                    grd.ColumnDefinitions.Add(def);

                    Image img = new Image();
                    BitmapImage myBitmap = new BitmapImage(new Uri(user.profile_image));
                    img.Source = myBitmap;
                    img.Height = 100;
                    img.Width = 100;
                    img.Stretch = Stretch.Fill;
                    grd.Children.Add(img);
                    Grid.SetColumn(img, 0);

                    StackPanel sp = new StackPanel();
                    TextBlock txtblk = new TextBlock();
                    txtblk.FontSize = 25;
                    txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    txtblk.Text = user.display_name;

                    sp.Children.Add(txtblk);

                    if (user.location != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                        txtblk.Text = user.location;

                        sp.Children.Add(txtblk);
                    }
                    grd.Children.Add(sp);
                    Grid.SetColumn(sp, 1);

                    item.Content = grd;
                    item.Tag = user;
                    lstUserList.Items.Add(item);
                }

                return true;
            }
            else
            {
                return false;
                //// Add this code.
                //if (this.Frame != null)
                //{
                //    this.Frame.Navigate(typeof(StackExchangeDetails));
                //}
            }
        }

        private void lstUserList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DisplayUserShortDesc();

            //try
            //{
            //    if (lstUserList.SelectedIndex < 0)
            //        return;

            //    ProggressBarVisible(true);
            //    lstOptions.IsEnabled = false;

            //    if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.Twitter.Users))
            //    {
            //        await LoadTwitterUserDetails();

            //        Server.Model.Twitter.Users tUser = (Server.Model.Twitter.Users)(((ListBoxItem)lstUserList.SelectedItem).Tag);
            //        twitterUserRoot.items.Clear();
            //        twitterUserRoot.items.Add(tUser);

            //        await LoadStackExData(StackExchangeAPIType.UserSearch, Constants.ServiceMethod_GetUserDetail, Constants.ObjUserToSearch);
            //        bool IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.StackExchange);

            //        if (IsMultiUserResult)
            //        {
            //            txtMessage.Text = "Multiple Stack Exchange users found with same name, Please select one user to proceed.";
            //            txtMessage.Visibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //        }
            //    }
            //    else if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.StackExchange.User))
            //    {
            //        await LoadStackExDetails();

            //        await LoadLinkedInData(LinkedInAPIType.UserSearch, Constants.ServiceMethod_LinkedIn_SearchUser, Constants.ObjUserToSearch);
            //        if (lPeoples != null)
            //        {
            //            await GetLinkedInGroupDetailWithComment(lPeoples);
            //            bool IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.LinkedIn);
            //            if (IsMultiUserResult)
            //            {
            //                txtMessage.Text = "Multiple Stack Exchange users found with same name, Please select one user to proceed.";
            //                txtMessage.Visibility = Visibility.Visible;
            //            }
            //            else
            //            {
            //                txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //            }
            //        }

            //        //if (this.Frame != null)
            //        //{
            //        //    this.Frame.Navigate(typeof(StackExchangeDetails));
            //        //}
            //    }
            //    else if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.LinkedIn.User))
            //    {
            //        txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //        await LoadLinkedInUserDetails();
            //    }
            //}
            //catch (Exception Ex)
            //{ txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed; }
            //finally
            //{
            //    ProggressBarVisible(false);
            //    lstOptions.IsEnabled = true;
            //}
            
        }

        private async void lstUserList_ManipulationCompleted_1(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (lstUserList.SelectedIndex < 0)
                return;

            await LoadStackExDetails();
        }

        #endregion


        private static MemoryStream GenerateSampleStream(string strSerializedText)
        {
            // Generate sample data.
            byte[] subData = new byte[strSerializedText.Length];
            for (int i = 0; i < subData.Length; i++)
            {
                subData[i] = Convert.ToByte(strSerializedText[i]);
            }

            return new MemoryStream(subData);
        }

        private async void btnSearch_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ProggressBarVisible(true);
                lstOptions.IsEnabled = false;

                bool IsMultiUserResult = false;
                try
                {
                    await LoadTwitterData(TwitterAPIType.UserSearch, Constants.ServiceMethod_Twitter_SearchUser, Constants.ObjUserToSearch);

                    if (twitterUserRoot != null)
                    {
                        if (twitterUserRoot.items != null)
                        {
                            if (twitterUserRoot.items.Count == 1)
                            {
                                await GetTwitterUserReTweets(Constants.ObjUserToSearch.skills);
                                await GetTwitterMentionList();
                            }
                        }
                    }
                    IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.Twitter);

                    if (IsMultiUserResult)
                    {
                        txtMessage.Text = "Multiple Twitter users found with same name, Please select one user to proceed.";
                        txtMessage.Visibility = Visibility.Visible;
                        gdrOptions.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        lstUserList.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        return;
                    }
                }
                catch (Exception Ex)
                { }

                try
                {
                    await LoadStackExData(StackExchangeAPIType.UserSearch, Constants.ServiceMethod_GetUserDetail, Constants.ObjUserToSearch);
                    IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.StackExchange);

                    if (IsMultiUserResult)
                    {
                        txtMessage.Text = "Multiple Stack Exchange users found with same name, Please select one user to proceed.";
                        txtMessage.Visibility = Visibility.Visible;
                        gdrOptions.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        lstUserList.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        return;
                    }
                }
                catch (Exception Ex)
                { }

                try
                {
                    //this.Frame.Navigate(typeof(StackExchangeDetails));
                    await LoadLinkedInData(LinkedInAPIType.UserSearch, Constants.ServiceMethod_LinkedIn_SearchUser, Constants.ObjUserToSearch);
                    if (lPeoples != null)
                    {
                        await GetLinkedInGroupDetailWithComment(lPeoples);
                        IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.LinkedIn);


                        if (IsMultiUserResult)
                        {
                            txtMessage.Text = "Multiple LinkedIn users found with same name, Please select one user to proceed.";
                            txtMessage.Visibility = Visibility.Visible;
                            gdrOptions.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            lstUserList.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            return;
                        }
                    }
                }
                catch (Exception Ex)
                { }
            }
            catch (Exception Ex)
            { }
            finally
            {
                ProggressBarVisible(false);
                lstOptions.IsEnabled = true;
            }
        }

        private void DisplayUserShortDesc()
        {
            grdUserDesc.Visibility = Windows.UI.Xaml.Visibility.Visible;

            if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.Twitter.Users))
            {
                Server.Model.Twitter.Users user = ((Server.Model.Twitter.Users)((ListBoxItem)lstUserList.SelectedItem).Tag);

                Image img = new Image();
                BitmapImage myBitmap = new BitmapImage(new Uri(user.profile_image_url));
                imgUserDescUserPic.Source = myBitmap;
                img.Height = 200;
                img.Width = 200;
                img.Stretch = Stretch.Fill;

                txtUserDescUserName.Text = user.name;
                txtUserDescScreenName.Text = user.screen_name;
                if (user.location != null)
                    txtUserDescLocation.Text = user.location;
                hybtnPublicProfileUrl.Content = user.url;
                gdrUserDescCurrentCompany.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                gdrUserDescDescription.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.StackExchange.User))
            {
                Server.Model.StackExchange.User user = ((Server.Model.StackExchange.User)((ListBoxItem)lstUserList.SelectedItem).Tag);

                Image img = new Image();
                BitmapImage myBitmap = new BitmapImage(new Uri(user.profile_image));
                imgUserDescUserPic.Source = myBitmap;
                img.Height = 200;
                img.Width = 200;
                img.Stretch = Stretch.Fill;

                txtUserDescUserName.Text = user.display_name;
                //txtUserDescAge.Text = DateTime.Parse(Convert.ToString(user.age));
                if (user.location != null)
                    txtUserDescLocation.Text = user.location;

                hybtnPublicProfileUrl.Content = user.website_url;
                gdrUserDescCurrentCompany.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                gdrUserDescDescription.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.LinkedIn.User))
            {
                Server.Model.LinkedIn.User user = ((Server.Model.LinkedIn.User)((ListBoxItem)lstUserList.SelectedItem).Tag);

                Image img = new Image();
                BitmapImage myBitmap = new BitmapImage(new Uri(user.pictureUrl));
                imgUserDescUserPic.Source = myBitmap;
                img.Height = 200;
                img.Width = 200;
                img.Stretch = Stretch.Fill;

                String name = user.firstName;
                if (user.maidenname != null)
                    name += " " + user.maidenname;
                if (user.lastName != null)
                    name += " " + user.lastName;

                txtUserDescUserName.Text = name;
                gdrUserDescScreenName.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                if (user.headline != null)
                    txtUserDescHeadline.Text = user.headline;

                if (user.positions != null)
                {
                    if (user.positions.values != null)
                    {
                        foreach (Server.Model.LinkedIn.PositionsValues position in user.positions.values)
                        {
                            if (position.isCurrent == true)
                            {
                                txtUserCurrentCompany.Text = position.company.name;
                            }
                        }
                    }
                }
                if (user.location != null)
                    txtUserDescLocation.Text = user.location.name;

                hybtnPublicProfileUrl.Content = user.publicProfileUrl;
                gdrUserDescCurrentCompany.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                gdrUserDescDescription.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void btnSelectUser_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstUserList.SelectedIndex < 0)
                    return;

                grdUserDesc.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                ProggressBarVisible(true);
                lstOptions.IsEnabled = false;

                if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.Twitter.Users))
                {
                    await LoadTwitterUserDetails();

                    Server.Model.Twitter.Users tUser = (Server.Model.Twitter.Users)(((ListBoxItem)lstUserList.SelectedItem).Tag);
                    twitterUserRoot.items.Clear();
                    twitterUserRoot.items.Add(tUser);

                    await LoadStackExData(StackExchangeAPIType.UserSearch, Constants.ServiceMethod_GetUserDetail, Constants.ObjUserToSearch);
                    bool IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.StackExchange);

                    if (IsMultiUserResult)
                    {
                        txtMessage.Text = "Multiple Stack Exchange users found with same name, Please select one user to proceed.";
                        txtMessage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                }
                else if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.StackExchange.User))
                {
                    await LoadStackExDetails();

                    await LoadLinkedInData(LinkedInAPIType.UserSearch, Constants.ServiceMethod_LinkedIn_SearchUser, Constants.ObjUserToSearch);
                    if (lPeoples != null)
                    {
                        await GetLinkedInGroupDetailWithComment(lPeoples);
                        bool IsMultiUserResult = BuildMultiUserResultList(SocialNetworkSite.LinkedIn);
                        if (IsMultiUserResult)
                        {
                            txtMessage.Text = "Multiple Stack Exchange users found with same name, Please select one user to proceed.";
                            txtMessage.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                    //if (this.Frame != null)
                    //{
                    //    this.Frame.Navigate(typeof(StackExchangeDetails));
                    //}
                }
                else if ((((ListBoxItem)lstUserList.SelectedItem).Tag).GetType() == typeof(Server.Model.LinkedIn.User))
                {
                    txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    await LoadLinkedInUserDetails();
                }
            }
            catch (Exception Ex)
            { txtMessage.Visibility = Windows.UI.Xaml.Visibility.Collapsed; }
            finally
            {
                ProggressBarVisible(false);
                lstOptions.IsEnabled = true;
            }
        }
    }
}
