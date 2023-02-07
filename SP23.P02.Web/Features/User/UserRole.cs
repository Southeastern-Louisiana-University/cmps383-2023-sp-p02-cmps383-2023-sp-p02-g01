using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

    public class UserRole : IdentityUserRole<int>
    {
        public Role? Role { get; set; }
        public User? User { get; set; }

    }
