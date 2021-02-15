using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitor.WebApi.Data;
using PriceMonitor.WebApi.SignalR;

namespace PriceMonitor.WebApi.Setup
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MonitorContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("Development", b =>
                    b.WithOrigins("http://localhost:8080")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                options.AddPolicy("Production", b =>
                    b.WithMethods("GET", "PUT", "POST", "DELETE")
                    .WithOrigins("http://localhost:8080")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                    .AllowAnyHeader());
            });

            services.RegisterServices();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("Development");
            }
            else
                app.UseCors("Production");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ItemEventsHub>("/updates");
            });

            return app;
        }
    }
}
