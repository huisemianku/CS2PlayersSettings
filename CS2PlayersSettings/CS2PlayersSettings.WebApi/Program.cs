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

            // Add services to the container.

            // 添加 DbContext 服务
            builder.Services.AddDbContext<PlayerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // 使用依赖注入 注册BLL和DAL
            builder.Services.AddScoped<PlayerWebApiBLL>();
            builder.Services.AddScoped<PlayerWebApiDAL>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
