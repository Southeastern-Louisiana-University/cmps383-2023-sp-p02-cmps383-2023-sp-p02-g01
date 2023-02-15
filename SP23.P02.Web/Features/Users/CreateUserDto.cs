using System.ComponentModel.DataAnnotations;

namespace SP23.P02.Web.Features.Users
{
    public class CreateUserDto
    {


        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required, MinLength(1)]
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
