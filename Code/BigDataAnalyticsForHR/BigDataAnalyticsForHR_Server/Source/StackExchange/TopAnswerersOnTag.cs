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
    /// Returns the top 30 answerers active in a single tag, of either all-time or the last 30 days.
    /// </summary>
    public class TopAnswerersOnTag
    {
        // https://api.stackexchange.com/2.1/tags/c%23/top-answerers/all_time?site=stackoverflow&filter=!6QqZPjH*oJiob
        String Tags = null;
        JObject TagScoreObject;


        public string Filter
        {
            get
            {
                return "!6QqZPjH*oJiob";
            }
        }

        private String PrepareUrl()
        {
            String Url = "";
            Url = Constants.StackExchangeUrl + "tags/" + Tags + "/top-answerers/all_time?" + "site=" + Constants.api_site_parameter + "&filter=" + Filter;
            return Url;
        }

        /// <summary>
        /// {tags} is limited to 5 tags, passing more will result in an error. 
        /// eg; .../tags/c#;windows-phone-7/top-answers...
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Tag"></param>
        /// <returns></returns>
        public JObject GetTopAnswersOnTag(String Tags)
        {
            TagScoreObject = new JObject();
            this.Tags = Uri.EscapeDataString(Tags);

            String Url = PrepareUrl();
            Connect(Url);


            // Serialize JSON data into Tag_ScoreRoot Object.
            String strUserData = JsonConvert.SerializeObject(TagScoreObject, Formatting.Indented);
            Tag_ScoreRoot tagData = JsonConvert.DeserializeObject<Tag_ScoreRoot>(strUserData, new Tag_ScoreRootConverter());

            return TagScoreObject;
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
                    UpdateTopAnswersOnTag(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateTopAnswersOnTag(String result)
        {
            TagScoreObject = JObject.Parse(result);
        }

    }
}
