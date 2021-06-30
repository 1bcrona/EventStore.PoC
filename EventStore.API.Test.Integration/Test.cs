using EventStore.API.Commands.Customer;
using EventStore.API.Commands.Order;
using EventStore.API.Commands.Product;
using EventStore.API.Model.Response;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Test.Integration.Comparer;
using EventStore.Domain.Entity;
using EventStore.Domain.ValueObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EventStore.API.Test.Integration
{
    [TestCaseOrderer("EventStore.API.Test.Integration.TestCaseOrderer", "EventStore.API.Test.Integration")]
    public class Test : IClassFixture<SetupFixture>
    {
        #region Private Fields

        private SetupFixture _Fixture;

        #endregion Private Fields

        #region Public Constructors

        public Test(SetupFixture fixture)
        {
            _Fixture = fixture;
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact, TestPriority(2)]
        public async Task Return_Validation_Errors()
        {
            var tokenResponse = await _Fixture._HttpClient.GetAsync($"/Auth");

            var tokenData =
                JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await tokenResponse.Content.ReadAsStringAsync());
            _Fixture._HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenData.Data}");

            var addCustomerCommand = new AddCustomerCommand { Name = null, Surname = null, Address = null };
            var response = await _Fixture._HttpClient.PostAsync($"/Customer",
                 new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            var addProductCommand = new AddProductCommand() { Price = null, Title = null, Stock = 0, Url = null };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
               new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            var addOrderCommand = new AddOrderCommand() { };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{Guid.NewGuid().ToString("D")}",
               new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/123456789"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/123456789/orders"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.GetAsync($"/Product/123456789"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.GetAsync($"/Order/123456789"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.DeleteAsync($"/Product/123456789"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

            response = await _Fixture._HttpClient.DeleteAsync($"/Customer/123456789"); //NotGuid
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact, TestPriority(1)]
        public async Task Returns_Unauthorized()
        {
            var response = await _Fixture._HttpClient.GetAsync($"/Customer/CustomerId/orders");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/CustomerId");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.DeleteAsync($"/Customer/CustomerId");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.PostAsync($"/Customer",
                new StringContent(String.Empty, Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.GetAsync($"/Order/OrderId");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.PostAsync($"/Order/ProductId",
                new StringContent(String.Empty, Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.DeleteAsync($"/Product/ProductId");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.GetAsync($"/Product/ProductId");
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);

            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(String.Empty, Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        #endregion Public Methods

        [Fact, TestPriority(3)]
        public async void Returns_Order_Errors()
        {
            var addCustomerCommand = new AddCustomerCommand { Name = "Test", Surname = "User", Address = "Address" };
            var response = await _Fixture._HttpClient.PostAsync($"/Customer",
                new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            var customerId = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data.Id;
            Thread.Sleep(2000);

            var addProductCommand = new AddProductCommand() { Price = new Price() { Currency = "TRY", Value = 10 }, Title = "TesProduct", Stock = 10, Url = "TestProductUrl" };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.Created);
            var productId = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data.Id;

            Thread.Sleep(2000);
            var addOrderCommand = new AddOrderCommand() { Quantity = 11, CustomerId = customerId, ProductId = productId };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);

            var responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.True(responseAsError.ErrorCode == "INSUFFICIENT_AMOUNT");

            Thread.Sleep(2000);
            response = await _Fixture._HttpClient.DeleteAsync($"/Product/{productId}");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            Thread.Sleep(2000);
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);

            responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.True(responseAsError.ErrorCode == "PRODUCT_NOT_FOUND");

            Thread.Sleep(2000);
            response = await _Fixture._HttpClient.DeleteAsync($"/Customer/{customerId}");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            Thread.Sleep(2000);
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);

            responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.True(responseAsError.ErrorCode == "CUSTOMER_NOT_FOUND");
        }

        [Fact, TestPriority(4)]
        public async void Returns_Successful()
        {
            var customer = new Customer()
            { Active = true, Address = "Address", Id = Guid.Empty, Name = "Test", Surname = "User" };
            Thread.Sleep(2000);

            var addCustomerCommand = new AddCustomerCommand { Name = customer.Name, Surname = customer.Surname, Address = customer.Address };
            var response = await _Fixture._HttpClient.PostAsync($"/Customer",
                new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            customer.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var product = new Product()
            {
                ProductLocation = new ProductLocation("TestProductUrl"),
                ProductMetadata = new ProductMetadata("TestProduct"),
                Active = true,
                Price = new Price() { Currency = "TRY", Value = 10 },
                Stock = 10
            };
            Thread.Sleep(2000);

            var addProductCommand = new AddProductCommand() { Price = product.Price, Stock = product.Stock, Title = product.ProductMetadata.Title, Url = product.ProductLocation.Url };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            product.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var order = new Order()
            {
                Active = true,
                Quantity = 10,
                OrderCustomerId = customer.Id,
                OrderProductId = product.Id,
                TotalPrice = new Price() { Currency = "TRY", Value = 150 }
            };
            Thread.Sleep(2000);

            var addOrderCommand = new AddOrderCommand() { Quantity = order.Quantity, CustomerId = order.OrderCustomerId, ProductId = order.OrderProductId };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{order.OrderProductId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            order.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data.Id;

            #region Get Customer
            Thread.Sleep(2000);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            var customerGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<Customer>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.True(customerGot.Active);
            Assert.Equal(customerGot, customer, new CustomerEqualityComparer());

            #endregion Get Customer

            #region Get Customer Orders
            Thread.Sleep(2000);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}/orders");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            var orderGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<List<OrderDto>>>(await response.Content.ReadAsStringAsync()).Data;

            var existingOrder = orderGot.FirstOrDefault(o => o.OrderId == order.Id);

            Assert.NotNull(existingOrder);

            Assert.Equal(existingOrder.OrderId, order.Id);

            #endregion Get Customer Orders
        }
    }
}