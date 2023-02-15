using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.TrainStations;
using SP23.P02.Web.Features.Users;

namespace SP23.P02.Web.Data;

public static class SeedHelper
{
    public static async Task MigrateAndSeed(IServiceProvider serviceProvider)
    {

        var dataContext = serviceProvider.GetRequiredService<DataContext>();

        await dataContext.Database.MigrateAsync();

        await AddRoles(serviceProvider);
        await AddUsers(serviceProvider);

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
    }
    public static async Task AddRoles(IServiceProvider serviceProvider)
    {

        var roles = serviceProvider.GetRequiredService<RoleManager<Role>>();
        if (!roles.Roles.Any())
        {
            await roles.CreateAsync(new Role
            {
                Name = Role.Admin,
            });
            await roles.CreateAsync(new Role
            {
                Name = Role.User,
            });
        }
    }

    public static async Task AddUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        const string password = "Password123!";
        if (!await userManager.Users.AnyAsync())
        {

            var adminRole = new User
            {
                UserName = "galkadi",
            };
            await userManager.CreateAsync(adminRole, password);
            await userManager.AddToRoleAsync(adminRole, Role.Admin);
        }

        var userBob = new User
        {
            UserName = "bob"
        };
        await userManager.CreateAsync(userBob, password);
        await userManager.AddToRoleAsync(userBob, Role.User);

        var userSue = new User
        {
            UserName = "sue"
        };
        await userManager.CreateAsync(userSue, password);
        await userManager.AddToRoleAsync(userSue, Role.User);
    }
}