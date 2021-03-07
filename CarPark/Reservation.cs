using System;

namespace CarPark
{
    public class Reservation
    {
        public int Uid { get; private set; }
        public User User { get; private set; }
        public Car Car { get; private set; }
        public DateTime From { get; private set; }
        public DateTime Until { get; private set; }

        public Reservation(User user, Car car, DateTime from, DateTime until)
        {
            this.Uid = UID.newUID<Reservation>();
            this.User = user;
            this.Car = car;
            this.From = from;
            this.Until = until;
        }

    }
}
