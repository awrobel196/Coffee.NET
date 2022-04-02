using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Products.Commands.CreateProduct;
using Application.Products.Queries.GetProducts;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        private readonly IApplicationDbContext _context;

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

            _context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CoffeMemoryDB")
                .Options);
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

        [Fact]
        public async Task GetAll_WithoutParameters_ReturnOkWithProductsList()
        {
            //Arrange
            _context.Product.Add(new Product()
            {
                Id = new Guid(),
                Name = "Johan & Nystrm Coffe",
                Description = "Swedish coffee",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            });

            await _context.SaveChangesAsync(new CancellationToken());
            //Act
            var response = await _client.GetAsync(ApiRoutes.Products.GetAll);
            var responseContent = JsonConvert.DeserializeObject<List<ProductDto>>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetOneById_WithCorrectParameter_ReturnOkWithProduct()
        {
            //Arrange
            var productId = new Guid("2a74faa9-91bf-4768-9f73-3e376a4c8147");
            _context.Product.Add(new Product()
            {
                Id = productId,
                Name = "Johan & Nystrm Coffe",
                Description = "Swedish coffee",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            });

            await _context.SaveChangesAsync(new CancellationToken());

            //Act
            var response = await _client.GetAsync(ApiRoutes.Products.GetById.Replace("{id}", productId.ToString()));
            var responseContent = JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNull();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("aaa-bbb-ccc")]
        public async Task GetOneById_WithIncorrectParameter_ReturnBadRequest(string productId)
        {
            //Arrange

            //Act
            var response = await _client.GetAsync(ApiRoutes.Products.GetById.Replace("{id}", productId));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("aaa-bbb-ccc")]
        public async Task Delete_WithIncorrectParameter_ReturnBadRequest(string productId)
        {
            //Arrange

            //Act
            var response = await _client.DeleteAsync(ApiRoutes.Products.Delete.Replace("{id}", productId));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WithGuidWhoNotExistInDb_ReturnNotFound()
        {
            //Arrange
            var productId = Guid.NewGuid();

            //Act
            var response = await _client.DeleteAsync(ApiRoutes.Products.Delete.Replace("{id}", productId.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Delete_WithCorrectParameter_ReturnOk()
        {
            //Arrange
            var productId = Guid.NewGuid();
            _context.Product.Add(new Product()
            {
                Id = productId,
                Name = "Johan & Nystrm Coffe",
                Description = "Swedish coffee",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            });
            await _context.SaveChangesAsync(new CancellationToken());

            //Act
            var response = await _client.DeleteAsync(ApiRoutes.Products.Delete.Replace("{id}", productId.ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
