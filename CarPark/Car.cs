using System;


public class Car
{
    public Guid Guid { get; private set; }
    public Brand Brand { get; private set; }
    public Model Model { get; private set; }
    public Type Type { get; private set; }
    public double ConsumptionPer100km { get; private set; }
}


public enum Brand
{
    Skoda,
}

public enum Model
{
    Octavia,
}


public enum Type
{
    Osobni,
    Nakladni
}
