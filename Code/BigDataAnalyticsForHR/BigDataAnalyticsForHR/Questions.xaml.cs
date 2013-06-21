using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Server.Model.StackExchange;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BigDataAnalyticsForHR
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Questions : BigDataAnalyticsForHR.Common.LayoutAwarePage
    {
        public static QuestionRoot Question;

        public Questions()
        {
            this.InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (Question != null)
            {
                foreach (Question Que in Question.items)
                {
                    ListBoxItem item = new ListBoxItem();

                    StackPanel sp = new StackPanel();
                    TextBlock txtblk = new TextBlock();

                    if (Que.title != null)
                    {
                        txtblk.FontSize = 25;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                        txtblk.Text = Que.title;
                        txtblk.TextTrimming = TextTrimming.WordEllipsis;
                        txtblk.Height = 50;
                        sp.Children.Add(txtblk);
                    }
                    if (Que.body != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                        txtblk.Text = Que.body;
                        txtblk.Height = 50;
                        txtblk.TextTrimming = TextTrimming.WordEllipsis;
                        sp.Children.Add(txtblk);
                    }
                    item.Content = sp;
                    item.Tag = Que;
                    lstQuestionList.Items.Add(item);

                }
            }
            else
            {
                //lstQuestionList.DataContext = Constants.ObjUserScoreDetailRoot.lstAnswer;
                foreach (Answer ans in StackExchangeDetails.ObjUser.lstAnswer)
                {
                    ListBoxItem item = new ListBoxItem();

                    StackPanel sp = new StackPanel();
                    TextBlock txtblk = new TextBlock();

                    if (ans.title != null)
                    {
                        txtblk.FontSize = 25;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                        txtblk.Text = ans.title;
                        txtblk.TextTrimming = TextTrimming.WordEllipsis;
                        txtblk.Height = 50;
                        sp.Children.Add(txtblk);
                    }
                    if (ans.body != null)
                    {
                        txtblk = new TextBlock();
                        txtblk.FontSize = 20;
                        txtblk.Foreground = new SolidColorBrush(Windows.UI.Colors.Brown);
                        txtblk.Text = ans.body;
                        txtblk.Height = 50;
                        txtblk.TextTrimming = TextTrimming.WordEllipsis;
                        sp.Children.Add(txtblk);
                    }
                    item.Content = sp;
                    item.Tag = ans;
                    lstQuestionList.Items.Add(item);

                }
            }
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

        private void lstQuestionList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (lstQuestionList.SelectedIndex < 0)
                return;

            if (Question != null)
            {
                Question Que = (Question)((ListBoxItem)lstQuestionList.SelectedItem).Tag;

                txtQuestionDesc.Text = Que.title;
                txtAnswerDesc.Text = Que.body;
                viewinbrowser.NavigateUri = new Uri(Que.link);
            }
            else
            {
                Answer ans = (Answer)((ListBoxItem)lstQuestionList.SelectedItem).Tag;

                txtQuestionDesc.Text = ans.title;
                txtAnswerDesc.Text = ans.body;
                viewinbrowser.NavigateUri = new Uri(ans.link);
            }
        }
    }
}
