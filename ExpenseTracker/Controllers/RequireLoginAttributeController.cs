using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Controllers
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var username = context.HttpContext.Session.GetString("Nickname");
            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
