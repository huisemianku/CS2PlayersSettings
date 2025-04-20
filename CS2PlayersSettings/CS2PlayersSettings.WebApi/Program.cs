using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.WebApi;
using Microsoft.EntityFrameworkCore;

namespace CS2PlayersSettings.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Allow your frontend origin
                          .AllowAnyHeader()                    // Allow any headers
                          .AllowAnyMethod()                    // Allow any HTTP methods (GET, POST, etc.)
                          .AllowCredentials();                 // Allow credentials (optional, if needed)
                });
            });

            // Add services to the container.

            // ��� DbContext ����
            builder.Services.AddDbContext<PlayerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // ʹ������ע�� ע��BLL��DAL
            builder.Services.AddScoped<PlayerWebApiBLL>();
            builder.Services.AddScoped<PlayerWebApiDAL>();

            builder.Services.AddScoped<NavbarWebApiBLL>();
            builder.Services.AddScoped<NavbarWebApiDAL>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            // ���� CORS �м��
            app.UseCors("AllowFrontend");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
