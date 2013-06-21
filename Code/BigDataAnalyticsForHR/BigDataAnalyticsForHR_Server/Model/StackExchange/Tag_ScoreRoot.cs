using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.StackExchange
{
    public class Tag_ScoreRoot
    {
        public List<Tag_Score> items { get; set; }
        public int quota_remaining { get; set; }
        public int quota_max { get; set; }
        public bool has_more { get; set; }
    }

    public class Tag_ScoreRootConverter : CustomCreationConverter<Tag_ScoreRoot>
    {
        public override Tag_ScoreRoot Create(Type objectType)
        {
            return new Tag_ScoreRoot();
        }
    }
}
