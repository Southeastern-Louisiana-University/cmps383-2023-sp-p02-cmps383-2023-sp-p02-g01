using Microsoft.AspNetCore.Identity;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.Users;

namespace SP23.P02.Web.Features.Authorization
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual Role? Role { get; set; }
        public virtual User? User { get; set; }
    }

}
