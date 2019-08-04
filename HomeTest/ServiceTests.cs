using System;
using System.Net.Http;
using System.Threading.Tasks;
using HomeTest.DTO;
using NUnit.Framework;
using RestApiHelper;

namespace HomeTest
{
    [TestFixture]
    [Category("Service")]
    [Timeout(30000)]
    public class ServiceTests
    {
        private RestClient _restClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _restClient = new RestClient("http://localhost:3030/", "services");
        }
        
        [Test]
        public async Task GetServices()
        {
            var response = await _restClient.GetAllItems<ServicesModel>();
        }
        
        [Test]
        [Description("Successful getting service")]
        public async Task GetService_Success()
        {
            #region PreSteps
            var service = new ServiceModel
            {
                name = "Test Service"
            };
            var serviceResult = await _restClient.CreateItem(service);
            var serviceId = serviceResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var response = await _restClient.GetItem(serviceId.ToString());
            var result = await response.Content.ReadAsAsync<ServiceModel>();
            Assert.That(result.name, Is.EqualTo(service.name), "Check service name");
        }
        
        [Test]
        [Description("Unsuccessful getting service with NotFound error")]
        public async Task GetService_NotFound()
        {
            var response = await _restClient.GetItem("0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task DeleteService_Success()
        {
            #region PreSteps
            var service = new ServiceModel
            {
                name = "Test Service"
            };
            var productResult = await _restClient.CreateItem(service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var response = await _restClient.DeleteItem(productId.ToString());
            var result = await response.Content.ReadAsAsync<ServiceModel>();
            Assert.That(result.name, Is.EqualTo(service.name), "Check service name");
        }
        
        [Test]
        public async Task DeleteService_NotFound()
        {
            var response = await _restClient.DeleteItem("0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task CreateService_Success()
        {
            var service = new ServiceModel
            {
                name = "Test name"
            };
            
            var response = await _restClient.CreateItem(service);
            var result = await response.Content.ReadAsAsync<ServiceModel>();
            
            Assert.That(result.name, Is.EqualTo(service.name), "Check service name");
        }
        
        [Test]
        public async Task CreateService_InvalidData()
        {
            var service = new ServiceModel 
            {
                name = 123
            };
            
            var response = await _restClient.CreateItem(service, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\"\r\n]"), "Check errors message");
        }
        
        [Test]
        public async Task UpdateService_Success()
        {
            #region PreSteps
            var service = new ServiceModel
            {
                name = "Test Service"
            };
            var productResult = await _restClient.CreateItem(service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var modifiedService = new ServiceModel
            {
                name = "Test123"
            };
            
            var response = await _restClient.UpdateItem(productId.ToString(), modifiedService);
            var result = await response.Content.ReadAsAsync<ServiceModel>();

            
            Assert.That(result.name, Is.EqualTo(modifiedService.name), "Check service name");
        }
        
        [Test]
        public async Task UpdateService_NotFound()
        {
            var modifiedService = new ServiceModel
            {
                name = "Test123"
            };
            
            var response = await _restClient.UpdateItem("0", modifiedService, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [Ignore("API allow update service with invalid data")]
        public async Task UpdateService_InvalidData()
        {
            #region PreSteps
            var service = new ServiceModel
            {
                name = "Test Service"
            };
            var productResult = await _restClient.CreateItem(service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var modifiedService = new ServiceModel
            {
                name = 123
            };
            
            var response = await _restClient.UpdateItem(productId.ToString(), modifiedService);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n]"), "Check errors message");
        }
    }
}