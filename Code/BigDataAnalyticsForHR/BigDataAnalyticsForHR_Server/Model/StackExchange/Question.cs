using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Question
    {
        public int question_id { get; set; }
        public int last_edit_date { get; set; }
        public int creation_date { get; set; }
        public int last_activity_date { get; set; }
        public int score { get; set; }
        public int answer_count { get; set; }
        public string body { get; set; }
        public string title { get; set; }
        public List<string> tags { get; set; }
        public int view_count { get; set; }
        public Owner owner { get; set; }
        public List<Comment> comments { get; set; }
        public List<Answer> answers { get; set; }
        public string link { get; set; }
        public bool is_answered { get; set; }
        public int? accepted_answer_id { get; set; }
    }

    public class Owner
    {
        public int user_id { get; set; }
        public string display_name { get; set; }
        public int reputation { get; set; }
        public string user_type { get; set; }
        public string profile_image { get; set; }
        public string link { get; set; }
        public int accept_rate { get; set; }
    }

    public class Comment
    {
        public int comment_id { get; set; }
        public int post_id { get; set; }
        public int creation_date { get; set; }
        public int score { get; set; }
        public bool edited { get; set; }
        public string body { get; set; }
        public Owner owner { get; set; }
    }
}
