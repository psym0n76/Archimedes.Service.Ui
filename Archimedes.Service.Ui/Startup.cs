using Archimedes.Library.Domain;
using Archimedes.Library.Message;
using Archimedes.Library.RabbitMq;
using Archimedes.Service.Price;
using Archimedes.Service.Ui.Http;
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
            services.AddLogging();
            services.Configure<Config>(Configuration.GetSection("AppSettings"));

            var config = Configuration.GetSection("AppSettings").Get<Config>();

            services.AddSingleton(Configuration);

            services.AddSignalR();

            services.AddControllers();
            services.AddHttpClient<IHttpRepositoryClient, HttpRepositoryClient>();
            services.AddHttpClient<IHttpHealthMonitorClient, HttpHealthMonitorClient>();

            services.AddHostedService<HealthSubscriptionService>();
            //services.AddHostedService<StrategySubscriptionService>();
            services.AddHostedService<MarketSubscriptionService>();
            services.AddHostedService<PriceSubscriptionService>();
            services.AddHostedService<PriceLevelSubscriptionService>();
            services.AddHostedService<TradeSubscriptionService>();
            
            services.AddTransient<IProducer<PriceMessage>>(x =>
                new Producer<PriceMessage>(config.RabbitHost, config.RabbitPort, config.RabbitExchange));
            services.AddTransient<IPriceRequestManager, PriceRequestManager>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", x =>
                {
                    x.WithOrigins("http://localhost:4200",
                            "http://angular-ui.dev.archimedes.com")
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

            app.UseCors("AllowAny");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HealthHub>("/hubs/health");
                endpoints.MapHub<StrategyHub>("/hubs/strategy");
                endpoints.MapHub<MarketHub>("/hubs/market");
                endpoints.MapHub<PriceHub>("/hubs/price");
                endpoints.MapHub<PriceLevelHub>("/hubs/price-level");
                endpoints.MapHub<TradeHub>("/hubs/trade");
            });
        }
    }
}
