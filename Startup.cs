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
            //services.AddRazorPages();

            //Uses all MVC methods
            services.AddMvc(options => options.EnableEndpointRouting = false);

            //Uses only the Core MVC methods. (During usage of Views it fails) Using this temporarily because auto routing is not happening to Home Controller
            //services.AddMvcCore(options => options.EnableEndpointRouting = false);

            //To get XML output
            //services.AddMvcCore(options => options.EnableEndpointRouting = false).AddXmlDataContractSerializerFormatters();

            services.AddSingleton<IEmployee, MockIEmployee>();
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
                app.UseExceptionHandler("/Error");
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
            //app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                //throw new Exception("This is my exception");
                //await context.Response.WriteAsync("5");
                await context.Response.WriteAsync(env.EnvironmentName);
            });

            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});
        }
    }
}
