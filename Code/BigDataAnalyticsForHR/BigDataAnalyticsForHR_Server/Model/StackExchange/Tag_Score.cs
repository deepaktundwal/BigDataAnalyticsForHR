using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Tag_Score
    {
        public Shallow_User user { get; set; }
        public int score { get; set; }
        public int post_count { get; set; }
    }
}
