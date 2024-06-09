using System.IO;
using System.Windows.Documents;
using System.Windows;
using KniznicaLibrary;
using System.Globalization;
using System.Windows.Controls;

namespace KniznicaWpfApp
{
     internal class PomocneFunkcie
     {
          public static void OdstranNoveSuboryVratStare(string hlavnySubor, List<string> zmeny)
          {
               string[] filePaths = Directory.GetFiles(hlavnySubor);
               foreach (var file in filePaths)
               {
                    if (zmeny.Contains(Path.GetFileName(file)))
                    {
                         File.Delete(file);
                    }
               }

               filePaths = Directory.GetFiles(hlavnySubor + "Kos");

               foreach (var file in filePaths)
               {
                    File.Move(file, hlavnySubor + $"{Path.GetFileName(file)}");
               }
          }

          public static void SkopirujNovyObrazok(string ciel, string? zdrojSubor, string staraCesta, out string novaCesta, ref List<string> zmeny)
          {
               if (zdrojSubor != null)
               {
                    if (staraCesta != "")
                    {
                         var zdrojPresunu = ciel + staraCesta;
                         var cielPresunu = ciel + "Kos/" + staraCesta;
                         File.Move(zdrojPresunu, cielPresunu);
                    }
                    

                    if (File.Exists(zdrojSubor))
                    {
                         File.Copy(zdrojSubor, ciel + Path.GetFileName(zdrojSubor));
                         novaCesta = Path.GetFileName(zdrojSubor);
                         zmeny.Add(novaCesta);
                         return;
                    }
               }
               novaCesta = staraCesta;
          }

          public static Uri DajObrazokUri(string obrazok, string zdroj, string predvolene)
          {
               
               if (obrazok != "")
               {
                    return new Uri($"{zdroj}{obrazok}", UriKind.Relative);
               }
               return new Uri($"{zdroj}{predvolene}", UriKind.Relative);
          }

          public static void NastavStatistiku(ref RichTextBox statistika, Statistiky stats)
          {
               statistika.Document.Blocks.Clear();

               Paragraph paragraph = new();
               PridajText(paragraph, "Počet kníh: ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.PocetKnih().ToString(), 15, FontWeights.Bold);
               PridajText(paragraph, "\nPriemer hodnotení: ", 15, FontWeights.Bold);
               var str = stats.PriemerHodnoteni().ToString(CultureInfo.CurrentCulture);
               PridajText(paragraph, str[..(str.Length < 4 ? str.Length : 4)], 15, FontWeights.Bold);
               PridajText(paragraph, "\nPočet kúpených: ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.PocetKupenych().ToString(), 15, FontWeights.Bold);
               PridajText(paragraph, "\nPomer prečítaných: ", 15, FontWeights.Bold);
               str = stats.PomerPrecitanych().ToString(CultureInfo.CurrentCulture);
               PridajText(paragraph, str[..(str.Length < 4 ? str.Length : 4)], 15, FontWeights.Bold);
               PridajText(paragraph, "\nNajstaršia kniha: ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.NajstarsiaKniha()?.Nazov ?? "", 15, FontWeights.Bold);
               PridajText(paragraph, "\nNajmladšia kniha ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.NajmladsiaKniha()?.Nazov ?? "", 15, FontWeights.Bold);
               PridajText(paragraph, "\nPočet autorov: ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.PocetAutorov().ToString(), 15, FontWeights.Bold);
               PridajText(paragraph, "\nAutor s najviac knihami: ", 15, FontWeights.Bold);
               PridajText(paragraph, stats.AutorNajviacKnih()?.Meno ?? "", 15, FontWeights.Bold);

               statistika.Document.Blocks.Add(paragraph);
          }

          public static void PridajText(Paragraph paragraph, string text, double fontSize, FontWeight fontWeight)
          {
               Run run = new(text)
               {
                    FontWeight = fontWeight,
                    FontSize = fontSize
               };
               paragraph.Inlines.Add(run);
          }
     }
}
