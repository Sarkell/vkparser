using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKParserUI
{

    class ModelLikeRepost
    {

        public ModelLikeRepost(Response newResponse, Error newError)
        {
            this.response = newResponse;
            this.error = newError;
        }

        [JsonProperty(PropertyName = "response")]
        public Response response { get; set; }

        [JsonProperty(PropertyName = "error")]
        public Error error { get; set; }

        public class Response
        {
            [JsonProperty(PropertyName = "count")]
            public int count { get; set; }

            [JsonProperty(PropertyName = "items")]
            public List<Item> arrayItem { get; set; }

            public class Item
            {
                public Item(string newType, int newUId, string newFirstName, string newLastName)
                {
                    type = newType;
                    uid = newUId;
                    FirstName = newFirstName;
                    LastName = newLastName;
                }

                [JsonProperty(PropertyName = "type")]
                public string type { get; set; }

                [JsonProperty(PropertyName = "uid")]
                public int uid { get; set; }

                [JsonProperty(PropertyName = "first_name")]
                public string FirstName { get; set; }

                [JsonProperty(PropertyName = "last_name")]
                public string LastName { get; set; }

                public string Signature
                {
                    get
                    {
                        return LastName + " " + FirstName;
                    }
                }
            }
        }

        public class Error
        {
            public Error(int newErrorCode, string newErrorMsg)
            {
                this.errorCode = newErrorCode;
                this.errorMsg = newErrorMsg;
            }

            [JsonProperty(PropertyName = "error_code")]
            public int errorCode { get; set; }

            [JsonProperty(PropertyName = "error_msg")]
            public string errorMsg { get; set; }
        }
    }
}
