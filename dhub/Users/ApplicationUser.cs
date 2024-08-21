using Microsoft.AspNetCore.Identity;

namespace dhub.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // Username, email, and passwordhash are default properties (among others)
        public string FullName { get; set; }
    }
}
