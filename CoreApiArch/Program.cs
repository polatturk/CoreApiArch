using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Repository;
using Business.Interfaces;
using Business.Services;
using Business.MapProfile;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiArch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddDbContext<APIContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MapProfile));


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IAuthorService,AuthorService>();
            builder.Services.AddScoped<IBookService,BookService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<ICategoryService,CategoryService>();


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
