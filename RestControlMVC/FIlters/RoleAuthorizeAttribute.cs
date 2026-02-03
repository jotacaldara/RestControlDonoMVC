using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestControlMVC.FIlters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Session.GetString("AuthToken");
            var role = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(token) || role != _role)
            {
                context.Result = new RedirectToActionResult(
                    "Login", "Auth", null);
            }
        }
    }

}
