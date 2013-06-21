using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.LinkedIn
{
    public class Post
    {
        public Attachment attachment { get; set; }
        public Comments comments { get; set; }
        public object creationTimestamp { get; set; }
        public Creator creator { get; set; }
        public Likes likes { get; set; }
        public string summary { get; set; }
        public string title { get; set; }
    }

    public class Attachment
    {
        public string contentDomain { get; set; }
        public string contentUrl { get; set; }
        public string imageUrl { get; set; }
        public string summary { get; set; }
        public string title { get; set; }
    }

    public class Creator
    {
        public string firstName { get; set; }
        public string headline { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public string pictureUrl { get; set; }
    }

    public class AvailableActionsValue
    {
        public string code { get; set; }
    }

    public class AvailableActions
    {
        public int _total { get; set; }
        public List<AvailableActionsValue> values { get; set; }
    }

    public class RelationToViewer
    {
        public AvailableActions availableActions { get; set; }
    }

    public class CommentsValue
    {
        public object creationTimestamp { get; set; }
        public Creator creator { get; set; }
        public string id { get; set; }
        public RelationToViewer relationToViewer { get; set; }
        public string text { get; set; }
    }

    public class Comments
    {
        public int _total { get; set; }
        public List<CommentsValue> values { get; set; }
    }

    public class Person
    {
        public string firstName { get; set; }
        public string headline { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public string pictureUrl { get; set; }
    }

    public class LikesValue
    {
        public Person person { get; set; }
    }

    public class Likes
    {
        public int _total { get; set; }
        public List<LikesValue> values { get; set; }
    }

    public class PostRoot
    {
        public int _total { get; set; }
        public List<Post> values { get; set; }
    }

    public class PostRootConverter : CustomCreationConverter<PostRoot>
    {
        public override PostRoot Create(Type objectType)
        {
            return new PostRoot();
        }
    }
}
