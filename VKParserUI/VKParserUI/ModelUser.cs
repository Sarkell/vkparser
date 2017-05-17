using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKParserUI
{
    class ModelUser
    {

        [JsonProperty(PropertyName = "response")]
        public List<User> users { get; set; }

        [JsonProperty(PropertyName = "error")]
        public Error error { get; set; }

        public class User
        {

            [JsonProperty(PropertyName = "uid")]
            public int uId { get; set; }

            [JsonProperty(PropertyName = "first_name")]
            public string name { get; set; }

            [JsonProperty(PropertyName = "last_name")]
            public string surname { get; set; }

        }

        public class Error
        {
            [JsonProperty(PropertyName = "error_code")]
            public int errorCode { get; set; }

            [JsonProperty(PropertyName = "error_msg")]
            public string errorMsg { get; set; }
        }

    }

}