namespace KniznicaLibrary
{

     public class Statistiky (Kniznica kniznica)
     {
          private ZoznamKnih? _zoznam;

          public ZoznamKnih? Zoznam
          {
               get => _zoznam;
               set => _zoznam = value;
          }

          public double PriemerHodnoteni()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;

               if (subor.Count == 0)
               {
                    return 0;
               }
               double priemer = (from kniha in subor
                    select kniha.Hodnotenie).Average();
               return priemer;
          }

          public int PocetKnih()
          {
               return _zoznam?.Count ?? kniznica.ZoznamVsetkychKnih.Count;
          }

          public int PocetPrecitanych()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return 0;
               }
               int pocet = (from kniha in subor
                    where kniha.Precitana
                    select kniha).Count();
               return pocet;
          }

          public int PocetKupenych()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return 0;
               }
               int pocet = (from kniha in subor
                    where kniha.Kupena
                    select kniha).Count();
               return pocet;
          }

          public double PomerPrecitanych()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return 0;
               }
               int pocet = (from kniha in subor
                    where kniha.Precitana
                    select kniha).Count();
               return (double)pocet / subor.Count;
          }

          public Kniha? NajstarsiaKniha()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return null;
               }
               Kniha najstarsia = (from kniha in subor
                    orderby kniha.RokVydania ascending 
                    select kniha).First();
               return najstarsia;
          }

          public Kniha? NajmladsiaKniha()
          {
               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return null;
               }
               Kniha najmladsia = (from kniha in subor
                    orderby kniha.RokVydania descending 
                    select kniha).First();
               return najmladsia;
          }

          public int PocetAutorov()
          {
               if (_zoznam != null)
               {
                    if (_zoznam.Count == 0)
                    {
                         return 0;
                    }
                    var autori = (from kniha in _zoznam
                         select kniha.AutorKnihy).Distinct();
                    return autori.Count();
               }
               return kniznica.ZoznamVsetkychAutorov.Count;
               
          }

          public Autor? AutorNajviacKnih()
          {

               ZoznamKnih subor = _zoznam ?? kniznica.ZoznamVsetkychKnih;
               if (subor.Count == 0)
               {
                    return null;
               }
               var autori = (from kniha in subor
                    select kniha.AutorKnihy).Distinct().ToList();
               var autorNaj = from autor in kniznica.ZoznamVsetkychAutorov
                         where autori.Contains(autor.Meno)
                         orderby autor.ZoznamKnihAutora.Count descending
                         select autor;
               return autorNaj.First();
          }
     }
}
