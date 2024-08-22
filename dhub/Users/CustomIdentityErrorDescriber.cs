using Microsoft.AspNetCore.Identity;

namespace dhub.Users
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit() => new IdentityError
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Passwords must contain at least one digit."
        };

        public override IdentityError PasswordRequiresLower() => new IdentityError
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Passwords must contain at least one lowercase digit."
        };

        public override IdentityError PasswordRequiresUpper() => new IdentityError
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Passwords must contain at least one uppercase digit."
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "Passwords must contain at least one non-alphanumeric character."
        };

        public override IdentityError PasswordTooShort(int length) => new IdentityError
        {
            Code = nameof(PasswordTooShort),
            Description = "Passwords must contain at least {length} characters."
        };

    }
}


