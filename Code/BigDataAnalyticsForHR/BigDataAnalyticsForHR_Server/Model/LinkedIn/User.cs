using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.LinkedIn
{
    public class User
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string maidenname { get; set; }
        public string headline { get; set; }
        public Location location { get; set; }
        public string pictureUrl { get; set; }
        public Positions positions { get; set; }
        public string publicProfileUrl { get; set; }
        public string specialties { get; set; }
        public string associations { get; set; }
        public Connections connections { get; set; }
        public DateOfBirth dateOfBirth { get; set; }
        public Educations educations { get; set; }
        public string emailAddress { get; set; }
        public Following following { get; set; }
        public GroupMemberships groupMemberships { get; set; }
        public string honors { get; set; }
        public ImAccounts imAccounts { get; set; }
        public string industry { get; set; }
        public string interests { get; set; }
        public JobBookmarks jobBookmarks { get; set; }
        public int numRecommenders { get; set; }
        public PhoneNumbers phoneNumbers { get; set; }
        public RecommendationsReceived recommendationsReceived { get; set; }
        public Skills skills { get; set; }
        public Suggestions suggestions { get; set; }
        public string summary { get; set; }
        public TwitterAccounts twitterAccounts { get; set; }
    }

    public class UserConverter : CustomCreationConverter<User>
    {
        public override User Create(Type objectType)
        {
            return new User();
        }
    }


}
