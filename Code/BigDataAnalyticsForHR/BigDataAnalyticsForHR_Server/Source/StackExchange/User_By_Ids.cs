﻿using System;
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
    public class User_By_Ids
    {       
        // https://api.stackexchange.com/docs
        // https://api.stackexchange.com/2.1/users/1852224?order=desc&sort=reputation&site=stackoverflow

        int UserId;
        JObject UserData;

        /// <summary>
        /// Filter results. It works on StackOverFlow API and not on OnStartUps
        /// </summary>
        public string Filter
        {
            get
            {
                return "&filter=!9hnGstXfD";//!9hnGstJFC";
            }
        }

        public string UserUrlStackOverFlow
        {
            get
            {
                // With Filter
                return Constants.StackExchangeUrl + "users/" + UserId + "?site=" + Constants.api_site_parameter + Filter;

                // Without Filter
                //return Constants.StackExchangeUrl + "users?" + "inname=" + UserName + "&site=stackoverflow";
            }
        }

        public JObject GetUserData(int UserId)
        {
            //ArrUserData = new JArray();
            UserData = new JObject();
            this.UserId = UserId;

            Connect(UserUrlStackOverFlow);
            //Connect(UserUrlOnStartups);

            //UserData.Add("UserDetail", ArrUserData);

            //// Serialize JSON data into StackExchangeRoot Object.
            //String strUserData = JsonConvert.SerializeObject(UserData, Formatting.Indented);
            //Model.UserRoot user = JsonConvert.DeserializeObject<Model.UserRoot>(strUserData, new UserConverter());

            return UserData;
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
                    UpdateUserData(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateUserData(String result)
        {
            UserData = JObject.Parse(result);
            UserRoot user = JsonConvert.DeserializeObject<UserRoot>(result, new UserRootConverter());
            //JObject json = JObject.Parse(result);
            //if (json.ContainsKey("items"))
            //{
            //    if (((JArray)json["items"]).Count > 0)
            //    {
            //        //Model.StackExchangeUserModel stackExchange = JsonConvert.DeserializeObject<Model.StackExchangeUserModel>(result, new StackExchangeConverter());
            //        if (ArrUserData.Count > 0)
            //        {
            //            ((JArray)((JObject)ArrUserData[0])["items"]).Add(json);
            //        }
            //        else
            //            ArrUserData.Add(json);
            //    }
            //}
        }
    }
}
