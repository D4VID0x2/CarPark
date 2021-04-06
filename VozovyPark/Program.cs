using System;
using System.Security;
using System.Collections.Generic;

namespace VozovyPark
{
    public class Program
    {

        private const string napoveda = "reservations [list|add|cancel]\n" +
                                        "changepassword\n" +
                                        "logout";

        private const string adminNapoveda = "reservations [list|add|cancel]\n" +
                                             "changepassword\n" +
                                             "logout";


        private static Uzivatel uzivatel = null;

        private static Databaze databaze;

        public static void Main(string[] args)
        {

            //WRITING TEST
            //databaze = new Databaze();
            //databaze.UlozDatabazi();


            //READING TEST

            databaze = Databaze.NactiDatabazi();

            //databaze.Test();

            //Console.WriteLine(Uzivatel.Hash("heslo"));

            //return;





            uzivatel = Prihlaseni();

            bool exit = false;
            while (!exit)
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

                        case "napoveda":
                        case "?":
                            Console.WriteLine(adminNapoveda);
                            break;

                        case "odhlasit-se":
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

                            DateTime od;
                            while (true)
                            {
                                Console.Write("Od: ");
                                if (DateTime.TryParse(Console.ReadLine(), out od))
                                {
                                    Console.WriteLine("Zadaná hodnota není typu DateTime");
                                    break;
                                }
                                if (od < DateTime.Now)
                                {
                                    Console.WriteLine("Zadaná hodnota nesmí být v minulosti");
                                    break;
                                }
                            }
                            DateTime @do;
                            while (true)
                            {
                                Console.Write("Do: ");
                                if (DateTime.TryParse(Console.ReadLine(), out @do))
                                {
                                    Console.WriteLine("Zadaná hodnota není typu DateTime");
                                    break;
                                }
                                if (@do < DateTime.Now)
                                {
                                    Console.WriteLine("Zadaná hodnota nesmí být v minulosti");
                                    break;
                                }
                                if (@do < od)
                                {
                                    Console.WriteLine("Zadaná hodnota musí být větší než hodnota Od");
                                    break;
                                }
                            }

                            List<Auto> volnaAuta = databaze.VolnaAuta(od, @do);

                            Console.WriteLine("Volná auta:");
                            foreach (Auto auto in volnaAuta)
                            {
                                Console.WriteLine(auto);
                            }

                            int carId;
                            while (true)
                            {
                                Console.WriteLine("Zvolte auto:");
                                if (int.TryParse(Console.ReadLine(), out carId))
                                {
                                    //TODO: if carId is valid
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Zadaná hodnota musí být číslo");
                                }
                            }

                            break;

                        case "zmenit heslo":
                            ZmenaHesla();
                            break;

                        case "napoveda":
                        case "?":
                            Console.WriteLine(napoveda);
                            break;

                        case "odhlasit-se":
                        case "exit":
                        case "konec":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Neznámý příkaz: {0}", cmd[0]);
                            Console.WriteLine(napoveda);
                            break;
                    }

                }
            }

            databaze.UlozDatabazi();

        }


        private static Uzivatel Prihlaseni()
        {

            while (true)
            {
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Heslo: ");
                string hash = NactiHeslo();
                Console.WriteLine();

                Uzivatel uzivatel = databaze.Prihlaseni(email, hash);

                if (uzivatel == null)
                {
                    Console.WriteLine("Neplatné přihlašovací údaje");
                    continue;
                }

                //TODO: nutna zmena hesla

                return uzivatel;
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
    }
}
