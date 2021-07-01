using EventStore.API.Commands.Customer;
using EventStore.API.Commands.Order;
using EventStore.API.Commands.Product;
using EventStore.API.Model.Response;
using EventStore.API.Model.Response.Dto;
using EventStore.API.Test.Integration.Comparer;
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
    public class Test : IClassFixture<EnvironmentFixture>
    {
        #region Private Fields

        #region Private Fields

        #region Private Fields

        private EnvironmentFixture _Fixture;

        #endregion Private Fields

        #endregion Private Fields

        #endregion Private Fields

        #region Public Constructors

        #region Public Constructors

        #region Public Constructors

        public Test(EnvironmentFixture fixture)
        {
            _Fixture = fixture;

            ; //Waiting for Db and HttpClient Starts
        }

        #endregion Public Constructors

        #endregion Public Constructors

        #endregion Public Constructors

        #region Public Methods

        #region Public Methods

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
            response = await _Fixture._HttpClient.PostAsync($"/Order/",
               new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);

            response = await _Fixture._HttpClient.PostAsync($"/Order/{Guid.NewGuid().ToString()}",
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

        [Fact, TestPriority(3)]
        public async void Returns_Business_Errors()
        {
            var addCustomerCommand = new AddCustomerCommand { Name = "Test", Surname = "User", Address = "Address" };
            var response = await _Fixture._HttpClient.PostAsync($"/Customer",
                new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var customerId = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data.Id;
            Thread.Sleep(100);

            var addProductCommand = new AddProductCommand() { Price = new Price() { Currency = "TRY", Value = 10 }, Title = "TesProduct", Stock = 10, Url = "TestProductUrl" };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));
            Assert.True(response.StatusCode == HttpStatusCode.Created);
            var productId = JsonConvert.DeserializeObject<BaseHttpServiceResponse<ProductDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            Thread.Sleep(100);
            var addOrderCommand = new AddOrderCommand() { Quantity = 11, CustomerId = Guid.Parse(customerId), ProductId = Guid.Parse(productId) };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            var responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.True(responseAsError.ErrorCode == "INSUFFICIENT_AMOUNT");

            Thread.Sleep(100);
            response = await _Fixture._HttpClient.DeleteAsync($"/Product/{productId}");

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            Thread.Sleep(100);
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.True(responseAsError.ErrorCode == "PRODUCT_NOT_FOUND");

            Thread.Sleep(100);
            response = await _Fixture._HttpClient.DeleteAsync($"/Customer/{customerId}");

            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);

            Thread.Sleep(100);
            response = await _Fixture._HttpClient.PostAsync($"/Order/{productId}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

            responseAsError = JsonConvert.DeserializeObject<BaseHttpServiceResponse<string>>(await response.Content.ReadAsStringAsync()).Error;

            Assert.Equal("CUSTOMER_NOT_FOUND", responseAsError.ErrorCode);
        }

        [Fact, TestPriority(4)]
        public async void Returns_Successful()
        {
            var customer = new CustomerDto()
            { Address = "Address", Name = "Test", Surname = "User" };
            Thread.Sleep(100);

            var addCustomerCommand = new AddCustomerCommand { Name = customer.Name, Surname = customer.Surname, Address = customer.Address };
            var response = await _Fixture._HttpClient.PostAsync($"/Customer",
                new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            customer.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var product = new ProductDto()
            {
                ProductLocation = new ProductLocation("TestProductUrl"),
                ProductMetadata = new ProductMetadata("TestProduct"),
                Price = new Price() { Currency = "TRY", Value = 10 },
                Stock = 10
            };
            Thread.Sleep(100);

            var addProductCommand = new AddProductCommand() { Price = product.Price, Stock = product.Stock, Title = product.ProductMetadata.Title, Url = product.ProductLocation.Url };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            product.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<ProductDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var order = new OrderDto()
            {
                Quantity = 10,
                Customer = customer,
                Product = product,
                TotalPrice = new Price() { Currency = "TRY", Value = 100 },
            };
            Thread.Sleep(100);

            var addOrderCommand = new AddOrderCommand() { Quantity = order.Quantity, CustomerId = Guid.Parse(order.Customer.Id), ProductId = Guid.Parse(order.Product.Id) };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{order.Product.Id}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            order.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<OrderDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            #region GetOrder

            response = await _Fixture._HttpClient.GetAsync($"/Order/{order.Id}");
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            var orderGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<OrderDto>>(await response.Content.ReadAsStringAsync()).Data;
            Assert.NotNull(orderGot);
            Assert.Equal(orderGot, order, new OrderEqualityComparer());

            #endregion GetOrder

            #region Get Customer

            Thread.Sleep(100);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var customerGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.Equal(customerGot, customer, new CustomerEqualityComparer());

            #endregion Get Customer

            #region Get Customer Orders

            Thread.Sleep(2000);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}/orders");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            var ordersGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<List<OrderDto>>>(await response.Content.ReadAsStringAsync()).Data;

            var existingOrder = ordersGot.FirstOrDefault(o => o.Id == order.Id);

            Assert.NotNull(existingOrder);
            Assert.Equal(existingOrder.Product, product, new ProductEqualityComparer());
            Assert.Equal(existingOrder.Customer, customer, new CustomerEqualityComparer());

            #endregion Get Customer Orders

            #region Get Product

            response = await _Fixture._HttpClient.GetAsync($"/Product/{product.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var productGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<ProductDto>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.Equal(productGot, product, new ProductEqualityComparer());
            Assert.Equal(0, productGot.Stock);

            #endregion Get Product

            #region GetProductNotFound

            response = await _Fixture._HttpClient.GetAsync($"/Product/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            #endregion GetProductNotFound

            #region GetCustomerNotFound

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            #endregion GetCustomerNotFound

            #region GetOrderNotFound

            response = await _Fixture._HttpClient.GetAsync($"/Order/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            #endregion GetOrderNotFound

            #region GetCustomerOrderNotFound

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{Guid.NewGuid()}/orders");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            #endregion GetCustomerOrderNotFound

            #region DeleteCustomerAndGetCustomerActiveFalse

            response = await _Fixture._HttpClient.DeleteAsync($"/Customer/{customer.Id}");

            Assert.True(response.StatusCode == HttpStatusCode.Accepted);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deletedCustomer = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.NotNull(deletedCustomer);
            Assert.False(deletedCustomer.Active);

            #endregion DeleteCustomerAndGetCustomerActiveFalse

            #region DeleteProductAndGetProductActiveFalse

            response = await _Fixture._HttpClient.DeleteAsync($"/Product/{product.Id}");

            Assert.True(response.StatusCode == HttpStatusCode.Accepted);

            response = await _Fixture._HttpClient.GetAsync($"/Product/{product.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deletedProduct = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.NotNull(deletedProduct);
            Assert.False(deletedProduct.Active);

            #endregion DeleteProductAndGetProductActiveFalse

            #region ReturnDeletedCustomerOrderButCustomerAndProductActiveFalse

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}/orders");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            ordersGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<List<OrderDto>>>(await response.Content.ReadAsStringAsync()).Data;

            existingOrder = ordersGot.FirstOrDefault(o => o.Id == order.Id);

            Assert.NotNull(existingOrder);
            Assert.False(existingOrder.Product.Active);
            Assert.False(existingOrder.Customer.Active);

            #endregion ReturnDeletedCustomerOrderButCustomerAndProductActiveFalse

            #region CreateCustomerAndReturnOrderEmpty

            customer = new CustomerDto()
            { Address = "Address2", Name = "Test2", Surname = "User2" };
            Thread.Sleep(100);

            addCustomerCommand = new AddCustomerCommand { Name = customer.Name, Surname = customer.Surname, Address = customer.Address };
            response = await _Fixture._HttpClient.PostAsync($"/Customer",
               new StringContent(JsonConvert.SerializeObject(addCustomerCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            customer.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<CustomerDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}/orders");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            ordersGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<List<OrderDto>>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.NotNull(ordersGot);
            Assert.Empty(ordersGot);

            #endregion CreateCustomerAndReturnOrderEmpty

            #region AddCustomerTwoOrdersAndReturn

            product = new ProductDto()
            {
                ProductLocation = new ProductLocation("TestProductUrl2"),
                ProductMetadata = new ProductMetadata("TestProduct2"),
                Price = new Price() { Currency = "TRY", Value = 10 },
                Stock = 10
            };
            Thread.Sleep(100);

            addProductCommand = new AddProductCommand() { Price = product.Price, Stock = product.Stock, Title = product.ProductMetadata.Title, Url = product.ProductLocation.Url };
            response = await _Fixture._HttpClient.PostAsync($"/Product",
                new StringContent(JsonConvert.SerializeObject(addProductCommand), Encoding.UTF8, "application/json"));

            Assert.True(response.StatusCode == HttpStatusCode.Created);

            product.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<ProductDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var firstOrder = new OrderDto()
            {
                Quantity = 5,
                Customer = customer,
                Product = product,
                TotalPrice = new Price() { Currency = "TRY", Value = 50 },
            };
            Thread.Sleep(100);

            addOrderCommand = new AddOrderCommand() { Quantity = firstOrder.Quantity, CustomerId = Guid.Parse(firstOrder.Customer.Id), ProductId = Guid.Parse(firstOrder.Product.Id) };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{firstOrder.Product.Id}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            firstOrder.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<OrderDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            var secondOrder = new OrderDto()
            {
                Quantity = 4,
                Customer = customer,
                Product = product,
                TotalPrice = new Price() { Currency = "TRY", Value = 40 },
            };
            Thread.Sleep(100);

            addOrderCommand = new AddOrderCommand() { Quantity = secondOrder.Quantity, CustomerId = Guid.Parse(secondOrder.Customer.Id), ProductId = Guid.Parse(secondOrder.Product.Id) };
            response = await _Fixture._HttpClient.PostAsync($"/Order/{secondOrder.Product.Id}",
                new StringContent(JsonConvert.SerializeObject(addOrderCommand), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            secondOrder.Id = JsonConvert.DeserializeObject<BaseHttpServiceResponse<OrderDto>>(await response.Content.ReadAsStringAsync()).Data.Id;

            Thread.Sleep(2000);

            response = await _Fixture._HttpClient.GetAsync($"/Customer/{customer.Id}/orders");

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            ordersGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<List<OrderDto>>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.True(ordersGot.Count == 2);

            var existingFirstOrder = ordersGot.FirstOrDefault(o => o.Id == firstOrder.Id);

            Assert.NotNull(existingFirstOrder);
            Assert.Equal(existingFirstOrder.Product, product, new ProductEqualityComparer());
            Assert.Equal(existingFirstOrder.Customer, customer, new CustomerEqualityComparer());

            var existingSecondOrder = ordersGot.FirstOrDefault(o => o.Id == secondOrder.Id);

            Assert.NotNull(existingSecondOrder);
            Assert.Equal(existingSecondOrder.Product, product, new ProductEqualityComparer());
            Assert.Equal(existingSecondOrder.Customer, customer, new CustomerEqualityComparer());

            response = await _Fixture._HttpClient.GetAsync($"/Product/{product.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            productGot = JsonConvert.DeserializeObject<BaseHttpServiceResponse<ProductDto>>(await response.Content.ReadAsStringAsync()).Data;

            Assert.Equal(productGot, product, new ProductEqualityComparer());
            Assert.Equal(1, productGot.Stock);

            #endregion AddCustomerTwoOrdersAndReturn
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

        #endregion Public Methods

        #endregion Public Methods
    }
}