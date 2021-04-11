using System;
using System.Collections.Generic;
using System.Linq;

namespace VozovyPark
{
    public class Program
    {

        private const string napoveda = "Použijte jeden z následujících příkazů:\n" +
                                        "  seznam rezervaci\n" +
                                        "  pridat rezervaci\n" +
                                        "  odebrat rezervaci\n" +
                                        "  zmenit heslo\n" +
                                        "  odhlasit se | exit | konec";

        private const string adminNapoveda = "Použijte jeden z následujících příkazů:\n" +
                                             "  pridat uzivatele\n" +
                                             "  odebrat uzivatele\n" +
                                             "  seznam uzivatelu\n" +
                                             "  pridat auto\n" +
                                             "  odebrat auto\n" +
                                             "  seznam aut\n" +
                                             "  pridat servisni ukon\n" +
                                             "  pridat rezervaci\n" +
                                             "  odebrat rezervaci\n" +
                                             "  seznam rezervaci podle uzivatele\n" +
                                             "  seznam rezervaci podle auta\n" +
                                             "  zmenit heslo\n" +
                                             "  odhlasit se | exit | konec";


        private static Uzivatel uzivatel = null;

        private static Databaze databaze;

        public static void Main(string[] args)
        {

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);


            databaze = Databaze.NactiDatabazi();
            Databaze.instance = databaze;


            Prihlaseni();

