using System;
using System.Security;

namespace VozovyPark
{
    public class Program
    {

        private const string helpText = "reservations [list|add|cancel]\n" +
                                        "changepassword\n" + 
                                        "logout";


        private static Uzivatel uzivatel = null;

        public static void Main(string[] args)
        {


            uzivatel = Login();

            bool exit = false;
            while (!exit)
            {

                Console.Write("$ ");
                string[] cmd = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (cmd.Length < 1) continue;

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
                            case "list":
                                //TODO: list all reservations by this uzivatel
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

                                Databaze.AddReservation(uzivatel, carId, od, @do);

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

                    case "changepassword":
                        Console.Write("Enter old password: ");
                        string oldPassword = ReadPassword();
                        Console.WriteLine();
                        Console.Write("Enter new password: ");
                        string newPassword = ReadPassword();
                        Console.WriteLine();
                        Console.Write("Enter new password again: ");
                        string newPasswordConfirm = ReadPassword();
                        Console.WriteLine();

                        Console.WriteLine("Password changed");
                        //TODO: password change
                        break;

                    case "help":
                    case "?":
                        Console.WriteLine(helpText);
                        break;

                    case "logout":
                    case "exit":
                    case "quit":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Unknown command: {0}", cmd[0]);
                        Console.WriteLine(helpText);
                        break;
                }
            }

        }


        private static Uzivatel Login () {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Surname: ");
            string surname = Console.ReadLine();

            Console.Write("Password: ");
            string password = ReadPassword();
            Console.WriteLine();


            return null; //TODO: get uzivatel from database
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