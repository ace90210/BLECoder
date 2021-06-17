using BLECoder.Blazor.Server.Authentication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockDoor.Data.Migrations;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts;
using RadzenTemplate.Server.AutoMapperProfiles;
using RadzenTemplate.Server.Repositories;
using RadzenTemplate.Server.Services;

namespace RadzenTemplate.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAutoMapper(typeof(UserConfigurationProfile));

            services.AddBlazorServerAuthentication("https://ids.runeclawgames.com/", "example.api", "example.secret");


            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<RadzenTemplateContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<RadzenTemplateRepository>();
            services.AddScoped<UserConfigurationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if(Configuration.GetValue<bool>("ApplyMigrationOnStartup"))
                app.ApplyMigrations<RadzenTemplateContext>();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
