using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GraphQl.AspNetCore;
using GraphQL;
using Kaazam.Models;
using Kaazam.Data;
using GraphQL.Types;
 
namespace Kaazam
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
            services.AddMvc();
            services.AddScoped<EasyStoreQuery>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<CategoryType>();
            services.AddTransient<ProductType>();
            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new EasyStoreSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<EasyStoreQuery>() });

            services.AddCors(options =>
            {
                //Create CORS Policy for any Origin,Specific methods and allowed headers
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                   .WithMethods(Configuration["AllowedMethods"])
                   .WithHeaders(Configuration["AllowedHeaders"]));

                //Create CORS Policy for specific Origin,Specific methods and allowed headers
                options.AddPolicy("CorsPolicy_With_Specific_Origin",
                    builder => builder.WithOrigins("*")
                   .WithMethods("POST", "GET", "PUT", "DELETE", "OPTIONS")
                   .WithHeaders("X-Requested-With,Content-Type,clientid,tokenid,applicationid,licenseKey,Content-Type,Accept,Chunk-Index,Chunk-Max,Rename,InstanceUserData,InstanceData,UploadId,FileId, ,isLastFileLastChunk,pageNumber,pageSize,googleTokenID")
                   );

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }           

            //Consume Created CORS Policy 
            app.UseCors("CorsPolicy");
            //allow static File rendering
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    //Disable caching for all static files .
                    context.Context.Response.Headers["Cache-Control"] = Configuration["StatucFiles:Headers:Cache-Control"];
                    context.Context.Response.Headers["Pragma"] = Configuration["StatucFiles:Headers:Pragma"];
                    context.Context.Response.Headers["Expires"] = Configuration["StatucFiles:Headers:Expires"];
                }
            }
           );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
