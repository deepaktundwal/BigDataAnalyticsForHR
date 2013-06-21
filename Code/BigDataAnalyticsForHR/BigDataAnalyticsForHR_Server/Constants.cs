using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Model.StackExchange;

namespace Server
{
    public class Constants
    {
        public static string StackExchangeUrl = "https://api.stackexchange.com/2.1/";
        public static string api_site_parameter = "stackoverflow";
        public static bool IsNetworkSiteLoaded = false;
        public static JObject ApiParameterDict;
        //public static UserScoreDetail ObjUserScoreDetail = new UserScoreDetail();
    }
}
