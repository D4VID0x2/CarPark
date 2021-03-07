using System;

namespace CarPark
{
    public class Reservation
    {
        public Guid Guid { get; private set; }
        public User User { get; private set; }
        public Car Car { get; private set; }
        public DateTime From { get; private set; }
        public DateTime Until { get; private set; }

    }
}
