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

        public class User
        {

            [JsonProperty(PropertyName = "uid")]
            public int uId { get; set; }

            [JsonProperty(PropertyName = "first_name")]
            public string name { get; set; }

            [JsonProperty(PropertyName = "last_name")]
            public string surname { get; set; }

        }

    }

}