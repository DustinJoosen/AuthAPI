using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Auth.API.Presentation.Attributes
{
    public class LogWhenRequestMadeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var name = descriptor?.ActionName;

            Log.Information($"An HTTP request is made at action '{name}'");

            base.OnActionExecuting(context);
        }
    }
}
