using System;
using System.Net.Http;
using System.Threading.Tasks;
using HomeTest.DTO;
using NUnit.Framework;
using RestApiHelper;

namespace HomeTest
{
    [TestFixture]
    [Category("Store")]
    [Timeout(30000)]
    public class StoreTests
    {
        private RestClient RestClient;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            RestClient = new RestClient();
        }
        
        [Test]
        public async Task GetStores()
        {
            var response = await RestClient.GetAllItems("stores");
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(10), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(25)]
        [Description("Get stores by limit")]
        public async Task GetCategories_ByLimit(int limit)
        {
            var response = await RestClient.GetAllItems("stores?$limit=" + limit);
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(limit), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [Description("Check max limit")]
        public async Task GetCategories_MaxLimit()
        {
            var response = await RestClient.GetAllItems("stores?$limit=99");
            var results = await response.Content.ReadAsAsync<StoresModel>();
            
            Assert.That(results.limit, Is.EqualTo(25), "Check limit in GetAllItems request");
            Assert.That(results.skip, Is.EqualTo(0), "Check skip in GetAllItems request");
            Assert.That(results.total, Is.GreaterThan(0), "Check total in GetAllItems request");
            Assert.That(results.data.Count, Is.EqualTo(results.limit), "Check count of data in GetAllItems request");
        }
        
        [Test]
        [Description("Successful getting store")]
        public async Task GetStore_Success()
        {
            #region PreSteps
            var store = new StoreModel
            {
                name = "Test Store",
                type = "Test",
                address = "City",
                address2 = "City",
                city = "UpcTest",
                state = "Description",
                zip = "Test Manufacturer",
                lat = 123,
                lng = 123,
                hours = "Test link"
            };
            var storeResult = await RestClient.CreateItem("stores", store);
            var storeId = storeResult.Content.ReadAsAsync<StoreModel>().Result.id;
            #endregion
            
            var response = await RestClient.GetItem("stores", storeId);
            var result = await response.Content.ReadAsAsync<StoreModel>();
            Assert.That(result.name, Is.EqualTo(store.name), "Check store name");
            Assert.That(result.type, Is.EqualTo(store.type), "Check store type");
            Assert.That(result.address, Is.EqualTo(store.address), "Check store address");
            Assert.That(result.address2, Is.EqualTo(store.address2), "Check store address2");
            Assert.That(result.city, Is.EqualTo(store.city), "Check store city");
            Assert.That(result.state, Is.EqualTo(store.state), "Check store state");
            Assert.That(result.zip, Is.EqualTo(store.zip), "Check store zip)");
            Assert.That(result.lat, Is.EqualTo(store.lat), "Check store lat)");
            Assert.That(result.lng, Is.EqualTo(store.lng), "Check store lng");
            Assert.That(result.hours, Is.EqualTo(store.hours), "Check store hours");
        }
        
        [Test]
        [Description("Unsuccessful getting store with NotFound error")]
        public async Task GetStore_NotFound()
        {
            var response = await RestClient.GetItem("stores", "0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task DeleteStore_Success()
        {
            #region PreSteps
            var store = new StoreModel
            {
                name = "Test Store",
                type = "Test",
                address = "asd",
                address2 = "eweq",
                city = "UpcTest",
                state = "Description",
                zip = "Test Manufacturer",
                lat = 123,
                lng = 456,
                hours = "Test hours"
            };
            var productResult = await RestClient.CreateItem("stores", store);
            var productId = productResult.Content.ReadAsAsync<StoreModel>().Result.id;
            #endregion
            
            var response = await RestClient.DeleteItem("stores", productId);
            var result = await response.Content.ReadAsAsync<StoreModel>();
            Assert.That(result.name, Is.EqualTo(store.name), "Check store name");
            Assert.That(result.type, Is.EqualTo(store.type), "Check store type");
            Assert.That(result.address, Is.EqualTo(store.address), "Check store address");
            Assert.That(result.address2, Is.EqualTo(store.address2), "Check store address2");
            Assert.That(result.city, Is.EqualTo(store.city), "Check store city");
            Assert.That(result.state, Is.EqualTo(store.state), "Check store state");
            Assert.That(result.zip, Is.EqualTo(store.zip), "Check store zip");
            Assert.That(result.lat, Is.EqualTo(store.lat), "Check store lat");
            Assert.That(result.lng, Is.EqualTo(store.lng), "Check store lng");
            Assert.That(result.hours, Is.EqualTo(store.hours), "Check store hours");
        }
        
        [Test]
        public async Task DeleteStore_NotFound()
        {
            var response = await RestClient.DeleteItem("stores", "0", false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        public async Task CreateStore_Success()
        {
            var store = new StoreModel
            {
                name = "Test name",
                type = "Test type",
                address = "Test address",
                address2 = "Test address2",
                city = "Test city",
                state = "Test state",
                zip = "Test zip",
                lat = 111,
                lng = 222,
                hours = "Test hours"
            };
            
            var response = await RestClient.CreateItem("stores", store);
            var result = await response.Content.ReadAsAsync<StoreModel>();
            
            Assert.That(result.name, Is.EqualTo(store.name), "Check store name");
            Assert.That(result.type, Is.EqualTo(store.type), "Check store type");
            Assert.That(result.address, Is.EqualTo(store.address), "Check store address");
            Assert.That(result.address2, Is.EqualTo(store.address2), "Check store address2");
            Assert.That(result.city, Is.EqualTo(store.city), "Check store city");
            Assert.That(result.state, Is.EqualTo(store.state), "Check store state");
            Assert.That(result.zip, Is.EqualTo(store.zip), "Check store zip");
            Assert.That(result.lat, Is.EqualTo(store.lat), "Check store lat");
            Assert.That(result.lng, Is.EqualTo(store.lng), "Check store lng");
            Assert.That(result.hours, Is.EqualTo(store.hours), "Check store hours");
        }
        
        [Test]
        public async Task CreateStore_InvalidData()
        {
            var store = new StoreModel 
            {
                name = 123,
                type = "Test",
                address = "12.01m",
                address2 = "1.99m",
                city = "Test",
                state = "Test",
                zip = "Test",
                lat = "123",
                lng = 213,
                hours = "Test"
            };
            
            var response = await RestClient.CreateItem("stores", store, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();
            
            Assert.That(result.name, Is.EqualTo("BadRequest"), "Check error name");
            Assert.That(result.code, Is.EqualTo(400), "Check error code");
            Assert.That(result.message, Is.EqualTo("Invalid Parameters"), "Check error message");
            Assert.That(result.className, Is.EqualTo("bad-request"), "Check error className");
            Assert.That(result.errors.ToString(), Is.EqualTo(
                "[\r\n  \"'name' should be string\",\r\n  \"'lat' should be number\"\r\n]"), "Check errors message");
        }
        
        [Test]
        public async Task UpdateStore_Success()
        {
            #region PreSteps
            var store = new StoreModel
            {
                name = "Test Store",
                type = "Test",
                address = "Address",
                address2 ="Address2",
                city = "City",
                state = "State",
                zip = "Zip",
                lat = 111,
                lng = 222,
                hours = "Test hours"
            };
            var productResult = await RestClient.CreateItem("stores", store);
            var productId = productResult.Content.ReadAsAsync<StoreModel>().Result.id;
            #endregion
            
            var modifiedStore = new StoreModel
            {
                name = "Test123",
                type = "Test123",
                address = "asd",
                address2 = "qwe",
                city = "Test123",
                state = "Test123",
                zip = "Test123",
                lat = 123,
                lng = 123,
                hours = "Test123"
            };
            
            var response = await RestClient.UpdateItem("stores", productId, modifiedStore);
            var result = await response.Content.ReadAsAsync<StoreModel>();

            
            Assert.That(result.name, Is.EqualTo(modifiedStore.name), "Check store name");
            Assert.That(result.type, Is.EqualTo(modifiedStore.type), "Check store type");
            Assert.That(result.address, Is.EqualTo(modifiedStore.address), "Check store address");
            Assert.That(result.address2, Is.EqualTo(modifiedStore.address2), "Check store address2");
            Assert.That(result.city, Is.EqualTo(modifiedStore.city), "Check store city");
            Assert.That(result.state, Is.EqualTo(modifiedStore.state), "Check store state");
            Assert.That(result.zip, Is.EqualTo(modifiedStore.zip), "Check store zip");
            Assert.That(result.lat, Is.EqualTo(modifiedStore.lat), "Check store lat");
            Assert.That(result.lng, Is.EqualTo(modifiedStore.lng), "Check store lng");
            Assert.That(result.hours, Is.EqualTo(modifiedStore.hours), "Check store hours");
        }
        
        [Test]
        public async Task UpdateStore_NotFound()
        {
            var modifiedStore = new StoreModel
            {
                name = "Test123",
                type = "Test123",
                address = "asd",
                address2 = "qwe",
                city = "Test123",
                state = "Test123",
                zip = "Test123",
                lat = 123,
                lng = 123,
                hours = "Test123"
            };
            
            var response = await RestClient.UpdateItem("stores", "0", modifiedStore, false);
            var result = await response.Content.ReadAsAsync<ErrorModel>();

            
            Assert.That(result.name, Is.EqualTo("NotFound"), "Check error name");
            Assert.That(result.code, Is.EqualTo(404), "Check error name");
            Assert.That(result.message, Is.EqualTo("No record found for id '0'"), "Check error name");
            Assert.That(result.className, Is.EqualTo("not-found"), "Check error name");
        }
        
        [Test]
        [Ignore("API allow update store with invalid data")]
        public async Task UpdateStore_InvalidData()
        {
            #region PreSteps
            var store = new StoreModel
            {
                name = "Test Store",
                type = "Test",
                address = "Address",
                address2 ="Address2",
                city = "City",
                state = "State",
                zip = "Zip",
                lat = 111,
                lng = 222,
                hours = "Test hours"
            };
            var productResult = await RestClient.CreateItem("stores", store);
            var productId = productResult.Content.ReadAsAsync<StoreModel>().Result.id;
            #endregion
            
            var modifiedStore = new StoreModel
            {
                name = 123,
                type = "Test123",
                address = "asd",
                address2 = "qwe",
                city = "Test123",
                state = "Test123",
                zip = "Test123",
                lat = "Test123",
                lng = 123,
                hours = "Test123"
            };
            
            var response = await RestClient.UpdateItem("stores", productId, modifiedStore);
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