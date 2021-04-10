using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace VozovyPark
{
    [DataContract]
    public class Rezervace
    {
        [DataMember(Name="id")]
        public int Uid { get; private set; }
        [DataMember(Name="uzivatel")]
        public int Uzivatel { get; private set; }
        [DataMember(Name="auto")]
        public int Auto { get; private set; }
        [DataMember(Name="od")]
        public DateTime Od { get; private set; }
        [DataMember(Name="do")]
        public DateTime Do { get; private set; }

        public Rezervace(int uzivatel, int auto, DateTime od, DateTime @do)
        {
            this.Uid = UID.newUID<Rezervace>();
            this.Uzivatel = uzivatel;
            this.Auto = auto;
            this.Od = od;
            this.Do = @do;
        }


        public override string ToString()
        {
            return string.Format("#{0}:\n  Auto: {1}\n  Uživatel: {2}\n  Od: {3}\n  Do: {4}\n", this.Uid, Databaze.instance.Auto(this.Auto), Databaze.instance.Uzivatel(this.Uzivatel), this.Od, this.Do);
        }

    }
}
