using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Shallow_User
    {
        public int user_id { get; set; }
        public string display_name { get; set; }
        public int reputation { get; set; }
        public string user_type { get; set; }
        public string profile_image { get; set; }
        public string link { get; set; }
        public int accept_rate { get; set; }
    }
}
