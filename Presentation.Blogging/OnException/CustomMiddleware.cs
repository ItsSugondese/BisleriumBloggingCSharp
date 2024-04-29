using Domain.Blogging.enums;
using Domain.Blogging.view;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Blogging.OnException
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<CustomExceptionFilterAttribute> _logger;


        public CustomMiddleware(RequestDelegate next
            //, ILogger<CustomExceptionFilterAttribute> logger
            )
        {
            _next = next;
            //_logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                //_logger.LogError(e, e.Message);

                //var apiError = new ApiError
                //{
                //    Status = (int)ResponseStatus.Fail,
                //    HttpCode = 500, // You may customize this as needed
                //    Message = e.Message,
                //    Errors = new List<string> { e.Message }
                //};

                var response = context.Response;
                await response.WriteAsync(e.Message);
                //await response.WriteAsJsonAsync(apiError);

            }
        }


    }
}
