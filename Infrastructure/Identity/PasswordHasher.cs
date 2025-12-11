
using Application.Abstractions.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{

    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            var passwordHasher = new PasswordHasher<string>();
            string hashedPassword = passwordHasher.HashPassword(null, password);
            return hashedPassword;
        }

        public bool Verify(string password, string hash)
        {
            var passwordHasher = new PasswordHasher<string>();
            return passwordHasher.VerifyHashedPassword(null!, hash, password) == PasswordVerificationResult.Success;
        }
    }
}