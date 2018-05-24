using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Middleware;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Model;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace JoergIsAGeek.Workshop.IdentityServer4.ResourceServer
{
  public class Startup
  {
    public IConfigurationRoot Configuration { get; set; }

    private IHostingEnvironment _env { get; set; }

    public Startup(IHostingEnvironment env)
    {
      Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Verbose()
          .Enrich.WithProperty("App", "JoergIsAGeek.Workshop.IdentityServer4.ResourceServer")
          .Enrich.FromLogContext()
          .WriteTo.File("../Log/JoergIsAGeek.Workshop.IdentityServer4.ResourceServer")
          .CreateLogger();

      _env = env;
      var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {

      var cert = new X509Certificate2(Path.Combine(_env.ContentRootPath, "damienbodserver.pfx"), "");


      //Add Cors support to the service
      services.AddCors();

      var policy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy();

      policy.Headers.Add("*");
      policy.Methods.Add("*");
      policy.Origins.Add("*");
      policy.SupportsCredentials = true;
      //services.AddTransient(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));

      services.AddCors(x => x.AddPolicy("corsGlobalPolicy", policy));

      var guestPolicy = new AuthorizationPolicyBuilder()
          .RequireAuthenticatedUser()
          .RequireClaim("scope", "dataEventRecords")
          .Build();

      services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
      });

      services.AddAuthentication("Bearer")
          .AddIdentityServerAuthentication(options =>
          {
            options.Authority = "https://localhost:44318/";
            options.ApiSecret = "dataEventRecordsSecret";
            options.ApiName = "dataEventRecords";
            options.SupportedTokens = SupportedTokens.Both;            
          });


      services.AddAuthorization(options =>
      {
        options.AddPolicy("dataEventRecordsAdmin", policyAdmin =>
              {
                policyAdmin.RequireClaim("role", "dataEventRecords.admin");
              });
        options.AddPolicy("dataEventRecordsUser", policyUser =>
              {
                policyUser.RequireClaim("role", "dataEventRecords.user");
              });

      });

      services.AddMvc(options =>
      {
        options.Filters.Add(new AuthorizeFilter(guestPolicy));
      }).AddJsonOptions(options =>
      {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
      });

      services.AddScoped<IDataRepository, DataRepository>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole();
      loggerFactory.AddDebug();

      // Add Serilog to the logging pipeline
      loggerFactory.AddSerilog();

      app.UseExceptionHandler("/Home/Error");
      app.UseCors("corsGlobalPolicy");
      app.UseStaticFiles();

      app.UseAuthentication();
      app.UseCustomUserMapping();

      app.UseMvc();
    }
  }
}
