using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace VKParserUI
{

    class VkApi
    {

        private const int APP_ID = 6033617;
        private const string SERVICE_KEY_FOR_ACCESS = "43ac549643ac549643ac54961343f04447443ac43ac54961ab6460b3898cdfd5cbb0a3a";
        private const string VK_URL = "https://api.vk.com/method/";

        private String ACCESS_TOKEN = null;
        private String USER_ID = null;

        public VkApi() { }

        public VkApi(String accessToken, String userId)
        {
            this.ACCESS_TOKEN = accessToken;
            this.USER_ID = userId;
        }

        public ModelLikeRepost getLikesOrRepost(string _link, bool isLikes, int offset)
        {
            string _filter;
            if (isLikes)
            {
                _filter = "likes";
            }
            else
            {
                _filter = "copies";
            }

            string _type = "post";

            string _owner_id = "";
            string _item_id = "";
            string pattern = @"\d+";
            int count_match = 0;
            foreach (Match m in Regex.Matches(_link, pattern))
            {
                switch (count_match)
                {
                    case 0:
                        _owner_id = m.Value;
                        count_match++;
                        break;
                    case 1:
                        _item_id = m.Value;
                        break;
                }
            }

            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("access_token", SERVICE_KEY_FOR_ACCESS);
            GetInformation.AddUrlParam("owner_id", _owner_id);
            GetInformation.AddUrlParam("item_id", _item_id);
            GetInformation.AddUrlParam("type", _type);
            GetInformation.AddUrlParam("filter", _filter);
            GetInformation.AddUrlParam("extended", "1");
            GetInformation.AddUrlParam("offset", offset);

            string Result = GetInformation.Get(VK_URL + "likes.getList").ToString();
            ModelLikeRepost resultInModel = JsonConvert.DeserializeObject<ModelLikeRepost>(Result);

            return resultInModel;
        }

        public bool isNotAllLikeRepostResult(ModelLikeRepost modelLikeRepost)
        {
            return modelLikeRepost.response.count > modelLikeRepost.response.arrayItem.Count;
        }

        public ModelUser getUserInfo()
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("user_ids", USER_ID);
            GetInformation.AddUrlParam("access_token", ACCESS_TOKEN);
            GetInformation.AddUrlParam("fields", "city,country");

            string Result = GetInformation.Get(VK_URL + "users.get").ToString();
            ModelUser resultInModel = JsonConvert.DeserializeObject<ModelUser>(Result);

            return resultInModel;
        }

    }
}
