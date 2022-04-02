using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Products.Commands.CreateProduct;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebAPI.Contracts.V1;
using WebAPI.Tests.Helpers;
using Xunit;

namespace WebAPI.Tests
{
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public ProductsControllerTests()
        {
            _factory = new WebApplicationFactory<Program>().
                WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        
                        var dbContextOptions = services.SingleOrDefault(service => service
                            .ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("CoffeMemoryDB"));
                    });
                });

            _client = _factory.CreateClient();
        }


        [Fact]
        public async Task Create_ForCorrectParameters_ReturnOk()
        {
            //Arrange
            CreateProductCommand product = new()
            {
                Name = "Johan & Nystrm Coffe",
                Description = "Swedish coffee",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            };

            //Act
            var response = await _client.PostAsync(ApiRoutes.Products.Create, product.ToJsonHttpContent());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Create_ForCorrectParameters_ReturnGuid()
        {
            //Arrange
            CreateProductCommand product = new()
            {
                Name = "Johan & Nystrm Coffe",
                Description = "Swedish coffee",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            };

            //Act
            var response = await _client.PostAsync(ApiRoutes.Products.Create, product.ToJsonHttpContent());
            var responseContent = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            
            var isCorrectGuid = Guid.TryParse(responseContent, out Guid id);

            //Assert
            isCorrectGuid.Should().Be(true);
        }

        [Theory]
        [ClassData(typeof(CreateProductValidatorTestsData))]
        public async Task Create_ForInCorrectParameters_ReturnGuid
            (string? name, string? description, decimal? price, int? number, int? quantity)
        {
            //Arrange
            CreateProductCommand product = new()
            {
                Name = name,
                Description = description,
                Price = price,
                Number = number,
                Quantity = quantity
            };

            //Act
            var response = await _client.PostAsync(ApiRoutes.Products.Create, product.ToJsonHttpContent());


            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    public class CreateProductValidatorTestsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "Swedish coffee", 40.20m, 4, 5};
            yield return new object[] { "Johan & Nystrm Coffe", "Swedish coffee", null, 4, 5};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
