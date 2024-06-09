using Newtonsoft.Json;
using OpenLibraryNET.Data;
using OpenLibraryNET.Loader;

namespace KniznicaLibrary
{
     public class Kniznica
     {
          public ZoznamKnih ZoznamVsetkychKnih { get; set; }
          public ZoznamAutorov ZoznamVsetkychAutorov { get; set; }
          public List<ZoznamKnih> KniznicaList { get; set; }

          private readonly HttpClient _klient = new();

          public Kniznica(FileInfo jsonFileKnihy, FileInfo jsonFileAutori)
          {
               ZoznamVsetkychKnih = new ZoznamKnih("Všetky knihy");
               ZoznamVsetkychAutorov = [];
               KniznicaList =
               [
                    ZoznamVsetkychKnih
               ];

               StreamReader reader = jsonFileKnihy.OpenText();

               string json = reader.ReadToEnd();
               var knihyData = JsonConvert.DeserializeObject<Dictionary<string, List<Kniha>>>(json);

               if (knihyData != null)
                    foreach (var zoznam in knihyData)
                    {
                         var zoz = new ZoznamKnih(zoznam.Key);
                         foreach (var kniha in zoznam.Value)
                         {
                              ZoznamVsetkychKnih.Add(kniha);
                              zoz.Add(kniha);
                         }

                         KniznicaList.Add(zoz);
                    }

               reader.Close();

               reader = jsonFileAutori.OpenText();
               json = reader.ReadToEnd();
               ZoznamVsetkychAutorov = JsonConvert.DeserializeObject<ZoznamAutorov>(json) ?? [];

               foreach (var autor in ZoznamVsetkychAutorov)
               {
                    autor.ZoznamKnihAutora =
                         ZoznamVsetkychKnih.NajdiPodlaPodmienky((kniha => kniha.AutorKnihy == autor.Meno));
                    if (autor.Kod == null)
                    {
                         _ = NajdiAutora(autor);
                    }
               }
          }

          public void PridajKnihu(Kniha kniha)
          {
               ZoznamVsetkychKnih.Add(kniha);
               var autor = ZoznamVsetkychAutorov.NajdiPodlaPodmienky((autor => autor.Meno == kniha.AutorKnihy));
               autor?.ZoznamKnihAutora.Add(kniha);
          }

          public void PridajKnihu(Kniha kniha, string zoznam)
          {
               if (zoznam != ZoznamVsetkychKnih.NazovZoznamu)
                    ZoznamVsetkychKnih.Add(kniha);
               KniznicaList.Find((zoz => zoz.NazovZoznamu == zoznam))?.Add(kniha);
               var autor = ZoznamVsetkychAutorov.NajdiPodlaPodmienky((autor => autor.Meno == kniha.AutorKnihy));
               if (autor == null && kniha.AutorKnihy != "")
               {
                    PridajAutora(new Autor(kniha.AutorKnihy));
               }
               autor?.ZoznamKnihAutora.Add(kniha);
          }

          public void OdoberKnihu(Kniha kniha)
          {
               foreach (var zoznam in KniznicaList)
               {
                    zoznam.Remove(kniha);
               }
          }

          public void OdoberKnihy(Predicate<Kniha> podmienka)
          {
               foreach (var zoznam in KniznicaList)
               {
                    List<Kniha> knihyZmazat = [];
                    foreach (var kniha in zoznam)
                    {
                         if (podmienka(kniha))
                         {
                             knihyZmazat.Add(kniha);
                         }
                    }

                    foreach (var kniha in knihyZmazat)
                    {
                         zoznam.Remove(kniha);
                    }
               }
          }

          public void AktualizujAutora(Autor autor)
          {
               foreach (var kniha in autor.ZoznamKnihAutora)
               {
                    kniha.AutorKnihy = autor.Meno;
               }
               _ = NajdiAutora(autor);
          }

          public void PridajAutora(Autor autor)
          {
               if (!ZoznamVsetkychAutorov.Contains(autor))
               {
                    ZoznamVsetkychAutorov.Add(autor);
                    autor.ZoznamKnihAutora =
                         ZoznamVsetkychKnih.NajdiPodlaPodmienky((kniha) => kniha.AutorKnihy == autor.Meno);
                    _ = NajdiAutora(autor);
               }
          }

          /**
           * Pokúsi sa nájsť autora v Open Library databáze.
           * Ak ho nájde, doplní chýbajúce informácie a uloží do autora jeho kód na neskôršie použitie.
           */
          private async Task NajdiAutora(Autor autor)
          {
               var index = ZoznamVsetkychAutorov.IndexOf(autor);
               OLAuthorData[]? authorData = await OLSearchLoader.GetAuthorSearchResultsAsync(_klient, autor.Meno);
               if (authorData != null)
               {
                    foreach (var data in authorData)
                    {
                         if (data.BirthDate != "")
                         {
                              string datum = DateTime.Parse(data.BirthDate).ToString("dd.MM.yyyy");
                              if (autor.DatumNarodenia == null)
                              {
                                   autor.DatumNarodenia = datum;
                                   if (data.DeathDate != "")
                                   {
                                        datum = DateTime.Parse(data.DeathDate).ToString("dd.MM.yyyy");
                                        autor.DatumUmrtia = datum;
                                   }

                                   autor.Kod = data.Key;
                                   break;
                              }

                              if (autor.DatumNarodenia != null && autor.DatumNarodenia == datum)
                              {
                                   autor.Kod = data.Key;
                                   break;
                              }
                         }
                         else if (data.DeathDate != "")
                         {
                              string datum = DateTime.Parse(data.DeathDate).ToString("dd.MM.yyyy");
                              if (autor.DatumUmrtia == null)
                              {
                                   autor.DatumUmrtia = datum;
                                   autor.Kod = data.Key;
                                   break;
                              }

                              if (autor.DatumUmrtia != null && autor.DatumUmrtia == datum)
                              {
                                   autor.Kod = data.Key;
                                   break;
                              }
                         }
                    }

                    ZoznamVsetkychAutorov.RemoveAt(index);
                    ZoznamVsetkychAutorov.Insert(index, autor);
               }
          }
     }
}
