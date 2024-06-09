
namespace KniznicaLibrary
{
     public record Kniha(
          string Nazov,
          string AutorKnihy,
          int? RokVydania,
          string Vydavatelstvo = "",
          string Obrazok = "",
          string Popis = "",
          string Poznamky = "",
          double Hodnotenie = 0,
          bool Precitana = false,
          bool NaNeskor = false,
          bool Kupena = false,
          bool Pozicana = false)
     {
          public string Nazov { get; set; } = Nazov;
          public string AutorKnihy { get; set; } = AutorKnihy;
          public int? RokVydania { get; set; } = RokVydania;
          public string Vydavatelstvo { get; set; } = Vydavatelstvo;
          public string Obrazok { get; set; } = Obrazok;
          public string Popis { get; set; } = Popis;
          public string Poznamky { get; set; } = Poznamky;
          public double Hodnotenie { get; set; } = Hodnotenie;
          public bool Precitana { get; set; } = Precitana;
          public bool NaNeskor { get; set; } = NaNeskor;
          public bool Kupena { get; set; } = Kupena;
          public bool Pozicana { get; set; } = Pozicana;

     }
}
