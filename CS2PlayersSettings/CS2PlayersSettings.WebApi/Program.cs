using CS2PlayersSettings.Business.WebApi;
using CS2PlayersSettings.Data.Repository;
using CS2PlayersSettings.Data.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

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

            builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<NavbarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 配置 JWT 认证
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };

                // 从 Cookie 提取 token
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["auth_token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // 使用依赖注入 注册BLL和DAL
            builder.Services.AddScoped<PlayerWebApiBLL>();
            builder.Services.AddScoped<PlayerWebApiDAL>();

            builder.Services.AddScoped<NavbarWebApiBLL>();
            builder.Services.AddScoped<NavbarWebApiDAL>();

            builder.Services.AddScoped<UserWebApiBLL>();
            builder.Services.AddScoped<UserWebApiDAL>();
  
            builder.Logging.AddConsole(); // 添加控制台日志记录器
            builder.Logging.AddDebug(); // 添加调试日志记录器 (输出到 Visual Studio 的输出窗口)
            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("https://localhost:8080") // Allow your frontend origin
                          .AllowAnyHeader()                    // Allow any headers
                          .AllowAnyMethod()                    // Allow any HTTP methods (GET, POST, etc.)
                          .AllowCredentials();                 // Allow credentials (optional, if needed)
                });
            });


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
            // 启用 CORS 中间件
            app.UseCors("AllowFrontend");
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
