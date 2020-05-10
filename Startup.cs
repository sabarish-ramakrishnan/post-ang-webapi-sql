using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
//using NLog;
using post_ang_webapi_sql.DAL;
using post_ang_webapi_sql.Extensions;
using post_ang_webapi_sql.Services;

namespace post_ang_webapi_sql
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _logger = logger;
            NLog.LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }
        private readonly ILogger _logger;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            if (Convert.ToBoolean(Configuration["Secure"]))
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "yourdomain.com",
                        ValidAudience = "yourdomain.com",
                        IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                        };
                    });
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            });

            services.AddDbContext<BlogDBContext>
                (options => options.UseSqlServer(Configuration["ConnectionStrings:BlogDB"]));

            services.AddScoped<PostService>();
            services.AddScoped<UserService>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.ConfigureExceptionHandler(_logger);
            app.ConfigureCustomExceptionMiddleware(_logger);
            app.UseCors(builder =>
            {
                string[] headers = Convert.ToString(Configuration["CorsSettings:Headers"]).Split(',');
                string[] methods = Convert.ToString(Configuration["CorsSettings:Methods"]).Split(',');
                string[] origin = Convert.ToString(Configuration["CorsSettings:Origin"]).Split(',');
                builder.WithHeaders(headers);
                builder.WithMethods(methods);
                builder.WithOrigins(origin);

                //builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
            if (Convert.ToBoolean(Configuration["Secure"]))
            {
                app.UseAuthentication();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:5001");
                }
            });
        }
    }
}