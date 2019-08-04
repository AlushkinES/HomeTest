using System.Collections.Generic;

namespace HomeTest.DTO
{
    public class CategoriesModel : ItemsBaseModel
    {
        public List<CategoryModel> data { get; set; }
    }
}