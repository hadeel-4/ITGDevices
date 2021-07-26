using ITGDevices.Data;
using ITGDevices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGDevices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);


            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DeviceContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }


    public static class DbInitializer
    {
        public static void Initialize(DeviceContext context)
        {
            context.Database.EnsureCreated();


            if (context.users.Any())
            {
                return;
            }

            var users = new User[]
            {
         
            new User{FirstName="Admin",LastName="Admin",Password="12",username="Admin",Email="ha986562@gmail.com"}
            };

            foreach (User s in users)
            {
                context.users.Add(s);
            }
            context.SaveChanges();

            var roles = new Role[]
            {
           new Role{rolename="Employee"},
           new Role{rolename="OperationsManager"},
            new Role{rolename="Admin"}
            };
            foreach (Role c in roles)
            {
                context.roles.Add(c);
            }
            context.SaveChanges();
            var ur = new UserRole { userID = 1, roleID = 3 };
            context.userRoles.Add(ur);
            context.SaveChanges();


        }
    }
}
