using ChatRoom.Hub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatRoom
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSignalR()
                //Use Azure SignalR Service
                //.AddAzureSignalR(opts =>
                //{
                //    opts.ConnectionString = Configuration["AzureSignalRServiceEndpoint"];
                //});
                //Use a Redis Instance (AWS ElasticCache)
                .AddStackExchangeRedis(Configuration["CACHE_URL"],
                    opts =>
                    {
                        opts.Configuration.ChannelPrefix = "ChatRoom";
                    });
        }

        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatSampleHub>("/chat");
                endpoints.MapGet("/health", () => "Ok");
            });
        }
    }
}
