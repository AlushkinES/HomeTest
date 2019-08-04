using System.Collections.Generic;

namespace HomeTest.DTO
{
    public class ProductsModel
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int skip { get; set; }
        public List<ProductModel> data { get; set; }
    }
}