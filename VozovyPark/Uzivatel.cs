using System;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VozovyPark
{

    [DataContract]
    public class Uzivatel
    {
        [DataMember(Name="id")]
        public int Uid { get; private set; }
        [DataMember(Name="email")]
        public string Email { get; private set; }
        [DataMember(Name="jmeno")]
        public string Jmeno { get; private set; }
        [DataMember(Name="prijmeni")]
        public string Prijmeni { get; private set; }
        [DataMember(Name="prihlaseni")]
        public DateTime PosledniPrihlaseni { get; private set; }
        [DataMember(Name="admin")]
        public bool JeAdmin { get; private set; }
        [DataMember(Name="zmenahesla")]
        public bool NutnaZmenaHesla { get; private set; }

        [DataMember(Name="hash")]
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
        public bool OverHeslo(string heslo)
        {
            // Get hash bytes
            byte[] hashBytes = Convert.FromBase64String(this.hash);

            // Get salt
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(heslo, salt, ITERATIONS);
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

        public void ZmenitHeslo (string noveHeslo)
        {
            this.hash = Hash(noveHeslo);
            this.NutnaZmenaHesla= false;
        }


        public void AktualizovatDatumPrihlaseni(DateTime now)
        {
            PosledniPrihlaseni = now;
        }

        public void VynutitZmenuHesla ()
        {
            NutnaZmenaHesla = true;
        }


        public override string ToString()
        {
            return string.Format("{0} {1}", Jmeno, Prijmeni);
        }
        public string ToStringLong()
        {
            return string.Format("#{0}:\n  Email: {1}\n  Jm??no: {2}\n  P????jmen??: {3}\n  Je admin?: {4}\n  Posledn?? p??ihl????en??: {5}", Uid, Email, Jmeno, Prijmeni, JeAdmin ? "Ano" : "Ne", PosledniPrihlaseni);
        }
    }
}
