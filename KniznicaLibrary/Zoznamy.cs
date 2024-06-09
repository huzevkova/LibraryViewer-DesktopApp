using System.Collections;
using System.Collections.ObjectModel;

namespace KniznicaLibrary
{

     public class ZoznamKnih(string nazovZoznamu) : ObservableCollection<Kniha>
     {
          public string NazovZoznamu { get; set; } = nazovZoznamu;

          public ZoznamKnih NajdiPodlaPodmienky(Predicate<Kniha> predikat)
          {
               var result = new ZoznamKnih("");
               foreach (var kniha in this)
               {
                    if (predikat(kniha))
                    {
                         result.Add(kniha);
                    }
               }
               return result;
          }
     }

     public class ZoznamAutorov : ObservableCollection<Autor>
     {
          public Autor? NajdiPodlaPodmienky(Predicate<Autor> predikat)
          {
               Autor? result = null;
               foreach (var autor in this)
               {
                    if (predikat(autor))
                    {
                         result = autor;
                         break;
                    }
               }
               return result;
          }

          public IEnumerable DajLenMena()
          {
               List<string> mena = new List<string>();
               foreach (var autor in this)
               {
                    mena.Add(autor.Meno);
               }
               return mena;
          }
     }
}
