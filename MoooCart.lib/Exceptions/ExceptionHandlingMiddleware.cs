using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoooCart.lib.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.Exceptions
{
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                var logger = context.RequestServices.GetRequiredService<IAppLoger<ExceptionHandlingMiddleware>>();
                var innerEx = ex.InnerException as SqlException;

                if (innerEx != null) 
                {
                    logger.LogError(innerEx, "SQL Exception");
                    switch(innerEx.Number)
                    {
                        case 2627:
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync($"Unique constraints violation : {innerEx.Message}");
                            break;
                        case 515:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync($"Can't Accept Null Value : {innerEx.Message}");
                            break;
                        case 547:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync($"Forign Key constraint violation  : {innerEx.Message}");
                            break;
                        default:
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync($"There is an Error in the server  : {innerEx.Message}");
                            break;
                    }
                }
                else
                {
                    logger.LogError(ex, "Non SQL Exception");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("There is an Error in the server");
                }
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetRequiredService<IAppLoger<ExceptionHandlingMiddleware>>();
                logger.LogError(ex, "Other Exception");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"There is an Error in the server  : {ex.Message}");
            }
        }
    }
}
