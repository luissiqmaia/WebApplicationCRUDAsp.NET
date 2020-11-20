using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplicationCRUD.Data;
using WebApplicationCRUD.Services;
using Microsoft.Extensions.Hosting;

namespace WebApplicationCRUD {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services) {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddControllersWithViews();

            services.AddDbContext<WebApplicationCRUDContext>(options =>
                    options.UseMySql(Configuration.GetConnectionString("WebApplicationCRUDContext"),
                    builder => builder.MigrationsAssembly("WebApplicationCRUD"))
                    );

            services.AddScoped<SellerService>();
            services.AddScoped<SeedingService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedingService seedingService) {
            var ptBr = new CultureInfo("pt-Br");
            var enUS = new CultureInfo("en-US");

            var localizationOptions = new RequestLocalizationOptions {
                DefaultRequestCulture = new RequestCulture(ptBr),
                SupportedCultures = new List<CultureInfo> { ptBr, enUS },
                SupportedUICultures = new List<CultureInfo> { ptBr, enUS }
            };
            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                seedingService.Seed();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
