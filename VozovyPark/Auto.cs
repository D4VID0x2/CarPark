using System;

namespace VozovyPark
{

    public class Auto
    {
        public int Uid { get; private set; }
        public ZnackaAuta Znacka { get; private set; }
        public ModelAuta Model { get; private set; }
        public TypAuta Typ { get; private set; }
        public double SpotrebaNa100km { get; private set; }

        public Auto(ZnackaAuta znacka, ModelAuta model, TypAuta typ, double spotreba)
        {
            Uid = UID.newUID<Auto>();
            this.Znacka = znacka;
            this.Model = model;
            this.Typ = typ;
            this.SpotrebaNa100km = spotreba;
        }

    }
    public enum ZnackaAuta
    {
        Skoda,
    }

    public enum ModelAuta
    {
        Octavia,
    }


    public enum TypAuta
    {
        Osobni,
        Nakladni
    }
}
