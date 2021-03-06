﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using ProblemNet.Extensions;
using Swashbuckle.AspNetCore.Swagger;

namespace Sample2_2
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
            services.AddProblemDetails(cfg => { cfg.DefaultTypeBaseUri = "https://sample2_2.com/problemdetails/"; });

            services.AddMvc().
                     UseProblemDetailsInvalidModelStateResponseFactory().
                     SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddJsonOptions(options =>
                                        {
                                            options.SerializerSettings.Formatting = Formatting.Indented;
                                            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                                            options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

                                            options.SerializerSettings.Converters.Clear();
                                            options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                                        });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Sample2_2", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseProblemDetailsExceptionHandler();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample2_2 V1"); });

            app.UseMvc();
        }
    }
}
