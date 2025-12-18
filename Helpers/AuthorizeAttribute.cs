using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using siapv_backend.Models.DTOResponses;
namespace siapv_backend.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        public AuthorizeAttribute()
        {

        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = (DTOUserContext?)context.HttpContext.Items["User"];
            if(usuario == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}