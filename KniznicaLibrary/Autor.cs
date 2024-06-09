using Newtonsoft.Json;

namespace KniznicaLibrary
{
     public record Autor(
          string Meno,
          string? DatumNarodenia = null,
          string? DatumUmrtia = null,
          string Obrazok = "",
          ZoznamKnih? ZoznamKnihAutora = null)
     {
          public string Meno { get; set; } = Meno;
          public string? DatumNarodenia { get; set; } = DatumNarodenia;
          public string? DatumUmrtia { get; set; } = DatumUmrtia;
          public string Obrazok { get; set; } = Obrazok;
          public string? Kod { get; set; } = null;

          [JsonIgnore]
          public ZoznamKnih ZoznamKnihAutora { get; set; } = ZoznamKnihAutora ?? new ZoznamKnih(Meno);

     }
}
