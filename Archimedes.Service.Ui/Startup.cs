using Archimedes.Library.Domain;
using Archimedes.Service.Ui.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Archimedes.Service.Ui
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
            services.Configure<Config>(Configuration.GetSection("AppSettings"));
            services.AddSignalR();
            services.AddControllers();

            var config = Configuration.GetSection("AppSettings").Get<Config>();

            //todo leave as example
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", x =>
                {
                    x.WithOrigins("http://localhost:4200",config.UserInterfaceBaseUrl,"http://localhost:1103","http://archimedes.com:1103")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //todo leave as example
            app.UseCors("AllowAny");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ValuesHub>("/Hubs/Values");
            });
        }
    }
}
