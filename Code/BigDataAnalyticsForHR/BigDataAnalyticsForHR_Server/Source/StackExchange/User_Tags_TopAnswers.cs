using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Server.Model.StackExchange;

namespace Server.Source.StackExchange
{
    /// <summary>
    /// Returns the top 30 answers a user has posted in response to questions with the given tags.
    /// https://api.stackexchange.com/docs/top-user-answers-in-tags
    /// </summary>
    public class User_Tags_TopAnswers
    {
        //https://api.stackexchange.com/2.1/users/1852224/tags/c%23/top-answers?site=stackoverflow&filter=!SYQNi9YnYj-)a4OhS.
        int UserId;
        String Tags = null;
        JObject TagObject;


        public string Filter
        {
            get
            {
                return "!bULULwD4DqAE.J";// "!-t6dD.mE";
            }
        }

        private String PrepareUrl()
        {
            String Url = "";
            Url = Constants.StackExchangeUrl + "users/" + UserId + "/tags/" + Tags + "/top-answers?" + "site=" + Constants.api_site_parameter + "&filter=" + Filter;
            return Url;
        }

        /// <summary>
        /// {id} can contain a single id
        /// {tags} is limited to 5 tags, passing more will result in an error. 
        /// eg; .../tags/c#;windows-phone-7/top-answers...
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public JObject GetUserTagsTopAnswers(int UserId, String Tags)
        {
            TagObject = new JObject();
            this.UserId = UserId;
            this.Tags = Uri.EscapeDataString(Tags);

            String Url = PrepareUrl();
            Connect(Url);


            // Serialize JSON data into StackExchangeRoot Object.
            String strUserData = JsonConvert.SerializeObject(TagObject, Formatting.Indented);
            AnswerRoot tagData = JsonConvert.DeserializeObject<AnswerRoot>(strUserData, new AnswerRootConverter());

            return TagObject;
        }

        private void Connect(String Url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    UpdateUserTagsTopAnswers(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateUserTagsTopAnswers(String result)
        {
            TagObject = JObject.Parse(result);
        }

    }
}
