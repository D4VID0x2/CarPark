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
            return reservations.FindAll(r => r.User == user);
        }
        public static List<Reservation> GetReservations(Car car)
        {
            return reservations.FindAll(r => r.Car == car);
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


        public static List<Car> GetAvailableCars (DateTime from, DateTime until)
        {
            List<Car> availableCars = new List<Car>();
            foreach (Car car in cars)
            {
                bool isFree = true;
                foreach (Reservation r in GetReservations(car))
                {
                    if (r.From < from && r.Until > from && r.Until < until) // beginning overlaps
                    {
                       isFree = false; 
                       break;
                    }
                    if (r.From > from && r.From < until && r.Until > from && r.Until < until) // middle overlaps
                    {
                       isFree = false; 
                       break;
                    }
                    if (r.From < from && r.From > until && r.Until > until) // end overlaps
                    {
                       isFree = false; 
                       break;
                    }
                }
                if (isFree)
                {
                    availableCars.Add(car);
                }
            }

            return availableCars;
        }
    }
}
