using System;

namespace VozovyPark
{

    public class Auto
    {
        public int Uid { get; private set; }
        public string Znacka { get; private set; }
        public string Model { get; private set; }
        public TypAuta Typ { get; private set; }
        public double SpotrebaNa100km { get; private set; }

        public Auto(string znacka, string model, TypAuta typ, double spotreba)
        {
            Uid = UID.newUID<Auto>();
            this.Znacka = znacka;
            this.Model = model;
            this.Typ = typ;
            this.SpotrebaNa100km = spotreba;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Znacka, this.Model);
        }

    }

    public enum TypAuta
    {
        Osobni,
        Nakladni
    }
}
