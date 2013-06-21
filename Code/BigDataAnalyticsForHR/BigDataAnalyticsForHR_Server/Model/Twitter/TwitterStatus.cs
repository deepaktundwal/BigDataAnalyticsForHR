﻿using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Twitter
{
    public class TwitterStatus
    {
        public DateTime created_at { get; set; }
        public decimal id { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        public string source { get; set; }
        public bool truncated { get; set; }
        public long? in_reply_to_status_id { get; set; }
        public string in_reply_to_status_id_str { get; set; }
        public int? in_reply_to_user_id { get; set; }
        public string in_reply_to_user_id_str { get; set; }
        public string in_reply_to_screen_name { get; set; }
        public Users user { get; set; }
        public object geo { get; set; }
        public object coordinates { get; set; }
        public object place { get; set; }
        public object contributors { get; set; }
        public int retweet_count { get; set; }
        public int favorite_count { get; set; }
        public bool favorited { get; set; }
        public bool retweeted { get; set; }
        public string lang { get; set; }
        public TwitterStatus retweeted_status { get; set; }
        public List<TwitterStatus> retweets { get; set; }
        public Entities entities { get; set; }
        public bool? possibly_sensitive { get; set; }
    }

    public class TwitterStatusRoot
    {
        public List<TwitterStatus> items { get; set; }
    }

    public class TwitterStatusRootConverter : CustomCreationConverter<TwitterStatusRoot>
    {
        public override TwitterStatusRoot Create(Type objectType)
        {
            return new TwitterStatusRoot();
        }
    }
}
