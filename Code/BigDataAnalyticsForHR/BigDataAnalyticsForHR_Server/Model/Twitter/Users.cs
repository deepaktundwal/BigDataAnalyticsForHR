using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Twitter
{
    public class Users
    {
        public string id_str { get; set; }
        public int id { get; set; }
        public string profile_sidebar_border_color { get; set; }
        public bool contributors_enabled { get; set; }
        public string time_zone { get; set; }
        //The number of tweets (including retweets) issued by the user.
        public long statuses_count { get; set; }
        public bool default_profile_image { get; set; }
        public bool profile_background_tile { get; set; }
        public Entities entities { get; set; }
        public int favourites_count { get; set; }
        public string profile_sidebar_fill_color { get; set; }
        public string created_at { get; set; }
        public int utc_offset { get; set; }
        public int followers_count { get; set; }
        public string name { get; set; }
        public bool geo_enabled { get; set; }
        public bool @protected { get; set; }
        public string profile_background_color { get; set; }
        public string lang { get; set; }
        public int friends_count { get; set; }
        public string screen_name { get; set; }
        public bool default_profile { get; set; }
        public object url { get; set; }
        public string profile_background_image_url { get; set; }
        public bool verified { get; set; }
        public bool follow_request_sent { get; set; }
        public string profile_link_color { get; set; }
        public bool is_translator { get; set; }
        public string location { get; set; }
        public bool notifications { get; set; }
        public string profile_image_url_https { get; set; }
        public bool following { get; set; }
        public string profile_image_url { get; set; }
        //The number of public lists that this user is a member of.
        public int listed_count { get; set; }
        public bool profile_use_background_image { get; set; }
        public string profile_background_image_url_https { get; set; }
        public string profile_text_color { get; set; }
        //Nullable. If possible, the user's most recent tweet or retweet.
        public TwitterStatusRoot statusRoot { get; set; }
        public object description { get; set; }
        public MentionRoot mentionList { get; set; }
    }

    public class Entities
    {
        public List<object> urls { get; set; }
        public List<object> user_mentions { get; set; }
        public List<object> hashtags { get; set; }
        public List<Media> media { get; set; }
        public Description description { get; set; }
    }

    public class Media
    {
        public string type { get; set; }
        public string display_url { get; set; }
        public object source_status_id { get; set; }
        public string media_url { get; set; }
        public string media_url_https { get; set; }
        public Sizes sizes { get; set; }
        public string expanded_url { get; set; }
        public List<int> indices { get; set; }
        public string url { get; set; }
        public string id_str { get; set; }
        public long id { get; set; }
    }

    public class Sizes
    {
        public Thumb thumb { get; set; }
        public Small small { get; set; }
        public Large large { get; set; }
        public Medium medium { get; set; }
    }

    public class Url
    {
        public string display_url { get; set; }
        public string expanded_url { get; set; }
        public List<int> indices { get; set; }
        public List<Url> urls { get; set; }
    }

    public class Description
    {
        public List<object> urls { get; set; }
    }

    public class Thumb
    {
        public int w { get; set; }
        public string resize { get; set; }
        public int h { get; set; }
    }

    public class Small
    {
        public int w { get; set; }
        public string resize { get; set; }
        public int h { get; set; }
    }

    public class Large
    {
        public int w { get; set; }
        public string resize { get; set; }
        public int h { get; set; }
    }

    public class Medium
    {
        public int w { get; set; }
        public string resize { get; set; }
        public int h { get; set; }
    }


}
