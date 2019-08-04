using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeTest.DTO
{
    public class ProductModel : ItemBaseModel
    {
        //dynamic type need to check unsuccessful responses
        public dynamic name { get; set; }
        public string type { get; set; }
        public dynamic price { get; set; }
        public decimal shipping { get; set; }
        public string upc { get; set; }
        public string description { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List <CategoryModel> categories { get; set; }
    }
}