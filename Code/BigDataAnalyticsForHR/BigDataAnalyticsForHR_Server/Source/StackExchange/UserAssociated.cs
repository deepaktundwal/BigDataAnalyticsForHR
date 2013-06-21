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
    /// Returns all of a user's associated accounts, given their account_ids in {ids}.
    /// </summary>
    public class UserAssociated
    {
        //https://api.stackexchange.com/2.1/users/9668/associated
        int AccountId;
        JObject network_users;

        public string UserUrl
        {
            get
            {
                return Constants.StackExchangeUrl + "users/" + AccountId + "/associated";
            }
        }

        public JObject GetUserAssociatedAccounts(int AccountId)
        {
            network_users = new JObject();
            this.AccountId = AccountId;

            Connect(UserUrl);


            // Serialize JSON data into StackExchangeRoot Object.
            String strUserData = JsonConvert.SerializeObject(network_users, Formatting.Indented);
            NetworkUserRoot networkUser = JsonConvert.DeserializeObject<NetworkUserRoot>(strUserData, new NetworkUserRootConverter());

            return network_users;
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
                    UpdateUserAssociatedAccounts(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateUserAssociatedAccounts(String result)
        {
            network_users = JObject.Parse(result);
        }
    }
}
