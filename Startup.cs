using Advantage.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Advantage.API
{
    public class Startup
    {
        private string _connectionString = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            _connectionString = Configuration["secretConnectionString"];

            services.AddMvc(
            );


            services.AddEntityFrameworkNpgsql().AddDbContext<ApiContext>(opt => opt.UseNpgsql(_connectionString));

            services.AddTransient<DataSeed>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var nCustomers = 20;
            var nOrders = 1000;
            seed.SeedData(nCustomers, nOrders);

            app.UseEndpoints(routes => routes.MapControllerRoute(
                "default", "api/{controller}/{action}/{id?}"
            ));

        }
    }
}