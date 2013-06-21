using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
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
    public sealed partial class MainScreen : BigDataAnalyticsForHR.Common.LayoutAwarePage
    {
        HttpClient httpClient;

        public MainScreen()
        {
            this.InitializeComponent();
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

        private async void ToggleButton_Checked_1(object sender, RoutedEventArgs e)
        {
            await LoadUserDataScreen();
        }

        private async Task LoadUserDataScreen()
        {
            MessageDialog dlg = null;
            try
            {
                if (txtFirstName.Text.Trim().Length <= 0)
                {
                    dlg = new MessageDialog("Please Enter First Name"); await dlg.ShowAsync();
                    return;
                }

                ProggressBarVisible(true);
                btnGetData.IsEnabled = false;
                Helpers.CreateHttpClient(ref httpClient);
                Constants.ObjUserToSearch = new UserToSearch();
                Constants.ObjUserToSearch.f_name = txtFirstName.Text.Trim();// "johngb";
                Constants.ObjUserToSearch.m_name = txtMiddleName.Text.Trim();
                Constants.ObjUserToSearch.l_name = txtLastName.Text.Trim();
                Constants.ObjUserToSearch.emailId = txtEMailID.Text.Trim();
                Constants.ObjUserToSearch.jobRole = txtJobRole.Text.Trim();
                Constants.ObjUserToSearch.skills = txtSkills.Text.Trim();

                //String strSerialized = JsonConvert.SerializeObject(Constants.ObjUserToSearch);
                //Stream stream = GenerateSampleStream(strSerialized);
                //StreamContent streamContent = new StreamContent(stream);

                //string resourceAddress = Constants.ServiceUrl + Constants.ServiceMethod_GetUserDetail;
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

                //request.Content = streamContent;
                //request.Headers.TransferEncodingChunked = true;
                //HttpResponseMessage response = await httpClient.SendAsync(request);
                //await Helpers.DisplayTextResult(response, txtEMailID);


                // Add this code.
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(UserDetail));
                }
            }
            catch (Exception Ex)
            {
                dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
            }
            finally
            {
                ProggressBarVisible(false);
                btnGetData.IsEnabled = true;
            }

            if (dlg != null)
                await dlg.ShowAsync();
        }

        private void ProggressBarVisible(bool visible)
        {
            ProgressRingLoad.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            ProgressRingLoad.IsActive = visible;
        }

        //private void PrepareOutputScrData()
        //{
        //    if (Constants.ObjUserScoreDetailRoot.UserScoreDetail != null)
        //    {
        //        foreach (User user in Constants.ObjUserScoreDetailRoot.lstUser)
        //        {
        //            ListBoxItem item = new ListBoxItem();
        //            Grid grd = new Grid();

        //            ColumnDefinition def = new ColumnDefinition();
        //            def.Width = GridLength.Auto;
        //            grd.ColumnDefinitions.Add(def);
        //            def = new ColumnDefinition();
        //            def.Width = GridLength.Auto;
        //            grd.ColumnDefinitions.Add(def);

        //            Image img = new Image();
        //            BitmapImage myBitmap = new BitmapImage(new Uri(user.profile_image));
        //            img.Source = myBitmap;
        //            img.Height = 100;
        //            img.Width = 100;
        //            img.Stretch = Stretch.Fill;
        //            grd.Children.Add(img);
        //            Grid.SetColumn(img, 0);

        //            StackPanel sp = new StackPanel();
        //            TextBlock txtblk = new TextBlock();
        //            txtblk.FontSize = 25;
        //            txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
        //            txtblk.Text = user.display_name;

        //            sp.Children.Add(txtblk);

        //            if (user.location != null)
        //            {
        //                txtblk = new TextBlock();
        //                txtblk.FontSize = 20;
        //                txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
        //                txtblk.Text = user.location;

        //                sp.Children.Add(txtblk);
        //            }
        //            grd.Children.Add(sp);
        //            Grid.SetColumn(sp, 1);

        //            item.Content = grd;
        //            item.Tag = user;
        //            lstUserList.Items.Add(item);
        //        }
        //    }
        //    else
        //    {
        //        // Add this code.
        //        if (this.Frame != null)
        //        {
        //            this.Frame.Navigate(typeof(UserDetail));
        //        }
        //    }
        //}

        //private static MemoryStream GenerateSampleStream(string strSerializedText)
        //{
        //    // Generate sample data.
        //    byte[] subData = new byte[strSerializedText.Length];
        //    for (int i = 0; i < subData.Length; i++)
        //    {
        //        subData[i] = Convert.ToByte(strSerializedText[i]);
        //    }

        //    return new MemoryStream(subData);
        //}

        //private async  void lstUserList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lstUserList.SelectedIndex < 0)
        //        return;

        //    await LoadStackExDetails();
        //}

        //private async Task LoadStackExDetails()
        //{
        //    MessageDialog dlg = null;

        //    try
        //    {
        //        ProggressBarVisible(true);

        //        lstUserList.IsEnabled = false;
        //        User selectedUser = ((User)((ListBoxItem)lstUserList.SelectedItem).Tag);

        //        Helpers.CreateHttpClient(ref httpClient);
        //        Constants.ObjUserToSearch = new UserToSearch();
        //        Constants.ObjUserToSearch.f_name = txtFirstName.Text.Trim();
        //        Constants.ObjUserToSearch.m_name = txtMiddleName.Text.Trim();
        //        Constants.ObjUserToSearch.l_name = txtLastName.Text.Trim();
        //        Constants.ObjUserToSearch.emailId = txtEMailID.Text.Trim();
        //        Constants.ObjUserToSearch.jobRole = txtJobRole.Text.Trim();
        //        Constants.ObjUserToSearch.skills = txtSkills.Text.Trim();
        //        Constants.ObjUserToSearch.stackEx_user_id = selectedUser.user_id;
        //        Constants.ObjUserToSearch.stackEx_account_id = selectedUser.account_id;

        //        String strSerialized = JsonConvert.SerializeObject(Constants.ObjUserToSearch);
        //        Stream stream = GenerateSampleStream(strSerialized);
        //        StreamContent streamContent = new StreamContent(stream);

        //        string resourceAddress = Constants.ServiceUrl + Constants.ServiceMethod_GetUserDetailById;
        //        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, resourceAddress);

        //        request.Content = streamContent;
        //        request.Headers.TransferEncodingChunked = true; // Assume we do not know the content length
        //        HttpResponseMessage response = await httpClient.SendAsync(request);
        //        await Helpers.DisplayTextResult(response, selectedUser);

        //        if (this.Frame != null)
        //        {
        //            this.Frame.Navigate(typeof(UserDetail));
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        dlg = new MessageDialog("Maximum Request Limit Reached For This IP!!!");
        //    }
        //    finally
        //    {
        //        ProggressBarVisible(false);
        //        lstUserList.IsEnabled = true;
        //    }
        //    if (dlg != null)
        //        await dlg.ShowAsync();
        //}

        //private async void lstUserList_ManipulationCompleted_1(object sender, ManipulationCompletedRoutedEventArgs e)
        //{
        //    if (lstUserList.SelectedIndex < 0)
        //        return;
            
        //    await LoadStackExDetails();
        //}

    }





}
