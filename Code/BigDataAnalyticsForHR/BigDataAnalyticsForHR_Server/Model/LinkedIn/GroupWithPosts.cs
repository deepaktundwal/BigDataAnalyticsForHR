using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.LinkedIn
{
    public class GroupWithPosts
    {
        public Group group { get; set; }
        public PostRoot groupPostsbyUser { get; set; }

        public string userId { get; set; }
        public string groupId { get; set; }
        public int groupIndex { get; set; }
    }

    public class GroupWithPostsConverter : CustomCreationConverter<GroupWithPosts>
    {
        public override GroupWithPosts Create(Type objectType)
        {
            return new GroupWithPosts();
        }
    }

}
