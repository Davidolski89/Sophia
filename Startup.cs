using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.DataBase;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace Sophia
{
    // Die wichtigsten Einstellungen werden hier gesetzt
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
            // Standardmethode um Controller und Views zu verwenden.
            services.AddControllersWithViews();

            // Verzeichnis wird geholt in dem das Programm gestertet wird, damit der Datenbankpfad für die SQLite DB erstellt werden kann.
            var GetDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // Selbterstellte Klasse zum erstellen der Datenbank mit Hilfe des Skripts Database/SophiaDBCreationString (falls noch nicht erstellt)
            SophiaDB.CheckAndCreateDatabase();

            // Hier wird der DatenbankContext gesetzt. Kannst SophiaDB in jedem Controller aufrufen/verwenden
            services.AddDbContext<SophiaDB>(options => options.UseSqlite($"Data Source={Path.Combine(GetDirectory, "Database", "Sophia.db")}"));

            // Identity Klasse zum speichern von Benutzern
            services.AddIdentity<SophiaUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<SophiaDB>()
                .AddDefaultTokenProviders();

            
            // Zum zwischenspeichern von SurveyZuständen die User gerade bearbeiten.
            services.AddDistributedMemoryCache();

            // Zum Identifizieren von Usern anhand einer SessionId.
            // Wird bei jeder anfrage vom Browser des Users an den Server mitgegeben.
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(6000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = "/Sophia"
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            // Hier wurde die DefaultRoute gesetzt
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "Sophia/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
