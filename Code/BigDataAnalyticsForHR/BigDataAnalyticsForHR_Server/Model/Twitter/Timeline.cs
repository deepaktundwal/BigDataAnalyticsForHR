using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.Twitter
{
    public class Timeline
    {
        public TwitterStatusRoot tStatusRoot { get; set; }
        public string screen_name { get; set; }
        public decimal id { get; set; }
        public int tweetIndex { get; set; }
    }

    public class TimelineConverter : CustomCreationConverter<Timeline>
    {
        public override Timeline Create(Type objectType)
        {
            return new Timeline();
        }
    }
}
