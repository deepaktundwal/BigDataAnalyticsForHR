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
    /// Returns all sites in the network.
    /// </summary>
    public class Sites
    {
        // https://api.stackexchange.com/2.1/sites?filter=!)QpaLg*uGUux1-cWa.0XugNr
        JObject SiteObject;

        public string Filter
        {
            get
            {
                return "!)QpaLg*uGUux1-cWa.0XugNr";
            }
        }

        private String PrepareUrl()
        {
            String Url = "";
            Url = Constants.StackExchangeUrl + "sites?" + "filter=" + Filter;
            return Url;
        }

        public JObject GetStackExchangeSites()
        {
            SiteObject = new JObject();

            String Url = PrepareUrl();
            Connect(Url);


            // Serialize JSON data into SiteRoot Object.
            String strSiteData = JsonConvert.SerializeObject(SiteObject, Formatting.Indented);
            SiteRoot siteData = JsonConvert.DeserializeObject<SiteRoot>(strSiteData, new SiteRootConverter());

            return SiteObject;
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
                    UpdateStackExchangeSites(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateStackExchangeSites(String result)
        {
            SiteObject = JObject.Parse(result);
        }

    }
}