            bool exit = false;
        loop: while (!exit)
            {

                if (uzivatel.JeAdmin)
                {

                    Console.Write("# ");
                    string cmd = Console.ReadLine().Trim().ToLower();

                    if (cmd.Length < 1) continue;
                    switch (cmd)
                    {
                        case "zmenit heslo":
                            ZmenaHesla();
                            break;

                        case "pridat rezervaci":
                            {
                                DateTime od = NactiDatum("Od: ");
                                DateTime @do = NactiDatum("Do: ");
                                if (@do < od)
                                {
                                    Console.WriteLine("Chyba: Zadaná hodnota Do nesmí být větší než hodnota Od");
                                    goto loop;
                                }

                                List<Auto> volnaAuta = databaze.VolnaAuta(od, @do);

                                Console.WriteLine("Volná auta:");
                                foreach (Auto auto in volnaAuta)
                                {
                                    Console.WriteLine(auto.ToStringLong());
                                }

                                int idAuta = NactiCeleCislo("Zadejte ID auta: ");

                                if (volnaAuta.Where(a => a.Uid == idAuta).FirstOrDefault() == null)
                                {
                                    Console.WriteLine("Chyba: Zadané ID auta neexistuje nebo není dostupné");
                                    goto loop;
                                }



                                List<Uzivatel> uzivatele = databaze.VsichniUzivatele();

                                bool vypsatUzivatele = NactiAnoNe("Vypsat všechny uživatele");

                                if (vypsatUzivatele)
                                {
                                    foreach (Uzivatel uzivatel in uzivatele)
                                    {
                                        Console.WriteLine(uzivatel);
                                    }
                                }

                                int idUzivatele = NactiCeleCislo("Zadejte ID uživatele: ");

                                if (uzivatele.Where(u => u.Uid == idUzivatele).FirstOrDefault() == null)
                                {
                                    Console.WriteLine("Chyba: Zadané ID uzivatle neexistuje");
                                    goto loop;
                                }

                                databaze.PridatRezervaci(idUzivatele, idAuta, od, @do);

                                Console.WriteLine("Rezervace úspěšně přidána");

                                break;
                            }

                        case "pridat uzivatele":
                            {

                                Console.Write("Zadejte email: ");
                                string email = Console.ReadLine();
                                if (!databaze.JeEmailUnikatni(email))
                                {
                                    Console.WriteLine("Chyba: email už v systému existuje");
                                    goto loop;
                                }
                                Console.Write("Zadejte jméno: ");
                                string jmeno = Console.ReadLine();
                                Console.Write("Zadejte příjmení: ");
                                string prijmeni = Console.ReadLine();
                                bool admin = NactiAnoNe("Je admin");

                                string heslo = "";
                                while (true)
                                {
                                    Console.Write("Zadejte uživatelovo heslo: ");
                                    heslo = NactiHeslo();
                                    Console.WriteLine();
                                    if (heslo == "") continue;

                                    Console.Write("Potvrďte uživatelovo heslo: ");
                                    string potvrzeniHesla = NactiHeslo();
                                    Console.WriteLine();

                                    if (heslo != potvrzeniHesla)
                                    {
                                        Console.WriteLine("Hesla nejsou stejná");
                                        continue;
                                    }

                                    break;
                                }

                                databaze.PridatUzivatele(email, jmeno, prijmeni, admin, Uzivatel.Hash(heslo));

                                Console.WriteLine("Uživatel byl úspěšně přidán");

                                break;
                            }

                        case "vynutit zmenu hesla":
                            {
                                int idUzivatele = NactiCeleCislo("Zadejte ID uživatele: ");

                                if (!databaze.VynutitZmenuHesla(idUzivatele))
                                {
                                    Console.WriteLine("Chyba: Zadané ID uzivatle neexistuje");
                                    goto loop;
                                }
                                Console.WriteLine("Uživatel bude při dalším přihlášení vynucen si změnit heslo");
                                break;
                            }

                        case "odebrat uzivatele":
                            {
                                int idUzivatele = NactiCeleCislo("Zadejte ID uživatele: ");

                                if (!databaze.OdebratUzivatele(idUzivatele))
                                {
                                    Console.WriteLine("Chyba: Zadané ID uživatle neexistuje nebo se nepodařilo uživatele odstraňit, protože má budoucí rezervace");
                                    goto loop;
                                }
                                Console.WriteLine("Uživatel byl úspěšně odebrán");
                                break;
                            }
                        case "odebrat auto":
                            {

                                int idAuta = NactiCeleCislo("Zadejte ID auta: ");

                                if (!databaze.OdebratAuto(idAuta))
                                {
                                    Console.WriteLine("Chyba: Zadané ID auta neexistuje nebo se nepodařilo auto odstraňit, protože má budoucí rezervace");
                                    goto loop;
                                }
                                Console.WriteLine("Auto bylo úspěšně odebráno");
                                break;
                            }

                        case "seznam rezervaci podle uzivatele":
                            {

                                int idUzivatele = NactiCeleCislo("Zadejte ID uživatele: ");

                                bool vypsatStare = NactiAnoNe("Vypsat i staré rezervace");

                                foreach (Rezervace rezervace in databaze.VsechnyRezervace(idUzivatele, vypsatStare))
                                {
                                    Console.WriteLine(rezervace);
                                }
                                break;
                            }

                        case "seznam rezervaci podle auta":
                            {
                                int idAuta = NactiCeleCislo("Zadejte ID auta: ");

                                bool vypsatStare = NactiAnoNe("Vypsat i staré rezervace");

                                foreach (Rezervace rezervace in databaze.VsechnyRezervacePodleAuta(idAuta, vypsatStare))
                                {
                                    Console.WriteLine(rezervace);
                                }
                                break;

                            }

                        case "seznam uzivatelu":
                            foreach (Uzivatel uzivatel in databaze.VsichniUzivatele())
                            {
                                Console.WriteLine(uzivatel.ToStringLong());
                            }
                            break;

                        case "seznam aut":
                            foreach (Auto auto in databaze.VsechnaAuta())
                            {
                                Console.WriteLine(auto.ToStringLong());
                                Console.WriteLine("  Servisní úkony:");
                                foreach (ServisniUkon ukon in auto.servisniUkony)
                                {
                                    Console.WriteLine(ukon);
                                }

                            }
                            break;

                        case "pridat servisni ukon":
                            {
                                int idAuta = NactiCeleCislo("Zadejte ID auta: ");

                                DateTime od = NactiDatum("Od: ");
                                DateTime @do = NactiDatum("Do: ");
                                if (@do < od)
                                {
                                    Console.WriteLine("Chyba: Zadaná hodnota Do nesmí být větší než hodnota Od");
                                    goto loop;
                                }

                                decimal cena = NactiDecimal("Zadejte cenu: ");

                                Console.Write("Zadejte popis: ");
                                string popis = Console.ReadLine();

                                int cisloFaktury = NactiCeleCislo("Zadejte číslo faktury: ");

                                if (!databaze.PridatServisniUkon(idAuta, od, @do, cena, popis, cisloFaktury))
                                {
                                    Console.WriteLine("Chyba: Servisní úkon se nepodařilo přidat, zadané auto neexistuje anebo je v tu dobu zarezervované");
                                    goto loop;
                                }
                                Console.WriteLine("Servisní úkon úspěšně přidán");
                                break;
                            }


                        case "pridat auto":
                            Console.Write("Zadejte značku: ");
                            string znacka = Console.ReadLine();
                            Console.Write("Zadejte model: ");
                            string model = Console.ReadLine();
                            Console.Write("Typy aut:\n  1: osobní\n  2: nákladní");
                            int zadanyTyp = NactiCeleCislo("Zadejte typ auta: ");

                            if (!Enum.IsDefined(typeof(TypAuta), zadanyTyp))
                            {
                                Console.WriteLine("Chyba: Zadaný typ auta neexistuje");
                                goto loop;
                            }
                            TypAuta typ = (TypAuta)zadanyTyp;

                            decimal spotreba = NactiDecimal("Zadejte spotřebu na 100km: ");

                            databaze.PridatAuto(znacka, model, typ, spotreba);

                            Console.WriteLine("Auto úspěšně přidáno");

                            break;

                        case "odebrat rezervaci":

                            int idRezervace = NactiCeleCislo("Zadejte ID rezervace: ");

                            if (!databaze.OdebratRezervaci(idRezervace, uzivatel.Uid))
                            {
                                Console.WriteLine("Chyba: Zadané ID rezervace neexistuje nebo je pro starou rezervaci");
                            }
                            else
                            {
                                Console.WriteLine("Rezervace úspěšně odebrána");
                            }

                            break;

                        case "napoveda":
                        case "?":
                            Console.WriteLine(adminNapoveda);
                            break;

                        case "odhlasit se":
                        case "exit":
                        case "konec":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Neznámý příkaz: {0}", cmd);
                            Console.WriteLine(adminNapoveda);
                            break;
                    }

                }



                else
                {
                    Console.Write("$ ");
                    string cmd = Console.ReadLine().Trim().ToLower();

                    if (cmd.Length < 1) continue;

                    switch (cmd)
                    {
                        case "seznam rezervaci":

                            bool vypsatStare = NactiAnoNe("Vypsat i staré rezervace");

                            foreach (Rezervace rezervace in databaze.VsechnyRezervace(uzivatel.Uid, vypsatStare))
                            {
                                Console.WriteLine(rezervace);
                            }
                            break;

                        case "pridat rezervaci":

                            DateTime od = NactiDatum("Od: ");
                            DateTime @do = NactiDatum("Do: ");
                            if (@do < od)
                            {
                                Console.WriteLine("Chyba: Zadaná hodnota Do nesmí být větší než hodnota Od");
                                goto loop;
                            }

                            List<Auto> volnaAuta = databaze.VolnaAuta(od, @do);

                            Console.WriteLine("Volná auta:");
                            foreach (Auto auto in volnaAuta)
                            {
                                Console.WriteLine(auto.ToStringLong());
                            }

                            int idAuta = NactiCeleCislo("Zadejte ID auta: ");

                            if (volnaAuta.Where(a => a.Uid == idAuta).FirstOrDefault() == null)
                            {
                                Console.WriteLine("Chyba: Zadané ID auta neexistuje nebo není dostupné");
                                goto loop;
                            }

                            databaze.PridatRezervaci(uzivatel.Uid, idAuta, od, @do);

                            Console.WriteLine("Rezervace úspěšně přidána");

                            break;

                        case "odebrat rezervaci":

                            int idRezervace = NactiCeleCislo("Zadejte ID rezervace: ");

                            if (!databaze.OdebratRezervaci(idRezervace, uzivatel.Uid))
                            {
                                Console.WriteLine("Chyba: Zadané ID rezervace neexistuje nebo je pro starou rezervaci");
                            }
                            else
                            {
                                Console.WriteLine("Rezervace úspěšně odebrána");
                            }

                            break;

                        case "zmenit heslo":
                            ZmenaHesla();
                            break;

                        case "napoveda":
                        case "?":
                            Console.WriteLine(napoveda);
                            break;

                        case "odhlasit se":
                        case "exit":
                        case "konec":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Neznámý příkaz: {0}", cmd);
                            Console.WriteLine(napoveda);
                            break;
                    }

                }
            }

