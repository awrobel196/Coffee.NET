using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public record HttpResponseHelper(HttpStatusCode StatusCode,string Message="", object? ResponseObject=null);
}
