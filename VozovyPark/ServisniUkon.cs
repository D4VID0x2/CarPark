using System;
using System.Runtime.Serialization;

namespace VozovyPark
{
    [DataContract]
    public class ServisniUkon
    {
        [DataMember(Name = "id")]
        public int Uid { get; private set; }
        [DataMember(Name = "od")]
        public DateTime Od { get; private set; }
        [DataMember(Name = "do")]
        public DateTime Do { get; private set; }
        [DataMember(Name = "cena")]
        public decimal Cena { get; private set; }
        [DataMember(Name = "popis")]
        public string Popis { get; private set; }
        [DataMember(Name = "faktura")]
        public int CisloFaktury { get; private set; }

        public ServisniUkon(DateTime od, DateTime @do, decimal cena, string popis, int cisloFaktury)
        {
            Uid = UID.newUID<ServisniUkon>();
            Od = od;
            Do = @do;
            Cena = cena;
            Popis = popis;
            CisloFaktury = cisloFaktury;
        }


    }
}
