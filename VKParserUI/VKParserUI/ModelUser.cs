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

        // just for mocking data
        public ModelUser()
        {
            users = new List<User>();
        }

        public class User
        {
            // just for mocking data
            public User(int uid, string name, string surname)
            {
                uId = uId;
                this.name = name;
                this.surname = surname;
            }

            [JsonProperty(PropertyName = "uid")]
            public int uId { get; set; }

            [JsonProperty(PropertyName = "first_name")]
            public string name { get; set; }

            [JsonProperty(PropertyName = "last_name")]
            public string surname { get; set; }

            public string signature
            {
                get
                {
                    return surname + " " + name;
                }
            }



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