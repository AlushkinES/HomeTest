using System;
using System.Net.Http;
using System.Threading.Tasks;
using HomeTest.DTO;
using NUnit.Framework;
using RestApiHelper;

namespace HomeTest
{
    [TestFixture]
    [Category("Category")]
    [Timeout(30000)]
    public class CategoryTests
    {
        private RestClient _restClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient("http://localhost:3030/", "categories");
        }
        
        [Test]
        public async Task GetCategories()
        {
            var response = await _restClient.GetAllItems<CategoriesModel>();
        }
        
        [Test]
        [Description("Successful getting category")]
        public async Task GetCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var storeResult = await _restClient.CreateItem(category);
            var storeId = storeResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var response = await _restClient.GetItem(storeId.ToString());
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
            Assert.That(result.id, Is.EqualTo(category.id), "Check category id");
        }
        
        [Test]
        [Description("Unsuccessful getting category with NotFound error")]
        public async Task GetCategory_NotFound()
        {
            var response = await _restClient.GetItem("0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task DeleteCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var productResult = await _restClient.CreateItem(category);
            var productId = productResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var response = await _restClient.DeleteItem(productId.ToString());
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
        }
        
        [Test]
        public async Task DeleteCategory_NotFound()
        {
            var response = await _restClient.DeleteItem("0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task CreateCategory_Success()
        {
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test name"
            };
            
            var response = await _restClient.CreateItem(category);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            
            Assert.That(result.name, Is.EqualTo(category.name), "Check category name");
            Assert.That(result.id, Is.EqualTo(category.id), "Check category id");
        }
        
        [Test]
        public async Task CreateCategory_InvalidData()
        {
            var category = new CategoryModel 
            {
                name = 123
            };
            
            var response = await _restClient.CreateItem(category, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"should have required property 'id'\"\r\n]"), "Check errors message");
        }
        
        [Test]
        public async Task UpdateCategory_Success()
        {
            #region PreSteps
            var category = new CategoryModel
            {
                id = Guid.NewGuid().ToString(),
                name = "Test Category"
            };
            var productResult = await _restClient.CreateItem(category);
            var productId = productResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var modifiedCategory = new CategoryModel
            {
                name = "Test123"
            };
            
            var response = await _restClient.UpdateItem(productId.ToString(), modifiedCategory);
            var result = await response.Content.ReadAsAsync<CategoryModel>();
            Assert.That(result.name, Is.EqualTo(modifiedCategory.name), "Check category name");
        }
        
        [Test]
        public async Task UpdateCategory_NotFound()
        {
            var modifiedCategory = new CategoryModel
            {
                name = "Test123"
            };
            
            var response = await _restClient.UpdateItem("0", modifiedCategory, false);
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
            var productResult = await _restClient.CreateItem(category);
            var productId = productResult.Content.ReadAsAsync<CategoryModel>().Result.id;
            #endregion
            
            var modifiedCategory = new CategoryModel
            {
                name = 123
            };
            
            var response = await _restClient.UpdateItem(productId.ToString(), modifiedCategory);
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