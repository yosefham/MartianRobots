using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MartianRobots.BL;
using MartianRobots.Common.Configuration;
using MartianRobots.Configuration;
using Microsoft.Extensions.Options;
using SimpleInjector;

namespace MartianRobots
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
            services.AddSimpleInjector(Program.Container, x =>
            {
                x.Services.AddOptions<Settings>()
                    .Bind(Configuration.GetSection(Settings.ConfigName))
                    .ValidateDataAnnotations();
                x.Services.AddSingleton<ISettings, Settings>(y => y.GetRequiredService<IOptions<Settings>>().Value);

                x.Services.AddBusinessLogic();
                
                x.Services.AddControllers();
                x.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MartianRobots", Version = "v1" });
                });

            });
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MartianRobots v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
