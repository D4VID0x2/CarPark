using System;

namespace VozovyPark
{
    public class Rezervace
    {
        public int Uid { get; private set; }
        public Uzivatel Uzivatel { get; private set; }
        public Auto Auto { get; private set; }
        public DateTime Od { get; private set; }
        public DateTime Do { get; private set; }

        public Rezervace(Uzivatel uzivatel, Auto auto, DateTime od, DateTime @do)
        {
            this.Uid = UID.newUID<Rezervace>();
            this.Uzivatel = uzivatel;
            this.Auto = auto;
            this.Od = od;
            this.Do = @do;
        }

    }
}