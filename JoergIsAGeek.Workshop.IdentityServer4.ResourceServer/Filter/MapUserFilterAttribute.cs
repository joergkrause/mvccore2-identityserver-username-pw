using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreResourceServer.Filter
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MapUserFilterAttribute : ActionFilterAttribute
    {

    public override void OnActionExecuting(ActionExecutingContext context)
    {

      var claims = context.HttpContext.User.Claims;
      // mapping
      context.HttpContext.Items.Add("MappedUser", "...");

      base.OnActionExecuting(context);
      
    }

  }
}
