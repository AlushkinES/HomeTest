using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using HomeTest.DTO;
using NUnit.Framework;
using RestApiHelper;

namespace HomeTest
{
    [TestFixture]
    [NUnit.Framework.Category("Category")]
    [Timeout(30000)]
    public class CategoryTests
    {
        private RestClient RestClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var restClient = new RestClient();
        }
        
        [Test]
        [NUnit.Framework.Description("Get categories without additional query")]
        public async Task GetCategories()
        {
            var response = await RestClient.GetAllItems("categories");
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(10), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(25)]
        [NUnit.Framework.Description("Get categories by limit")]
        public async Task GetCategories_ByLimit(int limit)
        {
            var response = await RestClient.GetAllItems("categories?$limit=" + limit);
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(limit), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [NUnit.Framework.Description("Check max limit")]
        public async Task GetCategories_MaxLimit()
        {
            var response = await RestClient.GetAllItems("categories?$limit=99");
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(25), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [NUnit.Framework.Description("Successful getting category")]
        public async Task GetCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var storeResult = await RestClient.CreateItem("categories", category);
            var storeId = storeResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion

            var response = await RestClient.GetItem("categories", storeId);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
            Assert.That(result.id, Is.EqualTo(category.id), "Check category id");
        }
        
        [Test]
        [NUnit.Framework.Description("Unsuccessful getting category with NotFound error")]
        public async Task GetCategory_NotFound()
        {
            var response = await RestClient.GetItem("categories", "0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [NUnit.Framework.Description("Successful deleting category")]
        public async Task DeleteCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var itemResult = await RestClient.CreateItem("categories", category);
            var itemId = itemResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var response = await RestClient.DeleteItem("categories", itemId);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
        }
        
        [Test]
        [NUnit.Framework.Description("Unsuccessful deleting category")]
        public async Task DeleteCategory_NotFound()
        {
            var response = await RestClient.DeleteItem("categories", "0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [NUnit.Framework.Description("Successful creating directory")]
        public async Task CreateCategory_Success()
        {
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test name"
            };
            
            var response = await RestClient.CreateItem("categories", category);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
            Assert.That(result.id, Is.EqualTo(category.id), "Check category id");
        }
        
        [Test]
        [NUnit.Framework.Description("Unsuccessful creating directory")]
        public async Task CreateCategory_InvalidData()
        {
            var category = new CategoryModel 
            {
                name = 123
            };
            
            var response = await RestClient.CreateItem("categories", category, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"should have required property 'id'\"\r\n]"), "Check errors message");
        }
        
        [Test]
        [NUnit.Framework.Description("Successful updating directory")]
        public async Task UpdateCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var itemResult = await RestClient.CreateItem("categories", category);
            var itemId = itemResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var modifiedCategory = new CategoryModel
            {
                name = "Test123"
            };
            
            var response = await RestClient.UpdateItem("categories", itemId, modifiedCategory);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(modifiedCategory.name), "Check category name");
        }
        
        [Test]
        [NUnit.Framework.Description("Unsuccessful creating category")]
        public async Task UpdateCategory_NotFound()
        {
            var modifiedCategory = new CategoryModel
            {
                name = "Test123"
            };
            
            var response = await RestClient.UpdateItem("categories", "0", modifiedCategory, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [Ignore("API allow update category with invalid data")]
        public async Task UpdateCategory_InvalidData()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                name = "Test Category"
            };
            var itemResult = await RestClient.CreateItem("categories", category);
            var itemId = itemResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var modifiedCategory = new CategoryModel
            {
                name = 123
            };
            
            var response = await RestClient.UpdateItem("categories", itemId, modifiedCategory);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"'lat' should be number\"\r\n]"), "Check errors message");
        }
    }
}