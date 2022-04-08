using Application.Common.Validation;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebAPI.Contracts.V1;

namespace WebAPI.Controllers.V1
{
    public class ProductsController : ApiControllerBase
    {
        [HttpPost]
        [Route(ApiRoutes.Products.Create)]
        public async Task<ActionResult<Guid>> GuidPost(CreateProductCommand product)
        {
            var validatedObj = product.NotNullOrEmpty(product.Description)
                .NotNullOrEmpty(product.Name).MaxLength(product.Description,200);

            return Ok(await Mediator.Send(validatedObj));
        }

        [HttpGet]
        [Route(ApiRoutes.Products.GetAll)]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            return Ok(await Mediator.Send(new GetProductsQuery()));
        }

        [HttpGet]
        [Route(ApiRoutes.Products.GetById)]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            
            var product = await Mediator.Send(new GetProductByIdQuery() {Id = id});
            return product != null ? Ok(product) : NotFound($"The product with the id {id} was not found");
        }

        [HttpDelete]
        [Route(ApiRoutes.Products.Delete)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await Mediator.Send(new DeleteProductCommand() {Id = id});

            return StatusCode((int) result.StatusCode,result.Message);
        }

        [HttpPut]
        [Route(ApiRoutes.Products.Update)]
        public async Task<ActionResult> Put(UpdateProductCommand product)
        {
            var validatedObj = product.NotNullOrEmpty(product.Description)
               .MaxLength(product.Description, 200);

            var result = await Mediator.Send(validatedObj);

            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