            databaze.UlozDatabazi();
        }


        private static void Prihlaseni()
        {

            while (true)
            {
                Console.Write("Email: ");
                string email = Console.ReadLine();

                Console.Write("Heslo: ");
                string hash = NactiHeslo();
                Console.WriteLine();

                uzivatel = databaze.Prihlaseni(email, hash);

                if (uzivatel == null)
                {
                    Console.WriteLine("Neplatné přihlašovací údaje");
                    continue;
                }

                if (uzivatel.NutnaZmenaHesla)
                {
                    Console.WriteLine("Nutná změna hesla");
                    ZmenaHesla();
                }

                Console.WriteLine("Přihlášen jako {0} {1}", uzivatel.Jmeno, uzivatel.Prijmeni);
                Console.WriteLine("Poslední přihlášení: {0}", uzivatel.PosledniPrihlaseni);

                uzivatel.AktualizovatDatumPrihlaseni(DateTime.Now);

                return;
            }
        }


        private static bool ZmenaHesla()
        {
            string noveHeslo = "";
            while (true)
            {
                Console.Write("Zadejte staré heslo: ");
                string stareHeslo = NactiHeslo();
                Console.WriteLine();
                if (!uzivatel.OverHeslo(stareHeslo))
                {
                    Console.WriteLine("Staré heslo není správné");
                    continue;
                }
                break;
            }
            while (true)
            {

                Console.Write("Zadejte nové heslo: ");
                noveHeslo = NactiHeslo();
                Console.WriteLine();
                if (noveHeslo == "") continue;

                Console.Write("Potvrďte nové heslo: ");
                string potvrzeniHesla = NactiHeslo();
                Console.WriteLine();


                if (noveHeslo != potvrzeniHesla)
                {
                    Console.WriteLine("Nová hesla nejsou stejná");
                    continue;
                }

                break;
            }

            if (noveHeslo != "")
            {
                uzivatel.ZmenitHeslo(noveHeslo);

                Console.WriteLine("Heslo změněno");
            }
            return true;
        }


