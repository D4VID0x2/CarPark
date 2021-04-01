using System;
using System.Security.Cryptography;

namespace VozovyPark
{

    public class Uzivatel
    {
        public int Uid { get; private set; }
        public string Email { get; private set; }
        public string Jmeno { get; private set; }
        public string Prijmeni { get; private set; }
        public DateTime PosledniPrihlaseni { get; private set; }
        public bool JeAdmin { get; private set; }
        public bool NutnaZmenaHesla { get; private set; }

        private string hash;

        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 20;
        private const int ITERATIONS = 100000;

        public Uzivatel (string email, string jmeno, string prijmeni, string hash, bool jeAdmin, bool nutnaZmenaHesla = true)
        {
            Uid = UID.newUID<Uzivatel>();
            this.Email = email;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
            this.PosledniPrihlaseni = new DateTime(0);
            this.hash = hash;
            this.JeAdmin = jeAdmin;
            this.NutnaZmenaHesla = nutnaZmenaHesla;
        }

        /// <summary>
        /// Vytvori hash z hesla
        /// </summary>
        /// <param name="password">heslo</param>
        /// <returns>vrati base64 z hashe hesla a soli</returns>
        public static string Hash(string password)
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
        /// Zkontroluje jestli je heslo spravne (Porovna oba hashe)
        /// </summary>
        /// <param name="password">hash hesla k overeni</param>
        /// <returns>Vrati true pokud je heslo spravne</returns>
        public bool OverHeslo(string password)
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

        public void ZmenitHeslo (string novyHash)
        {
            this.hash = novyHash;
        }
    }
}
