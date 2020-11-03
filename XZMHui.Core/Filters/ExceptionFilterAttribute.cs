using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using XZMHui.Core.Model;

namespace XZMHui.Core.Filters
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(new CoreResult<Exception>
            {
                Code = (int)status,
                Result = 0,
                Message = context.Exception.InnerException?.Message,
                Data = context.Exception.InnerException
            });
        }
    }
}