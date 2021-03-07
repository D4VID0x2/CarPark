using System;
using System.Security;

namespace CarPark
{
    public class Program
    {
        public static void Main(string[] args)
        {



            // Login:
            Console.Write("Jmeno: ");
            string name = Console.ReadLine();
            Console.Write("Prijmeni: ");
            string surname = Console.ReadLine();

            Console.Write("Heslo: ");
            string password = ReadPassword();
            Console.WriteLine();





            bool exit = false;
            while (!exit)
            {

                Console.Write("$ ");
                string[] cmd = Console.ReadLine().Split(' ');

                if (cmd.Length < 1) continue;

                switch (cmd[0].Strip())
                {
                    case "reservations":

                        if (cmd.Length < 2)
                        {
                            Console.WriteLine("reservations [list|add]");
                            continue;
                        }
                        switch (cmd[1])
                        {
                            case "list":
                                //TODO: list all reservations by this user
                                Console.WriteLine("Reservation #1:");
                                Console.WriteLine("\tCar #1");
                                Console.WriteLine("\tFrom 1.1.2021 12:00");
                                Console.WriteLine("\tUntil 2.1.2021 12:00");
                                break;

                            case "add":
                                int carId;
                                while (true)
                                {
                                    Console.WriteLine("Available cars:");
                                    //TODO: list all cars
                                    Console.WriteLine("1: Skoda octavia");
                                    Console.Write("Choose car: ");
                                    if (int.TryParse(Console.ReadLine(), out int carId)) {
                                        //TODO: if carId is valid
                                    }
                                }

                                break;

                            default:
                                Console.WriteLine("reservations [list|add]");
                                break;
                        }

                        break;

                    case "help":
                        Console.WriteLine("reservations [list|add]");
                        Console.WriteLine("logout");
                        break;

                    case "logout":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Unknown command: {0}", cmd[0]);
                        break;
                }
            }

        }


        public static string ReadPassword()
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
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    password += i.KeyChar;
                    Console.Write("*");
                }
            }
            return password;
        }
    }
}
