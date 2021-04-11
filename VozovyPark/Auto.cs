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
        public decimal SpotrebaNa100km { get; private set; }

        [DataMember(Name = "servisniukony")]
        public List<ServisniUkon> servisniUkony { get; private set; }

        public Auto(string znacka, string model, TypAuta typ, decimal spotreba)
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
            return string.Format("{0} {1} (#{2})", Znacka, Model, Uid);
        }

        public string ToStringLong()
        {
            return string.Format("#{0}:\n  Značka: {1}\n  Model: {2}\n  Typ: {3}\n  Spotřeba: {4}", this.Uid, this.Znacka, this.Model, this.Typ, this.SpotrebaNa100km);
        }

    }

    public enum TypAuta
    {
        Osobni = 1,
        Nakladni = 2,
    }
}
