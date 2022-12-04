using EFCore7Demo.Data;
using EFCore7Demo.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace EFCore7Demo {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<ChangeLogInterceptor>();
            builder.Services.AddDbContextFactory<SqlDbContext>((sp, options) => {
                var changeInterceptor = sp.GetService<ChangeLogInterceptor>()!;
                options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionStrings:EFCore7Demo"))
                    .AddInterceptors(changeInterceptor);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
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