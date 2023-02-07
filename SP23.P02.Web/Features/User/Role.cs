using Microsoft.AspNetCore.Identity;
using SP23.P02.Web.Features;

public class Role: IdentityRole<int>
{
    public ICollection<UserRole> Users { get; set; } = new List<UserRole>();
}