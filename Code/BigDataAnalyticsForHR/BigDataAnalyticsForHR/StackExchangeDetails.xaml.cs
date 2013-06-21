using ModernUI.Toolkit.Data.Charting.Charts.Series;
using Newtonsoft.Json;
using Server.Model.StackExchange;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BigDataAnalyticsForHR
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class StackExchangeDetails : BigDataAnalyticsForHR.Common.LayoutAwarePage
    {
        HttpClient httpClient;
        public static Server.Model.StackExchange.User ObjUser = new Server.Model.StackExchange.User();

        public StackExchangeDetails()
        {
            this.InitializeComponent();

            FillUserDetails();

            List<NameValueItem> items = new List<NameValueItem>();
            items.Add(new NameValueItem() { Name = "Test1", Value = 40 });
            items.Add(new NameValueItem() { Name = "Test2", Value = 50 });
            items.Add(new NameValueItem() { Name = "Test3", Value = 20 });
            items.Add(new NameValueItem() { Name = "Test4", Value = 10 });
            items.Add(new NameValueItem() { Name = "Test5", Value = 100 });

            ((AreaSeries)AreaChart.Series[0]).ItemsSource = items;
        }

        private void FillUserDetails()
        {
            if (ObjUser != null)
            {
                txtScore.Text = "Score :" + Convert.ToString(ObjUser.userScore.score_User_Tag_Top_Ans);
                BitmapImage myBitmap = new BitmapImage(new Uri(Convert.ToString(ObjUser.profile_image)));
                myBitmap.DownloadProgress += myBitmap_DownloadProgress;
                imgUserPic.Source = myBitmap;

                txtUserName.Text = Convert.ToString(ObjUser.display_name);

                txtReputationNo.Text = Convert.ToString(ObjUser.userScore.reputation);
                txtBadgeGold.Text = Convert.ToString(ObjUser.userScore.badge_count.gold);
                txtBadgeSilver.Text = Convert.ToString(ObjUser.userScore.badge_count.silver);
                txtBadgeBronze.Text = Convert.ToString(ObjUser.userScore.badge_count.bronze);

                txtAnswerAccepted.Text = Convert.ToString(ObjUser.userScore.is_acceptedCount);

                if(ObjUser.lstQuestion != null)
                    txtQuestionsFromUser.Text = Convert.ToString(ObjUser.lstQuestion.Count);

                if (ObjUser.lstAnswer != null)
                    txtAnswersByUser.Text = Convert.ToString(ObjUser.lstAnswer.Count);

                txtLocation.Text = ObjUser.location;
            }
            else
            {
                viewonmap.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        void myBitmap_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            try
            {
                if (e.Progress == 100)
                {
                    imgUserPicPlaceHolder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    imgUserPic.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
            catch (Exception)
            { }
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

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private async void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Questions.Question = null;
            if (lstStackEXOptions.SelectedIndex == 2)
            {
                await LoadUserQuestions();
            }
            //if (lstStackEXOptions.SelectedIndex == 0 || lstStackEXOptions.SelectedIndex == 1)
            //{
            //    if (this.Frame != null)
            //    {
            //        this.Frame.Navigate(typeof(Questions));
            //    }
            //}
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(Questions));
            }
        }


        private async Task LoadUserQuestions()
        {
            MessageDialog dlg = null;

            try
            {
                Constants.ObjUserToSearch.stackEx_user_id = ObjUser.user_id;
                ProggressBarVisible(true);
                Helpers.CreateHttpClient(ref httpClient);
                String strSerialized = JsonConvert.SerializeObject(Constants.ObjUserToSearch);
                Stream stream = GenerateSampleStream(strSerialized);
                StreamContent streamContent = new StreamContent(stream);

                string resourceAddress = Constants.ServiceUrl + Constants.ServiceMethod_GetUserQuestions;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

                request.Content = streamContent;
                request.Headers.TransferEncodingChunked = true; // Assume we do not know the content length
                HttpResponseMessage response = await httpClient.SendAsync(request);
                //await Helpers.PrepareQuestionObject(response);
                await PrepareQuestionObject(response);
            }
            catch (Exception Ex)
            {
                dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
            }
            finally
            {
                ProggressBarVisible(false);
            }
            if (dlg != null)
                await dlg.ShowAsync();
        }

        async Task PrepareQuestionObject(HttpResponseMessage response)
        {
            String ResponseString = await response.Content.ReadAsStringAsync();
            ResponseString = Helpers.GetUTF8String(ResponseString);

            //Stream responseStream = await response.Content.ReadAsStreamAsync();
            ////System.IO.StreamReader reader = new System.IO.StreamReader(responseStream);
            ////String ResponseString = reader.ReadToEnd();
            //byte[] resbuffer = new byte[responseStream.Length];
            //int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
            //String ResponseString = GetString(resbuffer);
            Questions.Question = JsonConvert.DeserializeObject<QuestionRoot>(ResponseString, new QuestionRootConverter());
        }

        private void ProggressBarVisible(bool visible)
        {
            ProgressRingLoad.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingLoad.IsActive = visible;
        }

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

    }


    public class NameValueItem
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public NameValueItem()
        {
        }
    }
}
