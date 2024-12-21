

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using StudySystem.Application.Service;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Permissions;

namespace StudySystem.Middlewares
{
    public class AuthPermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthPermissionMiddleware> _logger;
        public AuthPermissionMiddleware(RequestDelegate next, ILogger<AuthPermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }
        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
            }
            else
            {
                var filter = endpoint.Metadata.GetMetadata<AuthPermissionAttribute>();
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
                                    var ur = readJwtToken.Claims.FirstOrDefault(x => x.Type == "Roles")?.Value;
                                    
                                    bool validToken = await userService.UserPermissionRolesAuth(uid).ConfigureAwait(false);
                                    if (!validToken)
                                    {
                                        await ResponseStatusCode(context, StatusCodes.Status403Forbidden).ConfigureAwait(false);
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
                                await ResponseStatusCode(context, StatusCodes.Status403Forbidden).ConfigureAwait(false);
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
            if (statusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new StudySystemAPIResponse<StudySystemErrorResponseModel>
                {
                    Code = (int)HttpStatusCode.Unauthorized,
                    Response = new Response<StudySystemErrorResponseModel>(false, new StudySystemErrorResponseModel(StatusCodes.Status403Forbidden, Message._403))
                });
            }
        }
    }
}
