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
            services.AddScoped<StarWarsQuery>();
            services.AddTransient<ICharacterRepository, CharacterRepository>();
            services.AddTransient<IEpisodesRepository, EpisodesRepository>();
            services.AddTransient<IStarshipRepository, StarshipRepository>();
            //services.AddTransient<ICharacterTypeRepository, CharacterTypeRepository>();
            //services.AddTransient<ICharacterRepository, CharacterRepository>();
            //services.AddTransient<ICharacter_Category_Repository, Character_Category_Repository>();
            //services.AddTransient<IEpisode_Characters_Repository, Episode_Characters_Repository>();
            //services.AddTransient<IEpisode_Characters_Friends_Repository, Episode_Characters_Friends_Repository>();


            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddTransient<CharacterType>();
            services.AddTransient<StarshipType>();
            //services.AddTransient<Character_CategoryType>();
            services.AddTransient<EpisodeType>();
            //services.AddTransient<Episode_CharactersType>();
            //services.AddTransient<Episode_Characters_FriendsType>();
         
            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new StarWarsSchema(type => (GraphType)sp.GetService(type)) { Query = sp.GetService<StarWarsQuery>() });

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
