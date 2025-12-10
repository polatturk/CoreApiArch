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
            builder.Services.AddSwaggerGen();

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
