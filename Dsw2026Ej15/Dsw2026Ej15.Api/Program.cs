
using Dsw2026Ej15.Data.Persistence;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.UseMiddleware<Dsw2026Ej15.Api.Middlewares.ExceptionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            app.MapControllers();
            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
