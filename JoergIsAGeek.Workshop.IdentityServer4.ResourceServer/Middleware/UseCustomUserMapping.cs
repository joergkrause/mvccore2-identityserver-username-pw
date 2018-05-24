using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreResourceServer.Middleware
{
  public static class CustomUserMappingExtensions
  {

    public static CustomUser GetCustomUser(this HttpContext context)
    {
      var newUser = new CustomUser { Id = 1, Name = "Custom User" };
      context.Items.Add("CustomUser", newUser);
      return newUser;
    }

    public static void UseCustomUserMapping(this IApplicationBuilder app)
    {
      //app.Run(async context => context.Items.Add("", ""));
      app.Use(async (context, next) =>
      {
        var newUser = new CustomUser { Id = 1, Name = "Custom User" };
        context.Items.Add("CustomUser", newUser);
        await next.Invoke();
      });
    }
  }
}