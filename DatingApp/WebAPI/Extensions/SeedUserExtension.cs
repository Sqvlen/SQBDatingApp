using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;

namespace WebAPI.Extensions
{
	public class SeedUserExtension
    {
        public static async void SeedUsers(ServiceProvider serviceProvider)
        {
			try
			{
				var dataBaseContext = serviceProvider.GetService<DataBaseContext>();
				var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
				var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
				await dataBaseContext.Database.MigrateAsync();
				await Seed.ClearConnections(dataBaseContext);
				await Seed.SeedUsers(userManager, roleManager);
			}
			catch (Exception exception)
			{
				var logger = serviceProvider.GetService<ILogger>();
				logger.LogError(exception, "An error occured during migration");
			}
        }
    }
}
