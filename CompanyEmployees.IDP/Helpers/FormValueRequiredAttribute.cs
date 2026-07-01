using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CompanyEmployees.IDP.Helpers;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class FormValueRequiredAttribute(string name) : ActionMethodSelectorAttribute
{
    public override bool IsValidForRequest(RouteContext context, ActionDescriptor action)
    {
        if (string.Equals(context.HttpContext.Request.Method, HttpMethods.Post, StringComparison.OrdinalIgnoreCase))
        {
            if ((action as ControllerActionDescriptor)?.MethodInfo.GetCustomAttributes(inherit: true)
                .OfType<FormValueRequiredAttribute>()
                .Count() is > 1)
            {
                return context.HttpContext.Request.HasFormContentType &&
                       context.HttpContext.Request.Form.ContainsKey(name);
            }

            return !context.HttpContext.Request.HasFormContentType ||
                   context.HttpContext.Request.Form.ContainsKey(name);
        }

        return false;
    }
}
