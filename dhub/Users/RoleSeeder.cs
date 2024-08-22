using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;

namespace dhub.Users
{
    public class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Owner"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Owner"));
            }
        }
    }
}
