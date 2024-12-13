

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace StudySystem.Middlewares
{
    /// <summary>
    /// Authorization when user was login or logout
    /// </summary>
    public class AuthTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthTokenMiddleware> _logger;
        public AuthTokenMiddleware(RequestDelegate next, ILogger<AuthTokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context, IUserTokenService userTokenService)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
            }
            else
            {
                var filter = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();
                if (filter != null)
                {
                    string tokenHeader = context.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(tokenHeader))
                    {
                        try
                        {
                            var token = tokenHeader.Replace("Bearer ", "");
                            if (token != null)
                            {
                                var readJwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                                var jti = readJwtToken.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
                                var uid = readJwtToken.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
                                context.Items.Add("UserName", jti);
                                context.Items.Add("UserID", uid);
                                bool validToken = await userTokenService.AuthToken(token).ConfigureAwait(false);
                                if (!validToken)
                                {
                                    await ResponseStatusCode(context, StatusCodes.Status401Unauthorized).ConfigureAwait(false);
                                }
                                else
                                {
                                    await _next(context).ConfigureAwait(false);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            await ResponseStatusCode(context, StatusCodes.Status401Unauthorized).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        await _next(context).ConfigureAwait(false);
                    }
                }
                else
                {
                    await _next(context).ConfigureAwait(false);
                }

            }

        }

        public async Task ResponseStatusCode(HttpContext context, int statusCode)
        {
            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status401Unauthorized, Message.Unauthorize))
                });
            }
        }
    }
}
