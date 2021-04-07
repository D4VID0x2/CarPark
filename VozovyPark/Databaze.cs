using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VozovyPark
{

    [DataContract]
    public class Databaze
    {
        private const string jmenoSouboru = "databaze.xml";

        [DataMember(Name = "uzivatele")]
        private List<Uzivatel> uzivatele = new List<Uzivatel>();
        [DataMember(Name = "auta")]
        private List<Auto> auta = new List<Auto>();
        [DataMember(Name = "rezervace")]
        private List<Rezervace> rezervace = new List<Rezervace>();

        [DataMember(Name = "uid")]
        private UID uid;

        public void Test ()
        {
            Console.WriteLine(uzivatele.Count);
        }

        public Databaze()
        {
            uid = new UID();

            //TODO: remove
            uzivatele.Add(new Uzivatel("admin", "admin", "admin", "GV5oi3Tqi6jIqWedll18JU+Tqg78XAKRMKBmvCoVM2KgcBu8", true, false));
            uzivatele.Add(new Uzivatel("jan@novak.cz", "Jan", "Novák", "s1XjDjLdvqWrI8QzePYY2THm6ltay3umV9eef1JB2HUjWZc7", false, true));

            auta.Add(new Auto("Škoda", "Octavia", TypAuta.Osobni, 4.5));
            auta.Add(new Auto("Neco", "neco", TypAuta.Nakladni, 5.1));
        }

        public List<Rezervace> VsechnyRezervace(int uzivatel)
        {
            return rezervace.FindAll(r => r.Uzivatel == uzivatel);
        }
        public List<Rezervace> VsechnyRezervacePodleAuta(int auto)
        {
            return rezervace.FindAll(r => r.Auto == auto);
        }


        public bool JeEmailUnikatni(string email)
        {
            return uzivatele.Where(u => u.Email == email).FirstOrDefault() == null;
        }


        public Uzivatel Prihlaseni(string email, string hash)
        {
            Uzivatel uzivatel = uzivatele.Where(u => u.Email == email).FirstOrDefault();
            if (uzivatel != null)
            {
                if (uzivatel.OverHeslo(hash))
                {
                    return uzivatel;
                }
            }

            return null;
        }


        public bool PridatRezervaci(int uzivatel, int auto, DateTime od, DateTime @do)
        {

            rezervace.Add(new Rezervace(uzivatel, auto, od, @do));

            return true;
        }


        public List<Auto> VolnaAuta(DateTime od, DateTime @do)
        {
            List<Auto> volnaAuta = new List<Auto>();
            foreach (Auto auto in auta)
            {
                bool jeVolne = true;
                foreach (Rezervace r in VsechnyRezervacePodleAuta(auto.Uid))
                {
                    if (r.Od < od && r.Do > od && r.Do < @do) // beginning overlaps
                    {
                        jeVolne = false;
                        break;
                    }
                    if (r.Od > od && r.Od < @do && r.Do > od && r.Do < @do) // middle overlaps
                    {
                        jeVolne = false;
                        break;
                    }
                    if (r.Od < od && r.Od > @do && r.Do > @do) // end overlaps
                    {
                        jeVolne = false;
                        break;
                    }
                }
                if (jeVolne)
                {
                    volnaAuta.Add(auto);
                }
            }

            return volnaAuta;
        }

        public void PridatAuto(string znacka, string model, TypAuta typ, double spotreba)
        {
            auta.Add(new Auto(znacka, model, typ, spotreba));
        }






        public bool UlozDatabazi()
        {

            DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(Databaze));

            using (StreamWriter sw = new StreamWriter(File.Open(jmenoSouboru, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                using (XmlWriter xw = XmlWriter.Create(sw))
                {
                    xmlSerializer.WriteObject(xw, this);
                }
            }

            return true;
        }

        public static Databaze NactiDatabazi()
        {

            DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(Databaze));

            using (StreamReader sr = new StreamReader(File.Open(jmenoSouboru, FileMode.Open, FileAccess.Read)))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    return (Databaze) xmlSerializer.ReadObject(xr);
                }
            }

            return null;
        }
    }
}
