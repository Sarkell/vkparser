using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VKParserUI
{
    class ModelCountriesAndCities
    {

        [JsonProperty(PropertyName = "response")]
        public List<CountryOrCity> listOfCountryOrCity { set; get; }

        public class CountryOrCity
        {

            [JsonProperty(PropertyName = "cid")]
            public int id { set; get; }

            [JsonProperty(PropertyName = "title")]
            public string title { set; get; }
        }

    }
}
