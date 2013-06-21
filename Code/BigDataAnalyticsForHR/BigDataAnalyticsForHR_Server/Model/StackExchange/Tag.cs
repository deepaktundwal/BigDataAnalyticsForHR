using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Tag
    {
        // Filtered Fields
        public string name { get; set; }
        public int count { get; set; }
        public int user_id { get; set; }

        //// All Fields
        //public bool is_required { get; set; }
        //public bool is_moderator_only { get; set; }
        //public bool has_synonyms { get; set; }
    }
}
