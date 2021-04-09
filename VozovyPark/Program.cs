using System;
using System.Collections.Generic;
using System.Linq;

namespace VozovyPark
{
    public class Program
    {

        private const string napoveda = "Použijte jeden z následujících příkazů:\n" +
                                        "seznam rezervaci\n" +
                                        "pridat rezervaci\n" +
                                        "odebrat rezervaci\n" +
                                        "zmenit heslo\n" +
                                        "odhlasit se | exit | konec";

        private const string adminNapoveda = "Použijte jeden z následujících příkazů:\n" +
                                             "pridat uzivatele\n" +
                                             "odebrat uzivatele\n" +
                                             "pridat auto\n" +
                                             "odebrat auto\n" +
                                             "pridat rezervaci\n" +
                                             "odebrat rezervaci\n" +
                                             "seznam rezervaci\n" +
                                             "zmenit heslo\n" +
                                             "odhlasit se | exit | konec";


        private static Uzivatel uzivatel = null;

        private static Databaze databaze;

        public static void Main(string[] args)
        {

            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelHandler);


            //WRITING TEST
            //databaze = new Databaze();
            //databaze.UlozDatabazi();


            //READING TEST

            databaze = Databaze.NactiDatabazi();

            //databaze.Test();

            //Console.WriteLine(Uzivatel.Hash("heslo"));

            //return;



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
                                Console.WriteLine(auto);
                            }




                            int idAuta = NactiCislo("Zadejte ID auta: ");

                            if (volnaAuta.Where(a => a.Uid == idAuta).FirstOrDefault() == null)
                            {
                                Console.WriteLine("Chyba: Zadané ID auta neexistuje nebo není dostupné");
                                goto loop;
                            }



                            List<Uzivatel> uzivatele = databaze.VsichniUzivatele();

                            Console.Write("Vypsat všechny uživatele? (y|N) ");
                            char vypsatUzivatele = Console.ReadKey().KeyChar;

                            if (char.ToLower(vypsatUzivatele) == 'y')
                            {
                                foreach (Uzivatel uzivatel in uzivatele)
                                {
                                    Console.WriteLine(uzivatel);
                                }
                            }

                            int idUzivatele = NactiCislo("Zadejte ID uživatele: ");

                            if (uzivatele.Where(u => u.Uid == idUzivatele).FirstOrDefault() == null)
                            {
                                Console.WriteLine("Chyba: Zadané ID uzivatle neexistuje");
                                goto loop;
                            }

                            databaze.PridatRezervaci(idUzivatele, idAuta, od, @do);

                            Console.WriteLine("Rezervace úspěšně přidána");

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
                            foreach (Rezervace rezervace in databaze.VsechnyRezervace(uzivatel.Uid))
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
                                Console.WriteLine(auto);
                            }

                            int idAuta;
                            while (true)
                            {
                                Console.Write("Zadejte id auta: ");
                                if (!int.TryParse(Console.ReadLine(), out idAuta))
                                {
                                    Console.WriteLine("Zadaná hodnota musí být číslo");
                                    continue;
                                }

                                if (volnaAuta.Where(a => a.Uid == idAuta).FirstOrDefault() == null)
                                {
                                    Console.WriteLine("Chyba: Zadané ID auta neexistuje nebo není dostupné");
                                    goto loop;
                                }

                                break;
                            }

                            databaze.PridatRezervaci(uzivatel.Uid, idAuta, od, @do);

                            Console.WriteLine("Rezervace úspěšně přidána");

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

                Console.WriteLine("Přihlášen jako {0} {1})", uzivatel.Jmeno, uzivatel.Prijmeni);
                Console.WriteLine("Poslední přihlášení: {0})", uzivatel.PosledniPrihlaseni);

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


        public static string NactiHeslo()
        {
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

        public static int NactiCislo(string vyzva)
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

        protected static void CancelHandler(object sender, ConsoleCancelEventArgs args)
        {
            databaze.UlozDatabazi();
        }
    }
}
