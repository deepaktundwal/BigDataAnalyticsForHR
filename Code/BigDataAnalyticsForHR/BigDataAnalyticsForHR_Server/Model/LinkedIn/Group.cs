using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.LinkedIn
{
    public class Category
    {
        public string code { get; set; }
    }

    public class Group
    {
        public Category category { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string largeLogoUrl { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public string siteGroupUrl { get; set; }
        public string smallLogoUrl { get; set; }
        
        // custom field: Store user posts details within group
        public PostRoot groupPostsbyUser { get; set; }
    }

    public class GroupConverter : CustomCreationConverter<Group>
    {
        public override Group Create(Type objectType)
        {
            return new Group();
        }
    }

    public class GroupRoot
    {
        public Group people { get; set; }
    }

    public class GroupRootConverter : CustomCreationConverter<GroupRoot>
    {
        public override GroupRoot Create(Type objectType)
        {
            return new GroupRoot();
        }
    }
}
