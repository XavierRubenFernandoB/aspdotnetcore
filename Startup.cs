using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using NetCoreProj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using NetCoreProj.Security;

namespace NetCoreProj
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
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
                        .AddDefaultTokenProviders();

            //services.AddRazorPages()

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = true;//to enforce email confirmation during login
            }
            );

            //Uses all MVC methods
            services.AddMvc(options => options.EnableEndpointRouting = false);

            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    options.ClientId = "XXXXX";
            //    options.ClientSecret = "YYYYY";
            //});

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            //AUTH : WITHOUT FUNC
            /*
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRolePolicy",
                            policy => policy.RequireClaim("Create Role", "true").RequireClaim("Edit Role", "true").RequireClaim("Delete Role", "true")
                            );
                options.AddPolicy("SuperAdminPolicy",
                            policy => policy.RequireRole("Admin", "User", "Manager"));
            });
            */

            //AUTH : WITH FUNC
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RolesAdminPolicy", policy =>
                    policy.RequireAssertion(context => AuthorizeAccess(context)));
            });

            /*NOT WORKING AS INTENDED
            //AUTH : WITH CUSTOM AUTHORIZATION
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RolesAdminPolicy", policy =>
                    policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
            });
            // Register the first handler
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            // Register the second handler
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            */

            //Used to apply [Authorize] globally for for all Controllers in the Solution   
            //Did not work. Hence commenting
            //services.AddMvc(
            //(
            //    config => { var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //                config.Filters.Add(new AuthorizeFilter(policy));
            //    }
            //)
            //);

            //Uses only the Core MVC methods. (During usage of Views it fails) Using this temporarily because auto routing is not happening to Home Controller
            //services.AddMvcCore(options => options.EnableEndpointRouting = false);

            //To get XML output
            //services.AddMvcCore(options => options.EnableEndpointRouting = false).AddXmlDataContractSerializerFormatters();

            //For in-memory
            //services.AddTransient<IEmployee, MockIEmployee>();
            //For SQL
            services.AddScoped<IEmployee, SQLIEmployeeRepository>();
        }

        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return (
                       context.User.IsInRole("Admin")
                    && context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true")
                    && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                    && context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true")
                    )
                    ||
                    (
                        context.User.IsInRole("SuperAdmin")
                    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //NOT WORKING
                //Error() in ErrorController will be called
                app.UseExceptionHandler("/Error");

                //app.UseStatusCodePages();

                //WORKING
                //MyHttpStatusCodeHandler in ErrorController will be called
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //0
            //if (env.IsDevelopment())
            //{
            //    DeveloperExceptionPageOptions depo = new DeveloperExceptionPageOptions
            //    {
            //        SourceCodeLineCount = 1
            //    };
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //}

            //1
            //app.Run(async (context) => {
            //    await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //});


            //2
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync(Configuration["MyKey"]);
            //    logger.LogInformation("log1");
            //    await next();
            //    await context.Response.WriteAsync("2");
            //    logger.LogInformation("log2");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("3");
            //    await next();
            //    await context.Response.WriteAsync("4");
            //});

            //3
            //DefaultFilesOptions obj = new DefaultFilesOptions();
            //obj.DefaultFileNames.Clear();
            //obj.DefaultFileNames.Add("foo1.html");

            //app.UseDefaultFiles(obj);
            //app.UseStaticFiles();

            //4      
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();

            //app.Run(async (context) =>
            //{
            //    //throw new Exception("This is my exception");
            //    //await context.Response.WriteAsync("5");
            //    await context.Response.WriteAsync(env.EnvironmentName);
            //});

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=index}/{id?}");
            });

            //not working
            //app.UseMvc();

            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
        }
    }
}
