using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eFood.Catalog.WebApi.DAL;
using eFood.Common.InboxPattern;
using eFood.Common.InboxPattern.EntityFramework;
using eFood.Common.MassTransit;
using eFood.Common.Serilog;
using eFood.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eFood.Catalog.WebApi
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
            services.AddDbContextPool<CatalogDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DbContext")));

            services.AddControllers();

            services.AddSwagger((IConfigurationRoot)Configuration, c =>
            {
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "eFood.Catalog.WebApi.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddMediatR(typeof(Startup));
            services.AddMassTransit(Configuration, null, null);
            services.AddSingleton<IBusPublisher, MassTransitPublisher>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureSwagger(Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
