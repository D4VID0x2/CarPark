using System;
using System.Security.Cryptography;

namespace CarPark
{

    public class User
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime LastLogin { get; private set; }

        private string hash;

        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 20;
        private const int ITERATIONS = 100000;

        /// <summary>
        /// Vytvori hash z hesla
        /// </summary>
        /// <param name="password">heslo</param>
        /// <returns>vrati base64 z hashe hesla a soli</returns>
        public string Hash(string password)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALT_SIZE]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

            // Combine salt and hash
            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            return Convert.ToBase64String(hashBytes);
        }


        /// <summary>
        /// Zkontroluje jestli je heslo spravne
        /// </summary>
        /// <param name="password">heslo</param>
        /// <returns>Vrati true pokud je heslo spravne</returns>
        public bool Verify(string password)
        {
            // Get hash bytes
            byte[] hashBytes = Convert.FromBase64String(this.hash);

            // Get salt
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

            // Get result
            for (int i = 0; i < HASH_SIZE; i++)
            {
                if (hashBytes[i + SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
