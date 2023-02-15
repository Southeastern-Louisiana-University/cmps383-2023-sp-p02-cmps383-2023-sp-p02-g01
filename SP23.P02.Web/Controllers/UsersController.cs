using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace SP23.P02.Web.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            if (dto.UserName == null)
            {
                return BadRequest();
            }
            // Validate that at least one role is provided
            if (!dto.Roles.Any())
            {
                return BadRequest("At least one role must be provided.");
            }

            // Validate that the provided roles exist
            var allRoles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
            foreach (var role in dto.Roles)
            {
                if (!allRoles.Contains(role))
                {
                    return BadRequest($"The role '{role}' does not exist.");
                }
            }

            // Validate that the provided username is unique
            var existingUser = await userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
            {
                return BadRequest("The provided username is already taken or not unique.");
            }

            var newUser = new User
            {
                UserName = dto.UserName,
            };
            //validate password created
            var createResult = await userManager.CreateAsync(newUser, dto.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest();
            }

            var roleResult = await userManager.AddToRolesAsync(newUser, dto.Roles);

            if (!roleResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok(new UserDto
            {
                Id = newUser.Id,
                Roles = dto.Roles,
                UserName = newUser.UserName,
            });


        }
    }
}



