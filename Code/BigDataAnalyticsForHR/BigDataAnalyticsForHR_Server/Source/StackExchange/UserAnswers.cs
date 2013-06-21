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
    /// <summary>
    /// Returns the answers the users in {ids} have posted.
    /// {ids} can contain up to 100 semicolon delimited ids, to find ids programatically look for user_id on user or shallow_user objects.
    /// </summary>
    public class UserAnswers
    {
        // https://api.stackexchange.com/2.1/users/1852224/answers?site=stackoverflow&filter=!-.mgWLrlV*9w
        int AccountId;
        JObject ObjAnswer;

        public string FilterWithShallowUser
        {
            get
            {
                return "!-.mgWLrlV*9w";
            }
        }

        public string FilterWithOutShallowUser
        {
            get
            {
                return "!6MnqQ1SG99(CF";
            }
        }

        public string UserUrl
        {
            get
            {
                return Constants.StackExchangeUrl + "users/" + AccountId + "/answers?site=" + Constants.api_site_parameter + "&filter=" + FilterWithOutShallowUser;
            }
        }

        public JObject GetUserAnswers(int AccountId)
        {
            ObjAnswer = new JObject();
            this.AccountId = AccountId;

            Connect(UserUrl);

            // Serialize JSON data into StackExchangeRoot Object.
            String strAnswerData = JsonConvert.SerializeObject(ObjAnswer, Formatting.Indented);
            AnswerRoot networkUser = JsonConvert.DeserializeObject<AnswerRoot>(strAnswerData, new AnswerRootConverter());

            return ObjAnswer;
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
                    UpdateAnswerData(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateAnswerData(String result)
        {
            ObjAnswer = JObject.Parse(result);
        }
    }
}
