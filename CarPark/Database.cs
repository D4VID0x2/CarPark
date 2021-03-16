using System;
using System.Collections.Generic;
using System.Text;

namespace CarPark
{
    public static class Database
    {
        private static List<User> users = new List<User>();
        private static List<Car> cars = new List<Car>();
        private static List<Reservation> reservations = new List<Reservation>();


        public static List<Reservation> GetReservations(User user)
        {
            List<Reservation> output = new List<Reservation>();

            return output;
        }


        public static bool IsEmailUnique (string email)
        {
            foreach (User user in users)
            {
                if (user.Email == email) return false;
            }
            return true;
        }

        
        public static bool AddReservation(User user, int carId, DateTime from, DateTime until)
        {
            // TODO: check if the car is free at this time
            
            //Car car = cars[carId];

            //reservations.Add(new Reservation(user, car, from, until));


            return true;
        }
    }
}
