using dhub.Users;

namespace dhub.Data
{
    public class ApplicationDbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
             
                context.Users.Add(new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = "hashed_password",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

                context.SaveChanges();
            }
        }
    }
}
