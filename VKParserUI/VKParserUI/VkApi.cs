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

        private readonly List<string> requestTypeParams = new List<string> { "post", "comment", "photo", "video", "note", "market", "photo_comment", "video_comment", "topic_comment", "market_comment" };

        public VkApi() { }

        public VkApi(String accessToken, String userId)
        {
            this.ACCESS_TOKEN = accessToken;
            this.USER_ID = userId;
        }

        public VkApi(String accessToken)
        {
            this.ACCESS_TOKEN = accessToken;
        }

        public ModelLikeRepost getLikesOrRepost(string _link, bool isLikes, int offset, int selectedType)
        {
            if (_link == null)
            {
                return new ModelLikeRepost(null, new ModelLikeRepost.Error(-1, "Не задана ссылка на ресурс."));
            }

            string _filter = isLikes ? "likes" : "copies";
            string _type = requestTypeParams[selectedType];

            string[] ownerItemId = selectOwnerItemId(_link, selectedType);
            string _owner_id = ownerItemId[0];
            string _item_id = ownerItemId[1];

            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("access_token", ACCESS_TOKEN == null ? SERVICE_KEY_FOR_ACCESS : ACCESS_TOKEN);
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

        private string[] selectOwnerItemId(string _link, int selectedType)
        {
            string[] ownerItemId = new string[3];

            switch (requestTypeParams[selectedType])
            {
                case "post":
                    string patternPost = @"wall\d+_\d+";
                    string patternGroupPost = @"wall-\d+_\d+";
                    foreach (Match m in Regex.Matches(_link, patternPost))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupPost))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    break;
                case "comment":
                    string patternPostComment = @"wall\d+_\d+_r\d+";
                    string patternGroupPostComment = @"wall-\d+_\d+_r\d+";
                    foreach (Match m in Regex.Matches(_link, patternPostComment))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupPostComment))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    ownerItemId[1] = ownerItemId[2];
                    break;
                case "photo":
                    string patternPhoto = @"photo\d+_\d+";
                    string patternGroupPhoto = @"photo-\d+_\d+";
                    foreach (Match m in Regex.Matches(_link, patternPhoto))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupPhoto))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    break;
                case "photo_comment":
                    string patternPhotoComment = @"photo\d+_\d+_r\d+";
                    string patternGroupPhotoComment = @"photo-\d+_\d+_r\d+";
                    foreach (Match m in Regex.Matches(_link, patternPhotoComment))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupPhotoComment))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    ownerItemId[1] = ownerItemId[2];
                    break;
                case "video":
                    string patternVideo = @"video\d+_\d+";
                    string patternGroupViseo = @"video-\d+_\d+";
                    foreach (Match m in Regex.Matches(_link, patternVideo))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupViseo))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    break;
                case "video_comment":
                    string patternVideoComment = @"video\d+_\d+_r\d+";
                    string patternGroupVideoComment = @"video-\d+_\d+_r\d+";
                    foreach (Match m in Regex.Matches(_link, patternVideoComment))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupVideoComment))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    ownerItemId[1] = ownerItemId[2];
                    break;
                case "note":
                    string patternNote = @"note\d+_\d+";
                    string patternGroupNote = @"note-\d+_\d+";
                    foreach (Match m in Regex.Matches(_link, patternNote))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupNote))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    break;
                case "market":
                    string patternProduct = @"product\d+_\d+";
                    string patternGroupProduct = @"product-\d+_\d+";
                    foreach (Match m in Regex.Matches(_link, patternProduct))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupProduct))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    break;
                case "market_comment":
                    string patternProductComment = @"product\d+_\d+_r\d+";
                    string patternGroupProductComment = @"product-\d+_\d+_r\d+";
                    foreach (Match m in Regex.Matches(_link, patternProductComment))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupProductComment))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    ownerItemId[1] = ownerItemId[2];
                    break;
                case "topic_comment":
                    string patternTopicComment = @"topic\d+_\d+\?post=\d+";
                    string patternGroupTopicComment = @"topic-\d+_\d+\?post=\d+";
                    foreach (Match m in Regex.Matches(_link, patternTopicComment))
                        ownerItemId = getOwnerItemId(m.Value);
                    foreach (Match m in Regex.Matches(_link, patternGroupTopicComment))
                    {
                        ownerItemId = getOwnerItemId(m.Value);
                        ownerItemId[0] = "-" + ownerItemId[0];
                    }
                    ownerItemId[1] = ownerItemId[2];
                    break;
            }

            return ownerItemId;
        }

        private string[] getOwnerItemId(string from)
        {
            string pattern = @"\d+";
            int count_match = 0;
            string[] values = new string[3];

            foreach (Match m in Regex.Matches(from, pattern))
            {
                switch (count_match)
                {
                    case 0:
                        values[0] = m.Value;
                        count_match++;
                        break;
                    case 1:
                        values[1] = m.Value;
                        count_match++;
                        break;
                    case 2:
                        values[2] = m.Value;
                        count_match++;
                        break;
                }
            }

            return values;
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

            string Result = GetInformation.Get(VK_URL + "users.get").ToString();
            ModelUser resultInModel = JsonConvert.DeserializeObject<ModelUser>(Result);

            return resultInModel;
        }

        public ModelCountriesAndCities getCountries()
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("need_all", "1");
            GetInformation.AddUrlParam("count", "300");
            GetInformation.AddUrlParam("access_token", ACCESS_TOKEN == null ? SERVICE_KEY_FOR_ACCESS : ACCESS_TOKEN);

            string Result = GetInformation.Get(VK_URL + "database.getCountries").ToString();
            ModelCountriesAndCities resultInModel = JsonConvert.DeserializeObject<ModelCountriesAndCities>(Result);

            return resultInModel;
        }

        public ModelCountriesAndCities getCitiesByCountry(ModelCountriesAndCities.CountryOrCity country)
        {
            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("country_id", country.id.ToString());
            GetInformation.AddUrlParam("need_all", "0");
            GetInformation.AddUrlParam("count", "1000");
            GetInformation.AddUrlParam("access_token", ACCESS_TOKEN == null ? SERVICE_KEY_FOR_ACCESS : ACCESS_TOKEN);

            string Result = GetInformation.Get(VK_URL + "database.getCities").ToString();
            ModelCountriesAndCities resultInModel = JsonConvert.DeserializeObject<ModelCountriesAndCities>(Result);

            return resultInModel;
        }

        public ModelGroup searchGroups(string whatSearch, int searchType, ModelCountriesAndCities.CountryOrCity country,
            ModelCountriesAndCities.CountryOrCity city, bool isFutureEvent, bool isWithMarket, int sortType, int offset)
        {
            string sSearchType;
            switch (searchType)
            {
                case 0:
                    sSearchType = "group,event,page";
                    break;
                case 1:
                    sSearchType = "group";
                    break;
                case 2:
                    sSearchType = "event";
                    break;
                case 3:
                    sSearchType = "page";
                    break;
                default:
                    sSearchType = "group,event,page";
                    break;

            }

            HttpRequest GetInformation = new HttpRequest();
            GetInformation.AddUrlParam("q", whatSearch);
            GetInformation.AddUrlParam("type", sSearchType);
            GetInformation.AddUrlParam("country_id", country != null ? country.id.ToString() : "");
            GetInformation.AddUrlParam("city_id", city != null ? city.id.ToString() : "");
            GetInformation.AddUrlParam("future", isFutureEvent ? 1 : 0);
            GetInformation.AddUrlParam("market", isWithMarket ? 1 : 0);
            GetInformation.AddUrlParam("sort", sortType);
            GetInformation.AddUrlParam("offset", offset);
            GetInformation.AddUrlParam("access_token", ACCESS_TOKEN);

            string Result = GetInformation.Get(VK_URL + "groups.search").ToString();
            string pattern = @":\[\d+";
            int count = 0;
            foreach (Match m in Regex.Matches(Result, pattern))
            {
                string sCount = m.Value.Remove(0, 2);
                count = Convert.ToInt32(sCount);
            }
            int i = Result.IndexOf(",{");
            if (i != -1)
            {
                Result = Result.Remove(13, i - 12);
                ModelGroup resultInModel = JsonConvert.DeserializeObject<ModelGroup>(Result);
                resultInModel.count = count;
                return resultInModel;
            }
            else
            {
                return null;
            }
        }

    }
}
