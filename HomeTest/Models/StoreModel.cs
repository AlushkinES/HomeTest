using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeTest.DTO
{
    public class StoreModel : ItemBaseModel
    {
        public dynamic name { get; set; }
        public string type { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public dynamic lat { get; set; }
        public int lng { get; set; }
        public string hours { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ServiceModel> services { get; set; }
    }
}