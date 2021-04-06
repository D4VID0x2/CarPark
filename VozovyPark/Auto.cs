using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VozovyPark
{

    [DataContract]
    public class Auto
    {
        [DataMember(Name = "id")]
        public int Uid { get; private set; }
        [DataMember(Name = "znacka")]
        public string Znacka { get; private set; }
        [DataMember(Name = "model")]
        public string Model { get; private set; }
        [DataMember(Name = "typ")]
        public TypAuta Typ { get; private set; }
        [DataMember(Name = "spotreba")]
        public double SpotrebaNa100km { get; private set; }

        [DataMember(Name = "servisniukony")]
        public List<ServisniUkon> servisniUkony { get; private set; }

        public Auto(string znacka, string model, TypAuta typ, double spotreba)
        {
            Uid = UID.newUID<Auto>();
            this.Znacka = znacka;
            this.Model = model;
            this.Typ = typ;
            this.SpotrebaNa100km = spotreba;
            this.servisniUkony = new List<ServisniUkon>();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Znacka, this.Model);
        }

    }

    public enum TypAuta
    {
        Osobni,
        Nakladni,
        Obytne,
        Kamion
    }
}
