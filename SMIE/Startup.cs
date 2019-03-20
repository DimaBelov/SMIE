using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using SMIE.Core.Data;
using SMIE.Core.Data.Settings;
using SMIE.DAL.Interfaces;
using SMIE.DAL.Services;

namespace SMIE
{
    public class Startup
    {
        const string SMIE_CONNECTION_NAME = "SMIE";

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            loggerFactory.AddSerilog();
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(
                    options =>
                    {
                        options.DefaultChallengeScheme = AppConstants.DefaultAuthScheme;
                        options.DefaultAuthenticateScheme = AppConstants.DefaultAuthScheme;
                        options.DefaultScheme = AppConstants.DefaultAuthScheme;
                    })
                .AddCookie(AppConstants.DefaultAuthScheme, options =>
                {
                    options.AccessDeniedPath = "/Account/Login";
                    options.LoginPath = "/Account/Login";
                });

            services
                .Configure<ServersSettings>(Configuration.GetSection("ServersSettings"))
                .AddSingleton<IProviderFactory, ProviderFactory>()
                .AddScoped<IDbConnectionsManager, ConnectionsManager>()
                .AddScoped<IGenericRepository>(provider => new GenericRepository(
                    provider.GetService<IDbConnectionsManager>(),
                    SMIE_CONNECTION_NAME))
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICatalogService, CatalogService>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Videos")),
                RequestPath = "/Videos"
            });

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
