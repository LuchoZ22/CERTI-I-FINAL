using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TercerParcial.Middlewares.Models;
using UPB.BussinessLogic.Managers.Exceptions;

namespace TercerParcial.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";
                var responseModel = new ResponseModel<string>() { Succed = false, Message = ex.Message };

                switch (ex)
                {
                    case NonFoundPatientException e:
                        //The patient was not found
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseModel.Message = "The patient was not found";
                        break;
                    case FailedToGetDataException e:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel.Message = "The rest server couldn be connected";

                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
