using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PhotoBank.Broker.Api.Contracts;

namespace PhotoBank.Broker.Api.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string login, token;
            GetLoginToken(context, out login, out token);
            var authenticationManager = context.HttpContext.RequestServices.GetService<IAuthenticationManager>();
            if (authenticationManager.IsAuthenticated(login, token) == false)
            {
                context.Result = new NotFoundResult();
            }
            base.OnActionExecuting(context);
        }

        private void GetLoginToken(ActionExecutingContext context, out string login, out string token)
        {
            login = token = "";
            if (context.ActionArguments.Count == 1 && context.ActionArguments.First().Key == "request")
            {
                var request = context.ActionArguments.First().Value as AuthenticatedRequest;
                login = request.Login;
                token = request.Token;
            }
            else if (context.ActionArguments.Any(x => x.Key == "login") && context.ActionArguments.Any(x => x.Key == "token"))
            {
                login = context.ActionArguments["login"].ToString();
                token = context.ActionArguments["token"].ToString();
            }
        }
    }
}
