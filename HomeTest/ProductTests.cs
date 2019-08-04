using System;
using System.Net.Http;
using System.Threading.Tasks;
using HomeTest.DTO;
using NUnit.Framework;
using RestApiHelper;

namespace HomeTest
{
    [TestFixture]
    [Category("Product")]
    [Timeout(30000)]
    public class ProductTests
    {
        private RestClient RestClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            RestClient = new RestClient();
        }
        
        [Test]
        public async Task GetProducts()
        {
            var response = await RestClient.GetAllItems("products");
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(10), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(25)]
        [Description("Get products by limit")]
        public async Task GetCategories_ByLimit(int limit)
        {
            var response = await RestClient.GetAllItems("products?$limit=" + limit);
            var results = await response.Content.ReadAsAsync<CategoriesModel>();
            
            Assert.That(results.limit, Is.EqualTo(limit), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [Description("Check max limit")]
        public async Task GetCategories_MaxLimit()
        {
            var response = await RestClient.GetAllItems("products?$limit=99");
            var results = await response.Content.ReadAsAsync<CategoriesModel>();
            
            Assert.That(results.limit, Is.EqualTo(25), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [Description("Successful getting product")]
        public async Task GetProduct_Success()
        {
            #region PreSteps
            var product = new ProductModel
            {
                name = "Test Product",
                type = "Test",
                price = 1,
                shipping = 2,
                upc = "UpcTest",
                description = "Description",
                manufacturer = "Test Manufacturer",
                model = "Test Model",
                url = "Test link",
                image = "Test image"
            };
            var productResult = await RestClient.CreateItem("products", product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var response = await RestClient.GetItem("products", productId);
            var result = await response.Content.ReadAsAsync<ProductModel>();
            Assert.That(result.name, Is.EqualTo(product.name), "Check product name");
            Assert.That(result.type, Is.EqualTo(product.type), "Check product type");
            Assert.That(result.price, Is.EqualTo(product.price), "Check product price");
            Assert.That(result.shipping, Is.EqualTo(product.shipping), "Check product shipping");
            Assert.That(result.upc, Is.EqualTo(product.upc), "Check product upc");
            Assert.That(result.description, Is.EqualTo(product.description), "Check product description");
            Assert.That(result.manufacturer, Is.EqualTo(product.manufacturer), "Check product manufacturer");
            Assert.That(result.model, Is.EqualTo(product.model), "Check product model");
            Assert.That(result.url, Is.EqualTo(product.url), "Check product url");
            Assert.That(result.image, Is.EqualTo(product.image), "Check product image");
        }
        
        [Test]
        [Description("Unsuccessful getting product with NotFound error")]
        public async Task GetProduct_NotFound()
        {
            var response = await RestClient.GetItem("products","0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task DeleteProduct_Success()
        {
            #region PreSteps
            var product = new ProductModel
            {
                name = "Test Product",
                type = "Test",
                price = 1,
                shipping = 2,
                upc = "UpcTest",
                description = "Description",
                manufacturer = "Test Manufacturer",
                model = "Test Model",
                url = "Test link",
                image = "Test image"
            };
            var productResult = await RestClient.CreateItem("products", product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var response = await RestClient.DeleteItem("products",productId);
            var result = await response.Content.ReadAsAsync<ProductModel>();
            Assert.That(result.name, Is.EqualTo(product.name), "Check product name");
            Assert.That(result.type, Is.EqualTo(product.type), "Check product type");
            Assert.That(result.price, Is.EqualTo(product.price), "Check product price");
            Assert.That(result.shipping, Is.EqualTo(product.shipping), "Check product shipping");
            Assert.That(result.upc, Is.EqualTo(product.upc), "Check product upc");
            Assert.That(result.description, Is.EqualTo(product.description), "Check product description");
            Assert.That(result.manufacturer, Is.EqualTo(product.manufacturer), "Check product manufacturer");
            Assert.That(result.model, Is.EqualTo(product.model), "Check product model");
            Assert.That(result.url, Is.EqualTo(product.url), "Check product url");
            Assert.That(result.image, Is.EqualTo(product.image), "Check product image");
        }
        
        [Test]
        public async Task DeleteProduct_NotFound()
        {
            var response = await RestClient.DeleteItem("products","0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task CreateProduct_Success()
        {
            var product = new ProductModel
            {
                name = "Test",
                type = "Test",
                price = 12.01m,
                shipping = 1.99m,
                upc = "Test",
                description = "Test",
                manufacturer = "Test",
                model = "Test",
                url = "Test",
                image = "Test"
            };
            
            var response = await RestClient.CreateItem("products", product);
            var result = await response.Content.ReadAsAsync<ProductModel>();
            
            Assert.That(result.name, Is.EqualTo(product.name), "Check product name");
            Assert.That(result.type, Is.EqualTo(product.type), "Check product type");
            Assert.That(result.price, Is.EqualTo(product.price), "Check product price");
            Assert.That(result.shipping, Is.EqualTo(product.shipping), "Check product shipping");
            Assert.That(result.upc, Is.EqualTo(product.upc), "Check product upc");
            Assert.That(result.description, Is.EqualTo(product.description), "Check product description");
            Assert.That(result.manufacturer, Is.EqualTo(product.manufacturer), "Check product manufacturer");
            Assert.That(result.model, Is.EqualTo(product.model), "Check product model");
            Assert.That(result.url, Is.EqualTo(product.url), "Check product url");
            Assert.That(result.image, Is.EqualTo(product.image), "Check product image");
        }
        
        [Test]
        public async Task CreateProduct_InvalidData()
        {
            var product = new ProductModel 
            {
                name = 123,
                type = "Test",
                price = "12.01m",
                shipping = 1.99m,
                upc = "Test",
                description = "Test",
                manufacturer = "Test",
                model = "Test",
                url = "Test",
                image = "Test"
            };
            
            var response = await RestClient.CreateItem("products", product, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"'price' should be number\"\r\n]"), "Check errors message");
        }
        
        [Test]
        public async Task UpdateProduct_Success()
        {
            #region PreSteps
            var product = new ProductModel
            {
                name = "Test Product",
                type = "Test",
                price = 1,
                shipping = 2,
                upc = "UpcTest",
                description = "Description",
                manufacturer = "Test Manufacturer",
                model = "Test Model",
                url = "Test link",
                image = "Test image"
            };
            var productResult = await RestClient.CreateItem("products", product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var modifiedProduct = new ProductModel
            {
                name = "Test123",
                type = "Test123",
                price = 2,
                shipping = 3,
                upc = "Test123",
                description = "Test123",
                manufacturer = "Test123",
                model = "Test123",
                url = "Test123",
                image = "Test123"
            };
            
            var response = await RestClient.UpdateItem("products", productId, modifiedProduct);
            var result = await response.Content.ReadAsAsync<ProductModel>();

            
            Assert.That(result.name, Is.EqualTo(modifiedProduct.name), "Check product name");
            Assert.That(result.type, Is.EqualTo(modifiedProduct.type), "Check product type");
            Assert.That(result.price, Is.EqualTo(modifiedProduct.price), "Check product price");
            Assert.That(result.shipping, Is.EqualTo(modifiedProduct.shipping), "Check product shipping");
            Assert.That(result.upc, Is.EqualTo(modifiedProduct.upc), "Check product upc");
            Assert.That(result.description, Is.EqualTo(modifiedProduct.description), "Check product description");
            Assert.That(result.manufacturer, Is.EqualTo(modifiedProduct.manufacturer), "Check product manufacturer");
            Assert.That(result.model, Is.EqualTo(modifiedProduct.model), "Check product model");
            Assert.That(result.url, Is.EqualTo(modifiedProduct.url), "Check product url");
            Assert.That(result.image, Is.EqualTo(modifiedProduct.image), "Check product image");
        }
        
        [Test]
        [Description("Unsuccessful updating product")]
        public async Task UpdateProduct_NotFound()
        {
            var modifiedCategory = new ProductModel
            {
                name = "Test123"
            };
            
            var response = await RestClient.UpdateItem("products", "0", modifiedCategory, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [Ignore("API allow update store with invalid data")]
        public async Task UpdateProduct_InvalidData()
        {
            #region PreSteps
            var product = new ProductModel
            {
                name = "Test Product",
                type = "Test",
                price = 1,
                shipping = 2,
                upc = "UpcTest",
                description = "Description",
                manufacturer = "Test Manufacturer",
                model = "Test Model",
                url = "Test link",
                image = "Test image"
            };
            var productResult = await RestClient.CreateItem("products", product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var modifiedProduct = new ProductModel
            {
                name = 123,
                type = "Test123",
                price = "Test",
                shipping = 3,
                upc = "Test123",
                description = "Test123",
                manufacturer = "Test123",
                model = "Test123",
                url = "Test123",
                image = "Test123"
            };
            
            var response = await RestClient.UpdateItem("products", productId, modifiedProduct, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"'price' should be number\"\r\n]"), "Check errors message");
        }
    }
}