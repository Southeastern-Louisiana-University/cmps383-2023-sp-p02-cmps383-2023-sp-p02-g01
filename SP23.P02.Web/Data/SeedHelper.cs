using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Features.TrainStations;
using SP23.P02.Web.Features.User;

namespace SP23.P02.Web.Data;

public static class SeedHelper
{
    public static async Task MigrateAndSeed(DataContext dataContext, IServiceProvider services)
    {
        await dataContext.Database.MigrateAsync();

        var trainStations = dataContext.Set<TrainStation>();

        if (!await trainStations.AnyAsync())
        {
            for (int i = 0; i < 3; i++)
            {
                dataContext.Set<TrainStation>()
                    .Add(new TrainStation
                    {
                        Name = "Hammond",
                        Address = "1234 Place st"
                    });
            }

            await dataContext.SaveChangesAsync();
        }

        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        // check to see if Admin exists, if not, create
        if (!await roleManager.RoleExistsAsync(RoleNames.Admin))
        {
            await roleManager.CreateAsync(new Role()
            {
                Name = RoleNames.Admin
            });
        }
        // check to see if User exists, if not, create
        if (!await roleManager.RoleExistsAsync(RoleNames.User))
        {
            await roleManager.CreateAsync(new Role()
            {
                Name = RoleNames.User
            });
        }
        var userManager = services.GetRequiredService<UserManager<User>>();

        if (!userManager.Users.Any())
        {
            var adminUser = new User
            {
                UserName = "galkadi",
            };
            await userManager.CreateAsync(adminUser);
            await userManager.AddToRoleAsync(adminUser, RoleNames.Admin);

            var bobUser = new User
            {
                UserName = "bob",
            };
            await userManager.CreateAsync(bobUser);
            await userManager.AddToRoleAsync(bobUser, RoleNames.User);

            var sueUser = new User
            {
                UserName = "sue",
            };
            await userManager.CreateAsync(sueUser);
            await userManager.AddToRoleAsync(sueUser, RoleNames.User);
            // check to see if User exists, if not, create
        }
    }
}