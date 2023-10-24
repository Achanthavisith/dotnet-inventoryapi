using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace dotnet_inventoryapi.Models.utils
{
    public static class PasswordHasher
    {
        // Iteration count for PBKDF2 (adjust based on your security requirements)
        private const int IterationCount = 10000;

        public static string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2 with the random salt
            string hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: IterationCount,
                    numBytesRequested: 32 // 256 bits
                )
            );

            // Combine the salt and hashed password for storage
            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Split the stored value into salt and hashed password
            string[] parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                // Invalid format
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            string storedHashedPassword = parts[1];

            // Hash the entered password using the stored salt
            string hashedPasswordToCheck = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: IterationCount,
                    numBytesRequested: 32 // 256 bits
                )
            );

            // Compare the stored hashed password with the newly hashed password
            return string.Equals(storedHashedPassword, hashedPasswordToCheck);
        }
    }

}
