using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Validation;
using Application.Products.Commands.CreateProduct;
using WebAPI.Contracts.V1;
using WebAPI.Tests.Helpers;
using Xunit;

namespace WebAPI.Tests
{
    public class ValidatorTests
    {
        [Fact]
        public async Task Validate_ForCorrectObj_ThrowException()
        {
            //Arrange
            CreateProductCommand product = new()
            {
                Name = "Johan & Nystrm Coffe",
                Description = "",
                Price = 40.20m,
                Number = 4,
                Quantity = 5
            };

            //Act
            Action action = () => product.NotNullOrEmpty(product.Description).MaxLength(product.Description, 200);
            //Assert
            Assert.Throws<ValidateException>(action);
        }

    }
}
