using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Server.Model.StackExchange;

namespace WCFServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBigDataAnalyser" in both code and config file together.
    [ServiceContract]
    public interface IBigDataAnalyser
    {
        [OperationContract]
        void DoWork();

        [OperationContract(Name = "GetUserDetail")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/GetUserDetail")]
        String GetUserDetail(Stream userDetails);

        [OperationContract(Name = "GetUserDetailById")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/GetUserDetailById")]
        String GetUserDetailById(Stream userDetails);

        [OperationContract(Name = "GetUserQuestion")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/GetUserQuestion")]
        String GetUserQuestion(Stream userDetails);

        [OperationContract(Name = "Twitter_SearchUser")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/Twitter_SearchUser")]
        String Twitter_SearchUser(Stream userDetails);

        [OperationContract(Name = "Twitter_UserTimeline")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/Twitter_UserTimeline")]
        String Twitter_UserTimeline(Stream userDetails);

        [OperationContract(Name = "Twitter_GetRetweets")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/Twitter_GetRetweets")]
        String Twitter_GetRetweets(Stream userDetails);

        [OperationContract(Name = "Twitter_GetMentionList")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/Twitter_GetMentionList")]
        String Twitter_GetMentionList(Stream userDetails);

        [OperationContract(Name = "LinkedIn_SearchUser")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/LinkedIn_SearchUser")]
        String LinkedIn_SearchUser(Stream userDetails);

        [OperationContract(Name = "LinkedIn_UserDetail")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/LinkedIn_UserDetail")]
        String LinkedIn_UserDetail(Stream userDetails);

        [OperationContract(Name = "LinkedIn_GetGroupDetail")]
        [WebInvoke(Method = "POST",
         UriTemplate = "/LinkedIn_GetGroupDetail")]
        String LinkedIn_GetGroupDetail(Stream userDetails);
    }

}
