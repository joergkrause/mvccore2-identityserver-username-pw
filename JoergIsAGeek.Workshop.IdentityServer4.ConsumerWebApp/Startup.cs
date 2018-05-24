using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultipleFederationServices.Data;
using MultipleFederationServices.Models;
using MultipleFederationServices.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MultipleFederationServices
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

      services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

      services.AddAuthentication()
        .AddFacebook(options =>
               {
                 options.AppId = "demo";
                 options.AppSecret = "demo";
               });

      services.AddAuthentication()
        .AddOpenIdConnect(JwtBearerDefaults.AuthenticationScheme, "Unser ID Server", options =>
        {
          options.Authority = "https://localhost:44318/";
          options.ClientId = "dataEventRecords";
          options.ClientSecret = "dataEventRecordsSecret";
        });


      services.AddAuthentication()
        .AddMicrosoftAccount(options =>
        {
          options.ClientId = "d1";
          options.ClientSecret = "demo";
        });

      //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      //  .AddIdentityServerAuthentication(options =>
      //  {
      //    options.Authority = "https://localhost:44318/";
      //    options.ApiSecret = "dataEventRecordsSecret";
      //    options.ApiName = "dataEventRecords";
      //    options.SupportedTokens = SupportedTokens.Both;
      //  });


      // Add application services.
      services.AddTransient<IEmailSender, EmailSender>();

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
