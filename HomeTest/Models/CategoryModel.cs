using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeTest.DTO
{
    public class CategoryModel : ItemBaseModel
    {
        public dynamic name { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime createdAt { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime updatedAt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CategoryModel> subcategories{get; set;}
    }
}