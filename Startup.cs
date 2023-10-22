using Autofac;
using AutoMapper;
using Commander.BasicAuth;
using Commander.Controllers;
using Commander.Data;
using Commander.Dependency;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Commander
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
            ////  Basic Authentication
            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("BasicAuthentication",null);

            //  adding a DB Context for use within the REST of this application thru dependency injection & providing configuration which is our connection string
            services.AddDbContext<CommanderContext>(opt => opt.UseSqlServer
                (Configuration.GetConnectionString("CommanderConnection")));

            


            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });



            //  make AutoMapper available through Depenceny Injection to our application...using AutoMapper;
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //  AUTOFAC
            services.AddOptions();

            services.AddScoped<ICommanderRepository,SqlCommanderRepository>();
            services.AddScoped<ICommanderRepository,MockCommanderRepository>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",new OpenApiInfo { Title = "Commander API",Version = "v1" });
            });
        }

        //  AUTOFAC
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DependencyRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {
            

            //  Code commented out below due to developer exception info being shown when error occurs
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json","Commander API V1");
                });

                // app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();





            //  LOGGING
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var logger = appBuilder.ApplicationServices.GetRequiredService<ILogger<Startup>>();
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature.Error != null)
                    {
                        logger.LogError(feature.Error,"Exception Here!");

                         context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                        {
                            error = "Something went wrong!",
                            detial = feature.Error.Message
                        }));
                    }
                });

            });


            app.UseRouting();

            app.UseAuthentication();        //BASIC Authentication 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
