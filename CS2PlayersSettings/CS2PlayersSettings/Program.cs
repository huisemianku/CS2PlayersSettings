using CS2PlayersSettings.Business;
using CS2PlayersSettings.Data;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.Repository.Helper;
using CS2PlayersSettings.Data.Repository.Model;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            // register DbContext
            builder.Services.AddDbContext<PlayerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            // register PlayerDAL PlayerBLL
            builder.Services.AddScoped<PlayerDAL>();
            builder.Services.AddScoped<PlayerBLL>();
            builder.Services.AddScoped<NavbarBLL>();
            builder.Services.AddScoped<NavbarDAL>();

            builder.Services.AddScoped<List<DemoFileInfoModel>>();
            builder.Services.AddScoped<ImportDataByExcel>();
            builder.Services.AddScoped<PlayerDataService>();
            builder.Logging.AddConsole();

            // .dem file size
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 1048576000; // 1 GB
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1048576000; // 1 GB 
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
