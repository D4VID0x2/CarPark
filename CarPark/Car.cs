using System;

namespace CarPark
{

    public class Car
    {
        public int Uid { get; private set; }
        public CarBrand Brand { get; private set; }
        public CarModel Model { get; private set; }
        public CarType Type { get; private set; }
        public double ConsumptionPer100km { get; private set; }

        public Car(CarBrand brand, CarModel model, CarType type, double comsumption)
        {
            Uid = UID.newUID<Car>();
            this.Brand = brand;
            this.Model = model;
            this.Type = type;
            this.ConsumptionPer100km = comsumption;
        }

    }
    public enum CarBrand
    {
        Skoda,
    }

    public enum CarModel
    {
        Octavia,
    }


    public enum CarType
    {
        Osobni,
        Nakladni
    }
}
