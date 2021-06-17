using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MockDoor.Data.Migrations
{
    public static class StartupMigration
    {
        public static void ApplyMigrations<TContext>(this IApplicationBuilder app) where TContext : DbContext
        {
            // apply migratiosn if not applied (create if not exists too)
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            try
            {
                Console.Write("Upgrading database...");
                var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();

                Console.WriteLine("Complete.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error trying to seed data:\n" + ex.Message);
            }
        }
    }
}
