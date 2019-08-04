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
        private RestClient _restClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient("http://localhost:3030/", "products");
        }
        
        [Test]
        public async Task GetProducts()
        {
            var response = await _restClient.GetAllItems<ProductModel>();
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
            var productResult = await _restClient.CreateItem(product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var response = await _restClient.GetItem(productId.ToString());
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
            var response = await _restClient.GetItem("0", false);
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
            var productResult = await _restClient.CreateItem(product);
            var productId = productResult.Content.ReadAsAsync<ProductModel>().Result.id;
            #endregion
            
            var response = await _restClient.DeleteItem(productId.ToString());
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
            var response = await _restClient.DeleteItem("0", false);
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
            
            var response = await _restClient.CreateItem(product);
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
            
            var response = await _restClient.CreateItem(product, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"'price' should be number\"\r\n]"), "Check errors message");
        }
        
        [Test]
        public async Task UpdateProduct()
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
            var productResult = await _restClient.CreateItem(product);
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
            
            var response = await _restClient.UpdateItem(productId.ToString(), modifiedProduct);
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
    }
}