        public static string NactiHeslo(string prompt = "")
        {
            if (prompt != "")
            {
                Console.Write(prompt);
            }

            string password = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Remove(password.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000')
                {
                    password += i.KeyChar;
                    Console.Write("*");
                }
            }
            return password;
        }


        public static DateTime NactiDatum(string vyzva, bool povolitStarsiDatum = false)
        {
            DateTime dt;
            while (true)
            {
                Console.Write(vyzva);
                if (!DateTime.TryParse(Console.ReadLine(), out dt))
                {
                    Console.WriteLine("Zadaná hodnota není typu DateTime");
                    continue;
                }
                if (dt < DateTime.Now && !povolitStarsiDatum)
                {
                    Console.WriteLine("Zadaná hodnota nesmí být v minulosti");
                    continue;
                }
                break;
            }

            return dt;
        }

        public static int NactiCeleCislo(string vyzva)
        {
            int cislo;
            while (true)
            {
                Console.Write(vyzva);
                if (!int.TryParse(Console.ReadLine(), out cislo))
                {
                    Console.WriteLine("Zadaná hodnota musí být číslo");
                    continue;
                }
                break;
            }
            return cislo;
        }

        public static decimal NactiDecimal(string vyzva)
        {
            decimal cislo;
            while (true)
            {
                Console.Write(vyzva);
                if (!decimal.TryParse(Console.ReadLine(), out cislo))
                {
                    Console.WriteLine("Zadaná hodnota musí být číslo");
                    continue;
                }
                break;
            }
            return cislo;
        }

        public static bool NactiAnoNe(string vyzva)
        {
            Console.Write("{0}? (a|N)", vyzva);
            char ch = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();
            return ch == 'a';
        }

        protected static void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            databaze.UlozDatabazi();
        }
    }
}
