using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Model.StackExchange;


namespace Server.Source.StackExchange
{
    public class UserQuestions
    {
        // https://api.stackexchange.com/2.1/users/340334/questions?order=desc&sort=activity&site=stackoverflow&filter=!)Rv(Cykq3vES*a2KPI4B7nRZ
        int UserId;
        JObject ObjQuestion;

        public string Filter
        {
            get
            {
                return "!)Rv(Cykq3vES*a2KPI4B7nRZ";
            }
        }

        public string UserUrl
        {
            get
            {
                return Constants.StackExchangeUrl + "users/" + UserId + "/questions?order=desc&sort=activity&site=" + "stackoverflow" + "&filter=" + Filter;
            }
        }

        public JObject GetUserQuestions(int UserId)
        {
            ObjQuestion = new JObject();
            this.UserId = UserId;

            Connect(UserUrl);

            //// Serialize JSON data into StackExchangeRoot Object.
            //String strQuestionData = JsonConvert.SerializeObject(ObjQuestion, Formatting.Indented);
            //QuestionRoot Question = JsonConvert.DeserializeObject<Model.QuestionRoot>(strQuestionData, new QuestionRootConverter());

            return ObjQuestion;
        }

        private void Connect(String Url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    UpdateAnswerData(json);
                }
            }
            catch (Exception e)
            { throw e; }
        }

        private void UpdateAnswerData(String result)
        {
            ObjQuestion = JObject.Parse(result);
        }
    }
}
