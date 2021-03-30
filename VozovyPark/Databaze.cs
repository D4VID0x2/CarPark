using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VozovyPark
{
    public static class Databaze
    {
        private static List<Uzivatel> uzivatele = new List<Uzivatel>();
        private static List<Auto> auta = new List<Auto>();
        private static List<Rezervace> rezervace = new List<Rezervace>();


        public static List<Rezervace> VsechnyRezervace(Uzivatel uzivatel)
        {
            return rezervace.FindAll(r => r.Uzivatel == uzivatel);
        }
        public static List<Rezervace> VsechnyRezervace(Auto auto)
        {
            return rezervace.FindAll(r => r.Auto == auto);
        }


        public static bool JeEmailUnikatni (string email)
        {
            foreach (Uzivatel uzivatel in uzivatele)
            {
                if (uzivatel.Email == email) return false;
            }
            return true;
        }


        public static Uzivatel Prihlaseni(string email, string hash)
        {
            Uzivatel uzivatel = uzivatele.Where(uzivatel => uzivatel.email == email).FirstOrDefault();
            if (uzivatel != null)
            {
                if (uzivatel.OverHeslo(hash))
                {
                    return uzivatel;
                } 
            }

            return null;
        }

        
        public static bool PridatRezervaci(Uzivatel uzivatel, int autoId, DateTime od, DateTime @do)
        {
            // TODO: check if the auto is free at this time
            
            //Auto auto = autos[autoId];

            //reservations.Add(new Rezervace(uzivatel, auto, od, @do));


            return true;
        }


        public static List<Auto> VolnaAuta (DateTime od, DateTime @do)
        {
            List<Auto> volnaAuta = new List<Auto>();
            foreach (Auto auto in auta)
            {
                bool jeVolne = true;
                foreach (Rezervace r in VsechnyRezervace(auto))
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
    }
}
