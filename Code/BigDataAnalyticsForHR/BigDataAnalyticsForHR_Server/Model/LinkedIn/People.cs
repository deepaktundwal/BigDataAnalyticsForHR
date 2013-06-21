using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model.LinkedIn
{
    public class TwitterAccounts
    {
        public int _total { get; set; }
        public List<TwitterAccountDetail> values { get; set; }
    }

    public class TwitterAccountDetail
    {
        public string providerAccountId { get; set; }
        public string providerAccountName { get; set; }
    }

    public class Suggestions
    {
        public ToFollow toFollow { get; set; }
    }

    public class ToFollow
    {
        public Companies companies { get; set; }
        public Industries industries { get; set; }
        public NewsSources newsSources { get; set; }
        public People people { get; set; }
    }

    public class NewsSources
    {
        public int _total { get; set; }
        public List<NewsSourcesValues> values { get; set; }
    }

    public class NewsSourcesValues
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Industries
    {
        public int _total { get; set; }
        public List<IndustriesValues> values { get; set; }
    }

    public class IndustriesValues
    {
        public int id { get; set; }
    }

    public class Companies
    {
        public int _count { get; set; }
        public int _start { get; set; }
        public int _total { get; set; }
        public List<CompaniesValues> values { get; set; }
    }

    public class CompaniesValues
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Skills
    {
        public int _total { get; set; }
        public List<SkillsValues> values { get; set; }
    }

    public class SkillsValues
    {
        public int id { get; set; }
        public Skill skill { get; set; }
    }

    public class Skill
    {
        public string name { get; set; }
    }

    public class RecommendationsReceived
    {
        public int _total { get; set; }
        public List<RecommendationsReceivedValues> values { get; set; }
    }

    public class RecommendationsReceivedValues
    {
        public int id { get; set; }
        public string recommendationText { get; set; }
        public RecommendationType recommendationType { get; set; }
        public Recommender recommender { get; set; }
    }

    public class Recommender
    {
        public string firstName { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
    }

    public class RecommendationType
    {
        public string code { get; set; }
    }

    public class Positions
    {
        public int _total { get; set; }
        public List<PositionsValues> values { get; set; }
    }

    public class PositionsValues
    {
        public Company company { get; set; }
        public int id { get; set; }
        public bool isCurrent { get; set; }
        public StartDate startDate { get; set; }
        public string title { get; set; }
        public EndDate endDate { get; set; }
        public string summary { get; set; }
    }

    public class Company
    {
        public int id { get; set; }
        public string industry { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string ticker { get; set; }
        public string type { get; set; }
    }

    public class StartDate
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    public class EndDate
    {
        public int month { get; set; }
        public int year { get; set; }
    }

    public class PhoneNumbers
    {
        public int _total { get; set; }
    }

    public class Location
    {
        public Country country { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string code { get; set; }
    }

    public class JobBookmarks
    {
        public int _total { get; set; }
        public List<JobBookmarksValues> values { get; set; }
    }

    public class JobBookmarksValues
    {
        public object applyTimestamp { get; set; }
        public bool isApplied { get; set; }
        public bool isSaved { get; set; }
        public Job job { get; set; }
        public object savedTimestamp { get; set; }
    }

    public class Job
    {
        public bool active { get; set; }
        public Company company { get; set; }
        public string descriptionSnippet { get; set; }
        public int id { get; set; }
        public Position position { get; set; }
        public object postingTimestamp { get; set; }
    }

    public class Position
    {
        public string title { get; set; }
    }

    public class ImAccounts
    {
        public int _total { get; set; }
        public List<ImAccountsValues> values { get; set; }
    }

    public class ImAccountsValues
    {
        public string imAccountName { get; set; }
        public string imAccountType { get; set; }
    }

    public class GroupMemberships
    {
        public int _total { get; set; }
        public List<GroupMembershipsValues> values { get; set; }
    }

    public class GroupMembershipsValues
    {
        public string _key { get; set; }
        public Group group { get; set; }
        public MembershipState membershipState { get; set; }
    }

    public class MembershipState
    {
        public string code { get; set; }
    }

    public class Following
    {
        public Companies companies { get; set; }
        public Industries industries { get; set; }
        public People people { get; set; }
        public SpecialEditions specialEditions { get; set; }
    }

    public class SpecialEditions
    {
        public int _total { get; set; }
    }

    public class People
    {
        public int _count { get; set; }
        public int _start { get; set; }
        public int _total { get; set; }
        public List<User> values { get; set; }
    }

    public class Educations
    {
        public int _total { get; set; }
        public List<EducationsValues> values { get; set; }
    }

    public class EducationsValues
    {
        public string activities { get; set; }
        public string degree { get; set; }
        public EndDate endDate { get; set; }
        public string fieldOfStudy { get; set; }
        public int id { get; set; }
        public string notes { get; set; }
        public string schoolName { get; set; }
        public StartDate startDate { get; set; }
    }

    public class DateOfBirth
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Connections
    {
        public int _count { get; set; }
        public int _start { get; set; }
        public int _total { get; set; }
        public List<ConnectionsValues> values { get; set; }
    }

    public class ConnectionsValues
    {
        public ApiStandardProfileRequest apiStandardProfileRequest { get; set; }
        public string firstName { get; set; }
        public string headline { get; set; }
        public string id { get; set; }
        public string industry { get; set; }
        public string lastName { get; set; }
        public Location location { get; set; }
        public string pictureUrl { get; set; }
        public SiteStandardProfileRequest siteStandardProfileRequest { get; set; }
    }

    public class ApiStandardProfileRequest
    {
        public Headers headers { get; set; }
        public string url { get; set; }
    }

    public class Headers
    {
        public int _total { get; set; }
        public List<HeadersValues> values { get; set; }
    }

    public class HeadersValues
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class SiteStandardProfileRequest
    {
        public string url { get; set; }
    }

    public class PeopleRoot
    {
        public People people { get; set; }
    }

    public class PeopleRootConverter : CustomCreationConverter<PeopleRoot>
    {
        public override PeopleRoot Create(Type objectType)
        {
            return new PeopleRoot();
        }
    }
}
