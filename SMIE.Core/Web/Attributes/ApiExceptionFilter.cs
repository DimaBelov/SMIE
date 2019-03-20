using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace SMIE.Core.Web.Attributes
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            var baseEx = context.Exception.GetBaseException();

            Log.Error(baseEx, $"Exception in action {context.ActionDescriptor.DisplayName}");

            context.Result = new BadRequestObjectResult(new { error = baseEx.Message });
        }
    }
}
