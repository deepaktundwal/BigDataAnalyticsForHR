using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Network_Users
    {
        public string site_name { get; set; }
        public string site_url { get; set; }
        public int user_id { get; set; }
        public int reputation { get; set; }
        public int account_id { get; set; }
        public int creation_date { get; set; }
        public BadgeCounts badge_counts { get; set; }
        public int last_access_date { get; set; }
        public int answer_count { get; set; }
        public int question_count { get; set; }
    }
}
