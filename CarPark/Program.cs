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
