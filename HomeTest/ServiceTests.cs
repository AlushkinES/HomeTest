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
        private RestClient RestClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            RestClient = new RestClient();
        }
        
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(25)]
        [Description("Get services by limit")]
        public async Task GetCategories_ByLimit(int limit)
        {
            var response = await RestClient.GetAllItems("services?$limit=" + limit);
            var results = await response.Content.ReadAsAsync<ServicesModel>();
            
            Assert.That(results.limit, Is.EqualTo(limit), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [Description("Check max limit")]
        public async Task GetCategories_MaxLimit()
        {
            var response = await RestClient.GetAllItems("services?$limit=99");
            var results = await response.Content.ReadAsAsync<ServicesModel>();
            
            Assert.That(results.limit, Is.EqualTo(25), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        public async Task GetServices()
        {
            var response = await RestClient.GetAllItems("services");
            var results = await response.Content.ReadAsAsync<ServicesModel>();
            
            Assert.That(results.limit, Is.EqualTo(10), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
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
            var serviceResult = await RestClient.CreateItem("services", service);
            var serviceId = serviceResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var response = await RestClient.GetItem("services", serviceId);
            var result = await response.Content.ReadAsAsync<ServiceModel>();
            Assert.That(result.name, Is.EqualTo(service.name), "Check service name");
        }
        
        [Test]
        [Description("Unsuccessful getting service with NotFound error")]
        public async Task GetService_NotFound()
        {
            var response = await RestClient.GetItem("services", "0", false);
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
            var productResult = await RestClient.CreateItem("services", service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var response = await RestClient.DeleteItem("services", productId.ToString());
            var result = await response.Content.ReadAsAsync<ServiceModel>();
            Assert.That(result.name, Is.EqualTo(service.name), "Check service name");
        }
        
        [Test]
        public async Task DeleteService_NotFound()
        {
            var response = await RestClient.DeleteItem("services", "0", false);
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
            
            var response = await RestClient.CreateItem("services", service);
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
            
            var response = await RestClient.CreateItem("services", service, false);
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
            var productResult = await RestClient.CreateItem("services", service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var modifiedService = new ServiceModel
            {
                name = "Test123"
            };
            
            var response = await RestClient.UpdateItem("services", productId, modifiedService);
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
            
            var response = await RestClient.UpdateItem("services", "0", modifiedService, false);
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
            var productResult = await RestClient.CreateItem("services", service);
            var productId = productResult.Content.ReadAsAsync<ServiceModel>().Result.id;
            #endregion
            
            var modifiedService = new ServiceModel
            {
                name = 123
            };
            
            var response = await RestClient.UpdateItem("services", productId, modifiedService);
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