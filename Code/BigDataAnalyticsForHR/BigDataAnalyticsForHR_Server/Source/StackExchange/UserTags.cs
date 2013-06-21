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
    /// Returns the tags the users identified in {ids} have been active in.
    /// https://api.stackexchange.com/docs/tags-on-users
    /// </summary>
    public class UserTags
    {
        //https://api.stackexchange.com/2.1/users/1852224/tags?site=stackoverflow&filter=!-t6dD.mE
        int UserId;
        JObject TagObject;

        public string UserUrl
        {
            get
            {
                return Constants.StackExchangeUrl + "users/" + UserId + "/tags?" + "site=" + Constants.api_site_parameter + "&filter=" + Filter;
            }
        }

        public string Filter
        {
            get
            {
                return "!-t6dD.mE";
            }
        }

        public JObject GetUserTagList(int UserId)
        {
            TagObject = new JObject();
            this.UserId = UserId;

            Connect(UserUrl);


            // Serialize JSON data into StackExchangeRoot Object.
            String strUserData = JsonConvert.SerializeObject(TagObject, Formatting.Indented);
            TagRoot tagData = JsonConvert.DeserializeObject<TagRoot>(strUserData, new TagRootConverter());

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
                    UpdateTagsOnUser(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateTagsOnUser(String result)
        {
            TagObject = JObject.Parse(result);
        }
    }
}

