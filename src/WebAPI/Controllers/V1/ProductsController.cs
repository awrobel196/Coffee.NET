using Application.Products.Commands.CreateProduct;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.V1;

namespace WebAPI.Controllers.V1
{
    public class ProductsController : ApiControllerBase
    {
        [HttpPost]
        [Route(ApiRoutes.Products.Create)]
        public async Task<ActionResult<Guid>> GuidPost(CreateProductCommand product)
        {
            return Ok(await Mediator.Send(product));
        }
    }
}
