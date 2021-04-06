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
        [DataMember(Name="id")]
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
            return string.Format("#{0}:\n  \tAuto: {1}\n  \tOd: {2}\n  \tDo: {3}\n", this.Uid, this.Auto, this.Od, this.Od);
        }

    }
}
