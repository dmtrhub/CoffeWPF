using System;

namespace Classes
{
    [Serializable]
    public class Kafa
    {
        public string Naziv { get; set; }
        public DateTime Datum { get; set; }
        public int Index { get; set; }
        public string Slika { get; set; }
        public string Opis { get; set; }

        public Kafa(int index, string slika, string naziv, DateTime datum, string opis)
        {
            Naziv = naziv;
            Datum = datum;
            Index = index;
            Slika = slika;
            Opis = opis;
        }
    }
}
