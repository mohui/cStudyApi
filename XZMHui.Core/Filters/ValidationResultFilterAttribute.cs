using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using XZMHui.Core.Model;

namespace XZMHui.Core.Filters
{
    public class ValidationResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var objectResult = context.Result as BadRequestObjectResult;
            if (objectResult == null) return;

            var validationResults = objectResult.Value as Microsoft.AspNetCore.Mvc.ValidationProblemDetails;
            if (validationResults == null) return;

            context.Result = new ObjectResult(
                new CoreResult<object>()
                {
                    Code = 10004,
                    Result = 0,
                    Message = "参数验证失败",
                    Data = validationResults.Errors.Select(x => new
                    {
                        field = x.Key,
                        message = x.Value,
                        Rows = validationResults.Errors.Count
                    }),
                }
            );
        }
    }
}