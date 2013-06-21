using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Answer
    {
        // Filtered Fields
        public int question_id { get; set; }
        public int answer_id { get; set; }
        public int creation_date { get; set; }
        public int last_edit_date { get; set; }
        public int last_activity_date { get; set; }
        public int score { get; set; }
        public bool is_accepted { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public List<string> tags { get; set; }

        //// All Fields
        //public Shallow_User owner { get; set; }
        //public int up_vote_count { get; set; }
        //public int down_vote_count { get; set; }
    }
}
