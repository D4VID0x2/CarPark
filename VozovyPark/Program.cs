using System;
using System.Security;

namespace VozovyPark
{
    public class Program
    {

        private const string napoveda = "reservations [list|add|cancel]\n" +
                                        "changepassword\n" + 
                                        "logout";


        private static Uzivatel uzivatel = null;

        public static void Main(string[] args)
        {


            uzivatel = Prihlaseni();

            bool exit = false;
            while (!exit)
            {

                if (uzivatel.JeAdmin) {

                    Console.Write("# ");
                    string cmd = Console.ReadLine().Trim().ToLower();

                    if (cmd.Length < 1) continue;

                } else {

                    Console.Write("$ ");
                    string cmd = Console.ReadLine().Trim().ToLower();

                    if (cmd.Length < 1) continue;

                    switch (cmd)
                    {
                        case "seznam rezervaci":
                            foreach (Rezervace rezervace in Databaze.VsechnyRezervace(uzivatel))
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

                            break;

                        case "zmenit heslo":
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

                                Console.Write("Zadejte nové heslo: ");
                                string noveHeslo = NactiHeslo();
                                Console.WriteLine();
                                Console.Write("Potvrďte nové heslo: ");
                                string potvrzeniHesla = NactiHeslo();
                                Console.WriteLine();


                                if (noveHeslo != potvrzeniHesla) {
                                    Console.WriteLine("Nová hesla nejsou stejná");
                                    continue;
                                }
                            }

                            uzivatel.ZmenitHeslo(noveHeslo);

                            Console.WriteLine("Heslo změněno");

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
                /*
                switch (cmd[0].Trim())
                {



                    case "reservations":

                        if (cmd.Length < 2)
                        {
                            Console.WriteLine("reservations [list|add|cancel]");
                            continue;
                        }
                        switch (cmd[1])
                        {

                            case "add":
                                int carId;
                                while (true)
                                {
                                    Console.WriteLine("Available cars:");
                                    //TODO: list all cars
                                    Console.WriteLine("1: Skoda octavia");
                                    Console.Write("Choose car: ");
                                    if (int.TryParse(Console.ReadLine(), out carId))
                                    {
                                        //TODO: if carId is valid
                                        break;
                                    }
                                }
                                DateTime od;
                                while (true)
                                {
                                    Console.Write("From: ");
                                    if (DateTime.TryParse(Console.ReadLine(), out od))
                                    {
                                        break;
                                    }
                                }
                                DateTime @do;
                                while (true)
                                {
                                    Console.Write("Until: ");
                                    if (DateTime.TryParse(Console.ReadLine(), out @do))
                                    {
                                        break;
                                    }
                                }

                                Databaze.PridatRezervaci(uzivatel, carId, od, @do);

                                break;

                            case "cancel":
                                Console.WriteLine("Reservation has been canceled");
                                //TODO: cancel reservation
                                break;

                            default:
                                Console.WriteLine("Unknown option: {0}", cmd[1]);
                                Console.WriteLine("reservations [list|add|cancel]");
                                break;
                        }

                        break;
                }
                */
            }

        }


        private static Uzivatel Prihlaseni () {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            while (true)
            {
                Console.Write("Heslo: ");
                string hash = NactiHeslo();
                Console.WriteLine();

                Uzivatel uzivatel = Databaze.Prihlaseni(email, hash);

                if (uzivatel == null) 
                {
                    Console.WriteLine("Neplatné přihlašovací údaje");
                    continue;
                }
            }

            return uzivatel;
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
