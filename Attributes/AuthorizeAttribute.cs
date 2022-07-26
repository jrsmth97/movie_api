using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace movie_api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["UserId"];

            if (user == null)
            {
                context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            var role = context.HttpContext.Items["UserRole"]?.ToString();

            if (Roles != null && role != null && !Roles.Contains(role))
                context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}