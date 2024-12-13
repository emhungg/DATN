using Microsoft.AspNetCore.Diagnostics;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Resources;
using System.Net;

namespace StudySystem.Middlewares
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        /// <summary>
        /// Summary:
        /// Global Error Handler
        /// </summary>
        /// <param name="app"></param>
        /// <returns>StudySystemAPIResponse</returns>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        //Check if Error is BadHttpRequestException(Code 400)
                        if (contextFeature?.Error is BadHttpRequestException)
                        {
                            var errorMessageRaw = contextFeature.Error.Message;
                            var errorCode = string.Empty;
                            var errorMessage = string.Empty;
                            if (errorMessageRaw.Contains("(.)"))
                            {
                                var messageSplit = errorMessageRaw.Split("(.)");
                                errorCode = messageSplit[0];
                                errorMessage = Message.ResourceManager.GetString(messageSplit[1]);
                            }
                            else
                            {
                                errorCode = StatusCodes.Status400BadRequest.ToString();
                                errorMessage = errorMessageRaw;
                            }

                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                            {
                                Code = StatusCodes.Status400BadRequest,
                                Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status400BadRequest, errorMessage))
                            });


                        }
                        //Check if Error is UnauthorizedAccessException(Code 401)
                        if (contextFeature?.Error is UnauthorizedAccessException)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                            {
                                Code = StatusCodes.Status401Unauthorized,
                                Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status401Unauthorized, Message.Unauthorize))
                            });
                        }

                        if (context.Response.StatusCode == (int)HttpStatusCode.NotFound) // 404
                        {
                            await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                            {
                                Code = StatusCodes.Status404NotFound,
                                Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status404NotFound, Message._404))
                            });
                        }

                        if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError) // 500
                        {
                            await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                            {
                                Code = StatusCodes.Status500InternalServerError,
                                Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status500InternalServerError, Message._500))
                            });
                        }
                    }
                });
            });
        }
    }
}
