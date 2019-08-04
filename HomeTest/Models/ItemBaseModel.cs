using Newtonsoft.Json;

namespace HomeTest.DTO
{
    public class ItemBaseModel
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string id { get; set; }
    }
}