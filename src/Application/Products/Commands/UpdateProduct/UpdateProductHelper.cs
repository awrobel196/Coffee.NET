using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands.UpdateProduct
{
    public record UpdateProductHelper(HttpStatusCode StatusCode, string Message);
}
