using Newtonsoft.Json;
using System.Collections.Generic;

namespace VKParserUI
{
    class ModelGroup
    {

        public int count { get; set; }

        [JsonProperty(PropertyName = "response")]
        public List<Group> groups { get; set; }

        [JsonProperty(PropertyName = "error")]
        public Error error { get; set; }

        public class Group
        {
            [JsonProperty(PropertyName = "gid")]
            public int id { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string name { get; set; }

            [JsonProperty(PropertyName = "screen_name")]
            public string screenName { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string type { get; set; }

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
