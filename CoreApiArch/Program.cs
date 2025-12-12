using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Repository;
using Business.Interfaces;
using Business.Services;
using Business.MapProfile;
using Microsoft.Extensions.DependencyInjection;
using Core.Entities;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.RateLimiting;


namespace CoreApiArch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Log yapılandırması
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog(); // Serilog'u kullanmak için ekleyin

            //RateLimiting Yapılandırması
            builder.Services.AddRateLimiter(options =>
            {

                options.AddFixedWindowLimiter("RateLimiter", options =>
                {
                    options.PermitLimit = 5;
                    options.Window = TimeSpan.FromSeconds(10);
                    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2; // kuyruk yapısı burası 2 istek bekletilebilir
                });


                options.AddFixedWindowLimiter("RateLimiter2", options =>
                {
                    options.PermitLimit = 5;
                    options.Window = TimeSpan.FromSeconds(10);
                });

            });

            //Jwt Yapılandırması
            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddDbContext<APIContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                // 1. Swagger Belge Tanımı
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CoreApiArch",
                    Version = "v1"
                });

                // 2. Güvenlik Tanımı (JWT/Bearer Şeması)
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter a valid token", 
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer" 
                });

                // 3. Güvenlik Gereksiniminin Eklenmesi (Tüm Endpoint'ler için bu şemayı zorunlu kılmak)
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });


            builder.Services.AddAutoMapper(typeof(MapProfile));

            //Servis Implamantasyonları
            //AddTransient: Her istekte yeni bir instance oluşturur.
            //AddScoped: Her istekte bir instance oluşturur ve isteğin sonunda yok eder.
            builder.Services.AddScoped<IGenericRepository<Author>, GenericRepository<Author>>();
            builder.Services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();
            builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // ÖNERİLEN KOD (Dinamik ve Tek Satır)
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
