
using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<Dsw2026Ej15DbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<IPersistence, PersistenceEf>();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Dsw2026Ej15DbContext>();

                    context.SeedworkSpecialities("specialities.json");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error ejecutando el Data Seeding: {ex.Message}");
                }
            }

            if (app.Environment.IsDevelopment()){
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
