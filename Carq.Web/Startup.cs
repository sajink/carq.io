namespace Carq.Ops
{
    using Carq.Service;
    using Az.Storage;
    using Inda.Rp;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Linq;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            services.AddIndaIdentity(new IndaOptions("ltank"));
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
#else
            services.AddIndaIdentity(new IndaOptions("carqio"));
            services.AddControllersWithViews();
#endif
            services.AddResponseCompression(o =>
            {
                o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            services.AddResponseCaching();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            AddStorage(services);
        }

        private void AddStorage(IServiceCollection services)
        {
            services.AddAzStorage("DefaultEndpointsProtocol=https;AccountName=aegir00;AccountKey=5aEjjsyfxxQfOaMwBK/mvzI+HPULCCfnUSldL3iW5HO3JbwcjKEHZxxxSI4MIG5xJXxeYqSQwADV4bapaoDPCw==");
            services.AddSingleton<ReportService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIndaIdentity();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Spa}/{action=Board}/{id?}");
            });
        }
    }
}
