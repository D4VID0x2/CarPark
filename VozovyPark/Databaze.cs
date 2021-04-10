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

        public static Databaze instance;

        [DataMember(Name = "uzivatele")]
        private List<Uzivatel> uzivatele = new List<Uzivatel>();
        [DataMember(Name = "auta")]
        private List<Auto> auta = new List<Auto>();
        [DataMember(Name = "rezervace")]
        private List<Rezervace> rezervace = new List<Rezervace>();

        [DataMember(Name = "uid")]
        public UID uid;


        //public Databaze()
        //{
        //uid = new UID();

        ////TODO: remove
        //uzivatele.Add(new Uzivatel("admin", "admin", "admin", "GV5oi3Tqi6jIqWedll18JU+Tqg78XAKRMKBmvCoVM2KgcBu8", true, false));
        //uzivatele.Add(new Uzivatel("jan@novak.cz", "Jan", "Novák", "s1XjDjLdvqWrI8QzePYY2THm6ltay3umV9eef1JB2HUjWZc7", false, true));

        //auta.Add(new Auto("Škoda", "Octavia", TypAuta.Osobni, 4.5));
        //auta.Add(new Auto("Neco", "neco", TypAuta.Nakladni, 5.1));
        //}


        public Auto Auto(int id)
        {
            return auta.Where(a => a.Uid == id).FirstOrDefault();
        }
        public Uzivatel Uzivatel(int id)
        {
            return uzivatele.Where(u => u.Uid == id).FirstOrDefault();
        }
        public Rezervace Rezervace(int id)
        {
            return rezervace.Where(r => r.Uid == id).FirstOrDefault();
        }

        public List<Rezervace> VsechnyRezervace(int uzivatel, bool vsechny = false)
        {
            return rezervace.FindAll(r => r.Uzivatel == uzivatel && (r.Od > DateTime.Now || vsechny));
        }
        public List<Rezervace> VsechnyRezervacePodleAuta(int auto, bool vsechny = false)
        {
            return rezervace.FindAll(r => r.Auto == auto && (r.Od > DateTime.Now || vsechny));
        }

        public List<Uzivatel> VsichniUzivatele()
        {
            return uzivatele;
        }
        public List<Auto> VsechnaAuta()
        {
            return auta;
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

        public bool OdebratRezervaci(int idRezervace, int idUzivatele)
        {

            Rezervace rez = rezervace.Where(r => r.Uid == idRezervace).FirstOrDefault();

            if (rez == null)
            {
                return false;
            }

            if (rez.Uzivatel == idUzivatele || Uzivatel(idUzivatele).JeAdmin)
            {
                rezervace.Remove(rez);
            }
            else
            {
                return false;
            }


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

        public void PridatUzivatele(string email, string jmeno, string prijmeni, bool admin, string hash)
        {
            uzivatele.Add(new Uzivatel(email, jmeno, prijmeni, hash, admin));
        }

        public bool VynutitZmenuHesla(int idUzivatele)
        {
            Uzivatel uzivatel = uzivatele.Where(u => u.Uid == idUzivatele).FirstOrDefault();
            if (uzivatel == null)
            {
                return false;
            }

            uzivatel.VynutitZmenuHesla();

            return true;
        }

        public bool OdebratUzivatele(int idUzivatele)
        {
            Uzivatel uzivatel = uzivatele.Where(u => u.Uid == idUzivatele).FirstOrDefault();
            if (uzivatel == null)
            {
                return false;
            }

            List<Rezervace> rezervaceUzivatele = VsechnyRezervace(idUzivatele);
            if (rezervaceUzivatele.Count > 0) // pokud tento uzivatel ma budouci rezervace
            {
                return false;
            }

            uzivatele.Remove(uzivatel);

            return true;
        }


        public bool OdebratAuto(int idAuta)
        {
            Auto auto = auta.Where(a => a.Uid == idAuta).FirstOrDefault();
            if (auto == null)
            {
                return false;
            }

            List<Rezervace> rezervaceAuta = VsechnyRezervacePodleAuta(idAuta);
            if (rezervaceAuta.Count > 0) // pokud toto auto ma budouci rezervace
            {
                return false;
            }

            auta.Remove(auto);

            return true;
        }





        public bool UlozDatabazi()
        {

            DataContractSerializer xmlSerializer = new DataContractSerializer(typeof(Databaze));

            using (StreamWriter sw = new StreamWriter(File.Open(jmenoSouboru, FileMode.Create, FileAccess.Write)))
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
                    Databaze db = (Databaze)xmlSerializer.ReadObject(xr);
                    UID.instance = db.uid;
                    return db;
                }
            }

            return null;
        }
    }
}
