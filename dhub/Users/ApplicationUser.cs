using Microsoft.AspNetCore.Identity;

namespace dhub.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; } = string.Empty;
    }
}
