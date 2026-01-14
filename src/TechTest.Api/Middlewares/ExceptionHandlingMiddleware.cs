using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Net;
using FluentValidation;
using TechTest.Application.Common.Exceptions;

namespace TechTest.Api.Middlewares
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = "Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message
                });
            }
            catch (ValidationException ex)
            {
                var problem = new ProblemDetails
                {
                    Title = "Validation error",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = "Uno o más campos no son válidos."
                };

                problem.Extensions["errors"] = ex.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                });

                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (PostgresException ex) when (ex.SqlState == "22023")
            {
                var problem = new ProblemDetails
                {
                    Title = "Invalid parameter",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = ex.MessageText
                };

                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (Exception)
            {
                var problem = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = "Ocurrió un error inesperado."
                };

                context.Response.StatusCode = problem.Status.Value;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
