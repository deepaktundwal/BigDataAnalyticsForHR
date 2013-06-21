using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Server.Model.StackExchange;

namespace BigDataAnalyticsForHR
{
    internal static class Helpers
    {
        //internal static async Task DisplayTextResult(HttpResponseMessage response, User selectedUser)
        //{
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    //System.IO.StreamReader reader = new System.IO.StreamReader(responseStream);
        //    //String ResponseString = reader.ReadToEnd();
        //    byte[] resbuffer = new byte[responseStream.Length];
        //    int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
        //    String ResponseString = GetString(resbuffer);

        //    UserScoreDetailRoot user = JsonConvert.DeserializeObject<UserScoreDetailRoot>(ResponseString, new UserScoreDetailConverter());
        //    Constants.ObjUserScoreDetailRoot = new UserScoreDetailRoot();
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail = user.UserScoreDetail;
        //    Constants.ObjUserScoreDetailRoot.lstUser = user.lstUser;
        //    Constants.ObjUserScoreDetailRoot.lstNetwork_Users = user.lstNetwork_Users;
        //    Constants.ObjUserScoreDetailRoot.lstAnswer = user.lstAnswer;
        //    Constants.ObjUserScoreDetailRoot.lstTag_Score = user.lstTag_Score;

        //    Constants.ObjUserScoreDetailRoot.lstUser.Add(selectedUser);
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail.display_name = selectedUser.display_name;
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail.profile_image = selectedUser.profile_image;
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail.location = selectedUser.location;
        //}

        //internal static async Task DisplayTextResult(HttpResponseMessage response)
        //{
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    //System.IO.StreamReader reader = new System.IO.StreamReader(responseStream);
        //    //String ResponseString = reader.ReadToEnd();
        //    byte[] resbuffer = new byte[responseStream.Length];
        //    int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
        //    String ResponseString = GetString(resbuffer);
        //    UserScoreDetailRoot user = JsonConvert.DeserializeObject<UserScoreDetailRoot>(ResponseString, new UserScoreDetailConverter());
        //    Constants.ObjUserScoreDetailRoot = new UserScoreDetailRoot();
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail = user.UserScoreDetail;
        //    Constants.ObjUserScoreDetailRoot.lstUser = user.lstUser;
        //    Constants.ObjUserScoreDetailRoot.lstNetwork_Users = user.lstNetwork_Users;
        //    Constants.ObjUserScoreDetailRoot.lstAnswer = user.lstAnswer;
        //    Constants.ObjUserScoreDetailRoot.lstTag_Score = user.lstTag_Score;
        //}

        //internal static async Task Twitter_SerchUserResult(HttpResponseMessage response)
        //{
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    byte[] resbuffer = new byte[responseStream.Length];
        //    int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
        //    String ResponseString = GetString(resbuffer);
        //    Server.Model.Twitter.UsersRoot userRoot = JsonConvert.DeserializeObject<Server.Model.Twitter.UsersRoot>(ResponseString, new Server.Model.Twitter.UsersRootConverter());
        //}

        //internal static async Task LinkedIn_SerchUserResult(HttpResponseMessage response)
        //{
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    byte[] resbuffer = new byte[responseStream.Length];
        //    int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
        //    String ResponseString = GetString(resbuffer);
        //    Server.Model.LinkedIn.PeopleRoot lPeoples = JsonConvert.DeserializeObject<Server.Model.LinkedIn.PeopleRoot>(ResponseString, new Server.Model.LinkedIn.PeopleRootConverter());
        //}

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        internal static async Task PrepareQuestionObject(HttpResponseMessage response)
        {
            Stream responseStream = await response.Content.ReadAsStreamAsync();
            //System.IO.StreamReader reader = new System.IO.StreamReader(responseStream);
            //String ResponseString = reader.ReadToEnd();
            byte[] resbuffer = new byte[responseStream.Length];
            int i = responseStream.Read(resbuffer, 0, Convert.ToInt32(responseStream.Length));
            String ResponseString = GetString(resbuffer);
            Questions.Question = JsonConvert.DeserializeObject<QuestionRoot>(ResponseString, new QuestionRootConverter());
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static String GetUTF8String(String InputString)
        {
            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;
            byte[] utf8Bytes = utf_8.GetBytes(InputString);
            String strutf8 = utf_8.GetString(utf8Bytes, 0, utf8Bytes.Length);
            return strutf8;
        }

        //internal static async Task DisplayTextResult(HttpResponseMessage response, TextBox output)
        //{
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    System.IO.StreamReader reader = new System.IO.StreamReader(responseStream);
        //    String ResponseString = reader.ReadToEnd();

        //    UserScoreDetailRoot user = JsonConvert.DeserializeObject<UserScoreDetailRoot>(ResponseString, new UserScoreDetailConverter());
        //    Constants.ObjUserScoreDetailRoot = new UserScoreDetailRoot();
        //    Constants.ObjUserScoreDetailRoot.UserScoreDetail = user.UserScoreDetail;
        //    Constants.ObjUserScoreDetailRoot.lstUser = user.lstUser;
        //    Constants.ObjUserScoreDetailRoot.lstNetwork_Users = user.lstNetwork_Users;
        //    Constants.ObjUserScoreDetailRoot.lstAnswer = user.lstAnswer;
        //    Constants.ObjUserScoreDetailRoot.lstTag_Score = user.lstTag_Score;

        //    //UserToSearch user = JsonConvert.DeserializeObject<UserToSearch>(struserToSearch, new UserConverter());
        //    //UserToSearch user = JsonConvert.DeserializeObject<UserToSearch>(struserToSearch, new UserConverter());

        //    //string responseBodyAsText;

        //    //// We cast the StatusCode to an int so we display the numeric value (e.g., "200") rather than the
        //    //// name of the enum (e.g., "OK") which would often be redundant with the ReasonPhrase.
        //    //output.Text += ((int)response.StatusCode) + " " + response.ReasonPhrase + Environment.NewLine;

        //    //responseBodyAsText = await response.Content.ReadAsStringAsync();
        //    //responseBodyAsText = responseBodyAsText.Replace("<br>", Environment.NewLine); // Insert new lines
        //    //output.Text += responseBodyAsText;
        //}

        internal static void CreateHttpClient(ref HttpClient httpClient)
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            // HttpClient functionality can be extended by plugging multiple handlers together and providing
            // HttpClient with the configured handler pipeline.
            HttpMessageHandler handler = new HttpClientHandler();
            handler = new PlugInHandler(handler); // Adds a custom header to every request and response message.            
            httpClient = new HttpClient(handler);

            // The following line sets a "User-Agent" request header as a default header on the HttpClient instance.
            // Default headers will be sent with every request sent from this HttpClient instance.
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Sample", "v8"));
        }

        //internal static void ScenarioStarted(Button startButton, Button cancelButton)
        //{
        //    startButton.IsEnabled = false;
        //    cancelButton.IsEnabled = true;
        //}

        //internal static void ScenarioCompleted(Button startButton, Button cancelButton)
        //{
        //    startButton.IsEnabled = true;
        //    cancelButton.IsEnabled = false;
        //}
    }

    public class PlugInHandler : MessageProcessingHandler
    {
        public PlugInHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        // Process the request before sending it
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Get)
            {
                request.Headers.Add("Custom-Header", "CustomRequestValue");
            }
            return request;
        }

        // Process the response before returning it to the user
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.RequestMessage.Method == HttpMethod.Get)
            {
                response.Headers.Add("Custom-Header", "CustomResponseValue");
            }
            return response;
        }
    }

}
